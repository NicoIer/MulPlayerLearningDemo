using System;
using System.Collections.Generic;
using Kitchen;
using Unity.VisualScripting;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : MonoBehaviour
    {
        public static Player Singleton { get; private set; }

        #region Controller

        private BaseCounter SelectedCounter => selectCounterController.SelectedCounter;
        public PlayerSelectCounterController selectCounterController { get; private set; }

        public Action<Vector3> OnMoving;
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