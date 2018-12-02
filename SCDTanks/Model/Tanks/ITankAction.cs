using SCDTanks.Algorithm;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SCDTanks.Model
{
    public abstract class ITankAction
    {
        /// <summary>
        /// 防守
        /// </summary>
        public virtual   TanksAction Defend(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 进攻
        /// </summary>
        public virtual TanksAction Attack(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 探路
        /// </summary>
        public virtual TanksAction Find(TankInfo info)
        {
            //TODO:未完成
            Astar astar = new Astar();
            astar.Team = SharedResources.GameInfo.Team;
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
        public virtual TanksAction God(TankInfo info)
        {
           
            Astar astar = new Astar();
            astar.Team = SharedResources.GameInfo.Team;
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
        public virtual TanksAction Boss(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 撤退
        /// </summary>
        public virtual TanksAction Retreat(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 支援
        /// </summary>
        public virtual TanksAction Support(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 空指令
        /// </summary>

        public virtual TanksAction Null(TankInfo info)
        {
            return null;
        }
        /// <summary>
        /// 获取距离最近复活币
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected Point GetGodb(TankInfo info)
        {
            Point godb = new Point(0, 0);
            if (SharedResources.GodB.Count == 1)
                godb = SharedResources.GodB[0];
            else
            {
                int nowdistances = 99999;
                List<int> distances = new List<int>();
                foreach (Point p in SharedResources.GodB)
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
            string[,] maps = new string[int.Parse(SharedResources.GameInfo.MapInfo.RowLen), int.Parse(SharedResources.GameInfo.MapInfo.ColLen)];
            Array.Copy(SharedResources.GameInfo.MapInfo.Map, maps, maps.Length);
            if (SharedResources.NextPoint.Count > 0)
            {
                foreach (Point[] p in SharedResources.NextPoint)
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
            SharedResources.NextPoint.Add(points);
            return retPoint;
        }
    }
}
