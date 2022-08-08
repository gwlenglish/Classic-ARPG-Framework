
using UnityEngine;
using GWLPXL.ARPGCore.States.com;
using System;


namespace GWLPXL.ARPGCore.AI.com
{


    [System.Serializable]
    public class WalkerVars
    {
        public string AnimatorStateName = "Locomotion";
        public float BlendingTime = .02f;
        public int AnimatorLayer = 0;
        public float Speed = 1;
        public float Acceleration = 55;
        public float StoppingDistance = 1;
        public string VelocityParam = "Velocity";
   
    }


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/AI/Walker")]

    public class Walker : AIStateSO
    {

        public WalkerVars Vars;

        public override IState GetState(IAIEntity forEntity)
        {
            return stateDic[forEntity];
        }


        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {

            WalkerState state = new WalkerState(forEntity, Vars);
            Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasWalkingTarget());
            stateDic.Add(forEntity, state);
        }

      
    }

   
    public class WalkerState : IState
    {
        public IAIEntity Entity;
        WalkerVars vars;

        public WalkerState(IAIEntity entity, WalkerVars vars)
        {
            this.vars = vars;
            Entity = entity;
        }


        public void Enter()
        {
            Entity.GetActorHub().MyAnim.SetFloatParam(vars.VelocityParam, 1);
            Entity.GetActorHub().MyAnim.SetAnimatorState(vars.AnimatorStateName, vars.BlendingTime, vars.AnimatorLayer);


        }

        public void Exit()
        {

        }

        public void Tick()
        {
            if (Entity.GetMoveTarget() == null)
            {
                Debug.LogWarning("In Move state but no target to move towards");
                return;
            }
            Vector3 target = Entity.GetMoveTarget().transform.position;
            Entity.GetActorHub().MyMover.SetNewSpeed(vars.Speed, vars.Acceleration);
           // Vector3 direction = target - Entity.GetActorHub().MyTransform.position;
           // float sqrdmag = direction.sqrMagnitude;
            Entity.GetActorHub().MyMover.SetDesiredDestination(target, Entity.GetIdleDistance());
          
           




        }
    }
}