using System.Collections.Generic;
using System.Drawing;

namespace SCDTanks.Model
{
    /// <summary>
    /// 输出
    /// </summary>
    class K2Tank : TankActionBase
    {
        protected override TanksAction AbsGetAction(GameInfo controller, TankInfo info)
        {
            if (info.Destination == null)
            {
                info.Destination = new Point((int.Parse(controller.SourceInfo.MapInfo.ColLen) / 2) + 1, int.Parse(controller.SourceInfo.MapInfo.RowLen) / 2);
            }
            
            List<TankInfo> atttks = CanAttackEnemy();
            if(atttks.Count>0)
            {
               return  Attack(atttks,null);
            }

            if (controller.BossInfo.Location != null)
            {
                info.Destination = controller.BossInfo.Location.Value;
                return Boss();
            }
            atttks = FindNearEnemy();
            if(atttks.Count>0)
            {
                return Attack(null, atttks);
            }
            if (info.Destination.Value != info.Location.Value)
                return Find();
            return Defend();
        }
    }
}
