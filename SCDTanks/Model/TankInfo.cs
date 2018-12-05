using Newtonsoft.Json;
using SCDTanks.Algorithm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Model
{
    public class TankInfo:IComparable
    {
        [JsonProperty(PropertyName = "tId")]
        public string TId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "gongji")]
        public int Gongji { get; set; }
        [JsonProperty(PropertyName = "shengming")]
        public int ShengMing { get; set; }
        [JsonProperty(PropertyName = "shengyushengming")]
        public int ShengYuShengMing { get; set; }
        [JsonProperty(PropertyName = "yidong")]
        public int YiDong { get; set; }
        [JsonProperty(PropertyName = "shecheng")]
        public int SheCheng { get; set; }
        [JsonProperty(PropertyName = "shiye")]
        public int ShiYe { get; set; }

        /// <summary>
        /// 坦克行为操作类
        /// </summary>
        public TankActionBase ActionBase
        {
            get
            {
                switch (this.Adv)
                {
                    case TankAdv.Range:
                        return new C99Tank();
                    case TankAdv.Attack:
                        return new K2Tank();
                    case TankAdv.Defend:
                        return new T90Tank();
                    case TankAdv.Speed:
                        return new AMTank();
                    default: return new T90Tank();
                }
            }
        }
        /// <summary>
        /// 支援坦克对象
        /// </summary>
        public TankInfo SupportTank { get; set; }

        public bool IsDie { get { return ShengYuShengMing <= 0; } }
        /// <summary>
        /// 坦克实力
        /// </summary>
        public int Power
        {
            get { return this.ShengYuShengMing + (Gongji * 3) + (SheCheng * 3); }
        }
        /// <summary>
        /// 坦克特性
        /// </summary>
        public TankAdv Adv
        {
            get
            {
                TankAdv tankAdv = TankAdv.Defend;
                switch (this.Name)
                {
                    case "K2黑豹": tankAdv = TankAdv.Attack; break;
                    case "T-90": tankAdv = TankAdv.Defend; break;
                    case "阿马塔": tankAdv = TankAdv.Speed; break;
                    case "99主战坦克": tankAdv = TankAdv.Range; break;
                }
                return tankAdv;
            }
        }
        /// <summary>
        /// 下一步指令
        /// </summary>
        public TankActionEnum NextCommand
        {
            get; set;
        }
        public TankActionEnum LastCommand { get; set; }
        /// <summary>
        /// 当前坐标
        /// </summary>
        public Point? Location { get; set; } = null;
        /// <summary>
        /// 待命目的地
        /// </summary>
        public Point? Destination
        {
            get;
            set;
        }
        public void UpdateInfo(List<TankInfo> infos)
        {
            TankInfo info = infos.FirstOrDefault(p => p.TId.Equals(this.TId));
            if (info != null)
            {
                this.ShengYuShengMing = info.ShengYuShengMing;
                this.LastCommand = this.NextCommand;
                this.NextCommand = TankActionEnum.Null;
            }
        }
        public bool Contrast(TankInfo info)
        {
            if (this.SheCheng > info.SheCheng)
                return true;
            return this.ShengYuShengMing * Gongji > (info.ShengYuShengMing + 1) * Gongji;
        }
        public TanksAction GetAction(GameInfo game)
        {
            if (this.ActionBase != null)
            {
                return this.ActionBase.GetNextAction(game, this);
            }
            else
            {
                TanksAction tanks = new TanksAction
                {
                    ActionType = ActionTypeEnum.FIRE.ToString(),
                    Direction = DirectionEnum.UP.ToString(),
                    TId = this.TId,
                    Length = this.SheCheng,
                    UseGlod = false
                };
                return tanks;
            }
        }

        public int CompareTo(object obj)
        {
            TankInfo tank = obj as TankInfo;
            if(tank!=null)
            {
                return this.ShengYuShengMing - tank.ShengYuShengMing;
            }
            else
            {
                return -1;
            }
        }
    }
}
