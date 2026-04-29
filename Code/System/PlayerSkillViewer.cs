using CIW.Code.System.Events;
using PSW.Code.EventBus;
using System.Threading.Tasks;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CIW.Code.Player
{
    public class PlayerSkillViewer : MonoBehaviour
    {
        [Tooltip("����ŷ")]
        [SerializeField] LayoutElement maskLayout;

        [Tooltip("���� �г�")]
        [SerializeField] GameObject tooltipPanel;
        [SerializeField] Image IconImg;
        [SerializeField] TextMeshProUGUI nameTxt;
        [SerializeField] TextMeshProUGUI dmgTxt;
        [SerializeField] TextMeshProUGUI coolTxt;
        [SerializeField] TextMeshProUGUI descriptionTxt;

        float _targetWidth = 870.0f;
        bool _isOpening = false;

        private void Awake()
        {
            _targetWidth = maskLayout.preferredWidth;
            maskLayout.preferredWidth = 0;

            Bus<ShowTooltipEvent>.OnEvent += ShowTooltip;
            Bus<PointerExitEvent>.OnEvent += HideTooltip;
        }

        private void OnDestroy()
        {
            Bus<ShowTooltipEvent>.OnEvent -= ShowTooltip;
            Bus<PointerExitEvent>.OnEvent -= HideTooltip;
        }

        public void ShowTooltip(ShowTooltipEvent evt)
        {
            if (evt.skillData == null) return;

            nameTxt.text = evt.skillData.visualData.uiName;
            dmgTxt.text = $"DMG : <color=#0058de>{evt.skillData.damage}</color>";
            coolTxt.text = $"COOL : <color=#0058de>{evt.skillData.cooldownTurn}</color>";
            descriptionTxt.text = evt.skillData.visualData.itemDescription;
            IconImg.sprite = evt.skillData.visualData.icon;
        
            tooltipPanel.SetActive(true);
            TooltipAnim(true);
        }

        public void HideTooltip(PointerExitEvent evt)
        {
            TooltipAnim(false);
        }

        public async void TooltipAnim(bool isOpen)
        {
            _isOpening = isOpen;
            float duration = 0.2f; 
            float elapsed = 0f;

            float startWidth = maskLayout.preferredWidth;
            float endWidth = isOpen ? _targetWidth : 0f;

            float currentHeight = maskLayout.preferredWidth; 

            while (elapsed < duration)
            {
                if (_isOpening != isOpen) return;

                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float smoothedT = Mathf.Sin(t * Mathf.PI * 0.5f); 

                float currentWidth = Mathf.Lerp(startWidth, endWidth, smoothedT);

                maskLayout.preferredWidth = currentWidth;

                await Task.Yield();
            }

            maskLayout.preferredWidth = endWidth;
            if (!isOpen) tooltipPanel.SetActive(false);
        }
    }
}
