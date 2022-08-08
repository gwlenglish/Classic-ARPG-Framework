using GWLPXL.ARPGCore.Animations.com;
using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{


    public class PlayerHurt : IState
    {

        IActorHub hub;
        float timer;
        public PlayerHurt(IActorHub hub)
        {
            this.hub = hub;
        }
        public void Enter()
        {
            timer = 0;

            hub.MyMover.DisableMovement(true);
        }

        public void Exit()
        {

            hub.MyMover.DisableMovement(false);

        }

        public void Tick()
        {
            timer += Time.deltaTime;
        }


    }
}