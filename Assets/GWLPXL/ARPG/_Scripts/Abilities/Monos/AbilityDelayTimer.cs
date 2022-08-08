using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{
    public interface IAbilityTimer
    {

    }
    public class AbilityDelay
    {
        public float Timer = 0;
        public float Delay = 0;
        public AbilityController Holder = null;
        public Ability Ability = null;
        public IActorHub User = null;
        public AbilityDelay(float delay, AbilityController controller, Ability ability, IActorHub user)
        {
            Delay = delay;
            Holder = controller;
            Ability = ability;
            User = user;
        }
    }

    public class AbilityDelayTimer : MonoBehaviour, ITick, IAbilityTimer
    {
        public AbilityDelay Cooldown;
        bool ini = false;
      
        private void Start()
        {
            ini = true;
        }
        private void OnEnable()
        {
            AddTicker();
        }
        private void OnDisable()
        {
            RemoveTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (ini == false) return;

            Cooldown.Timer += GetTickDuration();
            if (Cooldown.Timer >= Cooldown.Delay)
            {
                Cooldown.Holder.StartAbility(Cooldown.User, Cooldown.Ability);
                Cooldown.Holder.RemoveDelay(this);
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