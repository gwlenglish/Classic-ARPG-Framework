using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.GameEvents.com;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Leveling.com;

namespace GWLPXL.ARPGCore.Statics.com
{
    [System.Obsolete("Use CombatHandlers, will be removed")]
    public static class PlayerCombatResolution
    {

    }

    [System.Obsolete("Use CombatHandlers, will be removed")]
    public static class EnemyCombatResolution
    {
        [System.Obsolete("Use CombatHandlers, will be removed")]
        public static int GetEnemyReducedWeaponDamage(IAttributeUser enemy)
        {
            return GetEnemyArmor(enemy);
        }

        [System.Obsolete]
        public static Dictionary<ElementType, ElementAttackResults> GetEnemyElementalDamageResistChecks(IAttributeUser enemy, IAttributeUser player, ResourceType type)
        {
            //Dictionary<ElementType, ElementAttack> attackElements =  player.GetRuntimeStats().GetRuntimeElementValues();
            Dictionary<ElementType, ElementAttackResults> attackDic = new Dictionary<ElementType, ElementAttackResults>();
            Attribute[] attackElements = player.GetRuntimeAttributes().GetAttributes(AttributeType.ElementAttack);
            for (int i = 0; i < attackElements.Length; i++)
            {
                ElementAttack eleAttack = (ElementAttack)attackElements[i];
                ElementType eletype = eleAttack.Type;
                int attack = eleAttack.NowValue;
                if (attack <= 0) continue;//we didn't do any dmg with this element

                int resist = enemy.GetRuntimeAttributes().GetElementResist(eletype);

                int newDmg = attack - resist;
                int resistAmount = attack - resist;//if want to notify resist amounts

                if (newDmg < 0)
                {
                    newDmg = 0;
                }

                if (newDmg > 0)
                {
                    enemy.GetRuntimeAttributes().ModifyNowResource(type, -newDmg);
                    attackDic.TryGetValue(eletype, out ElementAttackResults value);
                    if (value == null)
                    {
                        value = new ElementAttackResults(eletype, newDmg, null);
                        attackDic[eletype] = value;
                    }
                    else
                    {
                        value.Damage += newDmg;

                    }
                }



            }

            return attackDic;


        }

        [System.Obsolete("Use CombatHandlers, will be removed")]
        public static int DoEnemyElementalDamageWithResistChecks(IAttributeUser enemy, ElementType _type, float elementDamage, ResourceType type)
        {
            float absolute = Mathf.Abs(elementDamage);
            float resist = enemy.GetRuntimeAttributes().GetElementResist(_type);
            float newDmg = absolute - resist;
            int rounded = Mathf.FloorToInt(newDmg);
            if (rounded > 0)
            {
                enemy.GetRuntimeAttributes().ModifyNowResource(type, -rounded);



            }

            ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Resist: " + resist, enemy.GetInstance());
            ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("new dmg: " + newDmg, enemy.GetInstance());
            ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("rounded d: " + rounded, enemy.GetInstance());

            return rounded;
        }

        [System.Obsolete("Use CombatHandlers, will be removed")]
        public static int DoEnemyPhysicalReducedChecks(
           IAttributeUser enemyStats,
           int damageAmount,
           ResourceType healthType)
        {

            int reducedWeaponDamage = GetEnemyArmor(enemyStats);
            float dmgValue = damageAmount - reducedWeaponDamage;
            float resisted = damageAmount - dmgValue;
            ARPGDebugger.DebugMessage(reducedWeaponDamage + " reduced weap", null);
            if (dmgValue < 0)
            {
                dmgValue = 0;
            }
            int dmgInt = Mathf.FloorToInt(dmgValue);


            if (dmgInt > 0)
            {
                enemyStats.GetRuntimeAttributes().ModifyNowResource(healthType, -dmgInt);
            }

            return dmgInt;

            //raise ui
        }

        [System.Obsolete("Use CombatHandlers, will be removed")]
        private static int GetEnemyArmor(IAttributeUser enemy)
        {
            int scaled = 1;
            IScale scaler = enemy.GetInstance().GetComponent<IScale>();
            if (scaler != null)
            {
                scaled = scaler.GetScaledLevel();
            }
            return scaled + enemy.GetRuntimeAttributes().GetStatForCombat(CombatStatType.Armor);

        }
    }


    [System.Obsolete("Use the Combat Handlers. This will be removed")]
    public static class CombatResolution
    {
        [System.Obsolete]
        static int GetReducedElementDamage(IAttributeUser player, ElementType _type, float elementDamage)
        {
            if (player == null) return Mathf.FloorToInt(elementDamage);//if not attributes, can't resist so take full amount
            float resist = player.GetRuntimeAttributes().GetElementResist(_type);
            float newDmg = elementDamage - resist;
            int rounded = Mathf.RoundToInt(newDmg);
            return rounded;


        }

