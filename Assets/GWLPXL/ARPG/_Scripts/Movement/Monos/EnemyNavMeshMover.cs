
using GWLPXL.ARPGCore.com;

using GWLPXL.ARPGCore.DebugHelpers.com;

using GWLPXL.ARPGCore.States.com;


using UnityEngine;
using UnityEngine.AI;

namespace GWLPXL.ARPGCore.Movement.com
{
    


    //this seems like too much. separate out the ieenemyai and states.
    public class EnemyNavMeshMover : MonoBehaviour, IMover, IChangeStates, ITick, INavMeshMover
    {
        public NavMeshAgent Agent;
        protected float speedMulit = 1;
        protected float distanceToPlayerSquared;
        protected Vector3 startingPosition;
        protected float originalSpeed;
        protected float originalAccel;
        protected float baseheight = 0;
        protected IActorHub hub;


        protected virtual void Awake()
        {
            if (Agent == null)
            {
                Agent = hub.NavMeshAgent.GetAgent();
            }
            baseheight = Agent.baseOffset;
            originalAccel = Agent.acceleration;
            originalSpeed = Agent.speed;


        }
        protected virtual void OnDestroy()
        {
            //RemoveTicker();
        }

        protected virtual void Start()
        {
           
            SetUpMover();
            hub.MyAbilities.SetIntendedAbility(0);
        }

 

     

        public virtual void SetDesiredDestination(Vector3 newDestination, float stoppingDistance)
        {
            if (Agent.isActiveAndEnabled)
            {
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(newDestination, out navHit, stoppingDistance, NavMesh.AllAreas))
                {
                    newDestination = navHit.position;
                    //we need to check for our rotation. Using NavAgent rotatation makes it like we're steering a car.
                    SetDesiredRotation(newDestination, stoppingDistance);

                    //NavMeshAgent calls
                    Agent.stoppingDistance = stoppingDistance;
                    Agent.SetDestination(newDestination);

                }
                else
                {
                    ARPGDebugger.DebugMessage("No suitable NavMesh Path Found", this);
                }
                //Debug.Log("Desired called");

                //Agent.stoppingDistance = stoppingDistance;
                //Agent.SetDestination(newDestination);
            }

        }
        public virtual void SetDesiredRotation(Vector3 towards, float stoppingDistance)
        {
            Vector3 lookPositon = towards - transform.position;
            float sqrdMag = lookPositon.sqrMagnitude;
            if (sqrdMag > stoppingDistance * stoppingDistance)//this check is to prevent look rotations of Vector3.zero (if the player's position is on top of the destination). I use stopping distance just because it's available, 1f hard code works fine too.
            {
                //does a bit of a tilt
                lookPositon.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPositon);
            }
        }
        public virtual void StopAgentMovement(bool isStopped)
        {
            if (Agent.isActiveAndEnabled)
            {
                Agent.isStopped = isStopped;
                Agent.velocity = Vector3.zero;
            }



        }


        public virtual void DisableNavMeshAgent()
        {
            Agent.enabled = false;
        }


        public virtual void SetNewSpeed(float newTopSpeed, float newAcceleration)
        {
            Agent.speed = newTopSpeed;
            Agent.acceleration = newAcceleration;
        }

        public virtual void ResetSpeed()
        {
            SetNewSpeed(originalSpeed, originalAccel);
        }

        [System.Obsolete("Deprecated, use the state machine.")]
        public virtual void ResetState()
        {
           
        }

        public virtual void SetVelocity(Vector3 newVel)
        {
            Agent.velocity = newVel;
        }

        [System.Obsolete("Deprecated, use the state machine.")]
        public virtual void ChangeState(IState newstate)
        {
           


        }
        [System.Obsolete("Deprecated, use the state machine.")]
        public virtual void DefaultMoveState()
        {

        }

        public virtual GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public virtual void AddTicker()
        {
            TickManager.Instance.AddTicker(this);

        }
        [System.Obsolete("Deprecated, use the state machine.")]
        public virtual void DoTick()
        {
          

        }
        [System.Obsolete("Deprecated, use the state machine.")]
        public virtual void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);

        }

        public virtual float GetTickDuration()
        {
            return Time.deltaTime;
        }

        public virtual void SetUpMover()
        {
            
            AddTicker();
            startingPosition = transform.position;

         
        }

      
        public virtual IActorHub GetHub() => hub;
      

        public virtual void SetActorHub(IActorHub newHub) => hub = newHub;
       
        public virtual void DisableMovement(bool isStopped)
        {
            StopAgentMovement(isStopped);
        }

        public virtual float GetVelocitySquaredMag()
        {
            return Agent.velocity.sqrMagnitude;
        }

        public virtual void ModifySpeedMultiplier(float byAmount)
        {
            speedMulit += byAmount;
            Agent.speed = (originalSpeed * speedMulit);
            Agent.acceleration = (originalAccel* speedMulit);
        }

        public virtual bool GetMoverEnabled()
        {
            return Agent.isActiveAndEnabled;
        }

        public virtual NavMeshAgent GetAgent()
        {
            return Agent;
        }

        public virtual void SetAgentBaseHeight(float newheight)
        {
            Agent.baseOffset = newheight;
            Debug.Log("Base height " + Agent.baseOffset);
        }

        public virtual void ResetBaseHeight()
        {
            Agent.baseOffset = baseheight;
        }

        public virtual void EnableUpdate(bool updatePosition, bool updateRotation)
        {
            Agent.updatePosition = updatePosition;
            Agent.updateRotation = updateRotation;
        }

        public virtual void SetAgentPositionRotaion(Vector3 newpos, Quaternion newRot)
        {
            Agent.nextPosition = newpos;
        }

        public virtual float GetSpeedMultiplier() => speedMulit;

        public virtual Vector3 GetVelocityDirection() => GetAgent().velocity.normalized;
       
    }
}