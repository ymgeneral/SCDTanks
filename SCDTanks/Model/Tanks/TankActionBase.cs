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
        protected List<Point> CanAttackPoint { get; set; }
        /// <summary>
        /// 获取坦克下一步动作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public TanksAction GetNextAction(GameInfo controller, TankInfo info)
        {

            this.TankInfo = info;
            Controller = controller;
            CanAttackPoint = GetAttackRange();
            switch (info.NextCommand)
            {
                case TankActionEnum.Null:
                    return AbsGetAction(Controller, this.TankInfo);
                case TankActionEnum.Support:
                    return Support();
                case TankActionEnum.Attack:
                    return Attack();
                case TankActionEnum.Boss:
                    return Boss();
                case TankActionEnum.Defend:
                    return Defend();
                case TankActionEnum.Find:
                    return Find();
                case TankActionEnum.God:
                    return God();
                case TankActionEnum.Retreat:
                    return Retreat();
            }
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
        protected List<TankInfo> FindNearEnemy()
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
                    if ((Math.Abs(this.TankInfo.Location.Value.X - etank.Location.Value.X) + Math.Abs(this.TankInfo.Location.Value.Y - etank.Location.Value.Y)) < offset)
                    {
                        if (!tankInfos.Contains(etank))
                            tankInfos.Add(etank);
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
        protected List<TankInfo> FindNearFriendly()
        {
            return null;
        }
        protected List<Point> GetAttackRange()
        {
            List<Point> scopeList = new List<Point>();
            string str = "";
            //左
            for (int i = 1; i <= this.TankInfo.SheCheng; i++)
            {
                if (this.TankInfo.Location.Value.Y - i < 0) break;
                str = this.Controller.SourceInfo.MapInfo.Map[this.TankInfo.Location.Value.X, this.TankInfo.Location.Value.Y - i];
                if (str.Equals("M4") || str.Equals("M5") || str.Equals("M6") || str.Equals("M7") || str.Equals("M8"))
                {
                    continue;
                }
                scopeList.Add(new Point(this.TankInfo.Location.Value.X, this.TankInfo.Location.Value.Y - i));
            }
            //右
            for (int i = 1; i <= this.TankInfo.SheCheng; i++)
            {
                if (this.TankInfo.Location.Value.Y + i > (int.Parse(this.Controller.SourceInfo.MapInfo.ColLen) - 1)) break;
                str = this.Controller.SourceInfo.MapInfo.Map[this.TankInfo.Location.Value.X, this.TankInfo.Location.Value.Y + i];
                if (str.Equals("M4") || str.Equals("M5") || str.Equals("M6") || str.Equals("M7") || str.Equals("M8"))
                {
                    continue;
                }
                scopeList.Add(new Point(this.TankInfo.Location.Value.X, this.TankInfo.Location.Value.Y + i));
            }
            //上
            for (int i = 1; i <= this.TankInfo.SheCheng; i++)
            {
                if (this.TankInfo.Location.Value.X - i < 0) break;
                str = this.Controller.SourceInfo.MapInfo.Map[this.TankInfo.Location.Value.X - i, this.TankInfo.Location.Value.Y];
                if (str.Equals("M4") || str.Equals("M5") || str.Equals("M6") || str.Equals("M7") || str.Equals("M8"))
                {
                    continue;
                }
                scopeList.Add(new Point(this.TankInfo.Location.Value.X - i, this.TankInfo.Location.Value.Y));
            }
            //下
            for (int i = 1; i <= this.TankInfo.SheCheng; i++)
            {
                if (this.TankInfo.Location.Value.X + i > (int.Parse(this.Controller.SourceInfo.MapInfo.RowLen) - 1)) break;
                str = this.Controller.SourceInfo.MapInfo.Map[this.TankInfo.Location.Value.X + i, this.TankInfo.Location.Value.Y];
                if (str.Equals("M4") || str.Equals("M5") || str.Equals("M6") || str.Equals("M7") || str.Equals("M8"))
                {
                    continue;
                }
                scopeList.Add(new Point(this.TankInfo.Location.Value.X + i, this.TankInfo.Location.Value.Y));
            }

            return scopeList;
        }
        /// <summary>
        /// 在攻击范围内的敌人
        /// </summary>
        /// <param name="myTankInfo"></param>
        /// <returns></returns>
        protected List<TankInfo> CanAttackEnemy()
        {
            List<TankInfo> canAtt = new List<TankInfo>();
            foreach (TankInfo info in this.Controller.EnemyTanks)
            {
                if (info.Location == null) continue;
                if (this.CanAttackPoint.Contains(info.Location.Value))
                    canAtt.Add(info);
            }
            return canAtt;
        }
        /// <summary>
        /// 防守
        /// </summary>
        public virtual TanksAction Defend()
        {
            return null;
        }
        /// <summary>
        /// 进攻
        /// </summary>
        protected virtual TanksAction Attack(List<TankInfo> canAttTanks = null)
        {
            return null;
        }
        /// <summary>
        /// 探路
        /// </summary>
        protected virtual TanksAction Find()
        {
            if (this.TankInfo.Destination != null)
            {
                return Find(this.TankInfo.Destination.Value);
            }
            else
            {
                return null;
            }
        }
        private TanksAction Find(Point tar)
        {
            Astar astar = new Astar
            {
                Team = Controller.SourceInfo.Team
            };
            ANode node = astar.Star(this.TankInfo.Location.Value, tar, GetTempMaps());
            Stack<Point> points = GetDirection(node);
            Point next = points.Pop();
            int length = 1;
            if (this.TankInfo.YiDong > 1 && points.Count > 0)
            {
                if (points.Peek().X == this.TankInfo.Location.Value.X || points.Peek().Y == this.TankInfo.Location.Value.Y)
                {

                    next = points.Pop();
                    length = 2;
                }
            }
            TanksAction tanksAction = new TanksAction()
            {
                UseGlod = false,
                Length = length,
                Direction = this.Conversion(this.TankInfo.Location.Value, next).ToString(),
                TId = this.TankInfo.TId,
                ActionType = ActionTypeEnum.MOVE.ToString()
            };
            points.Clear();
            return tanksAction;
        }
        /// <summary>
        /// 抢复活币
        /// </summary>
        protected virtual TanksAction God()
        {
            return Find(GetGodb());
        }
        /// <summary>
        /// 攻略BOSS
        /// </summary>
        protected virtual TanksAction Boss()
        {
            return null;
        }
        /// <summary>
        /// 撤退
        /// </summary>
        protected virtual TanksAction Retreat()
        {
            return null;
        }
        /// <summary>
        /// 支援
        /// </summary>
        protected virtual TanksAction Support()
        {
            return null;
        }
        /// <summary>
        /// 空指令
        /// </summary>

        protected virtual TanksAction Null()
        {
            TanksAction tanksAction = new TanksAction()
            {
                UseGlod = false,
                Length = 0,
                Direction = DirectionEnum.UP.ToString(),
                TId = this.TankInfo.TId,
                ActionType = ActionTypeEnum.FFIRE.ToString()
            };
            return tanksAction;
        }
        /// <summary>
        /// 获取距离最近复活币
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected Point GetGodb()
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
                    if (Math.Abs(this.TankInfo.Location.Value.X - p.X) + Math.Abs(this.TankInfo.Location.Value.Y - p.Y) < nowdistances)
                    {
                        nowdistances = Math.Abs(this.TankInfo.Location.Value.X - p.X) + Math.Abs(this.TankInfo.Location.Value.Y - p.Y);
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
                return DirectionEnum.DOWN;
            if (npoint.X < spoint.X)
                return DirectionEnum.UP;
            if (npoint.Y > spoint.Y)
                return DirectionEnum.RIGHT;
            if (npoint.Y < spoint.Y)
                return DirectionEnum.LEFT;

            return DirectionEnum.WAIT;
        }
        /// <summary>
        /// 转换坐标
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected Stack<Point> GetDirection(ANode node)
        {
            Stack<Point> retPoint = new Stack<Point>();
            Point[] points = new Point[2];
            while (true)
            {

                if (node.Position == this.TankInfo.Location.Value) break;
                retPoint.Push(node.Position);
                node = node.Parent;
            }
            if (retPoint.Count == 0)
            {
                retPoint.Push(node.Position);
                node = node.Parent;
            }
            points[0] = node.Position; //当前位置
            points[1] = retPoint.Peek();//下一个位置
            Controller.NextPoint.Add(points);
            return retPoint;
        }
    }
}