        [System.Obsolete]
        public static int DoBreakableDamage(IAttributeUser victim, int amount, ResourceType type)
        {
            amount = Mathf.Abs(amount);
            //in case we want to do anything special for environment damage
            victim.GetRuntimeAttributes().ModifyNowResource(type, -amount);
            return amount;

        }

        [System.Obsolete]
        public static int DoElementalMeleeDamage(IAttributeUser caster, IReceiveDamage damageTarget)
        {
            List<ElementAttackValues> values = CombatStats.GetActorElementAttackValues(caster);
            return DefaultElementalDamageBehavior(damageTarget, values.ToArray());
        }

        [System.Obsolete]
        public static int DoElementalMeleeDamage(IAttributeUser caster, IReceiveDamage damageTarget, ElementDamageMultiplierActor[] damageArray)
        {
            return DefaultElementalDamageBehavior(caster, damageTarget, damageArray);
        }

        [System.Obsolete]
        public static int DoElementalDamageOverTime(IReceiveDamage damageTarget, ElementDamageMultiplierNoActor[] damageArray)
        {
            return DefaultElementalDamageBehavior(damageTarget, damageArray);
        }

        [System.Obsolete]
        public static int DoElementalDamageOverTime(IAttributeUser caster, IReceiveDamage damageTarget, ElementDamageMultiplierActor[] damageArray)
        {
            return DefaultElementalDamageBehavior(caster, damageTarget, damageArray);
        }

        [System.Obsolete]
        public static int DoElementalProjectileDamage(IAttributeUser caster, IReceiveDamage damageTarget)
        {
            List<ElementAttackValues> values = CombatStats.GetActorElementAttackValues(caster);
            return DefaultElementalDamageBehavior(damageTarget, values.ToArray());

        }

        [System.Obsolete]
        public static int DoElementalProjectileDamage(IAttributeUser caster, IReceiveDamage damageTarget, ElementDamageMultiplierActor[] damageArray)
        {
            return DefaultElementalDamageBehavior(caster, damageTarget, damageArray);

        }

        [System.Obsolete]
        public static int DoPhysicalDamage(IActorHub attacker, IActorHub damageTarget)
        {
            if (attacker == null || attacker.MyTransform == null) return 0;
            int phys = CombatStats.GetTotalActorDamage(attacker.MyTransform.gameObject);
            return DefaultPhysicalDamageBehavior(attacker, phys, damageTarget);

        }

        [System.Obsolete]
        public static int DoPhysicalDamage(IActorHub attacker, IActorHub damageTarget, PhysicalDamageMultiplier physdmg)
        {
            return DefaultPhysicalDamageBehavior(attacker, physdmg, damageTarget);

        }

        [System.Obsolete]
        public static bool DetermineAttackable(IReceiveDamage target, IReceiveDamage attacker, bool friendlyFire)
        {
            return DefaultDetermineLogic(target, attacker, friendlyFire);

        }

        [System.Obsolete]
        public static int DoPlayerElementDamageWithResistChecks(IAttributeUser player, ElementType _type, float elementDamage, ResourceType type)
        {
            float resist = player.GetRuntimeAttributes().GetElementResist(_type);
            float newDmg = elementDamage - resist;
            int rounded = Mathf.RoundToInt(newDmg);
            if (rounded > 0)
            {
                player.GetRuntimeAttributes().ModifyNowResource(type, -rounded);

            }


            return -rounded;


        }

        [System.Obsolete("Use CombatHandlers, will be removed")]
        public static int DoPlayerReducedElemental(IReceiveDamage damageTaker, IAttributeUser damageDealer, ResourceType healthResource, TookDamageEvent customEvent)
        {
            return 0;
        }

        [System.Obsolete]
        public static int DoPlayerReducedPhysical(IAttributeUser playerStats, IInventoryUser playerInv, int enemyLevel, float fullDamageAmount)
        {
            float mobMulti = Formulas.MobLevelMultiplier;
            if (DungeonMaster.Instance != null)
            {
                mobMulti = DungeonMaster.Instance.Variables.MobLevelMultiplier;
            }

            float DR = CombatStats.GetPlayerArmorAmount(playerStats, playerInv) / (mobMulti * enemyLevel + CombatStats.GetPlayerArmorAmount(playerStats, playerInv));
            float reducedPhysical = fullDamageAmount - DR;
            int rounded = Mathf.RoundToInt(reducedPhysical);
            playerStats.GetRuntimeAttributes().ModifyNowResource(ResourceType.Health, -rounded);
            return -rounded;
        }

        [System.Obsolete]
        private static bool DefaultDetermineLogic(IReceiveDamage target, IReceiveDamage attacker, bool friendlyFire)
        {
           // if (attacker == null) return true;//always lie the attack through if an attacker group isn't set
           // if (target == null) return false;//dont allow if there's no valid target

            if (friendlyFire == false)//allow any
            {
                CombatGroupType[] attackedgroup = target.GetMyCombatGroup();
                CombatGroupType[] attackergroup = attacker.GetMyCombatGroup();
                if (attackergroup.Intersect(attackedgroup).Any()) return false;

            }
            return true;
        }

