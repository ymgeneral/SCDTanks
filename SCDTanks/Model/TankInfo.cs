using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
