using Nico;

namespace Nico.Player
{
    public abstract class PlayerController: IController
    {
        protected readonly Player player;

        protected PlayerController(Player player)
        {
            this.player = player;
        }
        public abstract void Update();
    }
}