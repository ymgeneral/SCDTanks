using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;

namespace SCDTanks.Controller
{
    public class PlayerController: ApiController
    {
        [HttpPost]
        public JsonRequest<List<TanksAction>> Action()
        {
            return null;
        }
        [HttpPost]
        public JsonRequest<string> Init()
        {
            return null;
        }
    }
}
