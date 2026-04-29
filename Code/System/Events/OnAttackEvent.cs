using CIW.Code.Player.SkillCompo;
using PSW.Code.EventBus;
using System.Collections.Generic;
using Work.YIS.Code.Skills;

namespace CIW.Code.System.Events
{
    public struct OnAttackEvent : IEvent
    {
        public SkillEnum[] SkillIds;
        public bool[] ChainFlags;

        public OnAttackEvent(SkillEnum[] skillIds, bool[] chainFlags)
        {
            SkillIds = skillIds;
            ChainFlags = chainFlags;
        }
        
    }
}