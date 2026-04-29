using PSW.Code.EventBus;
using System;
using UnityEngine;
using YIS.Code.Skills;

namespace CIW.Code.System.Events
{
    public struct UnChainingEvent : IEvent
    {
        public SkillDataSO Skill;
        public Action callback;

        public UnChainingEvent(SkillDataSO skill, Action call)
        {
            Skill = skill;
            callback = call;
        }
    }
}
