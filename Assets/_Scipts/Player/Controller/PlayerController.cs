using Kitchen.Interface;

namespace Kitchen.Player
{
    public abstract class PlayerController: ICotroller
    {
        protected readonly Player player;

        protected PlayerController(Player player)
        {
            this.player = player;
        }
        public abstract void Update();
    }
}