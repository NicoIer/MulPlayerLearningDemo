using System;
using System.Collections.Generic;
using Nico;
using UnityEngine;

namespace Nico.Player
{
    public partial class Player : MonoBehaviour
    {
        public static Player Singleton { get; private set; }

        #region Counter

        private BaseCounter SelectedCounter => selectCounterController.SelectedCounter;
        public PlayerSelectCounterController selectCounterController { get; private set; }

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


        private void Awake()
        {
            input = new PlayerInput();
            if (Singleton != null)
            {
                Debug.LogError("More than one Player in scene!");
            }

            Singleton = this;
            InitializedMonoComponents();
            InitializedControllers();
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

        private void OnPerformInteract()
        {
            //这是多播委托的调用 其实没必要通知每个Counter
            if (SelectedCounter == null)
                return;
            SelectedCounter.Interact(this);
        }

        private void OnPerformInteractAlternate()
        {
            if (SelectedCounter == null) return;
            if (SelectedCounter.TryGetComponent(out IInteractAlternate interactAlternate))
            {
                interactAlternate.InteractAlternate(this);
            }
        }


        private void Update()
        {
            foreach (var controller in _controllers)
            {
                controller.Update();
            }
        }

        private void OnDrawGizmos()
        {
            var transform1 = transform;
            var position = transform1.position;
            Debug.DrawRay(position, transform1.forward * data.interactDistance, Color.red);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, data.playerRadius);
            Gizmos.DrawWireSphere(position + Vector3.up * data.playerHeight, data.playerRadius);
        }
    }
}