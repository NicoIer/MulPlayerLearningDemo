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
        public event Action OnInteractAlternatePerform;
        public event Action OnPause;

        public PlayerInput()
        {
            _standerInput = new StanderInput();
        }

        public void Enable()
        {
            _standerInput.Enable();
            _standerInput.Player.Interact.performed += _InteractPerformed;
            _standerInput.Player.Interact.started += _InteractStarted;
            _standerInput.Player.Interact.canceled += _InteractCanceled;
            _standerInput.Player.InteractAlternate.performed += _InteractAlternatePerformed;
            _standerInput.Player.Pause.performed += _OnPause;
        }

        public void Disable()
        {
            _standerInput.Disable();
            _standerInput.Player.Interact.performed -= _InteractPerformed;
            _standerInput.Player.Interact.started -= _InteractStarted;
            _standerInput.Player.Interact.canceled -= _InteractCanceled;
            _standerInput.Player.InteractAlternate.performed -= _InteractAlternatePerformed;
        }

        private void _OnPause(InputAction.CallbackContext obj)
        {
            OnPause?.Invoke();
        }

        private void _InteractAlternatePerformed(InputAction.CallbackContext obj)
        {
            OnInteractAlternatePerform?.Invoke();
        }


        private void _InteractCanceled(InputAction.CallbackContext obj)
        {
            // Debug.Log("Interact_canceled");
        }

        private void _InteractStarted(InputAction.CallbackContext obj)
        {
            // Debug.Log("Interact_started");
        }

        private void _InteractPerformed(InputAction.CallbackContext obj)
        {
            OnInteractPerform?.Invoke();
        }
    }
}