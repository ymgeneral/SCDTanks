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
        /// 游戏信息
        /// </summary>
        public static ReceiveInfo GameInfo;
        /// <summary>
        /// 下一个预定位置
        /// </summary>
        public static List<Point[]> NextPoint { get; set; } = new List<Point[]>();
        /// <summary>
        /// 我方坦克，持久化数据
        /// </summary>
        public static List<TankInfo> OurTanks { get; set; }

        public  static int GodCount { get; set; }

        /// <summary>
        /// Boss
        /// </summary>
        public static TankInfo BossInfo { get; set; }
        /// <summary>
        /// 敌方坦克
        /// </summary>
        public static List<TankInfo> EnemyTanks { get; set; }

        /// <summary>
        /// 复活币
        /// </summary>
        public static List<Point> GodB { get; set; }
    }
}
