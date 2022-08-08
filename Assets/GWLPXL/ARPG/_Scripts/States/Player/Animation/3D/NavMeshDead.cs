using System;

using UnityEngine;


namespace GWLPXL.ARPGCore.States.com
{



    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/3D/NavMesh Dead")]

    public class NavMeshDead : PlayerMovementState
    {
        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            return forEntity.GetActorHub().MyHealth.IsDead() == true;
        }

        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            DeadState state = new DeadState(forEntity.GetActorHub());
            Func<bool> Condition() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, Condition());
            stateDic.Add(forEntity, state);
        }
    }


   
}