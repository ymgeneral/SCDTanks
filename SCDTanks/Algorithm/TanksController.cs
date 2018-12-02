using SCDTanks.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Algorithm
{
    /// <summary>
    /// 坦克控制类
    /// </summary>
    public class TanksController
    {

        /// <summary>
        /// Boss
        /// </summary>
        public TankInfo BossInfo { get; set; }
        /// <summary>
        /// 敌方坦克
        /// </summary>
        public List<TankInfo> EnemyTanks { get; set; }
        /// <summary>
        /// 游戏信息
        /// </summary>
        private ReceiveInfo gameInfo;
        /// <summary>
        /// 复活币
        /// </summary>
        private List<Point> GodB { get; set; }
        /// <summary>
        /// 敌方表示
        /// </summary>
        private string enemy = "";
        private int mapRow, mapCol;
        /// <summary>
        /// 未探索路
        /// </summary>
        private List<Point> fogs;
        public TanksController(ReceiveInfo info)
        {
            GodB = new List<Point>();
            EnemyTanks = new List<TankInfo>();
            fogs = new List<Point>();
            gameInfo = info;
            FillTanks();
        }

        public List<TanksAction> GetJsonResults()
        {
            List<TanksAction> list = new List<TanksAction>();
            List<TankInfo> attackEnemys; //可攻击敌人
            List<TankInfo> nearEnemys;//附近敌人
            List<TankInfo> nearFriendly;//附近友军
            nearEnemys = FindNearEnemy();
            if (EnemyTanks.Count > 0)
            {
                //未发现敌人
            }
            if (GodB.Count == 0)
            {
                //未发现复活币
            }
            if (BossInfo == null)
            {
                //未发现boss
            }
            return list;
        }
        ///// <summary>
        ///// 获取我方坦克下一次行动指令
        ///// </summary>
        ///// <returns></returns>
        //public List<TankInfo> GetCommand()
        //{
        //    TankInfo enemyTank = null;
        //    List<TankInfo> attackEnemys;
        //    List<TankInfo> nearFriendly;
        //    int ourPower = 0, enemyPower = 0;

        //    foreach (TankInfo info in General.OurTanks)
        //    {
        //        if (info.Adv== TankAdv.Speed)
        //        {
        //            if (GodB.Count > 0)
        //            {
        //                info.NextCommand = TankActionEnum.God;
        //                continue;
        //            }
        //        }
        //        attackEnemys = CanAttackEnemy(info);
        //        if(attackEnemys!=null)
        //        {
        //            //开始战斗
        //            nearFriendly = FindNearFriendly(info);
        //            if(nearFriendly.Count>attackEnemys.Count)
        //            {
        //                info.NextCommand = TankActionEnum.Attack;
        //                continue;
        //            }
        //            if(nearFriendly.Count==attackEnemys.Count)
        //            {
        //                foreach(TankInfo etan in attackEnemys)
        //                {
        //                    enemyPower += etan.Power;
        //                }
        //                foreach (TankInfo ftan in nearFriendly)
        //                {
        //                    ourPower += ftan.Power;
        //                }
        //                if (enemyPower - ourPower > 0)
        //                {
        //                    //敌人强
        //                    info.NextCommand = TankActionEnum.Retreat;
        //                    continue;
        //                }
        //                else
        //                {
        //                    //可以一战
        //                    info.NextCommand = TankActionEnum.Attack;
        //                    foreach (TankInfo ftan in nearFriendly)
        //                    {
        //                        if (ftan.NextCommand == TankActionEnum.Null)
        //                            continue;
        //                        ftan.NextCommand = TankActionEnum.Support;
        //                        ftan.SupportTank = info;
        //                    }
        //                    continue;
        //                }
        //            }
        //            continue;
        //        }
        //        enemyTank = FindNearEnemy(info);
        //        if(enemyTank==null)
        //        {
        //            //附近未发现敌人
        //        }
        //        else
        //        {
        //           //附近发现敌人
        //        }
        //    }
        //    return null;
        //}
        /// <summary>
        /// 包括自身数量
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<TankInfo> FindNearFriendly(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 在攻击范围内的敌人
        /// </summary>
        /// <param name="myTankInfo"></param>
        /// <returns></returns>
        private List<TankInfo> CanAttackEnemy(TankInfo myTankInfo)
        {
            return null;
        }
        /// <summary>
        /// 有威胁的敌人
        /// </summary>
        /// <param name="myTankInfo"></param>
        /// <returns></returns>
        private List<TankInfo> FindNearEnemy()
        {
            List<TankInfo> tankInfos = new List<TankInfo>();
            int offset = 3;
            foreach (TankInfo etank in EnemyTanks)
            {
                switch (etank.Adv)
                {
                    case TankAdv.Range:
                    case TankAdv.Speed: offset = 5; break;
                    default: offset = 3; break;
                }
                if (etank.Location != null)
                {
                    foreach (TankInfo otank in General.OurTanks)
                    {

                        if (Math.Abs(otank.Location.Value.X - etank.Location.Value.X) < offset && Math.Abs(otank.Location.Value.Y - etank.Location.Value.Y) < offset)
                        {
                            if (!tankInfos.Contains(etank))
                                tankInfos.Add(etank);
                        }
                    }
                }
            }
            return tankInfos;
        }
        private void FillTanks()
        {
            if (gameInfo.Boss.Tanks != null && gameInfo.Boss.Tanks.Count > 0)
                BossInfo = gameInfo.Boss.Tanks[0];

            General.GodCount = gameInfo.Gold;
            if (gameInfo.Team.Equals("tB"))
            {
                if (General.OurTanks == null)
                {
                    General.OurTanks = new List<TankInfo>();
                    General.OurTanks.AddRange(gameInfo.TeamB.Tanks);
                }
                else
                {
                    foreach (TankInfo ftan in General.OurTanks)
                    {
                        ftan.UpdateInfo(gameInfo.TeamB.Tanks);
                    }
                }
                EnemyTanks.AddRange(gameInfo.TeamC.Tanks);
                enemy = "C";
            }
            if (gameInfo.Team.Equals("tC"))
            {
                if (General.OurTanks == null)
                {
                    General.OurTanks = new List<TankInfo>();
                    General.OurTanks.AddRange(gameInfo.TeamB.Tanks);
                }
                else
                {
                    foreach (TankInfo ftan in General.OurTanks)
                    {
                        ftan.UpdateInfo(gameInfo.TeamC.Tanks);
                    }
                }
                EnemyTanks.AddRange(gameInfo.TeamB.Tanks);
                enemy = "B";
            }
            mapRow = gameInfo.MapInfo.Map.GetLength(0);
            mapCol = gameInfo.MapInfo.Map.GetLength(1);
            for (int i = 0; i < mapRow; i++)
            {
                for (int j = 0; j < mapCol; j++)
                {
                    switch (gameInfo.MapInfo.Map[i, j])
                    {
                        case "M1": break;
                        case "M2":
                            GodB.Add(new Point(i, j));
                            break;
                        case "M3":
                            fogs.Add(new Point(i, j));
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
                                SetEnemyLocation(new Point(i, j), gameInfo.MapInfo.Map[i, j]);
                            }
                            else
                            {
                                SetOurLocation(new Point(i, j), gameInfo.MapInfo.Map[i, j]);
                            }
                            break;
                        case "C1":
                        case "C2":
                        case "C3":
                        case "C4":
                        case "C5":
                            if (enemy == "C")
                            {
                                SetEnemyLocation(new Point(i, j), gameInfo.MapInfo.Map[i, j]);
                            }
                            else
                            {
                                SetOurLocation(new Point(i, j), gameInfo.MapInfo.Map[i, j]);
                            }
                            break;
                    }
                }
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
            if (info != null)
                info.Location = point;
        }
        /// <summary>
        /// 设置我方坦克坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="tankId"></param>
        private void SetOurLocation(Point point, string tankId)
        {
            TankInfo info = General.OurTanks.FirstOrDefault(p => p.TId.Equals(tankId));
            if (info != null)
                info.Location = point;
        }
    }
}
