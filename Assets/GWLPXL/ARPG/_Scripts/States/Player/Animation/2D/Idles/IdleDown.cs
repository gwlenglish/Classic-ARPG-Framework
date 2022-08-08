
using UnityEngine;

using System;


namespace GWLPXL.ARPGCore.States.com
{

    
   

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Animation/Idle Down")]

    public class IdleDown : PlayerMovementState2D
    {


        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            if (forEntity.GetActorHub().PlayerControlled != null)
            {
                return forEntity.GetActorHub().InputHub.MoveInputs.GetHorizontalRaw() == 0 && forEntity.GetActorHub().InputHub.MoveInputs.GetVerticalRaw() == 0 && forEntity.Get2D().GetFacingDirection() == GlobalFacingDirection.Down;
            }
            else
            {
                return forEntity.GetAI().GetDirection().x == 0 && forEntity.GetAI().GetDirection().y == 0 && forEntity.Get2D().GetFacingDirection() == GlobalFacingDirection.Down;
            }

        }

        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            AnimateGenericMove2D state = new AnimateGenericMove2D(forEntity, Vars);
            Func<bool> HasWalkingTarget() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, HasWalkingTarget());
            stateDic.Add(forEntity, state);
        }
    }


  

    public class AnimateStop : IState
    {
        IStateMachineEntity entity;
        MoveVars2D vars;
        public AnimateStop(IStateMachineEntity entity, MoveVars2D vars)
        {
            this.entity = entity;
            this.vars = vars;
        }
        public void Enter()
        {
            entity.GetActorHub().MyAnimator.Play(vars.AnimatorStateName);

            
            vars.Locomotion.Vars = this.vars;
            if (entity.GetAI() != null)
            {
                entity.GetAI().SetDirection(new Vector3(0, 0, 0));
            }
        }

        public void Exit()
        {

        }

        public void Tick()
        {

        }
    }
}