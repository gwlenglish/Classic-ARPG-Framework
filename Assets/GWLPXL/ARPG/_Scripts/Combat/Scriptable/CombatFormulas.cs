using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.DebugHelpers.com;

using GWLPXL.ARPGCore.StatusEffects.com;
using System.Linq;


namespace GWLPXL.ARPGCore.Combat.com
{
    /// <summary>
    /// Formulas for combat. Inherit and override to create your own.
    /// </summary>
    /// 
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Combat/CombatFormulas")]
    public class CombatFormulas : CommonCombatFormulas
    {
        public PlayerDefault PlayerCombat = null;
        public EnemyDefault EnemyCombat = null;


        #region public override methods
        /// <summary>
        /// performs the projectile damage
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="damageDealer"></param>
        /// <param name="damageTarget"></param>
        /// <param name="damage"></param>
        /// <param name="projectileOptions"></param>
        public override AttackValues GetProjectileDamage(AttackValues values, IActorHub owner, IDoDamage damageDealer, IActorHub damageTarget, IDoActorDamage damage, IProjectile projectileOptions)
        {
            values.PhysicalAttack.Add(GetPhysicalAttackValue(owner));

            if (damage.GetActorDamageData().DamageVar.DamageOptions.InflictPhysicalDmg)
            {
                int phys = damage.GetActorDamageData().DamageVar.DamageMultipliers.PhysMultipler.GetPhysicalDamageAmount(owner);
                values.PhysicalAttack.Add(new PhysicalAttackResults(phys, "Projectile"));
                damage.GetActorDamageEvents().SceneEvents.OnPhysicalDamagedOther.Invoke(values, damageTarget.MyHealth);

            }

            if (damage.GetActorDamageData().DamageVar.DamageOptions.InfictElementalDmg)
            {
                for (int i = 0; i < damage.GetActorDamageData().DamageVar.DamageMultipliers.ElementMultiplers.Length; i++)
                {
                    ElementDamageMultiplierActor elev = damage.GetActorDamageData().DamageVar.DamageMultipliers.ElementMultiplers[i];
                    values.ElementAttacks.Add(new ElementAttackResults(elev.DamageType, elev.GetElementDamageAmount(owner), "Projectile"));
                    damage.GetActorDamageEvents().SceneEvents.OnElementalDamageOther.Invoke(values, damageTarget.MyHealth);
                }



            }

            int modlength = damageDealer.GetWeaponMods().Length;
            for (int i = 0; i < modlength; i++)
            {
                damageDealer.GetWeaponMods()[i].DoModification(values);
            }

            damageDealer.GetDamagedList().Add(damageTarget);

            if (projectileOptions.GetProjectileData().ProjectileVars.DisableOnTouch && projectileOptions.Disabled == false)//buffered destroy, destroy happens in the dotick check
            {
                projectileOptions.Disabled = true;
            }

            damageTarget.MyHealth.SetCharacterThatHitMe(owner);
            return values;
        }

