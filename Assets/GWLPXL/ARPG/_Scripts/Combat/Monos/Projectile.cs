

using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Combat.com
{

    /// <summary>
    /// add friendly fire to dots
    /// add dots multiplers
    /// </summary>
    public interface IProjectile
    {
        void SetDamageData(ActorDamageData newData);
        void SetProjectileData(ProjectileData newData);
        ProjectileData GetProjectileData();
        bool Disabled { get; set; }
    }
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IDoDamage, IDoActorDamage, IProjectile, IApplySOT, ITick
    {
        [Header("Damage Events")]
        [SerializeField]
        ActorDamageEvents damageEvents = new ActorDamageEvents();
        [Header("SoT Events")]
        [SerializeField]
        EnvironmentSotEvents sotEvents = new EnvironmentSotEvents();
        [Header("Damage Data")]

        [SerializeField]
        ActorDamageData damage;

        [Header("Projectile Specific")]
        [SerializeField]
        ProjectileData projectileOptions;


        #region fields
        Rigidbody rb = null;
        Collider[] hitBoxes = new Collider[0];
        IActorHub owner = null;

        List<IActorHub> damageList = new List<IActorHub>();
        List<SOT> sots = new List<SOT>();
        List<IActorHub> sotAppliedList = new List<IActorHub>();
        IWeaponModification[] changes = new IWeaponModification[0];
        bool ini = false;
        float chargedPercent = 0;

        public bool Disabled { get; set; }
        #endregion

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = this.gameObject.AddComponent<Rigidbody>();
            }

            hitBoxes = GetComponentsInChildren<Collider>();
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].isTrigger = true;
                hitBoxes[i].enabled = false;
            }
        
            rb.useGravity = false;
            rb.isKinematic = false;

            if (damage.DamageVar.CombatHandler == null) damage.DamageVar.SetCombatFormulas(DungeonMaster.Instance.CombatFormulas.GetCombatFormulas());
            if (projectileOptions == null) projectileOptions = ScriptableObject.CreateInstance<ProjectileData>();
            if (damage == null) damage = ScriptableObject.CreateInstance<ActorDamageData>();
        }


        protected virtual void Start()
        {

            AddTicker();
            ini = true;

        }

        protected virtual void OnDestroy()
        {
            RemoveTicker();
        }


        protected virtual void OnTriggerEnter(Collider other)
        {
            if (ini == false) return;
            if (Disabled) return;
            for (int i = 0; i < changes.Length; i++)
            {
                if (changes[i].DoChange(other.transform))
                {
                    //dont allow dmg
                    return;
                }
            }
            IActorHub otheractor = other.GetComponent<IActorHub>();
            if (otheractor == null) return;
            DamageLogic(otheractor);
            OnEnterSotLogic(otheractor);

            if (otheractor == GetActorOwner()) return;
            CombatHelper.HandleProjectileDestroy(projectileOptions, this.gameObject);

           
            

        }
        protected virtual void OnTriggerExit(Collider other)
        {
            if (Disabled) return;

            if (ini == false) return;
            OnExitSoTLogic(other);

        }

        protected virtual void OnExitSoTLogic(Collider other)
        {
            damage.DamageVar.CombatHandler.OnExitSoTLogic(owner, other.GetComponent<IActorHub>(), this, this);

        }







        protected virtual void OnEnterSotLogic(IActorHub attacked)
        {
            damage.DamageVar.CombatHandler.OnEnterSotLogic(owner, attacked, this, this);

        }
        public void SetProjectileData(ProjectileData newData) => projectileOptions = newData;
        public void EnableDamageComponent(bool isEnabled, IActorHub _actor)
        {
            SetActorOwner(_actor);
            
           
            damageList.Clear();
            changes = GetComponentsInChildren<IWeaponModification>();
            for (int i = 0; i < changes.Length; i++)
            {
                changes[i].SetUser(owner);
                changes[i].SetActive(isEnabled);
            }
           
            if (isEnabled)
            {
                damageEvents.SceneEvents.OnDamageComponentEnabled.Invoke(this);
            }
            else
            {
                damageEvents.SceneEvents.OnDamageComponentDisabled.Invoke(this);
            }

            if (hitBoxes == null)
            {
                hitBoxes = GetComponentsInChildren<Collider>();
            }
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].enabled = isEnabled;
            }

            Disabled = !isEnabled;


        }

        public void DamageLogic(IActorHub damageTarget)
        {
            if (DungeonMaster.Instance.CombatFormulas.GetCombatFormulas().CanProjectileAttack(owner, this, damageTarget, this, this))
            {
                AttackValues values = new AttackValues(owner, damageTarget);
                damage.DamageVar.CombatHandler.GetProjectileDamage(values, owner, this, damageTarget, this, this);
                values.Resolve();
            }
        


        }







        public Transform GetTransform()
        {
            return this.transform;
        }

      

        public void DoTick()
        {
            if (Disabled && gameObject.activeInHierarchy)//buffered destroy
            {
                gameObject.SetActive(false);
                return;
            }

           
           
        }

       
       

        public void SetDamageData(ActorDamageData newData)
        {
            damage = newData;
        }

        public ProjectileData GetProjectileData() => projectileOptions;

        public void SetActorOwner(IActorHub newOWner)
        {
            owner = newOWner;
           
        }

        public IActorHub GetActorOwner() => owner;

        public IWeaponModification[] GetWeaponMods() => changes;


        public List<IActorHub> GetDamagedList() => damageList;

        public ActorDamageData GetActorDamageData() => damage;


        public ActorDamageEvents GetActorDamageEvents() => damageEvents;

        public List<IActorHub> GetSoTAppliedList() => sotAppliedList;


        public List<SOT> GetSOTS() => sots;


        public EnvironmentSotEvents GetSOTEvents() => sotEvents;

        public void AddTicker() => TickManager.Instance.AddTicker(this);


        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);


        public float GetTickDuration() => Time.fixedDeltaTime;

        public float GetAdditionalChargePercent() => chargedPercent;

    }
}