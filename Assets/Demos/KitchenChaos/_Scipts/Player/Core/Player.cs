using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player : NetworkBehaviour
    {
        #region Controller

        private BaseCounter SelectedCounter => selectCounterController.SelectedCounter;
        private PlayerSelectCounterController _selectCounterController;
        public  PlayerMoveController moveController { get; private set; }
        private List<PlayerController> _controllers;
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

        

        #endregion

        #region Event


        #endregion

        #region MonoComponents
        private KitchenObj _kitchenObj;
        [field: SerializeField] public Transform topSpawnPoint { get; private set; }



        #endregion
        

        internal PlayerInput input;

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
        
        // [ServerRpc(RequireOwnership = false)]
        // internal void _MoveServerRpc(Vector3 inputDir,Vector3 moveDir)
        // {
        //     TransformSetter.Move(transform, moveDir, data.speed);
        //     TransformSetter.SetForward(transform, inputDir, data.rotateSpeed);
        // }
        
        private bool _initialized;

        protected void _Init()
        {
            if (!_initialized)
            {
                input = PlayerInput.Instance;//TODO Controller需要获取Input的引用，所以这里需要先初始化Input
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
            moveController = new PlayerMoveController(this);
            _controllers.Add(moveController);
            _selectCounterController = new PlayerSelectCounterController(this);
            _controllers.Add(_selectCounterController);
        }
    }
}