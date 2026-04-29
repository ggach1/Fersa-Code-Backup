using System;
using CIW.Code.Entities;
using Code.Scripts.Entities;
using PSB.Code.BattleCode.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;
using Work.CSH.Scripts.PlayerComponents;
using YIS.Code.Modules;

namespace CIW.Code.Player.Field
{
    public class PlayerFieldMovement : MonoBehaviour, IModule, IMover
    {
        [SerializeField] StatSO _moveSpeedStat;

        private Entity _entity;
        private Rigidbody2D _rigid;
        EntityAnimator _animator;
        EntityStat _statModule;
        StatSO _moveStat;

        public event Action<Vector2> OnVelocityChange; // 나중에 스탯 추가하면 변환한 값 받는용,
        
        public Rigidbody2D Rigid => _rigid;

        float _moveSpeed;
        float _moveSpeedMultiplier;
        private Vector2 _movement;

        public void Initialize(ModuleOwner owner)
        {
            _entity = owner as Entity;
            _rigid = owner.GetComponent<Rigidbody2D>();
            _animator = owner.GetModule<EntityAnimator>();
            _statModule = owner.GetModule<EntityStat>();

            _moveSpeedMultiplier = 1f;
            _moveStat = _statModule.GetStat(_moveSpeedStat);
        }
        
        public void SetMoveSpeedMultiplier(float value) => _moveSpeedMultiplier = value;
        
        public void AddForceToAgent(Vector2 force) => _rigid.AddForce(force, ForceMode2D.Impulse);

        public void StopImmediately(bool xAxis, bool yAxis)
        {
            if (xAxis)
            {
                _rigid.linearVelocityX = 0;
                _movement.x = 0;
            }

            if (yAxis)
            {
                _rigid.linearVelocityY = 0;
                _movement.y = 0;
            }
        }

        public void SetMovement(Vector2 xy)
        {
            _movement = xy;
        }

        private void Update()
        {
            Move();
        }

        public void Move()
        {
            int moveSpeedHash = Animator.StringToHash("MoveSpeed");
            _moveSpeed = _moveStat.Value;
            _rigid.linearVelocity = _movement * _moveSpeed * _moveSpeedMultiplier;
            _animator.SetParam(moveSpeedHash, _moveSpeed / 3.5f);
        }

        
       
    }
}

