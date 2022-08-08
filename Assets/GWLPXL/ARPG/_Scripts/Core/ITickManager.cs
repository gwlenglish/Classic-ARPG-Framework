
namespace GWLPXL.ARPGCore.com
{
    /// <summary>
    /// used to control the flow of game time and also update rates for things that tick (e.g. a Damage Over Time behavior).
    /// </summary>
    public interface ITickManager
    {
        void AddTicker(ITick newTick);
        void RemoveTicker(ITick remove);
        void DoTicks();
        void ClearTickers();

    }
}