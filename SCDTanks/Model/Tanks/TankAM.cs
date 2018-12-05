using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
namespace SCDTanks.Model
{
    /// <summary>
    /// 敏捷
    /// </summary>
    public class AMTank :  TankActionBase
    {
        protected override TanksAction AbsGetAction(GameInfo controller, TankInfo info)
        {
            if(SharedResources.AttTank!=null)
            {
                return Attack(null,null);
            }
            List<TankInfo> cantank = CanAttackEnemy();
            if (cantank.Count > 0)
            {
                Debug.WriteLine("阿斯玛发现可以攻击的敌人");
                base.Attack(cantank,null);
            }
            if (FindNearEnemy().Count>0)
            {
                base.Retreat();
            }
            if(base.Controller.Fogs.Count>0)
            {
               return Find();
            }
            TankInfo tinfo = SharedResources.OurTanks.FirstOrDefault(p => p.Adv == TankAdv.Defend && p.IsDie==false);
            if(tinfo==null)
                 tinfo = SharedResources.OurTanks.FirstOrDefault(p => p.Adv == TankAdv.Attack && p.IsDie == false);
            if (tinfo == null)
                tinfo = SharedResources.OurTanks.FirstOrDefault(p => p.Adv == TankAdv.Range && p.IsDie == false);
            if (tinfo == null)
                tinfo = SharedResources.OurTanks.FirstOrDefault(p => p.Adv == TankAdv.Speed && p.IsDie == false && p.Location!=info.Location);
            if (tinfo == null)
                return Defend();
            info.Destination = GetNearRoad(tinfo.Location.Value);
            return base.Find();
        }
        protected override TanksAction Find()
        {
            Point point = new Point(99, 99);
            int row = 0;
            int col = 0;
            int count = 9999;
            foreach(Point p in base.Controller.Fogs)
            {
                row = Math.Abs(this.TankInfo.Location.Value.X - p.X);
                col = Math.Abs(this.TankInfo.Location.Value.Y - p.Y);
                if(count>row+col)
                {
                    TankInfo.Destination = p;
                    count = row + col;
                }
            }
            return base.Find();
        }
    }
}
