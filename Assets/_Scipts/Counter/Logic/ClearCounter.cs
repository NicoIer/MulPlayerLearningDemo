using Kitchen;
using UnityEngine;

namespace Kitchen
{
    public class ClearCounter : BaseCounter
    {
        public override void Interact(Player.Player player)
        {
            //都有物体 判断是否可以叠加
            if (player.HasKitchenObj() && HasKitchenObj())
            {
                var playerHoldObj = player.GetKitchenObj();
                //玩家手里是盘子
                if (playerHoldObj is Plate) //如果是盘子
                {
                    //则将当前游戏物体放入盘子
                    var plate = playerHoldObj as Plate;
                    KitchenObjOperator.PutToPlate(kitchenObj, plate);
                    return;
                }
                //柜台上的物体是盘子
                if (kitchenObj is Plate)
                {
                    //将玩家手里的物体放入盘子
                    var plate = kitchenObj as Plate;
                    KitchenObjOperator.PutToPlate(playerHoldObj, plate);
                    return;
                }

                return;
            }

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