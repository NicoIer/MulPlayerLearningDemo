using System.Collections.Generic;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player
    {
        private bool _initialized;

        protected void _Init()
        {
            if (!_initialized)
            {
                input = new PlayerInput();
                InitializedMonoComponents();
                InitializedControllers();
                _initialized = true;
            }
        }

        private void InitializedMonoComponents()
        {
            animator = transform.Find("PlayerVisual").GetComponent<Animator>();
            topSpawnPoint = transform.Find("KitchenObjHoldPoint");
        }

        private void InitializedControllers()
        {
            _controllers = new List<PlayerController>();
            var moveController = new PlayerMoveController(this);
            moveController.onMoving += () => { onMoving?.Invoke(transform.position); };
            _controllers.Add(moveController);
            _selectCounterController = new PlayerSelectCounterController(this);
            _controllers.Add(_selectCounterController);
        }
    }
}