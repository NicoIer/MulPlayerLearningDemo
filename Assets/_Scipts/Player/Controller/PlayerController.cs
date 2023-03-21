using Nico;
using Nico.ECC;

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