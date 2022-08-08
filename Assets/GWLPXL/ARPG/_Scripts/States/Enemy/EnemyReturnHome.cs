

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Movement.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.States.com
{


    public class EnemyReturnHome : IState
    {
        IActorHub mover;
        Vector3 starting;
        public EnemyReturnHome(IActorHub _mover, Vector3 startingPosition)
        {
            mover = _mover;
            starting = startingPosition;
        }
        public void Enter()
        {
            mover.MyMover.SetDesiredDestination(starting, 1f);

        }



        public void Exit()
        {

        }


        public void Tick()
        {

        }
    }
}
