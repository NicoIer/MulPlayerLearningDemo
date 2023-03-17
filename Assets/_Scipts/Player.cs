using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kitchen
{
    public partial class Player : MonoBehaviour
    {
        private PlayerInput _playerInput;
        [SerializeField] private PlayerData playerData;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }


    }
}