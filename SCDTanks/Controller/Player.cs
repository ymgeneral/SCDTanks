using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using SCDTanks.Model;

namespace SCDTanks.Controller
{
    public class PlayerController: ApiController
    {
        [HttpPost]
        public JsonRequest<List<TanksAction>> Action(ReceiveInfo receiveInfo)
        {
            General.NowReceiveInfo = receiveInfo;
            return null;
        }
        [HttpPost]
        public JsonRequest<string> Init(ReceiveInfo receiveInfo)
        {
            General.NowReceiveInfo = receiveInfo;
            JsonRequest<string> json = new JsonRequest<string>();
            json.Action = @"/init";
            json.Code = "0";
            json.Msg = "succeeded";
            json.OK = true;
            json.Data = null;
            return json;
        }
    }
}
