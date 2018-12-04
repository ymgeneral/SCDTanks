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
        protected bool IsGod = false;
        /// <summary>
        /// 获取坦克下一步动作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public TanksAction GetNextAction(GameInfo controller, TankInfo info)
        {
            if (controller.GodCount > 0)
            {
                if (this.TankInfo.IsDie && this.TankInfo.Adv != TankAdv.Speed)
                {
                    IsGod = true;
                }
                if (this.TankInfo.Adv == TankAdv.Defend && this.TankInfo.ShengYuShengMing < 2)
                {
                    IsGod = true;
                }
            }
            this.TankInfo = info;
            Controller = controller;
            CanAttackPoint = GetAttackRange();
            switch (info.NextCommand)
            {
                case TankActionEnum.Null:
                    return AbsGetAction(Controller, this.TankInfo);
                case TankActionEnum.Attack:
                    return Attack(null, null);
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
            List<TankInfo> tankInfos = new List<TankInfo>();
            foreach (TankInfo info in SharedResources.OurTanks)
            {
                if (Math.Abs(info.Location.Value.X - this.TankInfo.Location.Value.X) <= 1 && Math.Abs(info.Location.Value.Y - this.TankInfo.Location.Value.Y) <= 1)
                {
                    tankInfos.Add(info);
                }
            }
            return tankInfos;
        }
        /// <summary>
        /// 获取攻击范围
        /// </summary>
        /// <returns></returns>
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
            if (this.Controller.BossInfo.Location != null)
            {
                if (this.CanAttackPoint.Contains(this.Controller.BossInfo.Location.Value))
                    canAtt.Add(this.Controller.BossInfo);
            }
            return canAtt;
        }
        /// <summary>
        /// 防守
        /// </summary>
        public virtual TanksAction Defend()
        {
            return new TanksAction()
            {
                UseGlod = IsGod,
                Length = this.TankInfo.SheCheng,
                Direction = DirectionEnum.WAIT.ToString(),
                TId = this.TankInfo.TId,
                ActionType = ActionTypeEnum.FFIRE.ToString()
            };
        }
        /// <summary>
        /// 进攻
        /// </summary>
        protected virtual TanksAction Attack(List<TankInfo> canAttTanks, List<TankInfo> nearTanks)
        {

            if (canAttTanks == null && nearTanks == null)
            {
                canAttTanks = CanAttackEnemy();
                nearTanks = FindNearEnemy();
            }
            if (SharedResources.AttTank != null)
            {
                if (canAttTanks.Contains(SharedResources.AttTank))
                {
                    return new TanksAction()
                    {
                        UseGlod = IsGod,
                        Length = this.TankInfo.SheCheng,
                        Direction = Conversion(this.TankInfo.Location.Value, SharedResources.AttTank.Location.Value).ToString(),
                        TId = this.TankInfo.TId,
                        ActionType = ActionTypeEnum.FFIRE.ToString()
                    };
                }
                else
                {
                    TankInfo tinfo = GetAttTank(canAttTanks);
                    return  this.Find(GetAttPoint(tinfo), false);
                }
            }
            if (canAttTanks != null)
            {
                TankInfo tinfo = GetAttTank(canAttTanks);
                return new TanksAction()
                {
                    UseGlod = IsGod,
                    Length = this.TankInfo.SheCheng,
                    Direction = Conversion(this.TankInfo.Location.Value, tinfo.Location.Value).ToString(),
                    TId = this.TankInfo.TId,
                    ActionType = ActionTypeEnum.FFIRE.ToString()
                };
            }
            else
            {
                if (nearTanks == null)
                {
                    return Defend();
                }
                TankInfo tinfo = GetAttTank(nearTanks);
                return  this.Find(GetAttPoint(tinfo), false);
            }
        }
        /// <summary>
        /// 获取进攻位置
        /// </summary>
        /// <returns></returns>
        private Point GetAttPoint(TankInfo tinfo)
        {
            Point point = tinfo.Location.Value;
            List<Point> listPoint = new List<Point>();
            int row = point.X - this.TankInfo.SheCheng >= 0 ? point.X - this.TankInfo.SheCheng : 0;
            int col = point.Y;
            Point newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                listPoint.Add(newPoint);
            }
            row = point.X + this.TankInfo.SheCheng < int.Parse(this.Controller.SourceInfo.MapInfo.RowLen) ? point.X + this.TankInfo.SheCheng : int.Parse(this.Controller.SourceInfo.MapInfo.RowLen) - this.TankInfo.SheCheng;
            newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                listPoint.Add(newPoint);
            }
            row = point.X;
            col = point.Y - this.TankInfo.SheCheng >= 0 ? point.Y - this.TankInfo.SheCheng : 0;
            newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                listPoint.Add(newPoint);
            }
            col = point.Y + this.TankInfo.SheCheng < int.Parse(this.Controller.SourceInfo.MapInfo.ColLen) ? point.Y + this.TankInfo.SheCheng : int.Parse(this.Controller.SourceInfo.MapInfo.ColLen) - this.TankInfo.SheCheng;
            newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                listPoint.Add(newPoint);
            }
            int index = 0, count = -1;
            int delta1;
            int delta2;
            for (int i = 0; i < listPoint.Count; i++)
            {
                delta1 = Math.Abs(listPoint[i].X - this.TankInfo.Location.Value.X);
                delta2 = Math.Abs(listPoint[i].Y - this.TankInfo.Location.Value.Y);
                if (count == -1 || count > delta1 + delta2)
                {
                    count = delta1 + delta2;
                    index = i;
                }
            }
            return listPoint[index];
        }
        /// <summary>
        /// 获取攻击目标
        /// </summary>
        /// <param name="canAttTanks"></param>
        /// <returns></returns>
        private TankInfo GetAttTank(List<TankInfo> canAttTanks)
        {
            if (canAttTanks == null || canAttTanks.Count == 0)
                return null;
            canAttTanks.Sort();
            TankInfo rinfo = canAttTanks[0];
            foreach (TankInfo info in canAttTanks)
            {
                if (rinfo == info)
                {
                    continue;
                }
                if (info.ShengYuShengMing <= this.TankInfo.Gongji)
                {
                    rinfo = info;
                    break;
                }
                if (info.Adv == TankAdv.Attack)
                {
                    rinfo = info;
                }
            }
            return rinfo;
        }
        /// <summary>
        /// 探路
        /// </summary>
        protected virtual TanksAction Find()
        {
            if (this.TankInfo.Destination != null)
            {
                if (!IsRoad(this.TankInfo.Destination.Value))
                {
                    this.TankInfo.Destination = GetNearRoad(this.TankInfo.Destination.Value);
                }
                return Find(this.TankInfo.Destination.Value,false);
            }
            else
            {
                return Null();
            }
        }
        /// <summary>
        /// 获取附近的路
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point GetNearRoad(Point point)
        {
            int row = point.X - 1 >= 0 ? point.X - 1 : 0;
            int col = point.Y;
            Point newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                return newPoint;
            }
            row = point.X + 1 < int.Parse(this.Controller.SourceInfo.MapInfo.RowLen) ? point.X + 1 : int.Parse(this.Controller.SourceInfo.MapInfo.RowLen) - 1;
            newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                return newPoint;
            }
            row = point.X;
            col = point.Y - 1 >= 0 ? point.Y - 1 : 0;
            newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                return newPoint;
            }
            col = point.Y + 1 < int.Parse(this.Controller.SourceInfo.MapInfo.ColLen) ? point.Y + 1 : int.Parse(this.Controller.SourceInfo.MapInfo.ColLen) - 1;
            newPoint = new Point(row, col);
            if (IsRoad(newPoint))
            {
                return newPoint;
            }
            newPoint = point;
            return newPoint;
        }
        private bool IsRoad(Point point)
        {
            string str = this.Controller.SourceInfo.MapInfo.Map[point.X, point.Y];
            if (str.Equals("M1") || str.Equals("M3") || str.Equals("M3"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private TanksAction Find(Point tar ,bool avoid)
        {
            Astar astar = new Astar
            {
                Team = Controller.SourceInfo.Team
            };
            ANode node = astar.Star(this.TankInfo.Location.Value, tar, GetTempMaps(avoid));
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
                UseGlod = IsGod,
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
            return Find(GetGodb(),false);
        }
        /// <summary>
        /// 攻略BOSS
        /// </summary>
        protected virtual TanksAction Boss()
        {

            List<TankInfo> canAtt = CanAttackEnemy();
            if (canAtt.Contains(this.Controller.BossInfo))
            {
                return new TanksAction()
                {
                    UseGlod = IsGod,
                    Length = this.TankInfo.SheCheng,
                    Direction = Conversion(this.TankInfo.Location.Value, this.Controller.BossInfo.Location.Value).ToString(),
                    TId = this.TankInfo.TId,
                    ActionType = ActionTypeEnum.FFIRE.ToString()
                };
            }
            else
            {
                return new TanksAction()
                {
                    UseGlod = IsGod,
                    Length = this.TankInfo.YiDong,
                    Direction = Conversion(this.TankInfo.Location.Value, GetAttPoint(this.Controller.BossInfo)).ToString(),
                    TId = this.TankInfo.TId,
                    ActionType = ActionTypeEnum.MOVE.ToString()
                };
            }
        }
        /// <summary>
        /// 撤退
        /// </summary>
        protected virtual TanksAction Retreat()
        {
            Point tempPoint = this.TankInfo.Location.Value;
            int row = this.TankInfo.Location.Value.X;
            int col = this.TankInfo.Location.Value.Y;
            List<int> list = new List<int>();
            int up = 999, down = 999, left = 999, right = 999;
            //上
            if (row - this.TankInfo.YiDong >= 0)
            {
                up = 0;
                this.TankInfo.Location = new Point(row - this.TankInfo.YiDong, col);
                up += CanAttackEnemy().Count * 100;
                up += FindNearEnemy().Count;
                list.Add(up);
            }
            //下
            if (row + this.TankInfo.YiDong < int.Parse(this.Controller.SourceInfo.MapInfo.RowLen))
            {

                this.TankInfo.Location = new Point(row + this.TankInfo.YiDong, col);
                down = 0;
                down += CanAttackEnemy().Count * 100;
                down += FindNearEnemy().Count;
                list.Add(down);
            }
            //左
            if (col - this.TankInfo.YiDong >= 0)
            {
                this.TankInfo.Location = new Point(row, col - this.TankInfo.YiDong);
                left = 0;
                left += CanAttackEnemy().Count * 100;
                left += FindNearEnemy().Count;
                list.Add(left);
            }
            //右
            if (col + this.TankInfo.YiDong < int.Parse(this.Controller.SourceInfo.MapInfo.ColLen))
            {
                this.TankInfo.Location = new Point(row, col + this.TankInfo.YiDong);
                right = 0;
                right += CanAttackEnemy().Count * 100;
                right += FindNearEnemy().Count;
                list.Add(right);
            }
            list.Sort();
            if (list.Count > 0)
            {
                string dir = "";
                do
                {
                    if (list[0] == up)
                    {
                        dir = DirectionEnum.UP.ToString();
                        break;
                    }
                    if (list[0] == left)
                    {
                        dir = DirectionEnum.LEFT.ToString();
                        break;
                    }
                    if (list[0] == right)
                    {
                        dir = DirectionEnum.RIGHT.ToString();
                        break;
                    }
                    if (list[0] == down)
                    {
                        dir = DirectionEnum.DOWN.ToString();
                        break;
                    }
                } while (false);
                return new TanksAction()
                {
                    UseGlod = IsGod,
                    Length = this.TankInfo.YiDong,
                    Direction = dir,
                    TId = this.TankInfo.TId,
                    ActionType = ActionTypeEnum.MOVE.ToString()
                };
            }
            else
            {
                return Attack(null, null);
            }
        }
        /// <summary>
        /// 空指令
        /// </summary>
        protected virtual TanksAction Null()
        {
            TanksAction tanksAction = new TanksAction()
            {
                UseGlod = IsGod,
                Length = this.TankInfo.SheCheng,
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
        protected string[,] GetTempMaps(bool avoid)
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
            if (avoid)
                foreach (TankInfo info in this.Controller.EnemyTanks)
                {
                    if (info.Location != null)
                    {
                        for (int i = 1; i < info.SheCheng; i++)
                        {
                            if (info.Location.Value.X - i < 0) break;
                            if (maps[info.Location.Value.X - i, info.Location.Value.Y].Equals("M1") || maps[info.Location.Value.X - i, info.Location.Value.Y].Equals("M3"))
                            {
                                maps[info.Location.Value.X - i, info.Location.Value.Y] = "M4";
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int i = 1; i < info.SheCheng; i++)
                        {
                            if (info.Location.Value.X + i < 0) break;
                            if (maps[info.Location.Value.X + i, info.Location.Value.Y].Equals("M1") || maps[info.Location.Value.X + i, info.Location.Value.Y].Equals("M3"))
                            {
                                maps[info.Location.Value.X + i, info.Location.Value.Y] = "M4";
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int i = 1; i < info.SheCheng; i++)
                        {
                            if (info.Location.Value.Y - i < 0) break;
                            if (maps[info.Location.Value.X, info.Location.Value.Y-i].Equals("M1") || maps[info.Location.Value.X, info.Location.Value.Y-i].Equals("M3"))
                            {
                                maps[info.Location.Value.X, info.Location.Value.Y-i] = "M4";
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int i = 1; i < info.SheCheng; i++)
                        {
                            if (info.Location.Value.Y + i < 0) break;
                            if (maps[info.Location.Value.X, info.Location.Value.Y + i].Equals("M1") || maps[info.Location.Value.X, info.Location.Value.Y + i].Equals("M3"))
                            {
                                maps[info.Location.Value.X, info.Location.Value.Y + i] = "M4";
                            }
                            else
                            {
                                break;
                            }
                        }
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
