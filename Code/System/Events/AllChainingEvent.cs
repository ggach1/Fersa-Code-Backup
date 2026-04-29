using System;
using PSW.Code.EventBus;

namespace CIW.Code.System.Events
{
    public struct AllChainingEvent : IEvent
    {
        public Action callback;
        
        public AllChainingEvent(Action call)
        {
            callback = call;
        }
    }
}