        /// <summary>
        /// performs any SOT exit logic 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="sots"></param>
        public override void OnExitSoTLogic(IActorHub caster, IActorHub target, IDoActorDamage damage, IApplySOT sots)
        {
            //fail conditions
            if (target == null) return;
            if (damage.GetActorDamageData().DamageVar.DamageOptions.InflictSoT == false) return;
            if (damage.GetActorDamageData().DamageVar.SoTOptions.ApplyAtExit == false) return;

            //removal and any final damage for the dot
            //get all sots the applysot currently has applied, remove the ones that match our target

            AttackValues values = new AttackValues(caster, target, true);
            for (int i = 0; i < sots.GetSOTS().Count; i++)
            {
                if (sots.GetSOTS()[i].Attackable == target)
                {

                    values = CombatHelper.GetElementalDamageNoActor(values, damage.GetActorDamageData().DamageVar.SoTOverTimeMultipliers.ElementalMultipliers);

                    damage.GetActorDamageEvents().SceneEvents.OnElementalDamageOther.Invoke(values, target.MyHealth);

                    bool dotsadded = damage.GetActorDamageData().DamageVar.CombatHandler.AddDOTS(caster, target, damage.GetActorDamageData().DamageVar.SoTOptions.AdditionalDOTs);
                    if (dotsadded)
                    {
                        sots.GetSOTEvents().SceneEvents.OnSoTApply.Invoke(target.MyStatusEffects);
                    }
                    sots.GetSOTS().RemoveAt(i);
                    break;
                }
            }

            target.MyHealth.TakeDamage(values);
        }
        /// <summary>
        /// checks if valid target
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="sots"></param>
        /// <returns></returns>
        public override bool CanApplySoT(IActorHub owner, IActorHub target, IDoActorDamage damage, IApplySOT sots)
        {
            if (target == null) return false;
            if (target.MyStatusEffects == null) return false;
            if (damage.GetActorDamageData().DamageVar.DamageOptions.InflictSoT == false) return false;
            if (damage.GetActorDamageData().DamageVar.CombatHandler.DetermineAttackable(target, owner, damage.GetActorDamageData().DamageVar.SoTOptions.FriendlyFIre) == false) return false;//cant attack
            if (sots.GetSoTAppliedList().Contains(target)) return false;//only allow one application per active swing
            return true;
        }
        /// <summary>
        /// performs enter logic
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="sots"></param>
        public override void OnEnterSotLogic(IActorHub caster, IActorHub target, IDoActorDamage damage, IApplySOT sots)
        {
            if (CanApplySoT(caster, target, damage, sots) == false) return;


            ///check iapplysot, have we applied our sot already? if so, add dot (stack)
            bool foundonlist = false;
            for (int i = 0; i < sots.GetSOTS().Count; i++)
            {
                if (sots.GetSOTS()[i].Attackable == target.MyHealth)
                {
                    foundonlist = true;
                    //alraedy on the list, reapply?
                    bool dotsadded = damage.GetActorDamageData().DamageVar.CombatHandler.AddDOTS(caster, sots.GetSOTS()[i].ActorHub, damage.GetActorDamageData().DamageVar.SoTOptions.AdditionalDOTs);
                    if (dotsadded)
                    {
                        sots.GetSOTEvents().SceneEvents.OnSoTApply.Invoke(sots.GetSOTS()[i].StatusChange);
                    }
                    break;
                }
            }

            //if not, create a new sot and add it
            if (foundonlist == false)
            {
                AttackValues values = new AttackValues(caster, target, true);
                SOT newDot = new SOT(target);
                sots.GetSOTS().Add(newDot);
                if (damage.GetActorDamageData().DamageVar.SoTOptions.ApplyAtEnter)
                {

                    values = CombatHelper.GetElementalDamageNoActor(values, damage.GetActorDamageData().DamageVar.SoTOverTimeMultipliers.ElementalMultipliers);



                    damage.GetActorDamageEvents().SceneEvents.OnElementalDamageOther.Invoke(values, target.MyHealth);//currently need to change

                }
                if (damage.GetActorDamageData().DamageVar.SoTOptions.ApplyDotAtEnter)
                {
                    bool dotsadded = damage.GetActorDamageData().DamageVar.CombatHandler.AddDOTS(caster, target, damage.GetActorDamageData().DamageVar.SoTOptions.AdditionalDOTs);
                    if (dotsadded)
                    {
                        sots.GetSOTEvents().SceneEvents.OnSoTApply.Invoke(target.MyStatusEffects);
                    }
                }

                target.MyHealth.TakeDamage(values);

            }
            sots.GetSoTAppliedList().Add(target);

        }

        /// <summary>
        /// switches if player or enemy returns respective value from their formulas
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cancrit"></param>
        /// <returns></returns>
        public override PhysicalAttackResults GetPhysicalAttackValue(IActorHub self)
        {
            if (self.PlayerControlled != null)
            {
                return PlayerCombat.GetAttackValue(self);
            }
            else
            {
                return EnemyCombat.GetAttackValue(self);
            }
        }


