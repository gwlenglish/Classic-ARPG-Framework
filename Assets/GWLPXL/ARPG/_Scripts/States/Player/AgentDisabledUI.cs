using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Movement.com;

namespace GWLPXL.ARPGCore.States.com
{
    public class AgentDisabledUI : IState
    {
        IActorHub myMover = null;


        public AgentDisabledUI(IActorHub mover)
        {
            myMover = mover;
        }
        public void Enter()
        {
            //Player.MyMover.SetDesiredDestination(Player.transform.position, 1f);
            myMover.MyMover.DisableMovement(true);
        }



        public void Exit()
        {
            //Player.MyMover.SetDesiredDestination(Player.transform.position, 1f);
            myMover.MyMover.DisableMovement(false);
        }



        public void Tick()
        {

        }
    }
}