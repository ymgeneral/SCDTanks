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
using System.Diagnostics;

namespace SCDTanks.Controller
{
    public class PlayerController : ApiController
    {
        [HttpPost]
        public JsonResults<List<TanksAction>> Action(ReceiveInfo receiveInfo)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                GameInfo gameInfo = new GameInfo(receiveInfo);
                List<TanksAction> tanksActions = new List<TanksAction>();
                TanksAction tanksAction = null;
                foreach (TankInfo info in SharedResources.OurTanks)
                {
                    tanksAction = info.GetAction(gameInfo);
                    if (tanksAction != null)
                        tanksActions.Add(tanksAction);
                }

                JsonResults<List<TanksAction>> ret = new JsonResults<List<TanksAction>>
                {
                    Action = @"/action",
                    Code = "0",
                    Msg = "succeeded",
                    OK = true,
                    Data = tanksActions
                };
                watch.Stop();
                Debug.WriteLine(watch.ElapsedMilliseconds);
                return ret;
            }
            catch (Exception ex)
            {
                List<TanksAction> tanksActions = new List<TanksAction>();
                foreach (TankInfo info in SharedResources.OurTanks)
                {
                    tanksActions.Add(new TanksAction {
                        ActionType = ActionTypeEnum.FIRE.ToString(),
                        Direction = "UP",
                       Length=info.SheCheng,
                       TId=info.TId,
                        UseGlod=false
                    });
                }
                JsonResults<List<TanksAction>> ret = new JsonResults<List<TanksAction>>
                {
                    Action = @"/action",
                    Code = "0",
                    Msg = "succeeded",
                    OK = true,
                    Data = tanksActions
                };
                return ret;
            }
        }
        [HttpPost]
        public JsonResults<string> Init(ReceiveInfo receiveInfo)
        {
            JsonResults<string> json = new JsonResults<string>
            {
                Action = @"/init",
                Code = "0",
                Msg = "succeeded",
                OK = true,
                Data = null
            };
            GameInfo tanks = new GameInfo(receiveInfo);
            return json;
        }
    }
}
