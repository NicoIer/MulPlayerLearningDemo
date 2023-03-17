using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public class PlayerInput
    {
        public Vector2 move => _standerInput.Player.move2D.ReadValue<Vector2>();
        private readonly StanderInput _standerInput;
        public event Action OnInteractPerform;

        public PlayerInput()
        {
            _standerInput = new StanderInput();
        }

        public void Enable()
        {
            _standerInput.Enable();
            _standerInput.Player.Interact.performed += _InteractPerformed;
            _standerInput.Player.Interact.started += InteractStarted;
            _standerInput.Player.Interact.canceled += InteractCanceled;
        }
        public void Disable()
        {
            _standerInput.Disable();
            _standerInput.Player.Interact.performed -= _InteractPerformed;
            _standerInput.Player.Interact.started -= InteractStarted;
            _standerInput.Player.Interact.canceled -= InteractCanceled;
        }
        

        private void InteractCanceled(InputAction.CallbackContext obj)
        {
            // Debug.Log("Interact_canceled");
        }

        private void InteractStarted(InputAction.CallbackContext obj)
        {
            // Debug.Log("Interact_started");
        }

        private void _InteractPerformed(InputAction.CallbackContext obj)
        {
            OnInteractPerform?.Invoke();
        }


    }
}