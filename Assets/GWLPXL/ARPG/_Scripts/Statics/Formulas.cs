
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;

using GWLPXL.ARPGCore.StatusEffects.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Statics.com
{
   
   
    public static class Formulas
    {
        public const int Hundred = 100;

        //if a dungeon master doesn't exist, these are used
        #region
        public const float MobLevelMultiplier = 1;
        const float MobXPMultiplier = 1;
        const float ILevelMultiplier = 20;
        const float ShopLevelMultiplier = 1;
        //used to random range for equipment traits
        const float lowerRandomRange = .9f;
        const float upperRandomRange = 1.1f;
        #endregion

        /// <summary>
        /// adds the parameters to the existing amounts.
        /// </summary>
        /// <param name="moblevel"></param>
        /// <param name="xpmmulti"></param>
        /// <param name="itemmulti"></param>
        public static void ModifyDungeonVars(float moblevel, float xpmmulti, float itemmulti)
        {
            DungeonVariables variables = DungeonMaster.Instance.Variables;
            variables.MobLevelMultiplier += moblevel;
            variables.ItemLevelMultiplier += itemmulti;
            variables.MobXPMultiplier += xpmmulti;
        }

        public static int GetEnemyXP(int baseValue)
        {
            float dungeonXPMulti = MobXPMultiplier;
            if (DungeonMaster.Instance != null)
            {
                dungeonXPMulti = DungeonMaster.Instance.Variables.MobXPMultiplier;
            }
          
            return Mathf.FloorToInt(baseValue * dungeonXPMulti);
        }

        public static int GetILevelMulti(int baseValue)
        {
            float iLevelMulti = ILevelMultiplier;
            if (DungeonMaster.Instance != null)
            {
                iLevelMulti = DungeonMaster.Instance.Variables.ItemLevelMultiplier;
            }
          
            return Mathf.FloorToInt(baseValue * iLevelMulti);
        }
        public static int GetShopLevel(int baseValue)
        {
            float shopMulti = ShopLevelMultiplier;
            if (DungeonMaster.Instance != null)
            {
                shopMulti = DungeonMaster.Instance.Variables.ShopLevelMultipler;
            }
            return Mathf.FloorToInt(baseValue * shopMulti);
        }
        public static int GetEnemyLevel(int baseValue)
        {
            float mobMulti = MobLevelMultiplier;
            if (DungeonMaster.Instance != null)
            {
                mobMulti = DungeonMaster.Instance.Variables.MobLevelMultiplier;
            }
           

            return Mathf.FloorToInt(baseValue * mobMulti);
        }
        public static int ConvertToInt(float value)
        {
            int conversaion = Mathf.FloorToInt(value * Hundred);
            return conversaion;
        }
        public static float RoundFloat(float value)
        {
            return value / (float)Formulas.Hundred;
        }
        public static int GetEquipmentTraitILevel(int iLevel)
        {
            float lowerRangeRando = lowerRandomRange;
            if (DungeonMaster.Instance != null)
            {
                lowerRangeRando = DungeonMaster.Instance.Variables.LootTraitVariance.LowerRandomRange;
            }
            float upperRangeRando = upperRandomRange;
            if (DungeonMaster.Instance != null)
            {
                upperRangeRando = DungeonMaster.Instance.Variables.LootTraitVariance.UpperRandomRange;
            }
            float floatLevel = (float)iLevel;
            float lowerRange = floatLevel * lowerRangeRando;
            float upperRange = floatLevel * upperRangeRando;
            float _value = UnityEngine.Random.Range(lowerRange, upperRange);//.9 and 1.1 are the variance
            int rounded = Mathf.RoundToInt(_value);
            return 1 * rounded;
        }

    

      
     

      
       
       
       

      
        /// <summary>
        /// returns total ele dmg done
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="damageTarget"></param>
        /// <param name="damageArray"></param>
     

        public static bool AddDOTS(IActorHub target, ModifyResourceVars[] damageOverTimeOptions)
        {
            if (target == null) return false;

                for (int i = 0; i < damageOverTimeOptions.Length; i++)
                {
                SoTHelper.AddDoT(target, damageOverTimeOptions[i]);
                //target.MyStatusEffects.AddDoT(damageOverTimeOptions[i]);
                }
            return true;
            
        }
     
      

        public static float GetPlayerSkillFactor(IAttributeUser player, IAbilityUser playerAbilities)
        {
            float baseSkill = 0;
            int skillMods = 0;
            float total = 1;
            if (playerAbilities != null)
            {
                if (playerAbilities.GetLastIntendedAbility() != null)
                {
                    baseSkill = playerAbilities.GetLastIntendedAbility().GetDamageMultiplier();
                    baseSkill = Mathf.Round(baseSkill * Hundred) / Hundred;
                    skillMods = player.GetRuntimeAttributes().GetAbilityMod(playerAbilities.GetLastIntendedAbility());
                    total = baseSkill + Mathf.Round(skillMods * Hundred) / Hundred;
                }

            }
            return total;
        }




        #region old original
        /*
        public static void PlayerAttackResult(Player player, Enemy enemy)
        {

            float baseStatFactor;
            float baseDamageFactor;
            float critFactor;
            float skillFactor;

           



            //base stat factor * base damage factor * crit factor * skill factor
            ActorStats stats = player.GetRuntimeStats();

            #region crits
            //crits
            float baseCrit = stats.GetCritChance();
            baseCrit = Mathf.Round(baseCrit * hundred) / hundred;
            float critdamage = stats.GetCritDamage();
            critdamage = Mathf.Round(critdamage * hundred) / hundred;
            critFactor = (baseCrit * critdamage) + 1;
            // Debug.Log(baseCrit + " base crit");
            //Debug.Log(critdamage + " crit dmg");
            //Debug.Log(critFactor + "critfactor");
            int critRando = Random.Range(0, 101);
            float critChance = stats.GetCritChance();
            if (critRando <= (critChance * hundred))
            {
                //we crit;
                Debug.Log("Crit!");
            }
            // Debug.Log("Crit Rando: " + critRando);
            //  Debug.Log("crit chance: " + critChance);
            //   Debug.Log("Converted: " + critChance * hundred);
            #endregion

            //from primary stat
            baseStatFactor = stats.GetBaseStatRunningValue();//current base stat value divide by 100
            baseStatFactor = baseStatFactor / hundred;

            //from equipment
            Debug.Log(baseStatFactor + " Base stat factor");
            baseDamageFactor = player.GetInventory().GetDamageFromEquipment();

            //resolve

            //base damage mitigation


 

            //from skills
            float baseSkill = player.GetActiveSkill().GetDamageMultiplier();
            baseSkill = Mathf.Round(baseSkill * hundred) / hundred;
            float skillMods = stats.GetSkillMod(player.GetActiveSkill());
            skillMods = baseSkill + (skillMods / hundred);

            //from elements
            float elementMods = stats.GetElementAttack();
            elementMods = elementMods / hundred;
        

            float result = (baseDamageFactor) + (1 + baseStatFactor *  skillMods *  elementMods);


            Debug.Log("being result");
            Debug.Log(baseStatFactor);
            Debug.Log(baseDamageFactor);
            Debug.Log(skillMods);
            Debug.Log(elementMods);
            Debug.Log(result + " result");



            
            // DR = 1 - (1 - x) * (1 - y) * (1 - z)

            //DR represents damage reduction. 1 represents you. The variables x,y,z represent % damage reduction you gain from items or abilities.

           // Damage reduction from armor is = (your armor) / (50 * enemyLevel + your armor).

//Damage reduction from resist is = (your resist)/ (5 * enemyLevel + your resist).


        }*/
        #endregion
    }
}
