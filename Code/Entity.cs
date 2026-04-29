using System;
using System.Collections.Generic;
using System.Linq;
using PSB_Lib.Dependencies;
using PSB_Lib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;
using YIS.Code.Modules;

namespace CIW.Code
{
    public abstract class Entity : ModuleOwner
    {
        public Transform Transform => transform;
        
        public bool IsDead { get; set; }
        public bool IsFreeze { get; set; }
        public Action<Entity> OnAttack;
        public UnityEvent OnHitEvent;
        public UnityEvent OnDeadEvent;
        
        protected Dictionary<Type, IModule> _components;

        protected override void Awake()
        {
            base.Awake();
            _components = new Dictionary<Type, IModule>();
            AddComponents();
        }

        
        protected virtual void Start()
        {
            
        }
        
        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IModule>().ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        
        public void DestroyEntity()
        {
            Die();
            Destroy(gameObject);
        }

        protected virtual void Die()
        {
        }
        
    }
}

