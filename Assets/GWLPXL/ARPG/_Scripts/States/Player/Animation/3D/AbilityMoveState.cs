using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.States.com
{
    /// <summary>
    /// delete
    /// </summary>
  //  [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/3D/Generic Ability")]

    //public class AbilityMoveState : PlayerAbilityState
    //{
    //    public AbilityVars Vars;

    //    public override bool GetTransition(IStateMachineEntity forEntity)
    //    {
    //        return forEntity.GetActorHub().MyAbilities.GetRuntimeController().GetAbilityActive(Vars.Ability) && Vars.Ready;
    //    }

    //    public override void SetState(IStateMachine onMachine, IStateMachineEntity forEntity)
    //    {
    //        GenericAbilityMove state = new GenericAbilityMove(forEntity, Vars);
    //        Func<bool> Condition() => () => this.GetTransition(forEntity);
    //        onMachine.AddAnyTransition(state, Condition());
    //        stateDic.Add(forEntity, state);
    //    }

       
    //}


    [System.Serializable]
    public class AbilityVars
    {
        [HideInInspector]
        public bool Ready = true;

        [HideInInspector]
        public Vector3 TargetPos = Vector3.zero;
        public Ability Ability = null;
        public Locomotion Locomotion = null;
        public EditorPhysicsType Type = EditorPhysicsType.Unity3D;

    }

    public class GenericAbilityMove : IState
    {
        AbilityVars vars;
        IStateMachineEntity entity;
        public GenericAbilityMove(IStateMachineEntity forEntity, AbilityVars vars)
        {
            entity = forEntity;
            this.vars = vars;

        }
        public void Enter()
        {
            entity.GetActorHub().MyMover.DisableMovement(true);
            entity.GetActorHub().MyAnim.SetAnimatorState(vars.Ability.Data.AnimationTrigger, vars.Ability.Data.AnimBlending, vars.Ability.Data.AnimationIndex);

           
            entity.GetActorHub().MyAbilities.GetRuntimeController().OnAbilityEnd += ExitAbility;

           
        }

        void ExitAbility(Ability ability)
        {
            if (ability == vars.Ability)
            {
                entity.GetActorHub().MyAbilities.GetRuntimeController().OnAbilityEnd -= ExitAbility;
                vars.Ready = false;

            }
        }
        public void Exit()
        {
            entity.GetActorHub().MyMover.DisableMovement(false);
          
            vars.Ready = true;
        }

        public void Tick()
        {
            if (vars.Locomotion == null) return;
            vars.Locomotion.DoLocomotion(entity, Time.deltaTime);
        }
    }
}
