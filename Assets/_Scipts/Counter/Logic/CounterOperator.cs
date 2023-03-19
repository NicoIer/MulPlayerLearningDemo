namespace Kitchen
{
    public static class CounterOperator
    {
        public static bool TryPlateOperator(Player.Player player, BaseCounter counter)
        {
            //都有物体 判断是否可以叠加
            if (player.HasKitchenObj() && counter.HasKitchenObj())
            {
                var kitchenObj = counter.GetKitchenObj();
                var playerHoldObj = player.GetKitchenObj();
                //玩家手里是盘子
                if (playerHoldObj is Plate plate1) //如果是盘子
                {
                    //则将当前游戏物体放入盘子
                    KitchenObjOperator.PutToPlate(kitchenObj, plate1);
                    return true;
                }

                //柜台上的物体是盘子
                if (kitchenObj is Plate plate2)
                {
                    //将玩家手里的物体放入盘子
                    KitchenObjOperator.PutToPlate(playerHoldObj, plate2);
                    return true;
                }

                return true;
            }

            return false;
        }
    }
}