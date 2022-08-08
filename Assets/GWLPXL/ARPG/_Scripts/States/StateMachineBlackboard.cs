using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.AI.com
{


    public class StateMachineBlackboard : MonoBehaviour, IAIEntity
    {
        public GameObject ActorHub = null;
        public Vector3 FacingDirection = new Vector3(0, 0, 0);
        [Tooltip("When name of ActiveState matches a key on the Enemy State Machine, it tries to switch states.")]
        public string ActiveState = "Idle";
        [Header("Attacking")]
        [Tooltip("Target to attack")]
        public GameObject AttackTarget;
        [Tooltip("What ability to use.")]
        public Ability ActiveAbility;
        [Header("Moving")]
        [Tooltip("Where to move, e.g. waypoint")]
        public GameObject MoveTarget;
        [Tooltip("If no ability, need a default stopping distance away from the move target. This is used.")]
        public float DefaultStoppingDistance = 3;

        IActorHub hub = null;
        protected virtual void Awake()
        {
            if (ActorHub == null) GetComponent<IActorHub>();
            if (ActorHub != null) hub = ActorHub.GetComponent<IActorHub>();

           
        }

     
        public virtual GameObject GetMoveTarget() => MoveTarget;

        public virtual bool HasMoveTarget() => MoveTarget != null;

        public virtual IActorHub GetActorHub() => hub;

        public virtual GameObject GetAttackTarget() => AttackTarget;

        public virtual void SetMoveTarget(GameObject newTarget) => MoveTarget = newTarget;

        public virtual void SetAttackTarget(GameObject newTarget) => AttackTarget = newTarget;

        public virtual float GetIdleDistance()
        {
            if (ActiveAbility != null)
            {
                return ActiveAbility.GetRangeWithBuffer();
            }
            else
            {
                return DefaultStoppingDistance;
            }
        }

        public virtual bool GetActiveAbilityInUse() 
        {
            if (ActiveAbility == null) return false;
            return hub.MyAbilities.GetRuntimeController().GetAbilityActive(ActiveAbility);
        }

        public virtual string GetStateKey() => ActiveState;

        public virtual void SetStateKey(string newState) => ActiveState = newState;

        public virtual void SetActiveAbility(Ability newActive) => ActiveAbility = newActive;

        public virtual Vector3 GetDirection() => FacingDirection;

        public virtual void SetDirection(Vector3 newDirection) => FacingDirection = newDirection;

        public virtual Ability GetActiveAbility() => ActiveAbility;
       
    }
}