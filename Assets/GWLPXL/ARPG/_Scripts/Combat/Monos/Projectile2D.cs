
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
    /// 2D version of the projectile
    /// </summary>

    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile2D : MonoBehaviour, IDoDamage, IDoActorDamage, IProjectile, IApplySOT
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
        Rigidbody2D rb = null;
        Collider2D[] hitBoxes = new Collider2D[0];

        IActorHub owner = null;
        List<IActorHub> damageList = new List<IActorHub>();
        List<SOT> sots = new List<SOT>();

        IWeaponModification[] changes = new IWeaponModification[0];
        List<IActorHub> appliedSotList = new List<IActorHub>();
        float chargedPercent = 0;
        public bool Disabled { get; set; }
        #endregion

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = this.gameObject.AddComponent<Rigidbody2D>();
            }

            hitBoxes = GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].isTrigger = true;
                hitBoxes[i].enabled = false;
            }
        
            rb.gravityScale = 0;
            rb.isKinematic = false;

            if (damage.DamageVar.CombatHandler == null) damage.DamageVar.SetCombatFormulas(DungeonMaster.Instance.CombatFormulas.GetCombatFormulas());
            if (projectileOptions == null) projectileOptions = ScriptableObject.CreateInstance<ProjectileData>();
            if (damage == null) damage = ScriptableObject.CreateInstance<ActorDamageData>();
        }

       
     

        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
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
        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (Disabled) return;

            IActorHub otheractor = other.GetComponent<IActorHub>();
            if (otheractor == null) return;



            OnExitSoTLogic(otheractor);

        }

        protected virtual void OnExitSoTLogic(IActorHub attackable)
        {
            damage.DamageVar.CombatHandler.OnExitSoTLogic(owner, attackable, this, this);

           
        }







        protected virtual void OnEnterSotLogic(IActorHub target)
        {
            damage.DamageVar.CombatHandler.OnEnterSotLogic(owner, target, this, this);
            
           
        }
        public void SetProjectileData(ProjectileData newData) => projectileOptions = newData;
        public void EnableDamageComponent(bool isEnabled, IActorHub _actor)
        {
            owner = _actor;
            if (hitBoxes == null)
            {
                hitBoxes = GetComponentsInChildren<Collider2D>();
            }
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].enabled = isEnabled;
            }
           
            damageList.Clear();

            SetActorOwner(_actor);

            if (isEnabled)
            {
                changes = GetComponents<IWeaponModification>();
                damageList.Clear();
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

       
        public float GetTickDuration()
        {
            return Time.deltaTime;
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

        public List<IActorHub> GetSoTAppliedList() => appliedSotList;



        public List<SOT> GetSOTS() => sots;


        public EnvironmentSotEvents GetSOTEvents() => sotEvents;

   
        public float GetAdditionalChargePercent() => chargedPercent;
     
    }
}