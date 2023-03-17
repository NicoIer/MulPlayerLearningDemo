using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : MonoBehaviour
    {
        public static Player Singleton { get; private set; }
        private ClearCounter SelectedCounter => selectCounterController.SelectedCounter;
        public PlayerSelectCounterController selectCounterController { get; private set; }


        internal PlayerInput input;
        private List<PlayerController> _controllers;
        [SerializeField] internal PlayerData data;
        internal Animator animator;

        private void Awake()
        {
            input = new PlayerInput();
            animator = transform.Find("PlayerVisual").GetComponent<Animator>();
            if (Singleton != null)
            {
                Debug.LogError("More than one Player in scene!");
            }

            Singleton = this;
            InitializedControllers();
        }




        private void OnEnable()
        {
            input.Enable();
            input.OnInteractPerform += OnPerformInteract;
        }


        private void OnPerformInteract()
        {
            if (SelectedCounter != null)
                SelectedCounter.Interact();
        }


        private void OnDisable()
        {
            input.Disable();
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