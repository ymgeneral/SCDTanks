using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks
{
    public class TanksAction
    {
        [JsonProperty(PropertyName = "tId")]
        public string TId { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string ActionType { get; set; }
        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
        [JsonProperty(PropertyName = "useGlod")]
        public bool UseGlod { get; set; }
    }
}
