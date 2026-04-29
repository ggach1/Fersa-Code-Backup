using System;
using System.Collections.Generic;
using UnityEngine;

namespace CIW.Code.FSM
{
    public class EntityStateMachine
    {
        public EntityState CurrentState { get; private set; }

        Dictionary<int, EntityState> _states;

        public EntityStateMachine(Entity entity, StateDataSO[] stateList)
        {
            _states = new Dictionary<int, EntityState>();
            foreach (StateDataSO state in stateList)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Finding type is null : {state.className}");
                EntityState entityState = Activator.CreateInstance(type, entity, state.animationHash) as EntityState;
                _states.Add(state.stateIndex, entityState);
            }
        }

        public void ChangeState(int newStateIndex, bool forced = false)
        {
            EntityState newState = _states.GetValueOrDefault(newStateIndex);
            Debug.Assert(newState != null, $"State null {newStateIndex}");

            if (!forced && CurrentState == newState)
                return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void UpdateStateMachine()
        {
            CurrentState?.Update();
        }
    }
}

