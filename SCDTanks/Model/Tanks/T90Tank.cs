using System.Collections.Generic;
using System.Drawing;

namespace SCDTanks.Model
{
    /// <summary>
    /// 肉盾
    /// </summary>
    class T90Tank :TankActionBase
    {
        protected override TanksAction AbsGetAction(GameInfo controller, TankInfo info)
        {
            if(info.Destination==null)
            {
                info.Destination = new Point(int.Parse(controller.SourceInfo.MapInfo.ColLen) / 2, int.Parse(controller.SourceInfo.MapInfo.RowLen) / 2);
            }
            if (base.Controller.GodCount == 0 && info.ShengYuShengMing == 1)
            {
                return base.Retreat();
            }
            if (controller.BossInfo.Location!=null)
            {
                if(controller.BossInfo.ShengYuShengMing<5)
                {
                    return base.Boss();
                }
                else
                {
                    return FindEnemy(true);
                }
            }
            if(info.Destination.Value!=info.Location.Value)
            {
                return FindEnemy(false);
            }
            else
            {
                return base.Defend();
            }
        }
        private TanksAction FindEnemy(bool isfindBoss)
        {
            List<TankInfo> canAttTanks = base.CanAttackEnemy();
            List<TankInfo> frindTanks = base.FindNearFriendly();
            if (canAttTanks.Count > 0)
            {
                if (canAttTanks.Count >= frindTanks.Count)
                {
                    return Attack(canAttTanks,null);
                }
                else
                {
                    return base.Retreat();
                }
            }
            List<TankInfo> enemyTanks = base.FindNearEnemy();
            if (enemyTanks.Count > 0)
            {
                if (frindTanks.Count > enemyTanks.Count)
                {
                    foreach (TankInfo tinfo in frindTanks)
                    {
                        if (tinfo != null)
                        {
                            tinfo.NextCommand = TankActionEnum.Attack;
                        }
                    }
                    return base.Attack(canAttTanks, enemyTanks);
                }
                if (enemyTanks.Count == 1)
                {
                    if (base.TankInfo.Contrast(enemyTanks[0]))
                    {
                        return base.Attack(canAttTanks, enemyTanks);
                    }
                    else
                    {
                        return base.Retreat();
                    }
                }
            }
            else
            {
               return base.Find();
            }
            if (isfindBoss)
            {
                return Boss();
            }
                return base.Find();
        }

    }
}
