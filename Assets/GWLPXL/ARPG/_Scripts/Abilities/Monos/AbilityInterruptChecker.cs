using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{


    public class AbilityInterruptChecker : MonoBehaviour, IInterruptAbilityChecker, ITick
    {
        public Ability ToInterrupt;
        public System.Action<Ability> OnInterrupt;
        public Ability[] Interruptors = new Ability[0];
        public IActorHub Hub = null;
        Ability current = null;
        bool interrupted = false;
        void Start()
        {
            AddTicker();
            if (Hub == null)
            {
                Debug.LogWarning("Cant interrupt abilities without an IAbilityUser interface", this);
                Remove();
            }
            OnInterrupt += Interrupted;

        }

        void Interrupted(Ability ability)
        {
            interrupted = true;
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (interrupted) return;

            current = Hub.MyAbilities.GetLastIntendedAbility();
            for (int i = 0; i < Interruptors.Length; i++)
            {
                if (current == Interruptors[i])
                {
                    OnInterrupt?.Invoke(ToInterrupt);
                    break;
                }
            }
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }

        public void Remove()
        {
            OnInterrupt -= Interrupted;
            Destroy(this);
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }
}