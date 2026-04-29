using PSW.Code.EventBus;
using UnityEngine;

namespace CIW.Code.System.Events
{
    public struct BattleEnd : IEvent
    {
        public bool IsVictory;

        public BattleEnd(bool isVictory)
        {
            IsVictory = isVictory;
        }
        
    }
}

