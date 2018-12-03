namespace SCDTanks.Model
{
    /// <summary>
    /// 敏捷
    /// </summary>
    public class AMTank :  TankActionBase
    {
        protected override TanksAction AbsGetAction(GameInfo controller, TankInfo info)
        {
            return null;
        }
    }
}