        /// <summary>
        /// performs melee damage (defined by the melee damage box)
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="damager"></param>
        /// <param name="attacked"></param>
        /// <param name="actorDmg"></param>
        /// <param name="meleeOptions"></param>
        public override AttackValues GetMeleeActorDamageLogic(AttackValues results, IActorHub owner, IDoDamage damager, IActorHub attacked, IDoActorDamage actorDmg, IMeleeWeapon meleeOptions)
        {
            results.PhysicalAttack.Add(GetPhysicalAttackValue(owner));

            //get physical dmg
            if (actorDmg.GetActorDamageData().DamageVar.DamageOptions.InflictPhysicalDmg)
            {

                int physdmg = actorDmg.GetActorDamageData().DamageVar.DamageMultipliers.PhysMultipler.GetPhysicalDamageAmount(owner);
                results.PhysicalAttack.Add(new PhysicalAttackResults(physdmg, CombatHelper.Melee));
                actorDmg.GetActorDamageEvents().SceneEvents.OnPhysicalDamagedOther.Invoke(results, attacked.MyHealth);

            }

            //get ele dmg
            if (actorDmg.GetActorDamageData().DamageVar.DamageOptions.InfictElementalDmg)
            {
                for (int i = 0; i < actorDmg.GetActorDamageData().DamageVar.DamageMultipliers.ElementMultiplers.Length; i++)
                {
                    results.ElementAttacks.Add(new ElementAttackResults(actorDmg.GetActorDamageData().DamageVar.DamageMultipliers.ElementMultiplers[i].DamageType,
                        actorDmg.GetActorDamageData().DamageVar.DamageMultipliers.ElementMultiplers[i].GetElementDamageAmount(owner),
                        CombatHelper.Melee));
                    actorDmg.GetActorDamageEvents().SceneEvents.OnElementalDamageOther.Invoke(results, attacked.MyHealth);
                }


            }

            //get mods
            int modlength = damager.GetWeaponMods().Length;
            for (int i = 0; i < modlength; i++)//try moving this to the damage logic. hmmm
            {
                damager.GetWeaponMods()[i].DoModification(results);
            }

            //add target to list
            damager.GetDamagedList().Add(attacked);
            return results;

        }
        /// <summary>
        /// performs melee damage (defined by the melee damage box)
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="damageTarget"></param>
        /// <returns></returns>


        /// <summary>
        /// applies DoTs to target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damageOverTimeOptions"></param>
        /// <returns></returns>, just a wrapper for the SotHelper.AddDot
        public override bool AddDOTS(IActorHub caster, IActorHub target, ModifyResourceVars[] damageOverTimeOptions)
        {
            if (target == null) return false;

            for (int i = 0; i < damageOverTimeOptions.Length; i++)
            {
                SoTHelper.AddDoT(target, damageOverTimeOptions[i]);
            }
            return true;

        }
        /// <summary>
        /// determines if can attack
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attacker"></param>
        /// <param name="friendlyFire"></param>
        /// <returns></returns>
        public override bool DetermineAttackable(IActorHub target, IActorHub attacker, bool friendlyFire)
        {
            if (attacker == null) return false;
            if (target == null) return false;

            if (friendlyFire == true) return true;
            CombatGroupType[] attackedgroup = target.MyHealth.GetMyCombatGroup();
            CombatGroupType[] attackergroup = attacker.MyHealth.GetMyCombatGroup();
            if (attackergroup.Intersect(attackedgroup).Any()) return false;
            return true;



        }


        /// <summary>
        /// determines valid melee targets
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="damager"></param>
        /// <param name="actorDmg"></param>
        /// <param name="attacked"></param>
        /// <param name="meleeOptions"></param>
        /// <returns></returns>
        public override bool CanMeleeAttack(IActorHub owner, IDoDamage damager, IDoActorDamage actorDmg, IActorHub attacked, IMeleeWeapon meleeOptions)
        {
            if (owner == null) return false;
            if (attacked == null) return false;
            if (attacked == owner) return false;
            if (actorDmg.GetActorDamageData().DamageVar.DamageOptions.InfictElementalDmg == false && actorDmg.GetActorDamageData().DamageVar.DamageOptions.InflictPhysicalDmg == false) return false;
            if (actorDmg.GetActorDamageData().DamageVar.CombatHandler.DetermineAttackable(attacked, owner, meleeOptions.GetMeleeOptions().MeleeVars.FriendlyFire) == false) return false;//cant attack
            if (damager.GetDamagedList().Contains(attacked) == true) return false;
            return true;
        }

        /// <summary>
        /// determines valid project targets
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="damageDealer"></param>
        /// <param name="damageTarget"></param>
        /// <param name="damage"></param>
        /// <param name="projectileOptions"></param>
        /// <returns></returns>
        public override bool CanProjectileAttack(IActorHub owner, IDoDamage damageDealer, IActorHub damageTarget, IDoActorDamage damage, IProjectile projectileOptions)
        {
            if (damage.GetActorDamageData().DamageVar.DamageOptions.InflictPhysicalDmg == false && damage.GetActorDamageData().DamageVar.DamageOptions.InfictElementalDmg == false) return false;//no dmg to do
            if (damageTarget == null) return false;
            if (damageTarget == owner) return false;

            if (damage.GetActorDamageData().DamageVar.CombatHandler.DetermineAttackable(damageTarget, owner, projectileOptions.GetProjectileData().ProjectileVars.FriendlyFire) == false) return false; //can't attack
            if (damageDealer.GetDamagedList().Contains(damageTarget) == true) return false;
            return true;
        }

