using CIW.Code.System.Events;
using PSW.Code.CombinationSkill;
using PSW.Code.EventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PSB.Code.BattleCode.Players;
using UnityEngine;
using Work.YIS.Code.Skills;
using YIS.Code.Defines;
using YIS.Code.Skills;

namespace CIW.Code.Player.SkillCompo
{
    public class SkillExecutionPacket
    {
        public SkillDataSO Data;
        public int UiIndex;
        public int targetIndex;
        public bool IsChain;
        public Action OnComplete;
    }

    public class PlayerSkillComponent : MonoBehaviour
    {
        [SerializeField] private List<SkillDataSO> _ownedSkills = new List<SkillDataSO>();

        [SerializeField] private PlayerSkillsCache skillsCache;

        private IRecipeChecker _checker;

        private readonly Dictionary<SkillDataSO, BaseSkill> _skillInstances = new Dictionary<SkillDataSO, BaseSkill>();
        private readonly List<SkillExecutionPacket> _executionList = new List<SkillExecutionPacket>();

        public void Initialize(IRecipeChecker checker)
        {
            _checker = checker;

            foreach (var data in _ownedSkills.ToArray())
                SetUpSkillInstance(data);

            Bus<SelectSkillsEvent>.OnEvent += HandleSelectSkill;
        }

        private void OnDestroy()
        {
            Bus<SelectSkillsEvent>.OnEvent -= HandleSelectSkill;
        }

        private void HandleSelectSkill(SelectSkillsEvent evt)
        {
            Debug.Log("들어옴");
            if (evt.skillDatas == null)
            {
                Debug.LogWarning("선택된 스킬 데이터가 없습니다.");
                return;
            }
            
            Bus<BeginAttackEvent>.Raise(new BeginAttackEvent());
            TryChainingSelectedSkills(evt.skillDatas);
        }

        public bool TryChainingSelectedSkills(SkillDataSO[] selectedSkills)
        {
            var validSkills = selectedSkills.Where(skill => skill != null).ToArray();
            if (validSkills.Length == 0) return false;

            foreach (var skillData in selectedSkills)
            {
                if (skillData != null && !_skillInstances.ContainsKey(skillData))
                    SetUpSkillInstance(skillData);
            }

            if (validSkills.Length > 1)
            {
                if (_checker.TryGetChainingSkill(validSkills, out SkillDataSO resultSkill) && resultSkill != null)
                {
                    PerformChaining(selectedSkills, resultSkill);
                    return true;
                }
            }

            if (selectedSkills.Length <= 0) return false;

            _executionList.Clear();
            HashSet<int> processedIndices = new HashSet<int>();

            bool[] links = (selectedSkills.Length >= 2) ? new bool[selectedSkills.Length - 1] : Array.Empty<bool>();

            for (int i = 0; i < links.Length; i++)
            {
                SkillDataSO leftSO = selectedSkills[i];
                SkillDataSO rightSO = selectedSkills[i + 1];
                if (leftSO == null || rightSO == null) continue;

                if (_skillInstances.TryGetValue(leftSO, out BaseSkill leftInst) &&
                    _skillInstances.TryGetValue(rightSO, out BaseSkill rightInst))
                {
                    bool leftToRight = (leftSO.checkSkillType == CheckType.Next) && leftInst.CanChainNext(rightInst);
                    bool rightToLeft = (rightSO.checkSkillType == CheckType.Previous) && rightInst.CanChainPrev(leftInst);

                    links[i] = leftToRight || rightToLeft;
                }
            }

            for (int i = 0; i < selectedSkills.Length; i++)
            {
                Debug.Log($"[루프체크] Index: {i}, 스킬 존재여부: {selectedSkills[i] != null}, " +
                          $"이미 처리됨: {processedIndices.Contains(i)}");
                if (processedIndices.Contains(i)) continue;

                SkillDataSO currentSO = selectedSkills[i];
                if (currentSO == null) continue;

                BaseSkill curInst = _skillInstances[currentSO];
                bool isChained = false;
                int targetIdx = i;
                
                if (currentSO.checkSkillType == CheckType.Next && i < selectedSkills.Length - 1)
                {
                    SkillDataSO nextSO = selectedSkills[i + 1];
                    if (nextSO != null && _skillInstances.TryGetValue(nextSO, out BaseSkill nextInst))
                    {
                        if (curInst.CanChainNext(nextInst))
                        {
                            isChained = true;
                            targetIdx = i + 1;
                        }
                    }
                }
                else if (currentSO.checkSkillType == CheckType.Previous && i > 0)
                {
                    SkillDataSO prevSO = selectedSkills[i - 1];
                    if (prevSO != null && _skillInstances.TryGetValue(prevSO, out BaseSkill prevInst))
                    {
                        if (curInst.CanChainPrev(prevInst))
                        {
                            isChained = true;
                            targetIdx = i - 1;
                        }
                    }
                }

                _executionList.Add(new SkillExecutionPacket
                {
                    Data = currentSO,
                    UiIndex = i,
                    targetIndex = targetIdx,
                    IsChain = isChained
                });
                Debug.LogWarning($"[패킷 생성됨] UI Index: {i}, Target Index: {targetIdx}, IsChain: {isChained}");
            }
            Debug.Log($"[최종결과] 실행 리스트에 담긴 총 개수: {_executionList.Count}");

            if (_executionList.Count > 0)
                StartCoroutine(ProcessingSkillSeq());

            return true;
        }

