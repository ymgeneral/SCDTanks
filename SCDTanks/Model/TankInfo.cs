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
        /// <summary>
        /// 坦克实力
        /// </summary>
        public int Power
        {
            get { return this.ShengYuShengMing + (Gongji*3) + (SheCheng * 3); }
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
    }
}
