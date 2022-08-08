using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.PlayerInput.com;
using System;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.AI.com;

namespace GWLPXL.ARPGCore.States.com
{
    public enum GlobalFacingDirection
    {
        Down = 0,
        DownLeft = 1,
        Left = 2,
        UpLeft = 3,
        Up = 4, 
        UpRight = 5,
        Right = 6,
        DownRight = 7
    }

    public enum GlobalMoveDirection
    {
        Down = 0,
        DownLeft = 1,
        Left = 2,
        UpLeft = 3,
        Up = 4,
        UpRight = 5,
        Right = 6,
        DownRight = 7
    }

    public interface I2DStateMachine
    {
        Rigidbody2D GetRigidbody();
        void SetFacingDirection(GlobalFacingDirection newDirection);
        void SetFacingDirection(Vector3 raw);
        void SetWalkingDirection(GlobalMoveDirection newDirection);
        void SetWalkingDirection(Vector3 raw);
        GlobalMoveDirection GetWalkingDirection();
        Vector3 GetFacingVector();
        GlobalFacingDirection GetFacingDirection();
    }
     public interface IStateMachineEntity
    {
        I2DStateMachine Get2D();
        IActorHub GetActorHub();
        void SetActorHub(IActorHub newHub);
        IAIEntity GetAI();


    }

    public class ActorStateMachine2D : MonoBehaviour, IStateMachineEntity, ITick, I2DStateMachine
    {
        public GameObject PlayerHub = null;
        [Tooltip("These have priority over movement states.")]
        public PlayerAbilityState2D[] AbilityStates2D = new PlayerAbilityState2D[0];

        public GenericAnimate2DSO WalkStates;
        public GenericAnimate2DSO IdleStates;
        public GenericAnimate2DSO DeathStates;
        #region
        IStateMachine machine = null;

        GlobalFacingDirection facing = GlobalFacingDirection.Down;
        GlobalMoveDirection walkDirection = GlobalMoveDirection.Down;

        Rigidbody2D rb2d;
        IActorHub hub = null;
        I2DStateMachine state2d = null;
        bool walking;

        protected virtual void Awake()
        {
            state2d = this as I2DStateMachine;
            if (hub != null)
            {
                hub = PlayerHub.GetComponent<IActorHub>();
            }
            else
            {
                hub = GetComponent<IActorHub>();
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

            for (int i = 0; i < DeathStates.States.Length; i++)
            {
                GenericAnimate2D vars = DeathStates.States[i];
                GlobalFacingDirection direction = DeathStates.States[i].FacingDirection;
                Func<bool> condition() => () => Dead(direction);
                AnimateGeneric2D move2d = new AnimateGeneric2D(this, vars);
                machine.AddAnyTransition(move2d, condition());
            }


            //added based on priority. These have priority over walk and idle
            for (int i = 0; i < AbilityStates2D.Length; i++)
            {
                Ability2DVars vars = AbilityStates2D[i].AbilityVars;
                Func<bool> condition() => () => Ability(vars.Ability);
                AbilityAnimate ability2d = new AbilityAnimate(this, vars);
                machine.AddAnyTransition(ability2d, condition());
                //Abilities[i].SetState(machine, this);
            }

            for (int i = 0; i < WalkStates.States.Length; i++)
            {
                GenericAnimate2D vars = WalkStates.States[i];
                GlobalMoveDirection move = WalkStates.States[i].MovementDirection;
                Func<bool> condition() => () => Walking(move);
                AnimateGeneric2D move2d = new AnimateGeneric2D(this, vars);
                machine.AddAnyTransition(move2d, condition());

            }

            for (int i = 0; i < IdleStates.States.Length; i++)
            {
                GenericAnimate2D vars = IdleStates.States[i];
                GlobalFacingDirection direction = IdleStates.States[i].FacingDirection;
                Func<bool> condition() => () => Idling(direction);
                AnimateGeneric2D move2d = new AnimateGeneric2D(this, vars);
                machine.AddAnyTransition(move2d, condition());
            }

         
          
            AddTicker();

           
        }

        protected virtual bool Dead(GlobalFacingDirection direction)
        {
            return hub.MyHealth.IsDead() == true && hub.MyStateMachine.Get2D().GetFacingDirection() == direction;
        }

        protected virtual bool Ability(Ability ability)
        {
            
            return hub.MyAbilities.GetRuntimeController().GetAbilityActive(ability);
        }
        protected virtual bool Walking(GlobalMoveDirection movement)
        {

            return walking == true && hub.MyStateMachine.Get2D().GetWalkingDirection() == movement;
        }
        protected virtual  bool Idling(GlobalFacingDirection facing)
        {
            return walking == false && hub.MyStateMachine.Get2D().GetFacingDirection() == facing;
        }


        protected virtual void OnDestroy()
        {
            RemoveTicker();
        }

        public void SetFacingDirection(GlobalFacingDirection direction) => facing = direction;
     

      
 
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            machine.Tick();
            Debug.Log(machine.GetCurrentlyRunnnig());
        }

      

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public float GetTickDuration() => Time.deltaTime;
     

        public Rigidbody2D GetRigidbody() => rb2d;

        public Animator GetAnimator() => hub.MyAnimator;

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

        public IAIEntity GetAI() => null;

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
                walking = true;
            }
            else
            {
                walking = false;
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