        private IEnumerator ProcessingSkillSeq()
        {
            bool isAllChained = _executionList.All(packet => packet.IsChain);
            if (isAllChained)
            {
                bool isAllEffectFinished = false;
                Bus<AllChainingEvent>.Raise(new AllChainingEvent(() => { isAllEffectFinished = true; }));
                yield return new WaitUntil(() => isAllEffectFinished);
            }
            else
            {
                for (int i = 0; i < _executionList.Count; i++)
                {
                    var packet = _executionList[i];
                    bool isEffectFinished = false;
                    packet.OnComplete = () => { isEffectFinished = true; };

                    if (packet.IsChain)
                        Bus<ChainingEvent>.Raise(new ChainingEvent(packet.Data, packet.UiIndex, packet.targetIndex, packet.OnComplete));
                    else
                        Bus<UnChainingEvent>.Raise(new UnChainingEvent(packet.Data, packet.OnComplete));

                    yield return new WaitUntil(() => isEffectFinished);
                }
            }

            var payload = BuildPayload();
            Bus<OnAttackEvent>.Raise(new OnAttackEvent(payload.ids, payload.flags));
        }

        private (SkillEnum[] ids, bool[] flags) BuildPayload()
        {
            int n = _executionList.Count;
            SkillEnum[] ids = new SkillEnum[n];
            bool[] flags = new bool[n];

            for (int i = 0; i < n; i++)
            {
                ids[i] = (SkillEnum)_executionList[i].Data.index;
                flags[i] = _executionList[i].IsChain;
            }

            return (ids, flags);
        }

        public void PerformChaining(SkillDataSO[] ingredients, SkillDataSO resultSkill)
        {
            foreach (var ingredient in ingredients)
            {
                if (ingredient == null ) continue;
                RemoveSkillInstance(ingredient);
            }

            AddSkill(resultSkill);
        }

        private void SetUpSkillInstance(SkillDataSO data)
        {
            if (data == null || _skillInstances.ContainsKey(data)) return;

            if (skillsCache == null)
            {
                Debug.LogError("[PlayerSkillComponent] skillsCache가 비어있음. 인스턴스 생성 불가");
                return;
            }

            if (!skillsCache.TryGetOrCreate(data, out BaseSkill compo) || compo == null)
            {
                Debug.LogError($"[Skill 생성 실패] 캐시에서 {data.skillName} 인스턴스를 가져올 수 없음");
                return;
            }
            
            compo.SetData(data);
            compo.Initialize();

            _skillInstances.Add(data, compo);
        }

        private void RemoveSkillInstance(SkillDataSO ingredient)
        {
            if (_ownedSkills.Contains(ingredient)) _ownedSkills.Remove(ingredient);

            if (_skillInstances.TryGetValue(ingredient, out BaseSkill instance))
            {
                _skillInstances.Remove(ingredient);
            }
        }

        public void AddSkill(SkillDataSO skill)
        {
            if (!_ownedSkills.Contains(skill) && skill != null)
            {
                _ownedSkills.Add(skill);
                SetUpSkillInstance(skill);
            }
        }

        public bool GetSkillInstance(SkillDataSO data, out BaseSkill instance)
            => _skillInstances.TryGetValue(data, out instance);

        public bool GetSkillInstanceById(SkillEnum id, out BaseSkill instance)
        {
            var pair = _skillInstances.FirstOrDefault(x => (SkillEnum)x.Key.index == id);
            instance = pair.Value;
            return instance != null;
        }

        public void SetActive(SkillEnum id, bool active)
        {
            if (skillsCache != null)
                skillsCache.SetActive(id, active);
        }
        
    }
}
