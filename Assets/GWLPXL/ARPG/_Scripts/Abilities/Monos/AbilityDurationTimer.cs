
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{

    public class AbilityCooldownTimer : ITick
    {

        public AbilityCooldownVars Vars;
        public AbilityCooldownTimer(AbilityCooldownVars vars)
        {
            this.Vars = vars;
            AddTicker();
        }
        public void AddTicker() => TickManager.Instance.AddTicker(this);


        public void DoTick()
        {
            Vars.Holder.RemoveCooldown(this);
        }

        public float GetTickDuration() => Vars.Duration;


        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);

    }

    [System.Serializable]
    public class AbilityCooldownVars
    {

        public float Duration = 1;
        public AbilityController Holder = null;
        public Ability skill = null;
        public IActorHub User = null;
        public bool Pause;
        public AbilityCooldownVars(float cooldowntime, AbilityController forSkill, Ability _skill, IActorHub _user)
        {
            Duration = cooldowntime;
            Holder = forSkill;
            skill = _skill;
            User = _user;

        }

       
    }
    /// <summary>
    /// duration for the abilities.
    /// </summary>
    public class AbilityDurationTimer : ITick, IAbilityTimer
    {
        public AbilityCooldown Cooldown;
        bool ini;
        public AbilityDurationTimer(AbilityCooldown vars)
        {
            Cooldown = vars;
            AddTicker();
        }
       

        private void TickCooldown()
        {
            if (Cooldown.Pause) return;

            Cooldown.timer += GetTickDuration();
            if (Cooldown.timer >= Cooldown.CooldownRate)
            {

                RemoveTicker();
            }
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            TickCooldown();
        }

        public void RemoveTicker()
        {
            Cooldown.Holder.RemoveTimer(Cooldown.skill, Cooldown.User, this);
            TickManager.Instance.RemoveTicker(this);
        }

        public float GetTickDuration() => Cooldown.CooldownRate;

    }
}