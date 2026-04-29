using System;
using CIW.Code;
using PSB.Code.BattleCode.Entities;
using Unity.VisualScripting;
using UnityEngine;
using YIS.Code.Modules;

namespace CIW.Code.Entities
{
    public class EntityAnimator : MonoBehaviour, IModule, IAnimator
    {
        [SerializeField] Animator animator;
        private SpriteRenderer _spriteRenderer;
        Entity _entity;
        
        public Animator Animator => animator;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        //private void Update()
        //{
        //    _spriteRenderer.sortingOrder = -(int)(_entity.Transform.position.y * 100);
        //}

        public bool ApplyRootMotion
        {
            get => animator.applyRootMotion;
            set => animator.applyRootMotion = value;
        }

        public void Initialize(ModuleOwner owner)
        {
            _entity = owner as Entity;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            int idleHash = Animator.StringToHash("IDLE");

            SetParam(idleHash, true);
        }

        public void PlayClip(int clipHash, int layer = 0, float normalPosition = float.NegativeInfinity)
            => animator.Play(clipHash, layer, normalPosition);

        /// <summary>
        /// 필드 플레이어 전용
        /// </summary>
        public void PlayClip(int clipHash, Vector2 dir, int layer = 0, float normalPosition = float.NegativeInfinity)
            => animator.Play(clipHash, layer, normalPosition);
        public void SetParam(int hash, float value, float dampTime)
            => animator.SetFloat(hash, value, dampTime, Time.deltaTime);

        public void SetParam(int hash, float value) => animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => animator.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => animator.SetBool(hash, value);
        public void SetParam(int hash) => animator.SetTrigger(hash);

        public void OffAnimator()
        {
            animator.enabled = false;
        }

        public void StopAnimation() => animator.speed = 0;
        public void PlayAnimation() => animator.speed = 1;
        public void FlipX(bool v)
        {
            _spriteRenderer.flipX = v;

        }
    }
}


