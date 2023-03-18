using Nico;
using UnityEngine;

namespace Kitchen
{
    public class ClearCounter : BaseCounter
    {
        public override void Interact(Player.Player player)
        {
            //玩家持有物体，当前柜子没有物体
            if (player.HasKitchenObj() && !HasKitchenObj())
            {
                var playerKitchenObj = player.GetKitchenObj();
                playerKitchenObj.SetHolder(this); //设置物体的柜子
                kitchenObj = playerKitchenObj; //把物体设置到当前的柜子中
                player.ClearKitchenObj(); //清空玩家的物体
                return;
            }

            //玩家没有持有物体，当前柜子有物体
            if (!player.HasKitchenObj() && HasKitchenObj())
            {
                var targetKitchenObj = GetKitchenObj();
                targetKitchenObj.SetHolder(player); //设置物体的柜子
                player.SetKitchenObj(targetKitchenObj); //把物体设置到玩家的物体中
                ClearKitchenObj(); //清空当前柜子的物体
                return;
            }
        }
        
    }
}