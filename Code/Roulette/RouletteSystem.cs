using CIW.Roulette;
using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;
using UnityEngine;

namespace CIW.Code.Roulette
{
    public class RouletteSystem : MonoBehaviour
    {
        [field : SerializeField] public RouletteTableSO RouletteTable;
        [SerializeField] InventoryCode inventory;

        public RouletteReward Spin(out int index)
        {
            float tatalWeight = 0;
            foreach (var reward in RouletteTable.rewards)
                tatalWeight += reward.weight;

            float randomVal = Random.Range(0, tatalWeight);
            float curWeight = 0;
            
            for (int i = 0; i < RouletteTable.rewards.Count; i++)
            {
                curWeight += RouletteTable.rewards[i].weight;
                if (randomVal <= curWeight)
                {
                    index = i;
                    return RouletteTable.rewards[i];
                }
            }

            index = 0;
            return RouletteTable.rewards[0]; // 이거는 예외 처리 용도
        }

        public void GiveReward(RouletteReward reward)
        {
            if (reward.itemData.itemType == YIS.Code.Defines.ItemType.Item)
            {
                // 아이템인 경우
                inventory.TryAddItem(reward.itemData);
                Debug.Log($"{reward.rewardName} 아이템을 {reward.amount}개 획득하였음 : {reward.weight}");
            }
            else
            {
                // 재화인 경우
                CurrencyContainer.Add(reward.itemData.itemType, reward.amount);
                Debug.Log($"{reward.rewardName} 재화를 {reward.amount}개 획득하였음 : {reward.weight}");
            }

            Bus<RouletteEndEvent>.Raise(new RouletteEndEvent());
        }
    }
}