using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CIW.Code.Roulette
{
    public class RouletteInfoUI : MonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TextMeshProUGUI nameTxt;
        [SerializeField] TextMeshProUGUI percentTxt;
        [SerializeField] Button upgradeBtn;
        [SerializeField] Button downgradeBtn;

        int _itemIndex;
        Action<int, bool> _onWeightChange;

        public void Initialize(int idx, Sprite icon, string name, float percent, bool isMax, bool isMin, Action<int, bool> onWeightChange)
        {
            _itemIndex = idx;
            itemIcon.sprite = icon;
            nameTxt.text = name;
            UpdateUI(percent, false, false);
    
            _onWeightChange = onWeightChange;
            upgradeBtn.onClick.RemoveAllListeners();
            upgradeBtn.onClick.AddListener(() => _onWeightChange?.Invoke(_itemIndex, true));
            downgradeBtn.onClick.RemoveAllListeners();
            downgradeBtn.onClick.AddListener(() => _onWeightChange?.Invoke(_itemIndex, false));
        }

        public void UpdateUI(float percent, bool isMax, bool isMin)
        {
            percentTxt.text = $"{percent * 100f:F1}%";
            upgradeBtn.interactable = !isMax;
            if (downgradeBtn != null)
                downgradeBtn.interactable = !isMin;
        }
    }
}

