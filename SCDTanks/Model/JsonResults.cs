using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SCDTanks
{
    public class JsonResults<T>
    {
        [JsonProperty(PropertyName ="code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }
        [JsonProperty(PropertyName = "ok")]
        public bool OK { get; set; }
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}
