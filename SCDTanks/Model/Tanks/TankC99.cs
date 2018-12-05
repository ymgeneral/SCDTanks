using SCDTanks.Algorithm;
using System.Collections.Generic;
using System.Drawing;

namespace SCDTanks.Model
{
    /// <summary>
    /// 远程
    /// </summary>
    public class C99Tank : TankActionBase
    {
        protected override TanksAction AbsGetAction(GameInfo controller, TankInfo info)
        {
            if (info.Destination == null)
            {
                info.Destination = new Point((int.Parse(controller.SourceInfo.MapInfo.ColLen) / 2)-1, int.Parse(controller.SourceInfo.MapInfo.RowLen) / 2);
            }
            if (controller.BossInfo.Location != null)
            {
                if (controller.BossInfo.ShengYuShengMing < 5)
                {
                    return base.Boss();
                }
                else
                {
                    return FindEnemy(true);
                }
            }
            if(SharedResources.AttTank!=null)
            {
                return Attack(null,null);
            }
            return FindEnemy(false);
        }
        private TanksAction FindEnemy(bool isfindBoss)
        {
            List<TankInfo> canAttTanks = base.CanAttackEnemy();
            if (canAttTanks.Count > 0)
            {
                return Attack(canAttTanks, null);
            }
            List<TankInfo> enemyTanks = base.FindNearEnemy();
            if (enemyTanks.Count > 0)
            {
                return base.Attack(canAttTanks, enemyTanks);
            }
            else
            {
                if(base.TankInfo.Destination.Value!=base.TankInfo.Location.Value)
                {
                    return base.Find();
                }
            }
            if (isfindBoss)
            {
                this.TankInfo.Destination = Controller.BossInfo.Location.Value;
                return Boss();
            }
            return base.Defend();
        }

    }
}
