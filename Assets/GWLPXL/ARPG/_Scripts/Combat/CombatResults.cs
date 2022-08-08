using UnityEngine;

using System.Collections.Generic;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.com;


namespace GWLPXL.ARPGCore.Combat.com
{
    /// <summary>
    /// combined data class for the combat results
    /// </summary>
    [System.Serializable]
    public class CombatResults
    {
        [Header("Totals")]
        [Tooltip("All damage, regardless of type")]
        public int TotalCombinedDamage;
        [Tooltip("All resist, regardless of type")]
        public int TotalCombinedResist;
        [Tooltip("All damage - all resist, regardless of type")]
        public int TotalReducedDamage;
        [Header("Attack Results")]
        public AttackValues AttackValues;
        [Header("Damage Results")]
        public DamageValues DamageValues;

        public CombatResults(AttackValues attackvalues, DamageValues damagevalues)
        {
            AttackValues = attackvalues;
            DamageValues = damagevalues;
            if (DamageValues == null) DamageValues = new DamageValues(new PhysicalDamageReport(0, 0, 0, new List<string>(1) { "Null Damage Values" }, false), new List<ElementalDamageReport>(), null);

            for (int i = 0; i < AttackValues.PhysicalAttack.Count; i++)
            {
                TotalCombinedDamage += AttackValues.PhysicalAttack[i].PhysicalDamage;
            }
            for (int i = 0; i < attackvalues.ElementAttacks.Count; i++)
            {
                TotalCombinedDamage += AttackValues.ElementAttacks[i].Damage;
            }

            
            for (int i = 0; i < DamageValues.ReportElementalDmg.Count; i++)
            {
                TotalCombinedResist += DamageValues.ReportElementalDmg[i].Resisted;
            }

            TotalCombinedResist += DamageValues.ReportPhysDmg.Resisted;

            TotalReducedDamage = TotalCombinedDamage - TotalCombinedResist;
        }
    }

    /// <summary>
    /// combined attack values data class
    /// </summary>
    [System.Serializable]
    public class AttackValues
    {
        public IActorHub Attacker = null;
        public List<IActorHub> Defenders;
        public bool Resolved = false;
        public bool IgnoreIFrame = false;
        public List<ElementAttackResults> ElementAttacks;
        public List<PhysicalAttackResults> PhysicalAttack;
        public AttackValues(IActorHub attacker, IActorHub defender, bool ignoreIframe = false)
        {
            IgnoreIFrame = ignoreIframe;
            Defenders = new List<IActorHub>();
            Defenders.Add(defender);
            Attacker = attacker;
            ElementAttacks = new List<ElementAttackResults>();
            PhysicalAttack = new List<PhysicalAttackResults>();
        }

        public virtual void Resolve()
        {
            for (int i = 0; i < Defenders.Count; i++)
            {
                Defenders[i].MyHealth.TakeDamage(this);
            }
            Resolved = true;
        }
    }
    /// <summary>
    /// element attack results
    /// </summary>
    [System.Serializable]
    public class ElementAttackResults
    {
        [Tooltip("What type of thing sent it")]
        public string Source;
        public ElementType Type;//none is physical
        [Tooltip("Damage sent by attacker")]
        public int Damage;//initial damage
        [Tooltip("Was a crit?")]
        public bool WasCrit = false;
        public ElementAttackResults(ElementType type, int dmg, string source)
        {
            Source = source;
            Type = type;
            Damage = dmg;

        }
    }

   
    /// <summary>
    /// physical attack results
    /// </summary>
    [System.Serializable]
    public class PhysicalAttackResults
    {
        [Tooltip("What type of thing sent it")]
        public string Source;
        [Tooltip("Damage sent")]
        public int PhysicalDamage;

        public PhysicalAttackResults(int damage, string source)
        {
            Source = source;
            PhysicalDamage = damage;

        }

       
    }

    /// <summary>
    /// combined data damage values
    /// </summary>
    [System.Serializable]
    public class DamageValues
    {
        public List<ElementalDamageReport> ReportElementalDmg;
        public PhysicalDamageReport ReportPhysDmg;
        public IReceiveDamage Target;
        public Transform TargetOwner;

        public DamageValues(PhysicalDamageReport phys, List<ElementalDamageReport> eles, IReceiveDamage target)
        {
            ReportPhysDmg = phys;
            ReportElementalDmg = eles;
            Target = target;
            if (target != null)
            {
                TargetOwner = target.GetInstance();
                if (Target.GetUser() != null)
                {
                    TargetOwner = target.GetUser().MyTransform;
                }
            }
 
           
        }
    }

    /// <summary>
    /// physical damage values
    /// </summary>
    [System.Serializable]
    public class PhysicalDamageReport
    {
        [Tooltip("Total damage sent")]
        public int TotalDamage;
        [Tooltip("Damage mitagated")]
        public int Resisted;
        [Tooltip("Total - Resisted")]
        public int Result;
        [Tooltip("What type of thing the damage originated from")]
        public List<string> Sources = new List<string>();
        public bool WasCrit = false;
        public PhysicalDamageReport(int total, int resisted, int reduced, List<string> sources, bool crit = false)
        {
            WasCrit = crit;
            Sources = sources;
            TotalDamage = total;
            Resisted = resisted;
            Result = reduced;
        }
    }

    /// <summary>
    /// elemental damage values
    /// </summary>
    [System.Serializable]
    public class ElementalDamageReport
    {
        public ElementType Type;
        [Tooltip("Total damage sent")]
        public int TotalDamage;
        [Tooltip("Damage mitagated")]
        public int Resisted;
        [Tooltip("Total - Resisted")]
        public int Result;
        [Tooltip("What type of thing the damage originated from")]
        public List<string> Sources;
        public bool WasCrit = false;
        public ElementalDamageReport(ElementType type, int total, List<string> sources, bool crit = false)
        {
            Sources = sources;
            TotalDamage = total;
            Type = type;
            WasCrit = crit;
        }
    }

   
}
