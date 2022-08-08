
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.States.com
{

    public class ActiveDungeonState : IState
    {
        IDungeonUI dungeonUI;
        int frequentUITick = 5;
        int infrequentUITick = 10;
        int counter = 0;
        public ActiveDungeonState(IDungeonUI _ui)
        {
            dungeonUI = _ui;
        }
        public void Enter()
        {

        }



        public void Exit()
        {

        }


        public void Tick()
        {

            TickManager.Instance.DoTicks();

            counter += 1;



            if (counter == 5)
            {
                //updates the player's UI visual info
                counter = 0;
            }
           


           
        }
    }
}