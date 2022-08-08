using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.Movement.com
{


    public class PlayerTopDown2DMover : MonoBehaviour, ITick, IMover
    {
        public float Speed = 5;
        public bool RotateTowardsVelocity = false;
        public float RotateSpeed = 5;
        protected float speedMulti = 1;
        protected float defaultSpeed;
        protected IPlayerMovementInput input;
        protected Rigidbody2D rb2d;
        protected bool stopped;
        protected Vector2 newPos = Vector2.zero;
        protected IActorHub hub;
        protected bool moverenabled;
        protected virtual void Awake()
        {
            defaultSpeed = Speed;
        }
        public void AddTicker() => TickManager.Instance.AddTicker(this);
        

        public virtual void DoTick()
        {
            if (stopped || hub.MyHealth.IsDead()) return;
            float x = hub.InputHub.MoveInputs.GetHorizontalRaw();
            float y = hub.InputHub.MoveInputs.GetVerticalRaw();
            hub.MyStateMachine.Get2D().SetWalkingDirection(new Vector3(x, y, 0));
            hub.MyStateMachine.Get2D().SetFacingDirection(new Vector3(x, y, 0));
            //SetVelocity(new Vector2(x, y));
            Vector2 dir2 = new Vector2(x, y).normalized;
            newPos = Vector3.Lerp((Vector2)transform.position, (Vector2)transform.position + dir2 * Speed, GetTickDuration());
            SetDesiredDestination(newPos, 1);

            if (RotateTowardsVelocity && Mathf.Abs(x) > 0 || RotateTowardsVelocity && Mathf.Abs(y) > 0)
            {
                Vector3 targetPosition = transform.position + new Vector3(x, y) * Speed * GetTickDuration();
                Vector3 dir = targetPosition - this.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);//assumes rigth is the forward
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * GetTickDuration());
            }


        }


        public virtual float GetTickDuration() => Time.deltaTime;

        public virtual void RemoveTicker() => TickManager.Instance.RemoveTicker(this);

        public virtual void SetUpMover()
        {
            rb2d = GetComponent<Rigidbody2D>();
            if (rb2d == null)
            {
                rb2d = hub.MyTransform.GetComponent<Rigidbody2D>();
           

            }
            rb2d.gravityScale = 0;
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            input = GetComponent<IPlayerMovementInput>();

            newPos = transform.position;
            AddTicker();
        }

        public virtual GameObject GetGameObject() => this.gameObject;
       

        public virtual void SetVelocity(Vector3 newVel)
        {
           
        }

        public virtual void ResetState()
        {
            
        }

        public virtual void SetDesiredDestination(Vector3 newDestination, float stoppingDistance)
        {
            rb2d.MovePosition(newDestination);
        }

        public virtual void SetDesiredRotation(Vector3 towards, float stoppingDistance)
        {
          
        }

      
        public virtual void SetNewSpeed(float newTopSpeed, float newAcceleration)
        {
            Speed = newTopSpeed;
        }

        public virtual void ResetSpeed()
        {
            Speed = defaultSpeed;
        }

        protected virtual void OnDestroy()
        {
            RemoveTicker();
        }

        public IActorHub GetHub()
        {
            return hub;
        }

        public virtual void SetActorHub(IActorHub newHub)
        {
            hub = newHub;
        }

        public virtual void DisableMovement(bool isStopped)
        {
            moverenabled = isStopped;

        }

        public virtual float GetVelocitySquaredMag()
        {
            return newPos.sqrMagnitude;
            Debug.Log(rb2d.velocity.sqrMagnitude);
            return rb2d.velocity.sqrMagnitude;
        }

        public virtual void ModifySpeedMultiplier(float byAmount)
        {
            speedMulti += byAmount;
            Speed = defaultSpeed * speedMulti;
        }

        public virtual bool GetMoverEnabled()
        {
            return moverenabled;
        }

        public virtual float GetSpeedMultiplier() => speedMulti;

        public virtual Vector3 GetVelocityDirection() => rb2d.velocity.normalized;
       
    }
}