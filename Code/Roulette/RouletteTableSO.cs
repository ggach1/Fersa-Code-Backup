using System;
using System.Collections.Generic;
using UnityEngine;
using YIS.Code.Items;

namespace CIW.Code.Roulette
{
    [Serializable]
    public struct RouletteReward
    {
        public string rewardName;
        public ItemDataSO itemData;
        public int amount;
        public float weight; // 다른 놈에 비해서 얼마나 잘 나오게 할지
    }

    [CreateAssetMenu(fileName = "Roulette Table", menuName = "SO/Roulette/Table")]
    public class RouletteTableSO : ScriptableObject
    {
        public List<RouletteReward> rewards = new List<RouletteReward>();
    }
}

