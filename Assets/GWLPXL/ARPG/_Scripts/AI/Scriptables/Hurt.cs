
using UnityEngine;
using GWLPXL.ARPGCore.States.com;
using System;
using UnityEngine.AI;

namespace GWLPXL.ARPGCore.AI.com
{


    [System.Serializable]
    public class HurtVars
    {
        public string AnimatorStateName = "Hurt";
        public float AnimBlending = .02f;
        public int AnimLayer = 0;

    }


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/AI/Hurt")]

    public class Hurt : AIStateSO
    {

        public HurtVars Vars;


        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {
            HurtState state = new HurtState(forEntity, Vars);
            Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasWalkingTarget());
            stateDic.Add(forEntity, state);
        }

      
    }

    [System.Serializable]
    public class HurtState : IState
    {
        public IAIEntity Entity;
        HurtVars vars;


        public HurtState(IAIEntity entity, HurtVars vars)
        {
            this.vars = vars;
            Entity = entity;
        }


        public void Enter()
        {
            Entity.GetActorHub().MyAnim.SetAnimatorState(vars.AnimatorStateName, vars.AnimBlending, vars.AnimLayer);
            Entity.GetActorHub().MyMover.SetDesiredDestination(Entity.GetActorHub().MyTransform.position, 1f);
            Entity.GetActorHub().MyMover.DisableMovement(true);
   
        }

        public void Exit()
        {

            Entity.GetActorHub().MyMover.DisableMovement(false);
        }

        public void Tick()
        {

           
        }
    }
}