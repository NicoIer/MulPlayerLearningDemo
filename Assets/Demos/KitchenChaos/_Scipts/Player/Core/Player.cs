using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Kitchen;
using Nico;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : NetworkBehaviour
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
        // public static Player Instance { get; private set; }

        protected void Awake()
        {
            // Instance = this;
            _Init();
        }

        // [CanBeNull]
        // public static Player GetInstanceUnSafe(bool throwError = false)
        // {
        //     if (Instance == null)
        //     {
        //         if (throwError)
        //         {
        //             throw new Exception("Application is quitting !!!!");
        //         }
        //
        //         return null;
        //     }
        //
        //     return Instance;
        // }

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
            //如果不是自己的玩家，就不更新 
            //IsOwner 是NetworkBehaviour的属性，表示是否是自己的玩家
            //也就是说 本地运行的玩家是自己的玩家，其他玩家是别人的玩家
            if (!IsOwner)
                return;
            foreach (var controller in _controllers)
            {
                controller.Update();
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        internal void _MoveServerRpc(Vector3 inputDir,Vector3 moveDir)
        {
            TransformSetter.Move(transform, moveDir, data.speed);
            TransformSetter.SetForward(transform, inputDir, data.rotateSpeed);
        }
    }
}