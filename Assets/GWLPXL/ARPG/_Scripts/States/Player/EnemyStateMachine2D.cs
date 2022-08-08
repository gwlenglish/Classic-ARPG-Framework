using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.PlayerInput.com;
using System;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.AI.com;
using GWLPXL.ARPGCore.Combat.com;

namespace GWLPXL.ARPGCore.States.com
{
  

    public class EnemyStateMachine2D : MonoBehaviour, IStateMachineEntity, ITick, I2DStateMachine
    {
        public GameObject ActorHub = null;
        [Tooltip("These have priority over movement states.")]
        public PlayerAbilityState2D[] Abilities = new PlayerAbilityState2D[0];
        public MovementStates2D MovementStates = null;
        [SerializeField]
        Animator animator;

        #region
        IStateMachine machine = null;

        GlobalFacingDirection facing = GlobalFacingDirection.Down;
        GlobalMoveDirection walkDirection = GlobalMoveDirection.Down;

        Rigidbody2D rb2d;
        IActorHub hub = null;
        I2DStateMachine state2d = null;
        IAIEntity ai = null;
        bool moving;
        protected virtual void Awake()
        {
            state2d = this as I2DStateMachine;
            if (ActorHub != null)
            {
                hub = ActorHub.GetComponent<IActorHub>();
            }
            else
            {
                hub = GetComponent<IActorHub>();
            }

            ai = GetComponent<IAIEntity>();

            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                {
                    animator = GetComponentInChildren<Animator>();
                }
            }
            machine = new IStateMachine();
            rb2d = GetComponent<Rigidbody2D>();
            if (rb2d == null)
            {
                rb2d = gameObject.AddComponent<Rigidbody2D>();
            }
            rb2d.gravityScale = 0;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        #endregion
        public IActorHub GetActorHub() => hub;
      
        protected virtual void Start()
        {
            machine = new IStateMachine();

            //added based on priority. These have priority over walk and idle
            for (int i = 0; i < Abilities.Length; i++)
            {
                Abilities[i].SetState(machine, this);
            }

            //these have priority over idle
            for (int i = 0; i < MovementStates.Moving.Length; i++)
            {
                MovementStates.Moving[i].SetState(machine, this);

            }

            for (int i = 0; i < MovementStates.Idle.Length; i++)
            {
                MovementStates.Idle[i].SetState(machine, this);
            }

            hub.MyHealth.OnDied += Died;

            AddTicker();

           
        }

        void Died(CombatResults results)
        {
            moving = false;

        }


        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (moving)
            {
                machine.Tick();
            }
           
            // Debug.Log(machine.GetCurrentlyRunnnig());
        }


        protected virtual void OnDestroy()
        {
            hub.MyHealth.OnDied -= Died;
            RemoveTicker();
        }

        public void SetFacingDirection(GlobalFacingDirection direction) => facing = direction;
     

      

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public float GetTickDuration() => Time.deltaTime;
     

        public Rigidbody2D GetRigidbody() => rb2d;

        public Animator GetAnimator() => animator;

        public GlobalFacingDirection GetFacingDirection() => facing;

        public IPlayerMovementInput GetInput() => hub.InputHub.MoveInputs;

        public void SetWalkingDirection(GlobalMoveDirection newDirection) => walkDirection = newDirection;

        public IAbilityUser GetAbilityUser() => hub.MyAbilities;

        public Transform GetTransform() => this.transform;

        public Vector3 GetFacingVector()
        {

           switch (facing)
            {
                case GlobalFacingDirection.Down:
                    return new Vector3(0, -1, 0);
                case GlobalFacingDirection.Up:
                    return new Vector3(0, 1, 0);
                case GlobalFacingDirection.Right:
                    return new Vector3(1, 0, 0);
                case GlobalFacingDirection.Left:
                    return new Vector3(-1, 0, 0);
                case GlobalFacingDirection.DownLeft:
                    return new Vector3(-1, -1, 0);
                case GlobalFacingDirection.DownRight:
                    return new Vector3(1, -1, 0);
                case GlobalFacingDirection.UpLeft:
                    return new Vector3(-1, 1, 0);
                case GlobalFacingDirection.UpRight:
                    return new Vector3(1, 1, 0);
            }
            return new Vector3(0, 0, 0);
        }

        public I2DStateMachine Get2D() => this as I2DStateMachine;
       

        public void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }

        public IAIEntity GetAI() => ai;

        public GlobalMoveDirection GetWalkingDirection() => walkDirection;

        public void SetWalkingDirection(Vector3 raw)
        {
            float x = raw.x;
            float y = raw.y;

            if (x == 0 && y > 0)
            {
                walkDirection = GlobalMoveDirection.Up;
            }
            else if (x == 0 && y < 0)
            {
                walkDirection = GlobalMoveDirection.Down;
            }
            else if (x > 0 && y == 0)
            {
                walkDirection = GlobalMoveDirection.Right;
            }
            else if (x < 0 && y == 0)
            {
                walkDirection = GlobalMoveDirection.Left;
            }
            else if (x < 0 && y < 0)
            {
                walkDirection = GlobalMoveDirection.DownLeft;
            }
            else if (x < 0 && y > 0)
            {
                walkDirection = GlobalMoveDirection.UpLeft;
            }
            else if (x > 0 && y < 0)
            {
                walkDirection = GlobalMoveDirection.DownRight;
            }
            else if (x > 0 && y > 0)
            {
                walkDirection = GlobalMoveDirection.UpRight;
            }

            if (Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0)
            {
                moving = true;
            }
            else
            {
                moving = false;
            }
        }

        public void SetFacingDirection(Vector3 raw)
        {
            float x = raw.x;
            float y = raw.y;

            if (x == 0 && y > 0)
            {
                facing = GlobalFacingDirection.Up;
            }
            else if (x == 0 && y < 0)
            {
                facing = GlobalFacingDirection.Down;
            }
            else if (x > 0 && y == 0)
            {
                facing = GlobalFacingDirection.Right;
            }
            else if (x < 0 && y == 0)
            {
                facing = GlobalFacingDirection.Left;
            }
            else if (x < 0 && y < 0)
            {
                facing = GlobalFacingDirection.DownLeft;
            }
            else if (x < 0 && y > 0)
            {
                facing = GlobalFacingDirection.UpLeft;
            }
            else if (x > 0 && y < 0)
            {
                facing = GlobalFacingDirection.DownRight;
            }
            else if (x > 0 && y > 0)
            {
                facing = GlobalFacingDirection.UpRight;
            }
        }
    }



   

}