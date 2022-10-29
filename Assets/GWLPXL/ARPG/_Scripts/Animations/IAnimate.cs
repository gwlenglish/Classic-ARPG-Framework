using UnityEngine;
namespace GWLPXL.ARPGCore.Animations.com
{


    /// <summary>
    /// some methods will be deprecated
    /// </summary>
    public interface IAnimate
    {
        Animator GetAnimator();
        void SetAnimatorSpeed(float newavalue);
        [System.Obsolete("Use State Machine instead")]
        void SetDead(bool isDead);
        [System.Obsolete("Use State Machine instead")]
        void SetHurt(bool isHurt);
        [System.Obsolete("Use State Machine instead")]
        void SetLooping(bool isLooping);
        [System.Obsolete("Use State Machine instead")]
        void SetBasicAttackIndex(int newIndex);
        [System.Obsolete("Use Set Animator State")]
        void TriggerAbilityAnimation(string trigger, int index, bool canLoop, float blend = .02f);
        [System.Obsolete("Use Set Animator State")]
        void TriggerBasicAttackAnimation(string trigger, int index, bool canLoop, float blend = .02f);
        [System.Obsolete("Use State Machine instead")]
        void SetMovement(float movement);

        void SetFloatParam(string param, float value);
        float GetCurrentAnimationLength();
        bool GetDelay();
        void DelayAnimation();
        void SetAnimatorState(string name, float blendduration = .02f, int layer = 0);

        bool InState(string stateName, int layer = 0);
       

    }
}