using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    /// <summary>
    /// enemy default combat formulas. Inherit and override to create your own.
    /// </summary>
    [CreateAssetMenu(menuName ="GWLPXL/ARPG/Combat/EnemyDefaultFormulas")]
    public class EnemyDefault : ActorCombatFormulas
    {

       

        /// <summary>
        /// calculate the armor value of the self
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public override int GetArmorValue(IActorHub attacker, IActorHub self)
        {
            int attackerLevel = attacker.MyStats.GetRuntimeAttributes().MyLevel;
            int scaled = 1;
            IScale scaler = self.MyStats.GetInstance().GetComponent<IScale>();
            if (scaler != null)
            {
                scaled = scaler.GetScaledLevel();
            }
            int armorvalue = scaled + self.MyStats.GetRuntimeAttributes().GetStatForCombat(CombatStatType.Armor);//figure attacker level here somewhere.

            ARPGDebugger.CombatDebugMessage(self.MyStats.GetRuntimeAttributes().ActorName + " Armor Value=" + armorvalue, self.MyStats.GetInstance());
            return armorvalue;
        }

       
        /// <summary>
        /// calculcate the attack value of the self
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public override PhysicalAttackResults GetAttackValue(IActorHub self)
        {

            int baseStatFactor = self.MyStats.GetRuntimeAttributes().GetStatForCombat(CombatStatType.Damage);//current base stat value divide by 100

            float baseWpnFactor = 0;
            if (self.MyInventory != null)
            {
                baseWpnFactor = self.MyInventory.GetInventoryRuntime().GetDamageFromEquipment();
            }
        

            float result = ((baseWpnFactor) + baseStatFactor);
            int rounded = Mathf.FloorToInt(result);

            PhysicalAttackResults phys = new PhysicalAttackResults(rounded, "Attack Value");
            return phys;
        }

        /// <summary>
        /// calculate the element resist value of the self
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override int GetElementResistValue(IActorHub attacker, IActorHub self, ElementType type)
        {
            int resist = self.MyStats.GetRuntimeAttributes().GetElementResist(type);//do something with level values if you want
            return resist;
        }

        /// <summary>
        /// calculate the take damage formula, the main call that determines physical and element reduction
        /// </summary>
        /// <param name="values"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public override CombatResults TakeDamageFormula(AttackValues values, IActorHub self)
        {
            IActorHub attacker = values.Attacker;
            List<ElementAttackResults> elements = values.ElementAttacks;
            List<PhysicalAttackResults> phys = values.PhysicalAttack;

            int totald = 0;

            List<string> sources = new List<string>();
            for (int i = 0; i < phys.Count; i++)
            {
                sources.Add(phys[i].Source);
                totald += phys[i].PhysicalDamage;

            }

            int critRando = UnityEngine.Random.Range(0, 101);
            float critFactor = 1;
            int critChance = attacker.MyStats.GetRuntimeAttributes().GetOtherAttributeNowValue(OtherAttributeType.CriticalHitChance);
            if (critRando <= (critChance))
            {
                //crit success
                int critdamage = attacker.MyStats.GetRuntimeAttributes().GetOtherAttributeNowValue(OtherAttributeType.CriticalHitDamage);

                critFactor = 1 + (((float)critChance * (float)critdamage)) / Formulas.Hundred;
                totald *= Mathf.RoundToInt(critFactor);

            }


            int resisted = GetArmorValue(attacker, self);
            int reduced = totald - resisted;
            //clucalte crit

           
            

            PhysicalDamageReport physreport = new PhysicalDamageReport(totald, resisted, reduced, sources, critFactor > 1);
            List<ElementalDamageReport> elereport = new List<ElementalDamageReport>();
            foreach (ElementType pieceType in Enum.GetValues(typeof(ElementType)))
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].Type == pieceType)
                    {
                        //found it
                        bool exists = false;
                        for (int j = 0; j < elereport.Count; j++)
                        {
                            if (elereport[j].Type == elements[i].Type)
                            {
                                //has already
                                exists = true;
                                elereport[j].TotalDamage += elements[i].Damage;
                                elereport[j].Sources.Add(elements[i].Source);
                                break;
                            }

                           
                        }

                        if (exists == false)
                        {
                            elereport.Add(new ElementalDamageReport(elements[i].Type, elements[i].Damage, new List<string>(1) { elements[i].Source }));
                        }
                    }
                }
            }

            for (int i = 0; i < elereport.Count; i++)
            {
                elereport[i].Resisted = GetElementResistValue(attacker, self, elereport[i].Type);
                elereport[i].Result = elereport[i].TotalDamage - elereport[i].Resisted;
            }

            DamageValues damagevalues = new DamageValues(physreport, elereport, self.MyHealth);
            CombatResults results = new CombatResults(values, damagevalues);
            return results;
        }
    }


}