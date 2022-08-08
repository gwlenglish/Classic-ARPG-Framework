using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;

using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.com
{


    public class AbilityInterruptHealthChecker : MonoBehaviour, IInterruptAbilityChecker
    {
        public Ability ToInterrupt;
        public System.Action<Ability> OnInterrupt;
        public AbilityInterruptOptions Options;
        public IActorHub Hub;
        bool interrupted = false;


        private void Start()
        {
            OnInterrupt += Interrupted;
            if (Options.OnCasterDamaged)
            {

                if (Hub != null && Hub.MyHealth != null)
                {
                    Hub.MyHealth.OnTakeDamage += TookDamage;
                    Hub.MyHealth.OnDied += Died;
                }
            }
        }

        void Interrupted(Ability ability)
        {
            interrupted = true;
        }

        void Died(CombatResults results)
        {
            if (Options.OnCasterDied)
            {
                OnInterrupt.Invoke(ToInterrupt);
               
            }
        }
        void TookDamage(CombatResults resource)
        {
            if (Options.OnCasterDamaged)
            {
                OnInterrupt.Invoke(ToInterrupt);
            }

           
           
        }
       

        public void Remove()
        {
            OnInterrupt -= Interrupted;
            if (Hub != null && Hub.MyHealth != null)
            {
                Hub.MyHealth.OnTakeDamage -= TookDamage;
                Hub.MyHealth.OnDied -= Died;
            }
            Destroy(this);
        }
    }
}