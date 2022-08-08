using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.AI.com;
using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Movement.com
{

    /// <summary>
    /// Example of 2D use of IMover, some methods empty because there's no 2D pathfinding included in ARPG.
    /// </summary>
    public class EnemyTopDownMover2D : MonoBehaviour, IMover
    {
        public Rigidbody2D Body = null;
        public float Speed = 5;
        public bool RotateTowardsVelocity = false;
        public float RotateSpeed = 5;
        float originalSpeed = 0;
        float speedMulti = 1;
        float stoppingD = 1;
        float defaultSpeed;
        bool stopped;
        Vector2 target;
        bool active;
        IActorHub hub;
        Vector3 direction;

        private void Awake()
        {
            SetUpMover();
            originalSpeed = Speed;
        }
   
        public GameObject GetGameObject() => this.gameObject;


        public float GetTickDuration() => Time.deltaTime;


       

        public void ResetSpeed()
        {
            Speed = defaultSpeed;
        }

        public void ResetState()
        {
            
        }
        /// <summary>
        /// this is where you would set the intended destination in world coordinates, and then pass it over to the pathfinder. The 3d passes it to Unity's navmesh agent.
        /// </summary>
        /// <param name="newDestination"></param>
        /// <param name="stoppingDistance"></param>
        public void SetDesiredDestination(Vector3 newDestination, float stoppingDistance)
        {
            if (hub.MyHealth.IsDead())
            {
                Body.velocity = new Vector2(0, 0);
                return;
            }
          
            target = (Vector2)newDestination;
            stoppingD = stoppingDistance;
            Vector3 nextpos = newDestination - hub.MyTransform.position;
            float sqrdmag = nextpos.sqrMagnitude;
            if (sqrdmag > stoppingDistance * stoppingDistance)
            {
                Body.MovePosition(Vector3.MoveTowards(transform.position, newDestination, Speed * Time.deltaTime));
            }
       
            Debug.Log("Desired Ran");
        }

        public void SetDesiredRotation(Vector3 towards, float stoppingDistance)
        {
            
        }

        public void SetNewSpeed(float newTopSpeed, float newAcceleration)
        {
            Speed = newTopSpeed;
        }

        public void SetUpMover()
        {
            target = transform.position;
            defaultSpeed = Speed;
            Body.gravityScale = 0;

            Body.constraints = RigidbodyConstraints2D.FreezeRotation;
    
        }

        public void SetVelocity(Vector3 newVel)
        {
         //
        }

        public void StopAgentMovement(bool isStopped)
        {
           //
        }

        public IActorHub GetHub()
        {
            return hub;
        }

        public void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
            SetUpMover();
        }

        public void DisableMovement(bool isStopped)
        {
            stopped = isStopped;
        }

        public float GetVelocitySquaredMag()
        {
            return Body.velocity.sqrMagnitude;
        }

        public void ModifySpeedMultiplier(float byAmount)
        {
            speedMulti += byAmount;
            Speed = ( originalSpeed * speedMulti);
        }

        public bool GetMoverEnabled()
        {
            return active;
        }

        public float GetSpeedMultiplier() => speedMulti;

        public Vector3 GetVelocityDirection() => direction;
       
    }
}