
using UnityEngine;


namespace GWLPXL.ARPGCore.States.com
{
    public class AnimateGeneric2D : IState
    {
        IStateMachineEntity entity;
        GenericAnimate2D vars;
        bool allowed = false;
        public AnimateGeneric2D(IStateMachineEntity entity, GenericAnimate2D vars)
        {
            this.entity = entity;
            this.vars = vars;
        }
        public void Enter()
        {
            entity.GetActorHub().MyAnimator.Play(vars.AnimatorStateName);


        }

        public void Exit()
        {

        }

        public void Tick()
        {

        }
    }
    public class AnimateGenericMove2D : IState
    {
        IStateMachineEntity entity;
        MoveVars2D vars;
        bool allowed = false;
        public AnimateGenericMove2D(IStateMachineEntity entity, MoveVars2D vars)
        {
            this.entity = entity;
            this.vars = vars;
        }
        public void Enter()
        {

            if (entity.GetActorHub().MyAnimator != null)
            {
                entity.GetActorHub().MyAnimator.Play(vars.AnimatorStateName);
            }

            if (entity.Get2D() != null)
            {
                entity.Get2D().SetFacingDirection(vars.FacingDirection);
                entity.Get2D().SetWalkingDirection(vars.WalkingDirection);
                vars.Locomotion.Vars = this.vars;
                allowed = true;
            }

            if (entity.GetAI() != null)
            {
                entity.GetAI().SetDirection(vars.Direction);
            }
  
      
        }

        public void Exit()
        {

        }

        public void Tick()
        {
            if (allowed == false) return;
            vars.Locomotion.DoLocomotion(entity, Time.deltaTime);
        }
    }
   
}