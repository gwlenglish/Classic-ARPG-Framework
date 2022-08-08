using GWLPXL.ARPGCore.Abilities.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{
    /// <summary>
    /// use this example to convert movement to one SO and same with Idle.
    /// </summary>

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Dash")]

    public class Dash2D : PlayerAbilityState2D
    {
        public DashVars Vars;
        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            return forEntity.GetActorHub().MyAbilities.GetRuntimeController().GetAbilityActive(Vars.DashAbility);

        }

        public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
        {
            ChargeState state = new ChargeState(forEntity, Vars);
            Func<bool> PerformCharge() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, PerformCharge());
            stateDic.Add(forEntity, state);
        }
    }

    public enum LocalActorDirection
    {
        Facing = 0,
        Right = 1,
        Left = 2,
        Behind = 3,

    }

    [System.Serializable]
    public class AbilityAnimatorState2D
    {
        public string AnimatorStateName = "Null";
        public int Layer = 0;
        [Tooltip("Global facing direction")]
        public GlobalFacingDirection FacingDirection = GlobalFacingDirection.Down;
    }

    [System.Serializable]
    public class DashVars
    {
        public AbilityAnimatorState2D[] AnimatorStates = new AbilityAnimatorState2D[0];
        public Ability DashAbility;
        [Tooltip("If true, will match to the Dash Ability. If false, will use the Duration inserted here.")]
        public bool MatchDurationToCooldown = true;
        public float Distance = 5;
        public float Duration = 1;
        [Tooltip("Relative to Facing. So Right will get the caster's face forward direction and rotate it 90 degrees to the right.")]
        public LocalActorDirection DashDirection = LocalActorDirection.Facing;
        [Tooltip("The Angle Axis which the character rotates. For 2D, this will generally be (0, 0, 1). For 3D, (0, 1, 0)")]
        public Vector3 RotateVector = new Vector3(0, 1, 0);
        public Dash DashBehavior = null;
    }

    public class ChargeState : IState
    {
        IStateMachineEntity entity;
        DashVars vars;

        Vector3 goal;
        Vector3 start;

        readonly int rightangle = -90;
        readonly int leftangle = 90;
        readonly int behindangle = 180;
        public ChargeState(IStateMachineEntity entity, DashVars vars)
        {
            this.entity = entity;
            this.vars = vars;
        }

     
        public void Enter()
        {
            Vector3 dir = new Vector3(0, 0, 0);
            switch (vars.DashDirection)
            {
                case LocalActorDirection.Facing:
                    dir =  entity.Get2D().GetFacingVector();
                    break;
                case LocalActorDirection.Right:
                    dir = Quaternion.AngleAxis(rightangle, vars.RotateVector) * entity.Get2D().GetFacingVector();
                    break;
                case LocalActorDirection.Left:
                    dir = Quaternion.AngleAxis(leftangle, vars.RotateVector) * entity.Get2D().GetFacingVector();
                    break;
                case LocalActorDirection.Behind:
                    dir = Quaternion.AngleAxis(behindangle, vars.RotateVector) * entity.Get2D().GetFacingVector();
                    break;
            }

            goal = entity.GetActorHub().MyTransform.position + (dir * vars.Distance);
            start = entity.GetActorHub().MyTransform.position;
            if (vars.MatchDurationToCooldown)
            {
                vars.DashBehavior.Vars.Duration = vars.DashAbility.Delay + vars.DashAbility.Duration;
            }
            else
            {
                vars.DashBehavior.Vars.Duration = vars.Duration;
            }

            vars.DashBehavior.Vars.Start = start;
            vars.DashBehavior.Vars.End = goal;
            vars.DashBehavior.Vars.Timer = 0;
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