namespace SCDTanks.Model
{
    public interface ITankAction
    {
        TanksAction GetAction(TankInfo info);
    }
}
