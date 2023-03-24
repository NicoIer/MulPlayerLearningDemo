namespace Kitchen
{
    public class ClearCounter : BaseCounter
    {
        public override void Interact(Player.Player player)
        {
            //尝试进行盘子的操作
            if (CounterOperator.TryPlateOperator(player, this)) return;
            
            //玩家持有物体，当前柜子没有物体
            if (player.HasKitchenObj() && !HasKitchenObj())
            {
                
                KitchenObjOperator.PutKitchenObj(player, this);
                return;
            }

            //玩家没有持有物体，当前柜子有物体
            if (!player.HasKitchenObj() && HasKitchenObj())
            {
                KitchenObjOperator.PutKitchenObj(this, player);
                return;
            }
        }
    }
}