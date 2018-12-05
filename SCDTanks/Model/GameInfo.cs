using SCDTanks.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Model
{
    /// <summary>
    /// 转换的游戏信息
    /// </summary>
    public class GameInfo
    {
        ///// <summary>
        ///// 剩余坦克数
        ///// </summary>
        //public int EnemyTanks { get; set; }
        public TankInfo Target { get; set; }
        /// <summary>
        /// 游戏信息
        /// </summary>
        public ReceiveInfo SourceInfo;
        /// <summary>
        /// 下一个预定位置
        /// </summary>
        public List<Point[]> NextPoint { get; set; } = new List<Point[]>();

        /// <summary>
        /// 我方复活币数量
        /// </summary>
        public int GodCount { get; set; }
        /// <summary>
        /// Boss
        /// </summary>
        public TankInfo BossInfo { get; set; }
        /// <summary>
        /// 敌方坦克
        /// </summary>
        public List<TankInfo> EnemyTanks { get; set; }

        /// <summary>
        /// 复活币
        /// </summary>
        public List<Point> GodB { get; set; }
        /// <summary>
        /// 敌方表示
        /// </summary>
        private string enemy = "";
        private int mapRow, mapCol;
        /// <summary>
        /// 未探索路
        /// </summary>
        public List<Point> Fogs { get; set; }
        public GameInfo(ReceiveInfo info)
        {
            GodB = new List<Point>();
            EnemyTanks = new List<TankInfo>();
            Fogs = new List<Point>();
            SharedResources.AttTank = null;
            SourceInfo = info;
            NextPoint.Clear();
            FillTanks();
        }

        private void FillTanks()
        {
            if (SourceInfo.Boss.Tanks != null && SourceInfo.Boss.Tanks.Count > 0)
                BossInfo = SourceInfo.Boss.Tanks[0];

            GodCount = SourceInfo.Gold+SourceInfo.Extend;
            if (SourceInfo.Team.Equals("tB"))
            {
                if (SharedResources.OurTanks == null)
                {
                    SharedResources.OurTanks = new List<TankInfo>();
                    SharedResources.OurTanks.AddRange(SourceInfo.TeamB.Tanks);
                }
                else
                {
                    foreach (TankInfo ftan in SharedResources.OurTanks)
                    {
                        ftan.UpdateInfo(SourceInfo.TeamB.Tanks);
                    }
                }
                EnemyTanks.AddRange(SourceInfo.TeamC.Tanks);
                enemy = "C";
            }
            if (SourceInfo.Team.Equals("tC"))
            {
                if (SharedResources.OurTanks == null)
                {
                    SharedResources.OurTanks = new List<TankInfo>();
                    SharedResources.OurTanks.AddRange(SourceInfo.TeamC.Tanks);
                }
                else
                {
                    foreach (TankInfo ftan in SharedResources.OurTanks)
                    {
                        ftan.UpdateInfo(SourceInfo.TeamC.Tanks);
                    }
                }
                EnemyTanks.AddRange(SourceInfo.TeamB.Tanks);
                enemy = "B";
            }
            mapRow = SourceInfo.MapInfo.Map.GetLength(0);
            mapCol = SourceInfo.MapInfo.Map.GetLength(1);
            for (int i = 0; i < mapRow; i++)
            {
                for (int j = 0; j < mapCol; j++)
                {
                    switch (SourceInfo.MapInfo.Map[i, j])
                    {
                        case "M1": break;
                        case "M2":
                            GodB.Add(new Point(i, j));
                            break;
                        case "M3":
                            Fogs.Add(new Point(i, j));
                            break;
                        case "M4":
                            break;
                        case "M5": break;
                        case "M6": break;
                        case "M7": break;
                        case "M8": break;
                        case "A1":
                            if (BossInfo != null)
                                BossInfo.Location = new Point(i, j);
                            break;
                        case "B1":
                        case "B2":
                        case "B3":
                        case "B4":
                        case "B5":
                            if (enemy == "B")
                            {
                                SetEnemyLocation(new Point(i, j), SourceInfo.MapInfo.Map[i, j]);
                            }
                            else
                            {
                                SetOurLocation(new Point(i, j), SourceInfo.MapInfo.Map[i, j]);
                            }
                            break;
                        case "C1":
                        case "C2":
                        case "C3":
                        case "C4":
                        case "C5":
                            if (enemy == "C")
                            {
                                SetEnemyLocation(new Point(i, j), SourceInfo.MapInfo.Map[i, j]);
                            }
                            else
                            {
                                SetOurLocation(new Point(i, j), SourceInfo.MapInfo.Map[i, j]);
                            }
                            break;
                    }
                }
            }
            if(GodB.Count>0)
            {
                SetGodTank();
            }
        }

        private void SetGodTank()
        {
            int delta1;
            int delta2;
            int count = 9999;
            TankInfo tankInfo = null;
            foreach (TankInfo tank in SharedResources.OurTanks)
            {
                foreach (Point point in GodB)
                {
                    delta1 = Math.Abs(tank.Location.Value.X - point.X);
                    delta2 = Math.Abs(tank.Location.Value.Y - point.Y);
                    if(count>(delta1+delta2)/tank.YiDong)
                    {
                        count = delta1 + delta2;
                        tankInfo = tank;
                    }
                }
            }
            if(tankInfo!=null)
            {
                tankInfo.NextCommand = TankActionEnum.God;
            }
        }
        /// <summary>
        /// 设置地方坦克坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="tankId"></param>
        private void SetEnemyLocation(Point point, string tankId)
        {
            TankInfo info = EnemyTanks.FirstOrDefault(p => p.TId.Equals(tankId));
            if (info != null && info.ShengYuShengMing>0)
                info.Location = point;
        }
        /// <summary>
        /// 设置我方坦克坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="tankId"></param>
        private void SetOurLocation(Point point, string tankId)
        {
            TankInfo info = SharedResources.OurTanks.FirstOrDefault(p => p.TId.Equals(tankId));
            if (info != null)
                info.Location = point;
        }
    }
}
