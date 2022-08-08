using UnityEngine;
using GWLPXL.ARPGCore.AI.com;
using System.Collections.Generic;
using GWLPXL.ARPGCore.Abilities.com;

namespace GWLPXL.ARPGCore.States.com
{
    public abstract class AIStateSO : ScriptableObject
    {
        public string StateKey = string.Empty; 
        [System.NonSerialized]
        protected Dictionary<IAIEntity, IState> stateDic = new Dictionary<IAIEntity, IState>();        
        /// <summary>
        /// Write the transition in a true and false fashion
        /// </summary>
        /// <param name="forEntity"></param>
        /// <returns></returns>
        public virtual bool GetTransition(IAIEntity forEntity)
        {
            return forEntity.GetStateKey() == StateKey;
        }
        /// <summary>
        /// returns the current state for the entity
        /// </summary>
        /// <param name="forEntity"></param>
        /// <returns></returns>
        public virtual IState GetState(IAIEntity forEntity)
        {
            return stateDic[forEntity];
        }
        /// <summary>
        /// the setup method, run this first
        /// </summary>
        /// <param name="onMachine"></param>
        /// <param name="forEntity"></param>
        public abstract void SetState(IStateMachine onMachine, IAIEntity forEntity);
       
    }
}

namespace GWLPXL.ARPGCore.States.com
{

    [System.Serializable]
    public class MoveVars
    {
        public string AnimatorStateName = string.Empty;
        public float BlendDuration = .02f;
        public int Layer = 0;
    }
    [System.Serializable]
    public class MoveVars2D
    {
        public string AnimatorStateName = string.Empty;
        public GlobalMoveDirection WalkingDirection = GlobalMoveDirection.Down;
        public GlobalFacingDirection FacingDirection = GlobalFacingDirection.Down;
        public Vector3 Direction = new Vector3(0, -1, 0);
        public float Speed = 1;
        public Movement2D Locomotion;
    }

    [System.Serializable]
    public abstract class PlayerState : ScriptableObject
    {
        [System.NonSerialized]
        protected Dictionary<IStateMachineEntity, IState> stateDic = new Dictionary<IStateMachineEntity, IState>();
        public abstract bool GetTransition(IStateMachineEntity forEntity);

        /// <summary>
        /// returns the current state for the entity
        /// </summary>
        /// <param name="forEntity"></param>
        /// <returns></returns>
        public virtual IState GetState(IStateMachineEntity forEntity)
        {
            return stateDic[forEntity];
        }
        /// <summary>
        /// the setup method, run this first
        /// </summary>
        /// <param name="onMachine"></param>
        /// <param name="forEntity"></param>
        public abstract void SetState(IStateMachine onMachine, IStateMachineEntity forEntity);
    }

    [System.Serializable]
    public abstract class PlayerMovementState2D : PlayerState
    {
        public MoveVars2D Vars;


    }

    [System.Serializable]
    public abstract class PlayerMovementState : PlayerState
    {
        public MoveVars Vars;


    }
    
    [System.Serializable]
    public abstract class EnemyMovementState2D : PlayerState
    {
        public MoveVars2D Vars;

    }
    [System.Serializable]
    public class Ability2DVars
    {
        public AbilityAnimatorState2D[] AnimatorStates = new AbilityAnimatorState2D[0];
        public Ability Ability;
    }

    public abstract class PlayerAbilityState2D : PlayerState
    {
        public Ability2DVars AbilityVars;

        public override bool GetTransition(IStateMachineEntity forEntity)
        {
            return forEntity.GetActorHub().MyAbilities.GetRuntimeController().GetAbilityActive(AbilityVars.Ability);

        }

       
    }
}