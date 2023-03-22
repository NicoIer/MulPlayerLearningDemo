using System;
using System.Collections.Generic;
using Kitchen;
using Nico;
using Unity.VisualScripting;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : MonoSingleton<Player>
    {
        #region Controller

        private BaseCounter SelectedCounter => selectCounterController.SelectedCounter;
        private PlayerSelectCounterController _selectCounterController;

        public PlayerSelectCounterController selectCounterController
        {
            get
            {
                if (_selectCounterController == null && !_initialized)
                {
                    _Init();
                }

                return _selectCounterController;
            }
        }

        public Action<Vector3> onMoving;

        #endregion

        #region MonoComponents

        internal Animator animator;

        #endregion

        #region KitchenObj

        private KitchenObj _kitchenObj;
        [field: SerializeField] public Transform topSpawnPoint { get; private set; }

        #endregion

        internal PlayerInput input;
        private List<PlayerController> _controllers;
        [SerializeField] internal PlayerData data;


        protected override void Awake()
        {
            base.Awake();
            _Init();
        }

        private void OnEnable()
        {
            input.Enable();
            input.OnInteractPerform += OnPerformInteract;
            input.OnInteractAlternatePerform += OnPerformInteractAlternate;
        }


        private void OnDisable()
        {
            input.Disable();
            input.OnInteractPerform -= OnPerformInteract;
            input.OnInteractAlternatePerform -= OnPerformInteractAlternate;
        }


        private void Update()
        {
            foreach (var controller in _controllers)
            {
                controller.Update();
            }
        }
    }
}