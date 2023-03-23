using System;
using System.Collections.Generic;
using Nico.DesignPattern.Singleton.Network;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : NetworkLocalSingleton<Player>
    {
        #region Controller

        private BaseCounter SelectedCounter => SelectCounterController.SelectedCounter;
        private PlayerSelectCounterController _selectCounterController;
        public PlayerMoveController MoveController { get; private set; }
        private List<PlayerController> _controllers;

        public PlayerSelectCounterController SelectCounterController
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

        #endregion

        #region Event

        public static event Action OnAnyPlayerSpawned;

        #endregion

        #region MonoComponents

        private KitchenObj _kitchenObj;
        [field: SerializeField] public Transform topSpawnPoint { get; private set; }

        #endregion


        internal PlayerInput input;

        [SerializeField] internal PlayerData data;


        protected void Awake()
        {
            // Instance = this;
            _Init();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            OnAnyPlayerSpawned?.Invoke();
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



        private bool _initialized;

        protected void _Init()
        {
            if (!_initialized)
            {
                input = PlayerInput.Instance; //TODO Controller需要获取Input的引用，所以这里需要先初始化Input
                InitializedMonoComponents();
                InitializedControllers();
                _initialized = true;
            }
        }

        private void InitializedMonoComponents()
        {
            topSpawnPoint = transform.Find("KitchenObjHoldPoint");
        }

        private void InitializedControllers()
        {
            _controllers = new List<PlayerController>();
            MoveController = new PlayerMoveController(this);
            _controllers.Add(MoveController);
            _selectCounterController = new PlayerSelectCounterController(this);
            _controllers.Add(_selectCounterController);
        }
    }
}