        [System.Obsolete]
        private static int DefaultPhysicalDamageBehavior(IActorHub attacker, int dmg, IActorHub damageTarget)
        {
           // int dmg = physdmg.GetPhysicalDamageAmount(attacker);
  
            //damageTarget.MyHealth.TakeDamage(dmg, attacker);
            return dmg;
        }

        [System.Obsolete]
        private static int DefaultPhysicalDamageBehavior(IActorHub attacker, PhysicalDamageMultiplier physdmg, IActorHub damageTarget)
        {
            int dmg = physdmg.GetPhysicalDamageAmount(attacker);
         //   damageTarget.MyHealth.TakeDamage(dmg, attacker);
            return dmg;
        }

        [System.Obsolete]
        private static int DefaultElementalDamageBehavior(IReceiveDamage damageTarget, ElementAttackValues[] damageArray)
        {
            float multi = 1;
            int baseprojectile = 0;
            int actorattack = 0;
            int combinedmdg = 0;
            ElementType type = ElementType.None;
            int total = 0;
            for (int i = 0; i < damageArray.Length; i++)
            {
                actorattack = 0;
                baseprojectile = 0;
                multi = 1;
                type = ElementType.None;

                baseprojectile = damageArray[i].AttackDamage;
                type = damageArray[i].Type;

                combinedmdg = Mathf.FloorToInt(baseprojectile + actorattack * multi);

                //Debug.Log(combinedmdg + " combined");
                //Debug.Log(baseprojectile + " baseP");
                //Debug.Log(actorattack + " actorAtt");
                //Debug.Log(multi + " multi");

                if (combinedmdg > 0)
                {
                   // damageTarget.TakeDamage(combinedmdg, type);
                    total += combinedmdg;
                }
            }

            return total;
        }

        [System.Obsolete]
        private static int DefaultElementalDamageBehavior(IReceiveDamage damageTarget, ElementDamageMultiplierNoActor[] damageArray)
        {
            float multi = 1;
            int baseprojectile = 0;
            int actorattack = 0;
            int combinedmdg = 0;
            ElementType type = ElementType.None;
            int total = 0;
            for (int i = 0; i < damageArray.Length; i++)
            {
                actorattack = 0;
                baseprojectile = 0;
                multi = 1;
                type = ElementType.None;

                baseprojectile = damageArray[i].BaseElementDamage;
                type = damageArray[i].DamageType;

                combinedmdg = Mathf.FloorToInt(baseprojectile + actorattack * multi);

                //Debug.Log(combinedmdg + " combined");
                //Debug.Log(baseprojectile + " baseP");
                //Debug.Log(actorattack + " actorAtt");
                //Debug.Log(multi + " multi");

                if (combinedmdg > 0)
                {
                   // damageTarget.TakeDamage(combinedmdg, type);
                    total += combinedmdg;
                }
            }

            return total;
        }

        [System.Obsolete]
        private static int DefaultElementalDamageBehavior(IAttributeUser caster, IReceiveDamage damageTarget, ElementDamageMultiplierActor[] damageArray)
        {
            List<ElementAttackValues> actorele = CombatStats.GetActorElementAttackValues(caster);
            float multi = 1;
            int baseprojectile = 0;
            int actorattack = 0;
            int combinedmdg = 0;
            ElementType type = ElementType.None;
            int total = 0;
            for (int i = 0; i < damageArray.Length; i++)
            {
                actorattack = 0;
                baseprojectile = 0;
                multi = 1;
                type = ElementType.None;

                multi *= damageArray[i].PercentOfCasterElement;
                baseprojectile = damageArray[i].BaseElementDamage;
                type = damageArray[i].DamageType;



                for (int j = 0; j < actorele.Count; j++)
                {
                    if (actorele[j].Type == damageArray[i].DamageType)
                    {
                        actorattack = actorele[j].AttackDamage;
                        break;
                    }
                }



                combinedmdg = Mathf.FloorToInt(baseprojectile + actorattack * multi);

                //Debug.Log(combinedmdg + " combined");
                //Debug.Log(baseprojectile + " baseP");
                //Debug.Log(actorattack + " actorAtt");
                //Debug.Log(multi + " multi");

                if (combinedmdg > 0)
                {
                  //  damageTarget.TakeDamage(combinedmdg, type);
                    total += combinedmdg;
                }
            }

            return total;
        }
    }

    public class ElementAttackValues
    {
        public ElementType Type;
        public int AttackDamage;
        public ElementAttackValues(ElementType type, int attackdamage)
        {
            Type = type;
            AttackDamage = attackdamage;
        }
    }
}
