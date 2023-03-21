using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public enum InputEnum
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause
    }

    public class PlayerInput
    {
        public Vector2 move => _standerInput.Player.move2D.ReadValue<Vector2>();
        private readonly StanderInput _standerInput;
        public StanderInput.PlayerActions Player => _standerInput.Player;
        public event Action OnInteractPerform;
        public event Action OnInteractAlternatePerform;
        public event Action OnPause;
        private const string _bingSaveKey = "player_bindings";

        public string GetBingingName(InputEnum actionName)
        {
            switch (actionName)
            {
                case InputEnum.MoveUp:
                    return _standerInput.Player.move2D.bindings[1].ToDisplayString();
                case InputEnum.MoveDown:
                    return _standerInput.Player.move2D.bindings[2].ToDisplayString();
                case InputEnum.MoveLeft:
                    return _standerInput.Player.move2D.bindings[3].ToDisplayString();
                case InputEnum.MoveRight:
                    return _standerInput.Player.move2D.bindings[4].ToDisplayString();
                case InputEnum.Interact:
                    return _standerInput.Player.Interact.bindings[0].ToDisplayString();
                case InputEnum.InteractAlternate:
                    return _standerInput.Player.InteractAlternate.bindings[0].ToDisplayString();
                case InputEnum.Pause:
                    return _standerInput.Player.Pause.bindings[0].ToDisplayString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionName), actionName, null);
            }
        }

        public PlayerInput()
        {
            _standerInput = new StanderInput();
            if (PlayerPrefs.HasKey(_bingSaveKey))
            {
                _standerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(_bingSaveKey));
                
            }
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

        public void SaveAsJson()
        {
            var str = _standerInput.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(_bingSaveKey, str);
        }
    }
}