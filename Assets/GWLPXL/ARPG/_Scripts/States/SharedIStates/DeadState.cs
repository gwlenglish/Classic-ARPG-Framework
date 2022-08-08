

using GWLPXL.ARPGCore.com;

using UnityEngine;
namespace GWLPXL.ARPGCore.States.com
{


    public class DeadState : IState
    {
        IActorHub mover;

        public DeadState(IActorHub _mover)
        {
            mover = _mover;

        }
        public void Enter()
        {
            GameObject obj = mover.MyTransform.gameObject;
            mover.MyMover.SetDesiredDestination(obj.transform.position, 1f);
            mover.MyMover.SetVelocity(Vector3.zero);
            mover.MyMover.DisableMovement(true);
            mover.MyAnim.SetAnimatorState("Dead");
            // myDamage.GetInstance().GetComponent<NavMeshAgent>().enabled = false;


        }


        public void Exit()
        {
            //myDamage.GetInstance().GetComponent<NavMeshAgent>().enabled = true;
            mover.MyMover.DisableMovement(false);

        }


        public void Tick()
        {

        }
    }
}