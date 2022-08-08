using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Factions.com
{
    public interface IFactionMember
    {
        FactionTypes GetFaction();
        void IncreaseRep(FactionTypes withFaction, int amount);
        void DecreaseRep(FactionTypes withFaction, int amount);
        int GetFactionRep(FactionTypes withFaction);
    }
}