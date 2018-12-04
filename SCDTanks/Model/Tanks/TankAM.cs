using System;
using System.Drawing;

namespace SCDTanks.Model
{
    /// <summary>
    /// 敏捷
    /// </summary>
    public class AMTank :  TankActionBase
    {
        protected override TanksAction AbsGetAction(GameInfo controller, TankInfo info)
        {
            if(base.Controller.GodB.Count>0)
            {
                return God();
            }
            if(base.Controller.Fogs.Count>0)
            {
                Find();
            }
            return null;
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
                if(count<row+col)
                {
                    TankInfo.Location = p;
                }
            }
            return base.Find();
        }
    }
}
