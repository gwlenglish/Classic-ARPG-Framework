using UnityEngine;
namespace GWLPXL.ARPGCore.States.com
{


    public class InActiveDungeonState : IState
    {
        public InActiveDungeonState()
        {

        }

        public void Enter()
        {
            Time.timeScale = 0;

        }



        public void Exit()
        {
            Time.timeScale = 1;
        }

        public void Tick()
        {

        }
    }

}
