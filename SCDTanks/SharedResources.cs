using SCDTanks.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks
{
    /// <summary>
    /// 共享资源类
    /// </summary>
    public class SharedResources
    {
        /// <summary>
        /// 我方坦克，持久化数据
        /// </summary>
        public static List<TankInfo> OurTanks { get; set; }

    }
}
