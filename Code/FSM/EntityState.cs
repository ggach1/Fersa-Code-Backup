using CIW.Code.Entities;
using System;
using UnityEngine;

namespace CIW.Code.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected int _animationHash;
        protected EntityAnimator _entityAnimator;
        protected EntityAnimatorTrigger _animatorTrigger;
        protected bool _triggerCall;

        protected EntityState(Entity entity, int animationHash)
        {
            _entity = entity;
            _animationHash = animationHash;
            _entityAnimator = entity.GetModule<EntityAnimator>();
            _animatorTrigger = entity.GetModule<EntityAnimatorTrigger>();
        }

        public virtual void Enter()
        {
            //_entityAnimator.SetParam(_animationHash, true);
            _entityAnimator.PlayClip(_animationHash);
            _triggerCall = false;
            _animatorTrigger.OnAnimationEndTrigger += OnAnimationEndTrigger;
        }

        public virtual void Update() { }

        public virtual void Exit()
        {
            _animatorTrigger.OnAnimationEndTrigger -= OnAnimationEndTrigger;
            _entityAnimator.SetParam(_animationHash, false);
        }

        private void OnAnimationEndTrigger() => _triggerCall = true;

        
    }
}
