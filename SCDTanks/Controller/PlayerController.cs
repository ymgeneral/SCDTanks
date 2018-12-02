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
        public JsonResults<List<TanksAction>> Action(ReceiveInfo receiveInfo)
        {
            try
            {
                TanksController tanks = new TanksController(receiveInfo);
                JsonResults<List<TanksAction>> ret = new JsonResults<List<TanksAction>>();
                ret.Action = @"/action";
                ret.Code = "0";
                ret.Msg = "succeeded";
                ret.OK = true;
                ret.Data = tanks.GetJsonResults();
                return ret;
            }
            catch(Exception ex)
            {
                JsonResults<List<TanksAction>> ret = new JsonResults<List<TanksAction>>();
                ret.Action = @"/action";
                ret.Code = "1";
                ret.Msg = ex.Message;
                ret.OK = false;
                return ret;
            }
        }
        [HttpPost]
        public JsonResults<string> Init(ReceiveInfo receiveInfo)
        {
            JsonResults<string> json = new JsonResults<string>();
            json.Action = @"/init";
            json.Code = "0";
            json.Msg = "succeeded";
            json.OK = true;
            json.Data = null;
            TanksController tanks = new TanksController(receiveInfo);
            return json;
        }
    }
}
