using System.Collections.Generic;
using UnityEngine;

namespace Kitchen.Player
{
    public partial class Player
    {
        private void InitializedMonoComponents()
        {
            animator = transform.Find("PlayerVisual").GetComponent<Animator>();
            topSpawnPoint = transform.Find("KitchenObjHoldPoint");
        }

        private void InitializedControllers()
        {
            _controllers = new List<PlayerController>();
            var moveController = new PlayerMoveController(this);
            moveController.OnMoving += () => { OnMoving?.Invoke(transform.position); };
            _controllers.Add(moveController);
            selectCounterController = new PlayerSelectCounterController(this);
            _controllers.Add(selectCounterController);
        }
    }
}