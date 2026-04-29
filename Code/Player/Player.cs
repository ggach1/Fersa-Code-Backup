using PSB.Code.BattleCode.Entities;
using PSB.Code.BattleCode.Players;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Work.CSH.Scripts.Interfaces;
using Work.CSH.Scripts.Managers;
using YIS.Code.Modules;

namespace CIW.Code.Player
{
    public class Player : Entity, ITurnable, IModule
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [field: FormerlySerializedAs("<turnManager>k__BackingField")] [field : SerializeField] public TurnManagerSO TurnManager { get; set; }

        PlayerTargetSelector _targetSelector;
        //PlayerAttack _attackComponent;

        public void Initialize(ModuleOwner owner)
        {
            _targetSelector = owner.GetModule<PlayerTargetSelector>();
            //_attackComponent = entity.GetCompo<PlayerAttack>();
            TurnManager.AddITurnableList(this);
        }

        protected override void Start()
        {
            base.Start();
            //PlayerManager.Instance.Register(this);
            //turnManager.RemoveITurnableList(this);
        }

        protected override void Die()
        {
            Debug.Log("Player has died. Game over");
        }

        public void OnStartTurn(bool isPlayerTurn)
        {
            if (!isPlayerTurn)
                return;

            _targetSelector.enabled = true;
            //_attackComponent.enabled = true;
        }

        public void OnEndTurn(bool isPlayerTurn)
        {
            if (isPlayerTurn)
                return;

            _targetSelector.enabled = true;
            //_attackComponent.enabled = false;
        }
        
    }
}

