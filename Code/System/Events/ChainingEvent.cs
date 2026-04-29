using PSW.Code.EventBus;
using System;
using UnityEngine;
using YIS.Code.Skills;

namespace CIW.Code.System.Events
{
    public struct ChainingEvent : IEvent
    {
        public SkillDataSO ChainingSkill;
        public int myIndex;
        public int targetIndex;
        public Action callback;

        public ChainingEvent(SkillDataSO chainingSkill, int my, int target, Action call)
        {
            ChainingSkill = chainingSkill;
            myIndex = my;
            targetIndex = target;
            callback = call;
        }
    }
}
