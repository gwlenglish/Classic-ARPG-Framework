

using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Statics.com;

using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;

namespace GWLPXL.ARPGCore.Combat.com
{

    //uses triggers to check its damage

    /// <summary>
    /// more like environmental damage
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class EnvironmentalDamage : MonoBehaviour, IDoDamage, ITick
    {
   
        [SerializeField]
        EnvironmentDamageEvents damageEvents = new EnvironmentDamageEvents();
        [SerializeField]
        EnvironmentSotEvents sotEvents = new EnvironmentSotEvents();
        [SerializeField]
        EnvironmentDamageData damage;

        Collider hitBox = null;
        Rigidbody rb = null;
        List<SOT> dots = new List<SOT>();
        List<IActorHub> damageList = new List<IActorHub>();
        IWeaponModification[] changes = new IWeaponModification[0];
        void Awake()
        {
            // adjust the settings
            hitBox = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();

            hitBox.isTrigger = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            if (damage == null) damage = ScriptableObject.CreateInstance<EnvironmentDamageData>();
            if (damage.DamageVars.CombatHandler == null) damage.DamageVars.SetCombatFormulas(DungeonMaster.Instance.CombatFormulas.GetCombatFormulas());
            changes = GetComponentsInChildren<IWeaponModification>();


        }
        void Start()
        {
            AddTicker();
            EnableDamageComponent(true, null);
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void EnableDamageComponent(bool isEnabled, IActorHub _actor)
        {
            hitBox.enabled = true;
            if (enabled)
            {
                damageEvents.SceneEvents.OnDamageComponentEnabled.Invoke(this);
            }
            else
            {
                damageEvents.SceneEvents.OnDamageComponentDisabled.Invoke(this);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            IActorHub actor = other.GetComponent<IActorHub>();
            if (actor == null) return;

            IReceiveDamage attackable = actor.MyHealth;
            if (attackable == null) return;
            bool foundonlist = false;
            for (int i = 0; i < dots.Count; i++)
            {
                if (dots[i].Attackable == attackable)
                {
                    foundonlist = true;
                    //alraedy on the list
                    break;
                }
            }

            if (foundonlist == false)
            {
                SOT newDot = new SOT(actor);
                dots.Add(newDot);
                if (damage.DamageVars.DamageOverTimeOptions.ApplyAtEnter)
                {
                    DamageLogic(actor);
                }
                if (damage.DamageVars.DamageOverTimeOptions.ApplyDotAtEnter)
                {
                    DotsLogic(actor);
                }
               
            }

        }
        /// <summary>
        /// applies the dot/refreshes
        /// </summary>
        /// <param name="statusReceiver"></param>
        private void DotsLogic(IActorHub statusReceiver)
        {
            bool dotsadded = damage.DamageVars.CombatHandler.AddDOTS(null, statusReceiver, damage.DamageVars.DamageOverTimeOptions.AdditionalDOTs);
            if (dotsadded)
            {
                sotEvents.SceneEvents.OnSoTApply.Invoke(statusReceiver.MyStatusEffects);
            }
        }

        void OnTriggerExit(Collider other)
        {
            IActorHub actor = other.GetComponent<IActorHub>();
            if (actor == null) return;
            IReceiveDamage attackable = actor.MyHealth;
            if (attackable == null) return;

            for (int i = 0; i < dots.Count; i++)
            {
                if (dots[i].Attackable == attackable)
                {
                    if (damage.DamageVars.DamageOverTimeOptions.ApplyAtExit)
                    {
                        DamageLogic(dots[i].ActorHub);
                    }
                    if (damage.DamageVars.DamageOverTimeOptions.ApplyDotAtExit)
                    {
                        DotsLogic(dots[i].ActorHub);
                    }
                    dots.RemoveAt(i);
                    break;
                }
            }

        }

        public void DamageLogic(IActorHub attacked)
        {
            if (attacked == null) return;

            AttackValues values = new AttackValues(null, attacked, true);
            values = CombatHelper.GetElementalDamageNoActor(values, damage.DamageVars.DamageMultiplers.ElementalMultipliers);
            attacked.MyHealth.TakeDamage(values);

           // damageEvents.SceneEvents.OnElementalDamageOther.Invoke(eleDmg, attacked.MyHealth);

            //on dots applied event here.

        }


        public Transform GetTransform()
        {
            return this.transform;
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            //need to figure this part out.
            for (int i = 0; i < dots.Count; i++)
            {
                dots[i].Duration += GetTickDuration();
                if (dots[i].Duration >= damage.DamageVars.DamageOverTimeOptions.DamageRate)
                {
                    dots[i].Duration = 0;
                    DamageLogic(dots[i].ActorHub);
                    DotsLogic(dots[i].ActorHub);
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

        public void SetActorOwner(IActorHub newOWner)
        {
            return;
        }

        public IActorHub GetActorOwner() => null;

        public IWeaponModification[] GetWeaponMods() => changes;


        public List<IActorHub> GetDamagedList() => damageList;

        public float GetAdditionalChargePercent() => 0;



    }
}