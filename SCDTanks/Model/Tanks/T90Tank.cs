namespace SCDTanks.Model
{
    /// <summary>
    /// 肉盾
    /// </summary>
    class T90Tank : ITankAction
    {
        public TanksAction GetAction(TankInfo info)
        {
            return new TanksAction();
        }
    }
}
