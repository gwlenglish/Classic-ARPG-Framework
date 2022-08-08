using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Items.com;


using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Statics.com
{


    public static class ModHelper 
    {
        #region aoe knockback
        public static void RemoveKnockbackAOEMod(IActorHub target, Dictionary<Transform, Knockback_AOE> dic)
        {
            IMeleeCombatUser meleeT = target.MyMelee;
            if (meleeT == null) return;
            Transform[] combatT = meleeT.GetMeleeTransforms();
            if (combatT == null || combatT.Length == 0) return;
            for (int i = 0; i < combatT.Length; i++)
            {
                if (combatT[i] == null) continue;

                dic.TryGetValue(combatT[i], out Knockback_AOE value);
                if (value != null)
                {
                    Component.Destroy(value);
                    dic.Remove(combatT[i]);
                }
            }
        }
        public static void ApplyKnockbackAOEMod(IActorHub target, KnockbackAOEVars Vars, Dictionary<Transform, Knockback_AOE> dic)
        {
            Transform[] meleeT = target.MyMelee.GetMeleeTransforms();
            for (int i = 0; i < meleeT.Length; i++)
            {
                if (meleeT[i] == null) continue;

                dic.TryGetValue(meleeT[i], out Knockback_AOE knockback);
                if (knockback == null)
                {

                    knockback = meleeT[i].gameObject.AddComponent<Knockback_AOE>();
                    AoEWeapoNVars aoe = new AoEWeapoNVars(Vars.AOE.Radius, Vars.AOE.Angle);
                    KnockBackVars knockbackvars = new KnockBackVars();
                    KnockbackAOEVars aoeknock = new KnockbackAOEVars(aoe, knockbackvars);
                    knockback.Vars = aoeknock;
                    IWeaponModification statusChange = knockback.GetComponent<IWeaponModification>();
                    statusChange.SetActive(true);
                    dic.Add(meleeT[i], knockback);
                }
                else
                {
                    knockback.GetComponent<IWeaponModification>().SetActive(true);
                }
            }

        }
        #endregion
        #region generate resource
        public static void ApplyGenerateResourceOnHitMod(IActorHub target, GWLPXL.ARPGCore.StatusEffects.com.GenerateRourseOnHitVars Vars, Dictionary<Transform, GenerateResourceOnHit> dic)
        {
            Transform[] meleeT = target.MyMelee.GetMeleeTransforms();
            for (int i = 0; i < meleeT.Length; i++)
            {
                dic.TryGetValue(meleeT[i], out GenerateResourceOnHit knockback);
                if (knockback == null)
                {
                    GenerateResourceOnHit generate = meleeT[i].gameObject.AddComponent<GenerateResourceOnHit>();
                    IWeaponModification statusChange = generate.GetComponent<IWeaponModification>();
                    statusChange.SetActive(true);
                    generate.Vars = Vars;
                    dic[meleeT[i]] = generate;
                }
                else
                {
                    knockback.GetComponent<IWeaponModification>().SetActive(true);
                }
            }

        }

        public static void RemoveGenerateResourceOnHitMod(IActorHub target, Dictionary<Transform, GenerateResourceOnHit> dic)
        {
            IMeleeCombatUser meleeT = target.MyMelee;
            if (meleeT == null) return;
            Transform[] combatT = meleeT.GetMeleeTransforms();
            if (combatT == null || combatT.Length == 0) return;
            for (int i = 0; i < combatT.Length; i++)
            {
                dic.TryGetValue(combatT[i], out GenerateResourceOnHit value);
                if (value != null)
                {
                    Component.Destroy(value);
                    dic.Remove(combatT[i]);
                }
            }
        }
        #endregion
        #region explosive mod
        public static void ApplyExplosiveMod(IActorHub target, ExplosionVars Vars, Dictionary<Transform, ExplosiveMod> dic)
        {
            Transform[] meleeT = target.MyMelee.GetMeleeTransforms();
            for (int i = 0; i < meleeT.Length; i++)
            {
                
                dic.TryGetValue(meleeT[i], out ExplosiveMod value);
                if (value == null)
                {
                    ExplosiveMod generate = meleeT[i].gameObject.AddComponent<ExplosiveMod>();
                    IWeaponModification statusChange = generate.GetComponent<IWeaponModification>();
                    statusChange.SetActive(true);
                    generate.Vars = Vars;
                    dic[meleeT[i]] = generate;
                }
                else
                {
                    value.GetComponent<IWeaponModification>().SetActive(true);
                }
            }

        }

        public static void RemoveExplosiveMod(IActorHub target, Dictionary<Transform, ExplosiveMod> dic)
        {
            foreach (var kvp in dic)
            {
                Debug.Log(kvp.Value);
                Debug.Log(kvp.Key);
            }
            IMeleeCombatUser meleeT = target.MyMelee;
            if (meleeT == null)
            {
                Debug.Log("No melee combat user");
            }
            Transform[] combatT = meleeT.GetMeleeTransforms();
            if (combatT == null || combatT.Length == 0)
            {
                Debug.Log("No transform to remove from ability");
            }
            for (int i = 0; i < combatT.Length; i++)
            {
                Debug.Log(combatT[i].name);
                dic.TryGetValue(combatT[i], out ExplosiveMod value);
                if (value != null)
                {
                    Component.Destroy(value);
                    dic.Remove(combatT[i]);
                }
            }
        }
        #endregion
        #region knockback mod
        public static void ApplyKnockbackMod(IActorHub target, KnockBackVars Vars, Dictionary<Transform, Knockback> dic)
        {
            Transform[] meleeT = target.MyMelee.GetMeleeTransforms();
            for (int i = 0; i < meleeT.Length; i++)
            {
                dic.TryGetValue(meleeT[i], out Knockback value);
                if (value == null)
                {
                    Knockback generate = meleeT[i].gameObject.AddComponent<Knockback>();
                    IWeaponModification statusChange = generate.GetComponent<IWeaponModification>();
                    statusChange.SetActive(true);
                    generate.Vars = Vars;
                    dic[meleeT[i]] = generate;
                }
                else
                {
                    value.GetComponent<IWeaponModification>().SetActive(true);
                }
            }

        }

        public static void RemoveKnockbackMod(IActorHub target, Dictionary<Transform, Knockback> dic)
        {
            IMeleeCombatUser meleeT = target.MyMelee;
            if (meleeT == null) return;
            Transform[] combatT = meleeT.GetMeleeTransforms();
            if (combatT == null || combatT.Length == 0) return;
            for (int i = 0; i < combatT.Length; i++)
            {
                dic.TryGetValue(combatT[i], out Knockback value);
                if (value != null)
                {
                    Component.Destroy(value);
                    dic.Remove(combatT[i]);
          
                }
            }
        }
        #endregion
        #region elemental wpn buff

        public static void ApplyElementalWpnDmgBuff(IActorHub target, ElementalWpnBuffVars vars, Dictionary<IActorHub, BuffedActor> Buffed)
        {
            if (target.MyInventory == null) return;
            Buffed.TryGetValue(target, out BuffedActor value);
            if (value == null)
            {
                //buffs the actor based on their damage from equipment
                int currentD = target.MyInventory.GetInventoryRuntime().GetDamageFromEquipment();
                float getNewElementAttack = (float)currentD * vars.PercentOfWpnDmg;
                int rounded = Mathf.FloorToInt(getNewElementAttack);
                //Debug.Log(rounded + "element attack");
                BuffedActor buffedA = new BuffedActor(vars.Element, rounded, currentD);
                Buffed[target] = buffedA;
                target.MyStats.GetRuntimeAttributes().ModifyElementAttackBaseValue(vars.Element, rounded);
                //Debug.Log("Modfiy element " + getNewElementAttack);
            }

        }

        public static void RemoveElementalWpnDmgBuff(IActorHub target, ElementalWpnBuffVars vars, Dictionary<IActorHub, BuffedActor> Buffed)
        {
            IInventoryUser invUser = target.MyInventory;
            if (invUser == null) return;
            Buffed.TryGetValue(target, out BuffedActor value);
            if (value != null)
            {
                target.MyStats.GetRuntimeAttributes().ModifyElementAttackBaseValue(value.Type, -value.Amount);
                Buffed.Remove(target);
            }
        }
        #endregion
        #region element flat buff

        public static void ApplyElementBuffFlat(IActorHub target, ElementBuffFlatVars vars, Dictionary<ActorAttributes, int> Buffed)
        {
            ActorAttributes stats = target.MyStats.GetRuntimeAttributes();
            Buffed.TryGetValue(stats, out int value);
            if (value == 0)
            {
                stats.ModifyElementAttackBaseValue(vars.Element, vars.BuffAmount);
                Buffed[stats] = vars.BuffAmount;

            }
            ARPGDebugger.DebugMessage("buffed " + vars.Element + " " + vars.BuffAmount + stats.ActorName, stats);
        }

        public static void RemoveElementBuffFlat(IActorHub target, ElementBuffFlatVars vars, Dictionary<ActorAttributes, int> Buffed)
        {
            ActorAttributes stats = target.MyStats.GetRuntimeAttributes();
            Buffed.TryGetValue(stats, out int value);
            if (value != 0)
            {
                stats.ModifyElementAttackBaseValue(vars.Element, -value);
                Buffed.Remove(stats);
            }
            ARPGDebugger.DebugMessage("removed " + vars.Element + " " + -vars.BuffAmount + stats.ActorName, stats);
        }
        #endregion


        public static void ApplyStatFlatMod(IActorHub target, StatModFlatVars vars, Dictionary<ActorAttributes, int> Buffed)
        {
            IAttributeUser statUser = target.MyStats;
            if (statUser == null) return;
            ActorAttributes stats = statUser.GetRuntimeAttributes();
            Buffed.TryGetValue(stats, out int value);
            if (value == 0)
            {
                stats.ModifyBaseStatValue(vars.Type, vars.Amount);
                Buffed[stats] = vars.Amount;
            }
        }
        public static void RemoveStatFlatMod(IActorHub target, StatModFlatVars vars, Dictionary<ActorAttributes, int> Buffed)
        {
            IAttributeUser statUser = target.MyStats;
            if (statUser == null) return;
            ActorAttributes stats = statUser.GetRuntimeAttributes();
            Buffed.TryGetValue(stats, out int value);
            if (value != 0)
            {
                stats.ModifyBaseStatValue(vars.Type, -vars.Amount);
                Buffed.Remove(stats);
            }
        }

        public static void ApplyStatPercentMod(IActorHub target, StatModPercentVars vars, Dictionary<ActorAttributes, StatIncreased> Buffed)
        {
            if (target.MyStats == null) return;
            ActorAttributes stats = target.MyStats.GetRuntimeAttributes();
            Buffed.TryGetValue(stats, out StatIncreased value);
            if (value == null)
            {
                float increaseAmount = stats.GetStatNowValue(vars.Stat) * vars.ByPercent;
                int rounded = Mathf.FloorToInt(increaseAmount);

                stats.ModifyBaseStatValue(vars.Stat, rounded);
                StatIncreased increased = new StatIncreased(vars.Stat, rounded);
                Buffed[stats] = increased;
            }
        }

        public static void RemoveStatPercentMod(IActorHub target, StatModPercentVars vars, Dictionary<ActorAttributes, StatIncreased> Buffed)
        {
            if (target.MyStats == null) return;
            ActorAttributes stats = target.MyStats.GetRuntimeAttributes();
            Buffed.TryGetValue(stats, out StatIncreased value);
            if (value != null)
            {
                stats.ModifyBaseStatValue(value.Stat, -value.ByAmount);
                Buffed.Remove(stats);
            }
        }
    }
}