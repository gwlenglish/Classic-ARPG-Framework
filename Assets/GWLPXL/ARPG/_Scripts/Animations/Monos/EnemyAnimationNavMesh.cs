
using GWLPXL.ARPGCore.DebugHelpers.com;
using System.Collections;
using UnityEngine;

namespace GWLPXL.ARPGCore.Animations.com
{
    /// <summary>
    /// deprecated unused methods
    /// adding new wrapper method for calling into animator states
    /// </summary>
    public class EnemyAnimationNavMesh : MonoBehaviour, IAnimate
    {
        [SerializeField]
        protected Animator animator;
        //protected bool delay = false;
        //[SerializeField]
        //protected float additionalCooldown = 1f;
        //[SerializeField]
        //protected string isHurt = "IsHurt";
        //[SerializeField]
        //protected string IsDead = "IsDead";
        //[SerializeField]
        //protected string abilityIndex = "AbilityIndex";
        //[SerializeField]
        //protected string basicattackIndex = "BasicAttackIndex";
        ////[SerializeField]
        ////string isLooping = "IsLooping";
        //[SerializeField]
        //protected string Movement = "Movement";

        //protected float animatorSpeed = 1f;

        protected virtual void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (animator == null)
            {
                Debug.LogError(this.gameObject + " needs an Animator Component in order to animate....");
                return;
            }
           
        }

        [System.Obsolete("Use State Machine instead")]
        public virtual void SetDead(bool isDead)
        {
            if (animator == null) return;
            //animator.SetBool(IsDead, isDead);
        }
        [System.Obsolete("Use SetAnimatorState()")]
        public virtual void TriggerAbilityAnimation(string trigger, int index, bool canLoop, float blend = .02f)
        {
            if (animator == null) return;
            SetAnimatorState(trigger, blend, index);
            //ARPGDebugger.DebugMessage("Enemy animations called with trigger and index " + trigger + " " + index, this);
           // animator.SetTrigger(trigger);
          // animator.SetInteger(abilityIndex, index);
        }



        public virtual float GetCurrentAnimationLength()
        {
            if (animator == null) return 0f;

            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
            if (clips == null || clips.Length == 0)
            {
                return 0;
            }
            float length = clips[0].clip.length;
            AnimatorTransitionInfo transinfo = animator.GetAnimatorTransitionInfo(0);
            float translength = transinfo.duration;
            length = length + translength;
            return length;
        }
        [System.Obsolete("Use State Machine instead")]
        public virtual void SetMovement(float movement)
        {
            if (animator == null) return;

            //animator.SetFloat(Movement, movement);
        }

        /// <summary>
        /// deprecated
        /// </summary>
        /// <returns></returns>
        /// 
        [System.Obsolete]
        public virtual bool GetDelay()
        {
            if (animator == null) return false;

            return false;
        }

        /// <summary>
        /// no longer used, deprecated
        /// </summary>
        public virtual void DelayAnimation()
        {
            StartCoroutine(AnimationDelay());
        }
        protected virtual IEnumerator AnimationDelay()
        {
            yield return null;
            //delay = true;
       
            //while (animator.GetCurrentAnimatorStateInfo(0).IsName(Movement) == false)
            //{
            //    yield return null;
            //}
            //yield return new WaitForSeconds(additionalCooldown);
            //delay = false;



        }

        public virtual Animator GetAnimator()
        {
            return animator;
        }

        [System.Obsolete("Use State Machine instead")]
        public virtual void SetHurt(bool _isHurt)
        {
            if (animator == null) return;

            //animator.SetBool(isHurt, _isHurt);
        }

        [System.Obsolete("Use SetAnimatorState()")]
        public virtual void TriggerBasicAttackAnimation(string trigger, int index, bool canLoop, float blend = .02f)
        {
            SetAnimatorState(trigger, blend, index);
           // animator.SetTrigger(trigger);
           // animator.SetInteger(basicattackIndex, index);
           // SetLooping(canLoop);
        }
        [System.Obsolete]
        public virtual void SetLooping(bool isLooping)
        {
         //
        }
        public virtual void SetAnimatorSpeed(float newvalue)
        {
            animator.speed = newvalue;
        }

        [System.Obsolete("Use SetAnimatorState()")]
        public virtual void SetBasicAttackIndex(int newIndex)
        {
            //animator.SetInteger(basicattackIndex, newIndex);
        }

        public virtual void SetAnimatorState(string name, float blendduration = 0.02F, int layer = 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(name)) return;

            animator.CrossFadeInFixedTime(name, blendduration, layer);
        }

        public void SetFloatParam(string param, float value)
        {
            animator.SetFloat(param, value);
        }

        public bool InState(string stateName, int layer = 0)
        {
           return animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName);
        }
    }
}