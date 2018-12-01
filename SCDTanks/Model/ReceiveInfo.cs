using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Model
{
    public class ReceiveInfo
    {
        [JsonProperty(PropertyName = "gold")]
        public int Gold { get; set; }
        [JsonProperty(PropertyName = "team")]
        public string Team { get; set; }
        [JsonProperty(PropertyName = "view")]
        public MapInfo MapInfo { get; set; }
        [JsonProperty(PropertyName = "tA")]
        public TeamInfo Boss { get; set; }
        [JsonProperty(PropertyName = "tB")]
        public TeamInfo TeamB { get; set; }
        [JsonProperty(PropertyName = "tC")]
        public TeamInfo TeamC { get; set; }

    }
    public class MapInfo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "rowLen")]
        public string RowLen { get; set; }
        [JsonProperty(PropertyName = "colLen")]
        public string ColLen { get; set; }
        [JsonProperty(PropertyName = "map")]
        public string[,] Map { get; set; }
    }
    public class TeamInfo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "tanks")]
        public List<TankInfo> Tanks { get; set; }
    }
}
