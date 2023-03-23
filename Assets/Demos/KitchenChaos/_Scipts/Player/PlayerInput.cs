using System;
using System.Diagnostics;
using Nico.Network.Singleton;
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
        Pause,
        GamePadInteract,
        GamePadInteractAlternate,
        GamePadPause,
    }

    public class PlayerInput: MonoSingleton<PlayerInput>
    {
        public Vector2 move => _standerInput.Player.move2D.ReadValue<Vector2>().normalized;
        private StanderInput _standerInput;
        public StanderInput.PlayerActions Player => _standerInput.Player;
        public event Action OnInteractPerform;
        public event Action OnInteractAlternatePerform;
        public event Action OnPause;
        private const string _bingSaveKey = "player_bindings";
        public event Action OnRebinding;

        protected override void Awake()
        {
            base.Awake();
            _standerInput = new StanderInput();
            if (PlayerPrefs.HasKey(_bingSaveKey))
            {
                _standerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(_bingSaveKey));
            }
        }

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
                case InputEnum.GamePadInteract:
                    return _standerInput.Player.Interact.bindings[1].ToDisplayString();
                case InputEnum.GamePadInteractAlternate:
                    return _standerInput.Player.InteractAlternate.bindings[1].ToDisplayString();
                case InputEnum.GamePadPause:
                    return _standerInput.Player.Pause.bindings[1].ToDisplayString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionName), actionName, null);
            }
        }

        // public PlayerInput()
        // {
        //
        // }

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

        public void Rebinding(InputEnum inputEnum, Action callbackAction)
        {
            _standerInput.Player.Disable();
            InputAction action;
            int idx;
            switch (inputEnum)
            {
                case InputEnum.MoveUp:
                    action = _standerInput.Player.move2D;
                    idx = 1;
                    break;
                case InputEnum.MoveDown:
                    action = _standerInput.Player.move2D;
                    idx = 2;
                    break;
                case InputEnum.MoveLeft:
                    action = _standerInput.Player.move2D;
                    idx = 3;
                    break;
                case InputEnum.MoveRight:
                    action = _standerInput.Player.move2D;
                    idx = 4;
                    break;
                case InputEnum.Interact:
                    action = _standerInput.Player.Interact;
                    idx = 0;
                    break;
                case InputEnum.InteractAlternate:
                    action = _standerInput.Player.InteractAlternate;
                    idx = 0;
                    break;
                case InputEnum.Pause:
                    action = _standerInput.Player.Pause;
                    idx = 0;
                    break;
                case InputEnum.GamePadInteract:
                    action = _standerInput.Player.Interact;
                    idx = 1;
                    break;
                case InputEnum.GamePadInteractAlternate:
                    action = _standerInput.Player.InteractAlternate;
                    idx = 1;
                    break;
                case InputEnum.GamePadPause:
                    action = _standerInput.Player.Pause;
                    idx = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputEnum), inputEnum, null);
            }

            action.PerformInteractiveRebinding(idx).OnComplete((callback) =>
            {
                callback.Dispose();
                _standerInput.Enable();
                OnRebinding?.Invoke();
                callbackAction?.Invoke();
            }).Start();
            SaveAsJson();
        }
    }
}