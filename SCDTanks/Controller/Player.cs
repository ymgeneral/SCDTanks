using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using SCDTanks.Model;
using SCDTanks.Algorithm;
using System.Drawing;

namespace SCDTanks.Controller
{
    public class PlayerController: ApiController
    {
        [HttpPost]
        public JsonRequest<List<TanksAction>> Action(ReceiveInfo receiveInfo)
        {
       
            return null;
        }
        [HttpPost]
        public JsonRequest<string> Init(ReceiveInfo receiveInfo)
        {
            JsonRequest<string> json = new JsonRequest<string>();
            json.Action = @"/init";
            json.Code = "0";
            json.Msg = "succeeded";
            json.OK = true;
            json.Data = null;
            TanksController tanks = new TanksController(receiveInfo);
            List<TankInfo> tankInfos= tanks.GetCommand();
            return json;
        }
    }
}
