using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Model
{
    public class TankInfo
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

        public TankInfo SupportTank { get; set; }

        public bool IsDie { get { return ShengYuShengMing <= 0; } }
        /// <summary>
        /// 坦克实力
        /// </summary>
        public int Power
        {
            get { return this.ShengYuShengMing + (Gongji*3) + (SheCheng * 3); }
        }
        /// <summary>
        /// 坦克特性
        /// </summary>
        public TankAdv Adv
        {
            get
            {
                TankAdv tankAdv= TankAdv.Defend;
                switch (this.Name)
                {
                    case "K2黑豹": tankAdv= TankAdv.Attack; break;
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
        public TankActionEnum NextCommand { get; set; }
        public TankActionEnum LastCommand { get; set; }
        /// <summary>
        /// 当前坐标
        /// </summary>
        public Point? Location { get; set; } = null;

        public void UpdateInfo(List<TankInfo> infos)
        {
            TankInfo info = infos.FirstOrDefault(p => p.TId.Equals(this.TId));
            if(info!=null)
            {
                this.ShengYuShengMing = info.ShengYuShengMing;
                this.LastCommand = this.NextCommand;
                this.NextCommand = TankActionEnum.Null;
            }
        }

        public TanksAction GetAction()
        {
            ITankAction tankAction=null;
            switch(this.Adv)
            {
                case TankAdv.Range:
                    tankAction = new C99Tank();
                    break;
                case TankAdv.Attack:
                    tankAction = new K2Tank();
                    break;
                case TankAdv.Defend:
                    tankAction = new T90Tank();
                    break;
                case TankAdv.Speed:
                    tankAction = new AMTank();
                    break;
            }
            if (tankAction != null)
            {
                return tankAction.GetAction(this);
            }
            else
            {
                TanksAction tanks = new TanksAction();
                tanks.ActionType = ActionTypeEnum.FFIRE.ToString();
                tanks.Direction = DirectionEnum.UP.ToString();
                tanks.TId = this.TId;
                tanks.Length = this.SheCheng;
                tanks.UseGlod = false;
                return tanks;
            }
        }
    }
}
