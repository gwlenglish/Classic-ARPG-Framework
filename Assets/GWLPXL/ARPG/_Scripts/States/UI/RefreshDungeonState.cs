using UnityEngine;

using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.States.com
{
    /// <summary>
    /// puts dungeon in active state for a limited number of ticks
    /// </summary>
    public class RefreshDungeonState : IState
    {
        IDungeonUI dungeonUI;
        int tickcount = 0;
        int totalTicks = 0;
        public RefreshDungeonState(IDungeonUI _ui, int tickAmount)
        {
            dungeonUI = _ui;
            totalTicks = tickAmount;
        }
        public void Enter()
        {
            tickcount = 0;
            Time.timeScale = 1;
        }



        public void Exit()
        {

        }


        public void Tick()
        {
            tickcount += 1;
            if (tickcount > totalTicks)
            {
                dungeonUI.SetDungeonState(0);
                return;
            }
            TickManager.Instance.DoTicks();


        }
    }
}