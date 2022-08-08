
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Statics.com
{
    public static class CombatStats
    {
    
        public static List<ElementAttackValues> GetActorElementAttackValues(IAttributeUser forUser)
        {
            List<ElementAttackValues> temp = new List<ElementAttackValues>();
            if (forUser == null) return temp;
            foreach (ElementType pieceType in System.Enum.GetValues(typeof(ElementType)))
            {
                if (pieceType == ElementType.None) continue;
                int attack = GetActorElementDamage(forUser, pieceType);
                temp.Add(new ElementAttackValues(pieceType, attack));
            }
            return temp;

        }
        public static int GetActorElementDamage(IAttributeUser forUser, ElementType damageType)
        {
            return forUser.GetRuntimeAttributes().GetElementAttack(damageType);
        }
        public static int GetTotalActorDamage(GameObject toGet)
        {
            if (toGet == null) return 0;
            IActorHub actorhub = toGet.GetComponent<IActorHub>();
            IAttributeUser stats = actorhub.MyStats;
            if (actorhub.PlayerControlled != null)
            {
                IInventoryUser playerInv = actorhub.MyInventory;
                IAbilityUser playerAbilities = actorhub.MyAbilities;
                return GetTotalPlayerAttackDamage(stats, playerInv, playerAbilities);
            }
            else
            {
                return GetEnemyAttackDamage(stats);
            }
        }
        public static int GetEnemyAttackDamage(IAttributeUser enemy)
        {
            //only using the base stat for damage
            return enemy.GetRuntimeAttributes().GetStatForCombat(CombatStatType.Damage);//the only thing considered in attack is the base stats. Higher values, more damage

        }
        public static int GetTotalPlayerAttackDamage(IAttributeUser playerStats, IInventoryUser playerInv, IAbilityUser playerAbilities)
        {
            int baseStatFactor;
            float baseWpnFactor;
            float critFactor;

            //base stat factor * base damage factor * crit factor * skill factor
            ActorAttributes stats = playerStats.GetRuntimeAttributes();

            //not implemented meaningfully yet
            #region crits
            //crits
            float baseCrit = stats.GetOtherAttributeNowValue(OtherAttributeType.CriticalHitChance);
            baseCrit = baseCrit / Formulas.Hundred;
            float critdamage = stats.GetOtherAttributeNowValue(OtherAttributeType.CriticalHitDamage);
            critdamage = critdamage / Formulas.Hundred;
            critFactor = 1 + (baseCrit * critdamage);//this seems wrong, but work on it later
            // Debug.Log(baseCrit + " base crit");
            //Debug.Log(critdamage + " crit dmg");
            //Debug.Log(critFactor + "critfactor");
            int critRando = Random.Range(0, 101);
            float critChance = stats.GetOtherAttributeNowValue(OtherAttributeType.CriticalHitChance);
            if (critRando <= (critChance * Formulas.Hundred))
            {
                //we crit;
                //ARPGDebugger.DebugMessage("Crit!");
            }
            // Debug.Log("Crit Rando: " + critRando);
            //  Debug.Log("crit chance: " + critChance);
            //   Debug.Log("Converted: " + critChance * hundred);
            #endregion

            //from primary stat
            //baseStatFactor = baseStatFactor / Hundred;

            //from equipment
            //Debug.Log(baseStatFactor + " Base stat factor");

            //resolve

            //from abilities
            float baseSkill = 0;
            float skillMods = 0;
            //ability mods
            if (playerAbilities != null)
            {
                if (playerAbilities.GetLastIntendedAbility() != null)
                {
                    baseSkill = playerAbilities.GetLastIntendedAbility().GetDamageMultiplier();
                    baseSkill = Mathf.Round(baseSkill * Formulas.Hundred) / Formulas.Hundred;
                    skillMods = stats.GetAbilityMod(playerAbilities.GetLastIntendedAbility());
                    skillMods = baseSkill + Mathf.Round(skillMods * Formulas.Hundred) / Formulas.Hundred;
                }

            }


            //from elements
            float elementMods = stats.GetAllElementAttackValues();
            elementMods = elementMods / Formulas.Hundred;
            baseStatFactor = stats.GetStatForCombat(CombatStatType.Damage);//current base stat value divide by 100
            baseWpnFactor = playerInv.GetInventoryRuntime().GetDamageFromEquipment();


            float result = (baseWpnFactor) + baseStatFactor + (1 * (float)skillMods) + (1 * (float)elementMods);//main dmg formula
            int rounded = Mathf.FloorToInt(result);


            ARPGDebugger.DebugMessage("being result", null);
            ARPGDebugger.DebugMessage(baseStatFactor + "base stat", null);
            ARPGDebugger.DebugMessage(baseWpnFactor + "base wpn", null);
            ARPGDebugger.DebugMessage(skillMods + "skill mods", null);
            ARPGDebugger.DebugMessage(elementMods + " ele mods", null);
            ARPGDebugger.DebugMessage(result + " result", null);

            return rounded;





        }
        public static float GetPlayerArmorAmount(IAttributeUser player, IInventoryUser playerInv)
        {
            float armorAmount = 0;
            armorAmount += player.GetRuntimeAttributes().GetStatForCombat(CombatStatType.Armor);
            armorAmount += playerInv.GetInventoryRuntime().GetArmorFromEquipment();
            return armorAmount;
        }
    }
}
