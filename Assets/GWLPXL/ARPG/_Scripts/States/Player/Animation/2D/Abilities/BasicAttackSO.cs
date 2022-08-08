using GWLPXL.ARPGCore.Abilities.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/BasicAttack")]

    public class BasicAttackSO : PlayerAbilityState2D
    {
        public AttackVariables Vars;
        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            return forEntity.GetActorHub().MyAbilities.GetRuntimeController().GetAbilityActive(Vars.Ability);

        }

        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            GenericAttackState2D state = new GenericAttackState2D(forEntity, Vars);
            Func<bool> PerformCharge() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, PerformCharge());
            stateDic.Add(forEntity, state);
        }
    }

  


    [System.Serializable]
    public class AttackVariables
    {
        public AbilityAnimatorState2D[] AnimatorStates = new AbilityAnimatorState2D[0];
        public Ability Ability;
        [Tooltip("If true, will match to the Dash Ability. If false, will use the Duration inserted here.")]
        public bool MatchDurationToCooldown = true;
        public float Duration = 1;
        [Tooltip("Relative to Facing. So Right will get the caster's face forward direction and rotate it 90 degrees to the right.")]
        public LocalActorDirection Direction = LocalActorDirection.Facing;
        public Locomotion DashBehavior = null;
    }
    public class GenericAttackState2D : IState
    {
        IStateMachineEntity entity;
        AttackVariables vars;

        public GenericAttackState2D(IStateMachineEntity entity, AttackVariables vars)
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
                        animator.Play(vars.AnimatorStates[i].AnimatorStateName);
                        break;
                    }
                }
            }
            //determine start, stop
            //

        }

        public void Exit()
        {

        }

        public void Tick()
        {
            vars.DashBehavior.DoLocomotion(entity, Time.deltaTime);
        }
    }
}