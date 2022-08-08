
using GWLPXL.ARPGCore.com;

using GWLPXL.ARPGCore.StatusEffects.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.com
{


    public class AbilityInterruptInflictionChecker : MonoBehaviour, IInterruptAbilityChecker, ITick
    {
        public Ability ToInterrupt;
        public System.Action<Ability> OnInterrupt;
        public AbilityInterruptOptions Options;
        public IActorHub Hub;
        bool interrupted = false;

        private void Start()
        {
            OnInterrupt += Interrupted;
            AddTicker();
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }

        void Interrupted(Ability ability)
        {
            interrupted = true;
            Debug.Log("Interrupted");
        }

        public void Remove()
        {
            OnInterrupt -= Interrupted;
            Destroy(this);
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (interrupted == true) return;

            for (int i = 0; i < Hub.MyStatusEffects.GetCurrentAppliedStatuses().Count; i++)
            {
                StatusEffectVars inflict = Hub.MyStatusEffects.GetCurrentAppliedStatuses()[i];
                for (int j = 0; j < Options.InterruptEffects.Length; j++)
                {
                    if (inflict.EffectName == Options.InterruptEffects[j])
                    {
                        OnInterrupt?.Invoke(ToInterrupt);
                    }
                }
            }
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }
    }
}