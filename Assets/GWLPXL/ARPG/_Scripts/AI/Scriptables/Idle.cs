
using UnityEngine;
using GWLPXL.ARPGCore.States.com;
using System;
using UnityEngine.AI;

namespace GWLPXL.ARPGCore.AI.com
{


    [System.Serializable]
    public class IdleVars
    {
        public string AnimatorStateName = "Locomotion";
        public float AnimBlendDuration = .02f;
        public int AnimLayer = 0;
        public string VelocityParam = "Velocity";
    }


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/AI/Idle")]

    public class Idle : AIStateSO
    {

        public IdleVars Vars;


        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {
            IdleState state = new IdleState(forEntity, Vars);
            Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasWalkingTarget());
            stateDic.Add(forEntity, state);
        }

      
    }

    [System.Serializable]
    public class IdleState : IState
    {
        public IAIEntity Entity;
        protected IdleVars vars;


        public IdleState(IAIEntity entity, IdleVars vars)
        {
            this.vars = vars;
            Entity = entity;
        }


        public void Enter()
        {
            Entity.GetActorHub().MyAnim.SetAnimatorState(vars.AnimatorStateName, vars.AnimBlendDuration, vars.AnimLayer);
            Entity.GetActorHub().MyMover.SetDesiredDestination(Entity.GetActorHub().MyTransform.position, 1f);
            Entity.GetActorHub().MyAnim.SetFloatParam(vars.VelocityParam, 0f);
   
        }

        public void Exit()
        {
          

        }

        public void Tick()
        {

           
        }
    }
}