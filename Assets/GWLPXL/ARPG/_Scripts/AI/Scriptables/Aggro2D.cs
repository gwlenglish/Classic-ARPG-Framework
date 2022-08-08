using GWLPXL.ARPGCore.AI.com;
using GWLPXL.ARPGCore.com;
using System;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/AI/Aggro2D")]

    public class Aggro2D : AIStateSO
    {
        public AggroVars2D Vars;

        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {
            GenericAggroState2D state = new GenericAggroState2D(forEntity, Vars);
            Func<bool> Condition() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, Condition());
            stateDic.Add(forEntity, state);

        }


    }


    [System.Serializable]
    public class AggroVars2D
    {
        public Ability2DVars Ability;
        public GenericAnimate2D[] ChaseStates = new GenericAnimate2D[0];
        public float ChaseSpeed = 3;
    }

    public class GenericAggroState2D : IState
    {
        IAIEntity Entity;
        AggroVars2D vars;
        public GenericAggroState2D(IAIEntity entity, AggroVars2D vars)
        {
            this.Entity = entity;
            this.vars = vars;
        }
        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Tick()
        {
            if (Entity.GetAttackTarget() == null)
            {
                Debug.LogWarning("In Aggro state but no attack target is assigned");
                return;
            }

            if (Entity.GetActorHub().MyAbilities.GetRuntimeController().GetAbilityActive(vars.Ability.Ability)) return;


            Vector3 direction = Entity.GetAttackTarget().transform.position - Entity.GetActorHub().MyTransform.position;
            float sqrdmag = direction.sqrMagnitude;
            direction.Normalize();
            float sensitivity = .1f;
            if (Mathf.Abs(direction.y) < sensitivity)
            {
                direction.y = 0;
            }
            if (Mathf.Abs(direction.x) < sensitivity)
            {
                direction.x = 0;
            }
            Entity.GetActorHub().MyStateMachine.Get2D().SetFacingDirection(direction);
            Entity.GetActorHub().MyStateMachine.Get2D().SetWalkingDirection(direction);
            AnimatorStateInfo statinfo = Entity.GetActorHub().MyAnimator.GetCurrentAnimatorStateInfo(0);

            //moving
            Entity.GetActorHub().MyMover.SetDesiredDestination(Entity.GetActorHub().MyTransform.position + direction * Time.deltaTime * vars.ChaseSpeed, vars.Ability.Ability.GetRangeWithBuffer());

            if (sqrdmag < vars.Ability.Ability.GetRangeSquaredWithBuffer() && // range
                vars.Ability.Ability.HasSight(Entity.GetActorHub(), Entity.GetAttackTarget().transform, EditorPhysicsType.Unity2D) == true)// sight
            {

                //ability activate
                bool success = Entity.GetActorHub().MyAbilities.TryCastAbility(vars.Ability.Ability);
                if (success)
                {
                    Entity.SetActiveAbility(vars.Ability.Ability);
                    for (int i = 0; i < vars.Ability.AnimatorStates.Length; i++)
                    {
                        if (vars.Ability.AnimatorStates[i].FacingDirection == Entity.GetActorHub().MyStateMachine.Get2D().GetFacingDirection())
                        {
                            Entity.GetActorHub().MyAnimator.Play(vars.Ability.AnimatorStates[i].AnimatorStateName, 0, 0);
                            break;
                        }
                    }

                }

               
            }
            else
            {

                for (int i = 0; i < vars.ChaseStates.Length; i++)
                {
                    if (vars.ChaseStates[i].MovementDirection == Entity.GetActorHub().MyStateMachine.Get2D().GetWalkingDirection())
                    {
                        if (statinfo.IsName(vars.ChaseStates[i].AnimatorStateName) == false)
                        {
                            Entity.GetActorHub().MyAnimator.Play(vars.ChaseStates[i].AnimatorStateName, 0, 0);
                            break;
                        }




                    }
                }

    
            }
        }
    }
}