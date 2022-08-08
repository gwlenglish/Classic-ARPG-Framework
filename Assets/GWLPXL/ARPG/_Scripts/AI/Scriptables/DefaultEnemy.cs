using GWLPXL.ARPGCore.AI.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{
   

    public class DefaultEnemy : AIStateSO
    {
       
        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {
          
        }

       
    }



    public class DefaultEnemyBehavior : IState
    {
        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}