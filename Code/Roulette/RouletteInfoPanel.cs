using System.Collections.Generic;
using System.Threading.Tasks;
using PSB.Code.CoreSystem.SaveSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using YIS.Code.Defines;

namespace CIW.Code.Roulette
{
    public class RouletteInfoPanel : MonoBehaviour
    {
        [Header("UI Info Settings")]
        [SerializeField] RouletteSystem rouletteSystem;
        [SerializeField] GameObject infoPrefab;
        [SerializeField] Transform contentPanel;
        [SerializeField] TextMeshProUGUI goldText;

        [Header("Upgrade Settings")]
        [SerializeField] float maxWeight = 80f;
        [SerializeField] float minWeight = 1f;
        [SerializeField] int upgradeCost = 1;

        List<RouletteInfoUI> _uiList = new List<RouletteInfoUI>();

        private async void Start()
        {
            await InitPanel();
        }

        private async Awaitable InitPanel()
        {
            await Awaitable.NextFrameAsync();

            InitList();
            UpdateGoldUI();
            RefreshAllUI();
        }

        private void InitList()
        {
            foreach (Transform child in contentPanel) 
                Destroy(child.gameObject);
            _uiList.Clear();

            var rewards = rouletteSystem.RouletteTable.rewards;
            float totalWeight = GetTotalWeight();

            for (int i = 0; i < rewards.Count; i++)
            {
                GameObject go = Instantiate(infoPrefab, contentPanel);
                var ui = go.GetComponent<RouletteInfoUI>();

                float chance = totalWeight > 0 ? (rewards[i].weight / totalWeight) : 0f;
                bool isMax = rewards[i].weight >= maxWeight;
                bool isMin = rewards[i].weight <= minWeight;
                ui.Initialize(i, rewards[i].itemData.visualData.icon, rewards[i].rewardName, chance, isMax, isMin, OnWeightChange);
                _uiList.Add(ui);
            }
        }

        public void OnWeightChange(int index, bool isUpgrade)
        {
            var rewards = rouletteSystem.RouletteTable.rewards;
            RouletteReward targetReward = rewards[index];

            if (isUpgrade)
            {
                if (targetReward.weight >= maxWeight) 
                {
                    Debug.Log("최대 확률입니다.");
                    return;
                }

                if (CurrencyContainer.Get(ItemType.Coin) < upgradeCost) return;
                
                targetReward.weight += 1f;
                CurrencyContainer.Spend(ItemType.Coin, upgradeCost);
            }
            else
            {
                if (targetReward.weight <= minWeight) 
                {
                    Debug.Log("최소 확률입니다.");
                    return;
                }

                targetReward.weight -= 1f;
                CurrencyContainer.Add(ItemType.Coin, upgradeCost);
            }

            rewards[index] = targetReward;
            RefreshAllUI();
            UpdateGoldUI();
        }

        public void OnUpgradeClick(int index)
        {
            if (CurrencyContainer.Get(ItemType.Coin) < upgradeCost) return;

            var rewards = rouletteSystem.RouletteTable.rewards;
            RouletteReward targetReward = rewards[index];
            targetReward.weight += 1f;
            rewards[index] = targetReward;

            CurrencyContainer.Spend(ItemType.Coin, upgradeCost);

            RefreshAllUI();
            UpdateGoldUI();
            Debug.Log($"{rewards[index].itemData.itemName} 등장 확률 증가");
        }

        public void RefreshAllUI()
        {
            var rewards = rouletteSystem.RouletteTable.rewards;
            float totalWeight = GetTotalWeight();

            for (int i = 0; i < rewards.Count; i++)
            {
                float chance = (totalWeight > 0) ? (rewards[i].weight / totalWeight) : 0f;

                bool isMax = rewards[i].weight >= maxWeight;
                bool isMin = rewards[i].weight <= minWeight;

                _uiList[i].UpdateUI(chance, isMax, isMin);
            }
        }

        public void UpdateGoldUI()
        {
            goldText.text = CurrencyContainer.Get(ItemType.Coin).ToString();
        }

        private float GetTotalWeight()
        {
            float total = 0;
            foreach (var r in rouletteSystem.RouletteTable.rewards) total += r.weight;
            return total;
        }
    }
}

