


using System.Collections.Generic;
using UnityEngine;


using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    
    /// <summary>
    /// 2D version of the melee weapon damage dealer
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class MeleeWeaponDD2D : MonoBehaviour, IDoDamage, IMeleeWeapon, IDoActorDamage, IApplySOT
    {
        [Header("Damage Events")]
        [SerializeField]
        protected ActorDamageEvents damageEvents = new ActorDamageEvents();
        [Header("SoT Events")]
        [SerializeField]
        protected EnvironmentSotEvents sotEvents = new EnvironmentSotEvents();
        [Header("Damage Data")]
        [SerializeField]
        protected ActorDamageData damage;

        [Header("Melee Specific")]
        [SerializeField]
        protected MeleeData meleeOptions;

        protected List<SOT> sots = new List<SOT>();
        protected Collider2D[] hitBoxes = new Collider2D[0];
        protected Rigidbody2D rb = null;
        protected IActorHub owner = null;
        protected IWeaponModification[] changes = new IWeaponModification[0];
        protected List<IActorHub> damageList = new List<IActorHub>();
        protected List<IActorHub> sotAppliedList = new List<IActorHub>();
        protected float chargedPercent = 0;
        protected virtual void Awake()
        {
            // adjust the settings
            rb = GetComponent<Rigidbody2D>();

            hitBoxes = GetComponentsInChildren<Collider2D>();
            if (hitBoxes.Length == 0)
            {
                Debug.LogError("No Colliders found on " + this.gameObject.name + ". Damage will not work until colliders are added");

            }
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].isTrigger = true;
                hitBoxes[i].enabled = false;
            }
         
            rb.gravityScale = 0;
            rb.isKinematic = true;
            if (meleeOptions == null) meleeOptions = ScriptableObject.CreateInstance<MeleeData>();
            if (damage == null) damage = ScriptableObject.CreateInstance<ActorDamageData>();
            if (damage.DamageVar.CombatHandler == null) damage.DamageVar.SetCombatFormulas(DungeonMaster.Instance.CombatFormulas.GetCombatFormulas());
        }

        public void SetUser(IActorHub forUser)
        {
            //any ini logic
            return;
        }

      public void SetMeleeOptionData(MeleeData newdata)
        {
            meleeOptions = newdata;
        }


        public void EnableDamageComponent(bool isEnabled, IActorHub _actor)
        {
            SetActorOwner(_actor);
            

            if (isEnabled)
            {
                damageList.Clear();
                sotAppliedList.Clear();
                changes = GetComponentsInChildren<IWeaponModification>();

                damageEvents.SceneEvents.OnDamageComponentEnabled.Invoke(this);
            }
            else
            {
                damageEvents.SceneEvents.OnDamageComponentDisabled.Invoke(this);
            }


            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i].enabled = isEnabled;
            }

        }



        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
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

          

        }
        protected virtual void OnTriggerExit2D(Collider2D other)
        {
         
            OnExitSoTLogic(other.GetComponent<IActorHub>());
        }
   

        #region sots
        private void OnEnterSotLogic(IActorHub target)
        {
            damage.DamageVar.CombatHandler.OnEnterSotLogic(owner, target, this, this);

        }
        private void OnExitSoTLogic(IActorHub attackable)
        {
            damage.DamageVar.CombatHandler.OnExitSoTLogic(owner, attackable, this, this);

        }
       
       

        #endregion

        public void DamageLogic(IActorHub attacked)
        {
            if (DungeonMaster.Instance.CombatFormulas.GetCombatFormulas().CanMeleeAttack(owner, this, this, attacked, this))
            {
                AttackValues values = new AttackValues(owner, attacked);
                damage.DamageVar.CombatHandler.GetMeleeActorDamageLogic(values, owner, this, attacked, this, this);
                values.Resolve();
            }

        }

        public Transform GetTransform()
        {
            return this.transform;
        }

     
      

        public IDoDamage GetDamageComponent()
        {
            return this;
        }

        public void SetDamageData(ActorDamageData newData) => damage = newData;

        public void SetActorOwner(IActorHub newOWner)
        {
            owner = newOWner;
           
        }

        public IActorHub GetActorOwner() => owner;

        public IWeaponModification[] GetWeaponMods() => changes;
       

        public List<IActorHub> GetDamagedList() => damageList;

        public MeleeData GetMeleeOptions() => meleeOptions;
     

        public ActorDamageData GetActorDamageData() => damage;

        public ActorDamageEvents GetActorDamageEvents() => damageEvents;

        public List<IActorHub> GetSoTAppliedList() => sotAppliedList;


        public List<SOT> GetSOTS() => sots;


        public EnvironmentSotEvents GetSOTEvents() => sotEvents;


        public float GetAdditionalChargePercent() => 0;
  
    }


}