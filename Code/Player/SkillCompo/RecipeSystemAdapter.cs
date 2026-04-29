using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YIS.Code.Defines;
using YIS.Code.Skills;

namespace CIW.Code.Player.SkillCompo
{
    public class RecipeSystemAdapter : MonoBehaviour, IRecipeChecker
    {
        [SerializeField] private List<SkillDataSO> _allSkillDatabase = new List<SkillDataSO>();

        private void Awake()
        {
            if (_allSkillDatabase.Count == 0)
            {
                _allSkillDatabase.AddRange(Resources.LoadAll<SkillDataSO>("Skills"));
            }
        }

        public bool TryGetChainingSkill(SkillDataSO[] inputSkills, out SkillDataSO chainingSkill)
        {
            chainingSkill = null;
            if (inputSkills == null || inputSkills.Length < 3) return false;

            Elemental firstElem = inputSkills[0].elemental;
            bool allSameElemental = inputSkills.All(s => s.elemental == firstElem && s.elemental != Elemental.Normal);

            if (allSameElemental)
            {
                chainingSkill = _allSkillDatabase.FirstOrDefault(s => s.elemental == firstElem && s.grade > inputSkills[0].grade);

                return chainingSkill != null;
            }

            return false;
        }
    }
}

