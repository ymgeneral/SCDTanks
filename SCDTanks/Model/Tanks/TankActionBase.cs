using SCDTanks.Algorithm;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SCDTanks.Model
{
    public abstract class TankActionBase
    {
        protected GameInfo Controller { get; set; }
        protected TankInfo TankInfo { get; set; }
        protected abstract TanksAction AbsGetAction(GameInfo controller, TankInfo info);
        protected abstract void SetNextCommand();
        /// <summary>
        /// 获取坦克下一步动作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public TanksAction GetNextAction(GameInfo controller, TankInfo info)
        {
            Controller = controller;
            this.TankInfo = info;
            return AbsGetAction(Controller, this.TankInfo);
        }
        /// <summary>
        /// 获取坦克指令
        /// </summary>
        /// <returns></returns>
        protected TankActionEnum GetActionEnum()
        {
            TankActionEnum tankAction = TankActionEnum.Null;
            if (Controller.BossInfo.Location != null)
            {
                tankAction = TankActionEnum.Boss;
                if (Controller.BossInfo.ShengYuShengMing < 5)
                {
                    return tankAction;
                }
            }
            if (Controller.GodB.Count > 0)
            {
                tankAction = TankActionEnum.God;
                return tankAction;
            }
            return tankAction;
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
            foreach (TankInfo etank in this.Controller.EnemyTanks)
            {
                switch (etank.Adv)
                {
                    case TankAdv.Range:
                    case TankAdv.Speed: offset = 5; break;
                    default: offset = 3; break;
                }
                if (etank.Location != null)
                {
                    foreach (TankInfo otank in SharedResources.OurTanks)
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
        /// 防守
        /// </summary>
        public virtual TanksAction Defend(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 进攻
        /// </summary>
        protected virtual TanksAction Attack(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 探路
        /// </summary>
        protected virtual TanksAction Find(TankInfo info)
        {
            //TODO:未完成
            Astar astar = new Astar
            {
                Team = Controller.SourceInfo.Team
            };
            ANode node = astar.Star(info.Location.Value, GetGodb(info), GetTempMaps());
            Point next = GetDirection(node);
            TanksAction tanksAction = new TanksAction()
            {
                UseGlod = false,
                Length = 1,
                Direction = this.Conversion(info.Location.Value, next).ToString(),
                TId = info.TId,
                ActionType = ActionTypeEnum.MOVE.ToString()
            };
            return tanksAction;
        }

        /// <summary>
        /// 抢复活币
        /// </summary>
        protected virtual TanksAction God(TankInfo info)
        {
            Astar astar = new Astar
            {
                Team = Controller.SourceInfo.Team
            };
            ANode node = astar.Star(info.Location.Value, GetGodb(info), GetTempMaps());
            Point next=GetDirection(node);
            TanksAction tanksAction = new TanksAction()
            {
                UseGlod = false,
                Length = 1,
                Direction = this.Conversion(info.Location.Value, next).ToString(),
                TId = info.TId,
                ActionType = ActionTypeEnum.MOVE.ToString()
            };
            return tanksAction;
        }
        /// <summary>
        /// 攻略BOSS
        /// </summary>
        protected virtual TanksAction Boss(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 撤退
        /// </summary>
        protected virtual TanksAction Retreat(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 支援
        /// </summary>
        protected virtual TanksAction Support(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 空指令
        /// </summary>

        protected virtual TanksAction Null(TankInfo info)
        {
            TanksAction tanksAction = new TanksAction()
            {
                UseGlod = false,
                Length = 0,
                Direction = DirectionEnum.UP.ToString(),
                TId = info.TId,
                ActionType = ActionTypeEnum.FFIRE.ToString()
            };
            return tanksAction;
        }
        /// <summary>
        /// 获取距离最近复活币
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected Point GetGodb(TankInfo info)
        {
            Point godb = new Point(0, 0);
            if (this.Controller.GodB.Count == 1)
                godb = Controller.GodB[0];
            else
            {
                int nowdistances = 99999;
                List<int> distances = new List<int>();
                foreach (Point p in Controller.GodB)
                {
                    if (Math.Abs(info.Location.Value.X - p.X) + Math.Abs(info.Location.Value.Y - p.Y) < nowdistances)
                    {
                        nowdistances = Math.Abs(info.Location.Value.X - p.X) + Math.Abs(info.Location.Value.Y - p.Y);
                        godb = p;
                    }
                }
            }
            return godb;
        }

        /// <summary>
        /// 获取临时地图
        /// </summary>
        /// <returns></returns>
        protected string[,] GetTempMaps()
        {
            string[,] maps = new string[int.Parse(Controller.SourceInfo.MapInfo.RowLen), int.Parse(Controller.SourceInfo.MapInfo.ColLen)];
            Array.Copy(Controller.SourceInfo.MapInfo.Map, maps, maps.Length);
            if (Controller.NextPoint.Count > 0)
            {
                foreach (Point[] p in Controller.NextPoint)
                {
                    maps[p[0].X, p[0].Y] = "M1";
                    maps[p[1].X, p[1].Y] = "M4";
                }
            }
            return maps;
        }
        /// <summary>
        /// 转换方向
        /// </summary>
        /// <param name="spoint"></param>
        /// <param name="npoint"></param>
        /// <returns></returns>
        protected DirectionEnum Conversion(Point spoint, Point npoint)
        {
            if (spoint == npoint)
                return DirectionEnum.WAIT;
            if (npoint.X > spoint.X)
                return DirectionEnum.RIGHT;
            if (npoint.X < spoint.X)
                return DirectionEnum.LEFT;
            if (npoint.Y > spoint.Y)
                return DirectionEnum.DOWN;
            if (npoint.Y < spoint.Y)
                return DirectionEnum.UP;

            return DirectionEnum.WAIT;
        }
        /// <summary>
        /// 获取下一个坐标
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected Point GetDirection(ANode node)
        {
            Point retPoint = new Point(0, 0);
            Point[] points = new Point[2];
            while (true)
            {
                if (node.Parent == null) break;
                retPoint = node.Position;
                node = node.Parent;
            }
            points[0] = node.Position;
            points[1] = retPoint;
            Controller.NextPoint.Add(points);
            return retPoint;
        }
    }
}
