using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.AI.com;
using GWLPXL.ARPGCore.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/AI/Aggro")]


    public class Aggro : AIStateSO
    {
        public AggroVars Vars;
      
        public override void SetState(IStateMachine onMachine, IAIEntity forEntity)
        {
            GenericAggroState state = new GenericAggroState(forEntity, Vars);
            Func<bool> Condition() => () => this.GetTransition(forEntity);
            onMachine.AddAnyTransition(state, Condition());
            stateDic.Add(forEntity, state);
            
        }

       
    }

    [System.Serializable]
    public class AggroVars
    {
        public string AbilityStateName = string.Empty;//animator
        public float AbilityBlendDuration = .02f;
        public int AbilityAnimLayer = 0;
        public Ability Ability = null;
        public string ChaseStateName = "Locomotion";
        public float ChaseBlendDuration = .02f;
        public int ChaseLayer = 0;
        public string VelocityParam = "Velocity";
    }

   
    public class GenericAggroState : IState
    {
        public IAIEntity Entity;
        protected AggroVars vars;
        bool attacking = false;
        bool walking = false;
        public GenericAggroState(IAIEntity entity, AggroVars vars)
        {
            this.vars = vars;
            Entity = entity;
        }


        public void Enter()
        {

            walking = true;
            attacking = false;
        }

       
        public void Exit()
        {

            attacking = false;
            walking = false;


        }


        protected virtual void EndAbility(Ability ability)
        {
            if (ability == vars.Ability)
            {
                Entity.GetActorHub().MyAbilities.GetRuntimeController().OnAbilityEnd -= EndAbility;
                attacking = false;
                walking = true;

            }
        }

    
        public void Tick()
        {
            if (Entity.GetAttackTarget() == null)
            {
                Debug.LogWarning("In Aggro state but no attack target is assigned");
                return;
            }

            if (attacking) return;

            Vector3 diff = Entity.GetActorHub().MyTransform.position - Entity.GetAttackTarget().transform.position;
   

            if (walking)
            {
                Entity.GetActorHub().MyAnim.SetFloatParam(vars.VelocityParam, 1);
                Entity.GetActorHub().MyMover.SetDesiredDestination(Entity.GetAttackTarget().transform.position, vars.Ability.GetRangeWithBuffer());
                Entity.GetActorHub().MyAnim.SetAnimatorState(vars.ChaseStateName, vars.ChaseBlendDuration, vars.ChaseLayer);

                float dst = Vector3.Distance(Entity.GetActorHub().MyTransform.position, Entity.GetAttackTarget().transform.position);
                if (dst <= vars.Ability.GetRange())
                {
                    if (vars.Ability.HasSight(Entity.GetActorHub(), Entity.GetAttackTarget().transform, EditorPhysicsType.Unity3D) == false)
                    {
                        Entity.GetActorHub().MyMover.SetDesiredRotation(Entity.GetAttackTarget().transform.position, 1);
                    }
                    attacking = Entity.GetActorHub().MyAbilities.TryCastAbility(vars.Ability);
                    if (attacking)
                    {
                        Entity.GetActorHub().MyAnim.SetFloatParam(vars.VelocityParam, 0);
                        walking = false;
                        Entity.GetActorHub().MyAnim.SetAnimatorState(vars.AbilityStateName, vars.AbilityBlendDuration, vars.AbilityAnimLayer);//used for nonlooping animation to restart the animation from the beginning
                        Entity.GetActorHub().MyAbilities.GetRuntimeController().OnAbilityEnd += EndAbility;
                        return;
                    }
                }
            }
          
           
        
          


           
            
           
        }
    }
}