using System;
using UnityEngine;
using YIS.Code.Modules;

namespace CIW.Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IModule
    {
        public Action OnAnimationEndTrigger;
        public Action OnAttackTrigger;

        private Entity _entity;

        public void Initialize(ModuleOwner owner)
        {
            _entity = owner as Entity;
        }

        private void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }
        private void Attack()
        {
            OnAttackTrigger?.Invoke();
        }
    }
}

