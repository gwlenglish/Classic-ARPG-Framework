
namespace GWLPXL.ARPGCore.com
{
    /// <summary>
    /// used to control game time and things that depend on time.
    ///The tick manager controls the update rates for things that 'tick'.
    /// </summary>
    public interface ITick
    {
        void AddTicker();
        void DoTick();
        void RemoveTicker();
        float GetTickDuration();
    }
}