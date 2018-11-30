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
        public string Action()
        {
            return "123";
        }
        [HttpPost]
        public string Init()
        {
            return "123";
        }
    }
}
