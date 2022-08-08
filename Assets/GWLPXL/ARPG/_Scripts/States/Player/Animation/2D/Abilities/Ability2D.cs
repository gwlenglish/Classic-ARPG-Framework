using System;

using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Ability 2D Animate")]

    public class Ability2D : PlayerAbilityState2D
    {
        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            AbilityAnimate state = new AbilityAnimate(forEntity, AbilityVars);
            Func<bool> Condition() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, Condition());
            stateDic.Add(forEntity, state);
        }

       
    }



    public class AbilityAnimate : IState
    {
        IStateMachineEntity entity;
        Ability2DVars vars;


        public AbilityAnimate(IStateMachineEntity entity, Ability2DVars vars)
        {
            this.entity = entity;
            this.vars = vars;
        }


        public void Enter()
        {
            
            Animator animator = entity.GetActorHub().MyAnimator;
            if (animator != null)
            {
                GlobalFacingDirection globaldir = entity.Get2D().GetFacingDirection();
                for (int i = 0; i < vars.AnimatorStates.Length; i++)
                {
                    if (globaldir == vars.AnimatorStates[i].FacingDirection)
                    {
                        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(vars.AnimatorStates[i].Layer);
                        if (info.IsName(vars.AnimatorStates[i].AnimatorStateName) == false)
                        {
                            animator.Play(vars.AnimatorStates[i].AnimatorStateName);
                            break;
                        }
             
                    }
                }
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