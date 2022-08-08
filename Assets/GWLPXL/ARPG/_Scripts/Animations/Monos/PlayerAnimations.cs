
using GWLPXL.ARPGCore.com;

using System.Collections;
using UnityEngine;

namespace GWLPXL.ARPGCore.Animations.com
{
    /// <summary>
    /// deprecated unused methods
    /// use the main SetAnimatorState method now
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimations : MonoBehaviour, IAnimate, ITick
    {

        #region fields
        protected  Animator animator = null;
        protected bool delay = false;
        //[SerializeField]
        //protected string isHurt = "IsHurt";
        //[SerializeField]
        //protected string IsDead = "IsDead";
        //[SerializeField]
        //protected string abilityIndex = "AbilityIndex";
        //[SerializeField]
        //protected string basicattackIndex = "BasicAttackIndex";
        //[SerializeField]
        //protected string isLooping = "IsLooping";
        //[SerializeField]
        //protected string Movement = "Movement";

        //protected int normalizedSpeed = 1;
        //protected float animatorSpeed = 1;
        #endregion

        protected IActorHub mover = null;



        #region private calls
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();

            mover = transform.root.GetComponent<IActorHub>();

        }
        #endregion


        #region public
        public virtual void SetAnimatorSpeed(float newvalue)
        {
            animator.speed = newvalue;
            ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Animator speed " + animator.speed, this);
        }

        public virtual void AddTicker() => TickManager.Instance.AddTicker(this);


        public virtual void DoTick()//helps sync the animator to the agent
        {
            //a source of great consternation
            Vector3 worldDeltaPosition = mover.MyTransform.position - transform.position;
            // Pull character towards agent
            if (worldDeltaPosition.magnitude > .01f)
            {
                transform.position = mover.MyTransform.position - 0.9f * worldDeltaPosition;

            }
            // SetLooping(mouseInput.GetMouseButtonOneDown());
            //     transform.rotation = Quaternion.Slerp(navmeshmover.GetAgent().transform.rotation, this.transform.localRotation, Time.deltaTime);
        }

        public virtual void RemoveTicker() => TickManager.Instance.RemoveTicker(this);


        public virtual float GetTickDuration() => Time.deltaTime;

        [System.Obsolete("Use SetAnimatorState()")]
        public virtual void SetBasicAttackIndex(int newIndex)
        {
            //animator.SetInteger(basicattackIndex, newIndex);
        }
        public virtual void SetAnimatorState(string name, float blendduration = 0.02F, int layer = 0)
        {
            animator.CrossFadeInFixedTime(name, blendduration, layer);
        }

        [System.Obsolete("Use SetAnimatorState()")]
        public virtual void TriggerBasicAttackAnimation(string trigger, int index, bool canLoop, float blend = .02f)
        {
            SetAnimatorState(trigger, blend, index);
            //animator.SetTrigger(trigger);
            //animator.SetInteger(basicattackIndex, index);
            //SetLooping(canLoop);
        }
        [System.Obsolete("Use SetAnimatorState()")]
        public virtual void TriggerAbilityAnimation(string trigger, int index, bool canLoop, float blend = .02f)
        {
            SetAnimatorState(trigger, blend, index);
            //animator.SetTrigger(trigger);
            //animator.SetInteger(abilityIndex, index);
            //SetLooping(canLoop);
        }
        [System.Obsolete()]
        public virtual void SetLooping(bool isLooping)
        {
            //animator.SetBool(this.isLooping, isLooping);
        }
        public virtual Animator GetAnimator()
        {
            return animator;
        }
        [System.Obsolete("Use State Machine instead")]
        public virtual void SetHurt(bool _isHurt)
        {
            //animator.SetBool(isHurt, _isHurt);
        }
        [System.Obsolete("Use State Machine instead")]
        public virtual void SetDead(bool isDead)
        {

            //animator.SetBool(IsDead, isDead);
        }


        public virtual float GetCurrentAnimationLength()
        {


            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
            if (clips == null || clips.Length == 0)
            {
                return 0;
            }
            float length = clips[0].clip.length;
            AnimatorTransitionInfo transinfo = animator.GetAnimatorTransitionInfo(0);
            float translength = transinfo.duration;
            length = length + translength;
            //ARPGDebugger.DebugMessage("Animator delay length: " + length);
            return length;
        }
        [System.Obsolete("Use State Machine instead")]
        public virtual void SetMovement(float movement)
        {

            //animator.SetFloat(Movement, movement);
        }
        [System.Obsolete]
        public virtual bool GetDelay()
        {

            return delay;
        }

        [System.Obsolete]
        public virtual void DelayAnimation()
        {
            StartCoroutine(AnimationDelay());
        }
        #endregion

        #region coroutines
        [System.Obsolete]
        protected virtual IEnumerator AnimationDelay()
        {
            yield return null;
            //delay = true;
     
            //while (animator.GetCurrentAnimatorStateInfo(0).IsName(Movement) == false)
            //{
            //    yield return null;
            //}
            //delay = false;
        }

        public void SetFloatParam(string param, float value)
        {
            animator.SetFloat(param, value);
        }






        #endregion
    }
}