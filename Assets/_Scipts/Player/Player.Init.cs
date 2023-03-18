using System.Collections.Generic;
using UnityEngine;

namespace Nico.Player
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
            var playerMoveController = new PlayerMoveController(this);
            _controllers.Add(playerMoveController);
            selectCounterController = new PlayerSelectCounterController(this);
            _controllers.Add(selectCounterController);
        }
    }
}