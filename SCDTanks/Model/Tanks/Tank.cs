namespace SCDTanks.Model.Tanks
{
    public class Tank
    {
        /// <summary>
        /// 坦克名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int ATK { get; set; }

        /// <summary>
        /// 生命值
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// 单步移动值
        /// </summary>
        public int MoveStep { get; set; }

        /// <summary>
        /// 射程
        /// </summary>
        public int Range { get; set; }

        /// <summary>
        /// 视野
        /// </summary>
        public int Vision { get; set; }

        /// <summary>
        /// 坦克特性
        /// </summary>
        public TankAdv Adv { get; set; }
    }
}
