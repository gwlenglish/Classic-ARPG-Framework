using GWLPXL.ARPGCore.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/3D/NavMesh Walk")]
    public class NavMeshWalk : PlayerMovementState
    {
        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            return forEntity.GetActorHub().MyMover.GetMoverEnabled() == true && 
                forEntity.GetActorHub().MyMover.GetVelocitySquaredMag() > 1 && 
                forEntity.GetActorHub().MyAbilities.GetRuntimeController().IsUsingAbility() == false;
        }

        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            WalkState state = new WalkState(forEntity, Vars);
            Func<bool> Condition() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, Condition());
            stateDic.Add(forEntity, state);
        }
    }

    public class WalkState : IState
    {
        MoveVars vars;
        IStateMachineEntity hub;
        public WalkState(IStateMachineEntity hub, MoveVars vars)
        {
            this.vars = vars;
            this.hub = hub;

        }
        public void Enter()
        {
            hub.GetActorHub().MyAnim.SetAnimatorState(vars.AnimatorStateName, vars.BlendDuration, vars.Layer);

           
        }

        public void Exit()
        {

        }

        public void Tick()
        {

        }
    }
}