
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{
    /// <summary>
    /// Used to do any delayed callbacks in the ability logic
    /// </summary>
    public class DelayedAbilityCallback : ITick
    {
        IActorHub actor;
        float timer;
        float delay;
        Ability skill;
        System.Action<IActorHub, Ability> abilityCallback;
        public DelayedAbilityCallback(IActorHub actor, float delay, Ability skill, System.Action<IActorHub, Ability> AbilityCallback)
        {
            this.abilityCallback = AbilityCallback;
            this.delay = delay;
            this.skill = skill;
            this.actor = actor;
            AddTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            timer += GetTickDuration();
            if (timer >= delay)
            {
                abilityCallback?.Invoke(actor, skill);
                RemoveTicker();
            }
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }
}