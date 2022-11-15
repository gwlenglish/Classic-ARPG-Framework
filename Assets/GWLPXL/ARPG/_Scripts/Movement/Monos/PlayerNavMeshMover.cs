


using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;

using GWLPXL.ARPGCore.States.com;

using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.Movement.com
{
   

    public class PlayerNavMeshMover : MonoBehaviour, IChangeStates, IMover, ITick, INavMeshMover
    {
        public GameObject PlayerHub;
        public MouseNavMeshVars MoveVars;

        #region fields
        IStateMachine locoMachine;
        float originalSpeed = 1;
        float originalAccel = 1;
        float speedMulti = 1;
        float baseheight = 0;
        #endregion

        #region states
        // MouseMove mouseMove;
        MouseMoveAbility mouseMove;
        AgentDisabledAttack agentDisabledAttack;
        AgentDisabledUI agentDisabledUI;
        PlayerHurt hurt;
        DeadState playerDead;
        #endregion
        IPlayerCanvasHub canvashub = null;
        IActorHub hub = null;
        bool moveEnabled;
        #region unity calls
        void Awake()
        {
            baseheight = MoveVars.Agent.baseOffset;
            originalAccel = MoveVars.Agent.acceleration;
            originalSpeed = MoveVars.Agent.speed;
            canvashub = PlayerHub.GetComponent<IPlayerCanvasHub>();
        }

        void Start()
        {
            DisableMovement(false);
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }



       
        #endregion



        #region public
        public NavMeshAgent GetAgent() => MoveVars.Agent;

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);

        }

        bool GetInventoryToggle()
        {
            for (int i = 0; i < canvashub.CanvasLinks.Length; i++)
            {
                bool canvasOpen = canvashub.CanvasLinks[i].GetCanvasEnabled();
                bool stopMovement = canvashub.CanvasLinks[i].GetFreezeMover();
                if (canvasOpen && stopMovement)
                {
                    //Debug.Log("Cants open " + canvasOpen);
                    return true;
                }
            }
            // Debug.Log("Cants open false ");

            return false;
        }

        /// <summary>
        /// sets up a statemachine, it's okay but there's much better controllers out there. 
        /// </summary>
        public void SetUpMover()
        {
            AddTicker();
            locoMachine = new IStateMachine();
            IActorHub hub = PlayerHub.GetComponent<IActorHub>();
            //auto configure
            if (MoveVars.GraphicRayers.Length == 0)
            {
                GraphicRaycaster gcaster = DungeonMaster.Instance.GetDungeonUISceneRef().GetGraphicRaycaster();
                if (gcaster != null)
                {
                    MoveVars.GraphicRayers = new GraphicRaycaster[1] { gcaster };
                }
            }
            if (MoveVars.EventSystem == null)
            {
                MoveVars.EventSystem = FindObjectOfType<EventSystem>();
            }
            //
            mouseMove = new MouseMoveAbility(hub, MoveVars, Camera.main);
            agentDisabledAttack = new AgentDisabledAttack(hub, hub.MyAbilities);
            hurt = new PlayerHurt(hub);
            playerDead = new DeadState(hub);
            agentDisabledUI = new AgentDisabledUI(hub);
            //state machine isnt working so great, maybe reconsider for player
            Func<bool> IsDead() => () => hub.MyHealth.IsDead();
            Func<bool> IsHurt() => () => hub.MyHealth.IsHurt() == true;
            Func<bool> UsingAbility() => () => hub.MyAbilities.GetRuntimeController().IsUsingAbility() == true;
            Func<bool> CanMove() => () => (hub.MyHealth.IsDead() == false && hub.MyAbilities.GetInCooldown() == false && GetInventoryToggle() == false);
            Func<bool> UIOpen() => () => (GetInventoryToggle() == true);
            Func<bool> InCooldown() => () => hub.MyAbilities.GetInCooldown() == true;

            locoMachine.AddAnyTransition(mouseMove, CanMove());

            At(mouseMove, agentDisabledAttack, InCooldown());
            At(mouseMove, agentDisabledAttack, UsingAbility());
            At(mouseMove, agentDisabledUI, UIOpen());
            At(mouseMove, hurt, IsHurt());

            At(agentDisabledAttack, mouseMove, CanMove());
            At(agentDisabledUI, mouseMove, CanMove());

            At(hurt, mouseMove, CanMove());
 

            locoMachine.AddAnyTransition(agentDisabledAttack, UsingAbility());
            locoMachine.AddAnyTransition(playerDead, IsDead());

            ChangeState(mouseMove);

            void At(IState to, IState from, Func<bool> condition)
            {
                locoMachine.AddTransition(to, from, condition);
            }
        }



        public void DoTick()
        {

            locoMachine.Tick();
            Debug.Log(locoMachine.GetCurrentlyRunnnig());
            if (MoveVars.Agent == null) return;

            Vector3 worldDeltaPosition = MoveVars.Agent.nextPosition - transform.position;
            if (worldDeltaPosition.magnitude > MoveVars.Agent.radius)
            {
                // transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
                MoveVars.Agent.nextPosition = transform.position + .9f * worldDeltaPosition;
            }


        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);

        }

        public void SetDesiredDestination(Vector3 newDestination, float stoppingDistance)
        {
            if (GetAgent().isActiveAndEnabled == false) return;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(newDestination, out navHit, stoppingDistance, NavMesh.AllAreas))
            {
                newDestination = navHit.position;
                //we need to check for our rotation. Using NavAgent rotatation makes it like we're steering a car.
                SetDesiredRotation(newDestination, stoppingDistance);
            
                //NavMeshAgent calls
                MoveVars.Agent.stoppingDistance = stoppingDistance;
                MoveVars.Agent.SetDestination(newDestination);

            }
            else
            {
                ARPGDebugger.DebugMessage("No suitable NavMesh Path Found", this);
            }
        }

        public void SetDesiredRotation(Vector3 towards, float stoppingDistance)
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


        public void StopAgentMovement(bool isStopped)
        {
            if (MoveVars.Agent.isActiveAndEnabled == false) return;
            MoveVars.Agent.isStopped = isStopped;
            if (isStopped == true)
            {
               
                SetDesiredDestination(transform.position, 1f);
            }
            else
            {
                SetDesiredDestination(transform.position, 1f);
                //MoveVars.Agent.Warp(transform.position);
            }
         
        }
        public void ChangeState(IState newstate)
        {
            locoMachine.SetState(newstate);

        }

        public void SetNewSpeed(float newTopSpeed, float newAcceleration)
        {
            MoveVars.Agent.speed = newTopSpeed;
            MoveVars.Agent.acceleration = newAcceleration;
        }

        public void ResetSpeed()
        {
            SetNewSpeed(originalSpeed, originalAccel);
        }



        public void ResetState()
        {
            ChangeState(mouseMove);
            SetDesiredDestination(transform.position, 1f);
        }

        public void SetVelocity(Vector3 newVel)
        {
            MoveVars.Agent.velocity = newVel;
        }

        public void DefaultMoveState()
        {
            ChangeState(mouseMove);
        }
        public GameObject GetGameObject()
        {
            return transform.root.gameObject;
        }



        #endregion
        public IActorHub GetHub() => hub;



        public void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }

        public void DisableMovement(bool isStopped)
        {
            moveEnabled = isStopped;
            StopAgentMovement(isStopped);
        }

        public float GetVelocitySquaredMag()
        {
            return MoveVars.Agent.velocity.sqrMagnitude;
        }


        public void ModifySpeedMultiplier(float byAmount)
        {
            speedMulti += byAmount;
            MoveVars.Agent.speed = originalSpeed * speedMulti;
        }
        public bool GetMoverEnabled()
        {
            return enabled;
        }
        public void SetAgentBaseHeight(float newHeight)
        {
            MoveVars.Agent.baseOffset = newHeight;
        }
        public void ResetBaseHeight()
        {
            MoveVars.Agent.baseOffset = baseheight;
        }
        public void EnableUpdate(bool updatePosition, bool updateRotation)
        {
            MoveVars.Agent.updatePosition = updatePosition;
            MoveVars.Agent.updateRotation = updateRotation;
        }

        public void SetAgentPositionRotaion(Vector3 newpos, Quaternion newRot)
        {
            MoveVars.Agent.nextPosition = newpos;

        }

        public float GetSpeedMultiplier() => speedMulti;

        public Vector3 GetVelocityDirection() => GetAgent().velocity.normalized;




#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            float SphereCastRadius = .5f;//for gizmos
            Gizmos.DrawWireSphere(MoveVars.LastHit, SphereCastRadius);
            if (Application.isPlaying == false) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(MoveVars.Agent.destination, 1f);
        }


       













#endif


    }

    #region helper classes

    [System.Serializable]
    public class MouseNavMeshVars
    {
        [HideInInspector]
        public RaycastHit[] AttackableHit;
        [HideInInspector]
        public Vector3 LastHit;
        public NavMeshAgent Agent = null;
        [Header("GameObject Layers for Raycasts")]
        [Tooltip("The traversable layer")]
        public LayerMask Ground;
        [Tooltip("Click and interact, e.g. the loot")]
        public LayerMask Interactable;
        [Tooltip("Click and attack, e.g. Enemy")]
        public LayerMask Attackable;
        public int RayLength = 100;
        [Header("Blocking Canvases")]
        public GraphicRaycaster[] GraphicRayers = new GraphicRaycaster[0];
        public EventSystem EventSystem = null;
    }

    #endregion
}