using System.Collections.Generic;

namespace Kitchen.Player
{
    public partial class Player
    {
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