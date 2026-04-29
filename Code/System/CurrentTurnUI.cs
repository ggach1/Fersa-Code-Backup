using System.Collections;
using TMPro;
using UnityEngine;
using Work.CSH.Scripts.Interfaces;
using Work.CSH.Scripts.Managers;
using DG.Tweening;
using UnityEngine.UI;
using PSW.Code.EventBus;
using CIW.Code.System.Events;
using System;

namespace CIW.Code.System
{
    public class CurrentTurnUI : MonoBehaviour, ITurnable
    {
        [field : SerializeField] public TurnManagerSO TurnManager { get; set; }

        [SerializeField] RectTransform maskRt;
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] GameObject contents;
        [SerializeField] GameObject defenseArea;
        [SerializeField] TextMeshProUGUI currentTurnText;
        [SerializeField] private Material turnUiMap;
        [SerializeField] private ColorData colorData;

        [SerializeField] float duration = 0.5f;
        [SerializeField] float stopTime = 0.5f;

        float _targetWidth = 560;
        bool _isBattleEnd = false;

        private void Awake()
        {
            Vector3 tmp = maskRt.sizeDelta;
            tmp.x = 0;
            maskRt.sizeDelta = tmp;

            TurnManager.AddITurnableList(this);

            TurnManager.OnTurnDelayed += HandleTurnDelay;

            defenseArea.SetActive(false);
            Bus<BattleEnd>.OnEvent += HandleBattleEnd;
        }

        private void OnDestroy()
        {
            TurnManager.RemoveITurnableList(this);
            TurnManager.OnTurnDelayed -= HandleTurnDelay;
            Bus<BattleEnd>.OnEvent -= HandleBattleEnd;
        }

        private void HandleTurnDelay()
        {
            if (_isBattleEnd)
            {
                Debug.Log("전투 끝났다");
                return;
            }

            bool nextTurnIsPlayer = !TurnManager.Turn;

            defenseArea.SetActive(true);
            SetText(nextTurnIsPlayer);
            turnUiMap.SetColor("_GlowColor", colorData.GetPopColor(nextTurnIsPlayer));

            /*RectTransform content = maskRt.GetChild(0) as RectTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            _targetWidth = content.rect.width;*/

            PanelAnim(() => TurnManager.ChangeTurnActual());
        }

        public void PanelAnim(Action onComplete = null)
        {
            Debug.Log("패널 애니메이션 시작");
            DOTween.Kill(maskRt);
            DOTween.Kill(contents.transform);

            maskRt.anchoredPosition = Vector2.zero;
            maskRt.sizeDelta = new Vector2(0, maskRt.sizeDelta.y);

            Sequence seq = DOTween.Sequence();
            
            seq.Append(maskRt.DOSizeDelta(new Vector2(_targetWidth, maskRt.sizeDelta.y), duration).SetEase(Ease.OutExpo))
                /*.Join(_canvasGroup.DOFade(1f, duration / 2.5f)).SetEase(Ease.OutQuad)*/
                .Join(contents.transform.DOScale(1f, duration).SetEase(Ease.OutExpo));

            seq.AppendInterval(stopTime);

            seq.Append(maskRt.DOSizeDelta(new Vector2(0, maskRt.sizeDelta.y), duration * 2f).SetEase(Ease.OutExpo))
                /*.Join(_canvasGroup.DOFade(0f, duration * 2.5f)).SetEase(Ease.InQuad)*/
                .Join(contents.transform.DOScale(0.3f, duration).SetEase(Ease.InExpo));

            seq.OnComplete(() =>
            {
                defenseArea.SetActive(false);
                onComplete?.Invoke();
            });
        }

        private void HandleBattleEnd(BattleEnd evt) => _isBattleEnd = true;

        private void SetText(bool isTurn) => currentTurnText.SetText(isTurn ? "플레이어 턴" : "적 턴");

        public void OnStartTurn(bool isPlayerTurn) { }
        public void OnEndTurn(bool isPlayerTurn) { }
    }
}

[Serializable]
public struct ColorData
{
    public Color UpColor;
    public Color DownColor;

    public Color GetPopColor(bool isUp)
    {
        return isUp ? UpColor : DownColor;
    }
}

