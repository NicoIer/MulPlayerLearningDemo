using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class DeliveryCounter : BaseCounter
    {
        HashSet<KitchenObjEnum> _order = new HashSet<KitchenObjEnum>();
        public override void Interact(Player.Player player)
        {
            //检查玩家拿的物体是否符合订单需求
            if (!player.HasKitchenObj()) return;
            //首先玩家必须拿的是盘子
            var playerKitchenObj = player.GetKitchenObj();
            if (playerKitchenObj is not Plate plate) return;
            
            //ToDO 其次检查盘子里的东西是否合格 
            var ingredients = plate.GetIngredients();
            if (DeliveryManager.Instance.TryDeliverOrder(transform.position,ingredients))
            {
                //如果合格则销毁盘子 并且销毁玩家手上的物体
                playerKitchenObj.DestroySelf();
                //然后完成当前订单
                
            }
        }
    }
}