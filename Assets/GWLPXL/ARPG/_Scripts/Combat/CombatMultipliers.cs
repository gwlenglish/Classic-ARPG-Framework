using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{


    /// <summary>
    /// mostly for environment dmg or static dmg. Things with casters, aka the player and enemies should stick to stats and equipment. 
    /// </summary>
    [System.Serializable]
    public class ElementDamageMultiplierNoActor
    {
        public ElementType DamageType => damageType;
        public int BaseElementDamage => baseElementDamageAmount;

        [SerializeField]
        protected string editorDescription = string.Empty;
        [Header("Base")]
        [SerializeField]
        ElementType damageType = ElementType.Fire;
        [SerializeField]
        protected int baseElementDamageAmount = 0;

        public ElementDamageMultiplierNoActor(ElementType type, int basedmg)
        {
            damageType = type;
            baseElementDamageAmount = basedmg;
        }


    }
    [System.Serializable]
    public class PhysicalDamageMultiplierNoActor
    {
        public int BasePhysicalDamage => basePhysicalDamageAmount;
        [SerializeField]
        protected string editorDescription = string.Empty;
        [Header("Base")]
        [SerializeField]
        protected int basePhysicalDamageAmount = 0;

        public PhysicalDamageMultiplierNoActor(int basedmg)
        {
            basePhysicalDamageAmount = basedmg;
        }


    }

    [System.Serializable]
    public class DamageOverTimeMultipliers
    {
        public ElementDamageMultiplierNoActor[] ElementalMultipliers => elementalMultipliers;
        [SerializeField]
        ElementDamageMultiplierNoActor[] elementalMultipliers = new ElementDamageMultiplierNoActor[0];
    }

    [System.Serializable]
    public class DamageMultipliers_NoActor
    {
        public PhysicalDamageMultiplierNoActor PhysicalMultipliers = new PhysicalDamageMultiplierNoActor(0);
        public ElementDamageMultiplierNoActor[] ElementMultipliers = new ElementDamageMultiplierNoActor[0];
    }
    [System.Serializable]
    public class CombatMultipliers
    {
        public DamageMultiplers_Actor DamageMultipliers = new DamageMultiplers_Actor();
    }

    [System.Serializable]
    public class DamageMultiplers_Actor
    {
        public PhysicalDamageMultiplier PhysMultipler => physicalMultipler;
        public ElementDamageMultiplierActor[] ElementMultiplers => elementMultipliers;
        [Header("Damage Multis")]
        [SerializeField]
        protected PhysicalDamageMultiplier physicalMultipler = new PhysicalDamageMultiplier();
        [SerializeField]
        protected ElementDamageMultiplierActor[] elementMultipliers = new ElementDamageMultiplierActor[0];


    }
    [System.Serializable]
    public class ElementDamageMultiplierActor
    {
        public ElementType DamageType => damageType;
        public int BaseElementDamage => baseElementDamageAmount;

        public float PercentOfCasterElement => percentOfCasterElement;
        [SerializeField]
        protected string editorDescription = string.Empty;
        [Header("Base")]
        [SerializeField]
        ElementType damageType = ElementType.Fire;

        protected int baseElementDamageAmount = 0;
        [Tooltip("Min and Max Damage Range")]
        [SerializeField]
        protected int minD = 1;
        [SerializeField]
        protected int maxD = 3;
        [SerializeField]
        [Range(0, 2f)]
        [Header("Multiplier")]
        [Tooltip("In percent, so 1 = 100%.")]
        protected float percentOfCasterElement = 0;
        public virtual void SetBaseDmgAmount(int newbaseamount) => baseElementDamageAmount = newbaseamount;
        public ElementDamageMultiplierActor(ElementType type, int basedmg, float percentofcasterelementtoapply)
        {
            damageType = type;
            baseElementDamageAmount = basedmg;
            percentOfCasterElement = percentofcasterelementtoapply;
        }

        void RollNewAmount()
        {
            baseElementDamageAmount = Random.Range(minD, maxD + 1);
        }
        public virtual int GetElementDamageAmount(IActorHub forUser)
        {
            RollNewAmount();
            if (forUser == null) return baseElementDamageAmount;
            float baseUserAttack = (float)forUser.MyStats.GetRuntimeAttributes().GetElementAttack(damageType);
            baseUserAttack *= (percentOfCasterElement);
            return Mathf.FloorToInt(baseUserAttack + baseElementDamageAmount);
        }


    }
    [System.Serializable]
    public class PhysicalDamageMultiplier
    {
        public int BasePhysicalDamage => baseDamageAmount;
        public float PercentOfCasterAttack => percentOfCasterAttack;
        [Header("Base")]
      //  [SerializeField]
        protected int baseDamageAmount = 0;

        [Tooltip("Base damage range. Will random roll to get the range.")]
        [SerializeField]
        protected int minD = 1;
        [SerializeField]
        protected int maxD = 3;
        [Range(0, 2f)]
        [Header("Multiplier")]
        [Tooltip("In percent, so 1 = 100%.")]
        [SerializeField]
        protected float percentOfCasterAttack = 0;

        public virtual void SetBaseAmount(int min, int max)
        {
            minD = min;
            maxD = max;
        }

       
        void RollNewBaseAmount()
        {
            baseDamageAmount = Random.Range(minD, maxD + 1);
        }
        public virtual int GetPhysicalDamageAmount(IActorHub forUser)
        {
            RollNewBaseAmount();
            if (forUser == null)
            {
                return BasePhysicalDamage;
            }

            PhysicalAttackResults baseUserAttack = DungeonMaster.Instance.CombatFormulas.GetCombatFormulas().GetPhysicalAttackValue(forUser);
            float v = baseUserAttack.PhysicalDamage;
            v *= (PercentOfCasterAttack);
            return Mathf.FloorToInt(v + (BasePhysicalDamage));
            ///eventually move this over to formulas, enemy attack damage and elemental
        }



    }




    [System.Serializable]
    public class PhysicalArmorMultipler
    {
        public int BasePhysicalArmor => basePhysicalArmorAmount;
        public float PercentOfCasterAttack => percentofdefenderdefense;
        [Header("Base")]
        [SerializeField]
        protected int basePhysicalArmorAmount = 0;
        [Range(0, 2f)]
        [Header("Multiplier")]
        [Tooltip("In percent, so 1 = 100%.")]
        [SerializeField]
        protected int percentofdefenderdefense = 0;
        public virtual int GetPhysicalDamageAmount(IAttributeUser forUser)
        {
            if (forUser == null)
            {
                return BasePhysicalArmor;
            }

            float baseUserAttack = CombatStats.GetTotalActorDamage(forUser.GetInstance().gameObject);
            baseUserAttack *= (PercentOfCasterAttack / Formulas.Hundred);
            return Mathf.FloorToInt(baseUserAttack + (BasePhysicalArmor));
            ///eventually move this over to formulas, enemy attack damage and elemental
        }

    }
}