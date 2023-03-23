using Nico.Network.Singleton;
using Nico.Network.Singleton.ECC;

namespace Kitchen.Player
{
    public abstract class PlayerController: IController<Player>
    {

        protected PlayerController(Player player)
        {
            this.Owner = player;
        }

        public Player Owner { get; set; }
        public abstract void Update();
    }
}