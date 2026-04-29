using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CIW.Code.Player.Field
{
    [CreateAssetMenu(fileName = "FieldInput", menuName = "SO/Player/Field/Input")]
    public class PlayerFieldInputSO : ScriptableObject, FieldControls.IPlayerActions
    {
        public Action OnInteractPressed;
        public Action OnAttackPressed;
        public Action<int> OnChangeSelectedIndexPressed;
        public Vector2 Direction { get; private set; } 
        

        FieldControls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new FieldControls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void EnableInput()
        {
            _controls.Player.Enable();
        }
        public void DisableInput()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Direction = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed) 
                OnInteractPressed?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed) 
                OnAttackPressed?.Invoke();
        }

        public void OnChangeSelectedIndex(InputAction.CallbackContext context)
        {
            if(context.performed) 
            {
                int modifier = (int)context.ReadValue<Vector2>().y;
                OnChangeSelectedIndexPressed?.Invoke(modifier);
            }
        }
    }
}

