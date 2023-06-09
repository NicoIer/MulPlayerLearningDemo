using System;
using System.Collections.Generic;
using Kitchen.Visual;
using Nico.Network;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : NetLocalSingleton<Player>
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
        public static event Action<Vector3> OnAnyPickUpSomeThing;

        #endregion

        #region MonoComponents

        private KitchenObj _kitchenObj;
        [field: SerializeField] public Transform topSpawnPoint { get; private set; }

        #endregion

        [SerializeField] private PlayerVisual playerVisual;
        internal PlayerInput input;

        [SerializeField] internal PlayerData data;

        [SerializeField] internal List<Vector3> spawnPoints;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _Init();
            //TODO 这里会有BUG 
            
            
            var config = GameManager.Instance.GetPlayerConfig(OwnerClientId);
            var color = GameManager.Instance.GetColor(config.colorId);
            playerVisual.SetColor(color);
            transform.position = spawnPoints[config.spawnPointId];
            
            
            OnAnyPlayerSpawned?.Invoke();
            input.Enable();
            input.OnInteractPerform += OnPerformInteract;
            input.OnInteractAlternatePerform += OnPerformInteractAlternate;

            
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void OnClientDisconnect(ulong clientId)
        {
            //如果掉线的玩家 是自己的玩家，就销毁自己所持有的物体
            if (OwnerClientId == clientId)
            {
                KitchenObjOperator.DestroyKitchenObj(_kitchenObj);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            input.Disable();
            input.OnInteractPerform -= OnPerformInteract;
            input.OnInteractAlternatePerform -= OnPerformInteractAlternate;


            KitchenObjOperator.DestroyKitchenObj(GetKitchenObj());
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
                topSpawnPoint = transform.Find("KitchenObjHoldPoint");

                _controllers = new List<PlayerController>();
                MoveController = new PlayerMoveController(this);
                _controllers.Add(MoveController);
                _selectCounterController = new PlayerSelectCounterController(this);
                _controllers.Add(_selectCounterController);

                _initialized = true;
            }
        }

        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
        }
    }
}