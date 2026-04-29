using PSW.Code.EventBus;
using UnityEngine;

namespace CIW.Code.System.Events
{
    public struct DamageText : IEvent
    {
        public float Damage;

        public DamageText(float dmg)
        {
            Damage = dmg;
        }
    }
}
