using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCDTanks.Model
{
    /// <summary>
    /// 行动枚举
    /// </summary>
    public enum TankActionEnum
    {
        /// <summary>
        /// 防守
        /// </summary>
        Defend,
        /// <summary>
        /// 进攻
        /// </summary>
        Attack,
        /// <summary>
        /// 探路
        /// </summary>
        Fint,
        /// <summary>
        /// 抢复活币
        /// </summary>
        God,
        /// <summary>
        /// 攻略BOSS
        /// </summary>
        Boss,
        /// <summary>
        /// 撤退
        /// </summary>
        Retreat,
        /// <summary>
        /// 支援
        /// </summary>
        Support,
        /// <summary>
        /// 空指令
        /// </summary>
        Null
    }
}