        #endregion
    }



    public abstract class CommonCombatFormulas : ScriptableObject
    {
        /// <summary>
        /// determine if valid melee target
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="damager"></param>
        /// <param name="actorDmg"></param>
        /// <param name="attacked"></param>
        /// <param name="meleeOptions"></param>
        /// <returns></returns>
        public abstract bool CanMeleeAttack(IActorHub owner, IDoDamage damager, IDoActorDamage actorDmg, IActorHub attacked, IMeleeWeapon meleeOptions);
        /// <summary>
        /// determine if valid projectile target
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="damageDealer"></param>
        /// <param name="damageTarget"></param>
        /// <param name="damage"></param>
        /// <param name="projectileOptions"></param>
        /// <returns></returns>
        public abstract bool CanProjectileAttack(IActorHub owner, IDoDamage damageDealer, IActorHub damageTarget, IDoActorDamage damage, IProjectile projectileOptions);
        /// <summary>
        /// get physical attack value of the self
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public abstract PhysicalAttackResults GetPhysicalAttackValue(IActorHub self);
        /// <summary>
        /// calculate the melee damage formula, write the values on the attack values
        /// </summary>
        /// <param name="results"></param>
        /// <param name="owner"></param>
        /// <param name="damager"></param>
        /// <param name="attacked"></param>
        /// <param name="actorDmg"></param>
        /// <param name="meleeOptions"></param>
        /// <returns></returns>
        public abstract AttackValues GetMeleeActorDamageLogic(AttackValues results, IActorHub owner, IDoDamage damager, IActorHub attacked, IDoActorDamage actorDmg, IMeleeWeapon meleeOptions);
        /// <summary>
        /// calculcate the projectile damage formula, write the values on the attack values
        /// </summary>
        /// <param name="results"></param>
        /// <param name="owner"></param>
        /// <param name="damageDealer"></param>
        /// <param name="damageTarget"></param>
        /// <param name="damage"></param>
        /// <param name="projectileOptions"></param>
        /// <returns></returns>
        public abstract AttackValues GetProjectileDamage(AttackValues results, IActorHub owner, IDoDamage damageDealer, IActorHub damageTarget, IDoActorDamage damage, IProjectile projectileOptions);

        /// <summary>
        /// wrapper for adding dots to targets
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <param name="damageOverTimeOptions"></param>
        /// <returns></returns>
        public abstract bool AddDOTS(IActorHub caster, IActorHub target, ModifyResourceVars[] damageOverTimeOptions);
        /// <summary>
        /// if valid target
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="sots"></param>
        /// <returns></returns>
        public abstract bool CanApplySoT(IActorHub owner, IActorHub target, IDoActorDamage damage, IApplySOT sots);
        /// <summary>
        /// logic for OnEnterTrigger for sots
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="sots"></param>
        public abstract void OnEnterSotLogic(IActorHub owner, IActorHub target, IDoActorDamage damage, IApplySOT sots);
        /// <summary>
        /// logic for OnExitTrigger for sots
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="sots"></param>
        public abstract void OnExitSoTLogic(IActorHub caster, IActorHub target, IDoActorDamage damage, IApplySOT sots);

        /// <summary>
        /// if valid target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attacker"></param>
        /// <param name="friendlyFire"></param>
        /// <returns></returns>
        public abstract bool DetermineAttackable(IActorHub target, IActorHub attacker, bool friendlyFire);

    }



    /// <summary>
    /// used to track crits, the attacker and amount is the key
    /// </summary>
    /// 
    [System.Obsolete]
    public class CritLog
    {
        public IAttributeUser Attacker;
        public int Amount;
        public CritLog(IActorHub attacker, int amount)
        {
            Amount = amount;
            // Attacker = attacker;
        }
    }
    /// <summary>
    /// used to record critical hits to pass that info on to whoever needs it, such as the UI
    /// </summary>
    /// 
    [System.Obsolete]
    public static class CritHelper
    {
        public static List<CritLog> Crits = new List<CritLog>();


        /// <summary>
        /// Removes crit from log if returned true.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static bool WasCrit(IAttributeUser attacker, int amount, bool removeOnTrue = true)
        {
            for (int i = 0; i < Crits.Count; i++)
            {
                CritLog crit = Crits[i];
                if (crit.Amount == amount && crit.Attacker == attacker)
                {
                    if (removeOnTrue)
                    {
                        Crits.RemoveAt(i);
                    }

                    return true;
                }
            }
            return false;
        }
    }
}