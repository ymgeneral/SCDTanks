using SCDTanks.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks
{
    public class General
    {
        /// <summary>
        /// 下一个预定位置
        /// </summary>
        public static List<Point> NextPoint { get; set; }
        /// <summary>
        /// 我方坦克，持久化数据
        /// </summary>
        public static List<TankInfo> OurTanks { get; set; }

        public static int GodCount { get; set; }
    }
}
