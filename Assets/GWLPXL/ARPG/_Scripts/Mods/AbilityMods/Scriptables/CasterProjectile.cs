
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;
using System.Collections.Generic;
namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class ProjectileSlot
    {
        [Tooltip("Delay to shoot the projectile normalized to the cooldown timer. E.g. a .5f would mean fire it halfway between the cooldown timer.")]
        [Range(0f, 1f)]
        public float Delay = 0;
        public ProjectileVariables Vars;
    }
    [System.Serializable]
    public class ProjectileOffsets
    {
        [Header("Position Offset")]
        public Vector3 StartOffset = Vector3.zero;
        [Header("Rotation Offsets")]
        [Range(-90, 90)]
        public float XTiltAngle = 0;
        [Range(-90, 90)]
        public float YTiltAngle = 0;
        [Range(-90, 90)]
        public float ZTiltAngle = 0;
    }

    [System.Serializable]
    public class ProjectileDamageOverrides
    {
        public ActorDamageData DamageOverride = null;
        public ProjectileData OptionsOverride = null;
    }
    [System.Serializable]
    public class ChargingDamageVars
    {

        [Tooltip("Charge will start at the base value set in the inspector and then move towards this percentage based on the charge length. A number of 2 is 200%, so at max charge the ability will do twice the dmg. Setting it to 0 will disable any charge modifications.")]
        public float MaximumChargePercentPhys = 2f;
        [Tooltip("Charge will start at the base value set in the inspector and then move towards this percentage based on the charge length. A number of 2 is 200%, so at max charge the ability will do twice the dmg. Setting it to 0 will disable any charge modifications.")]
        public float MaximumChargePercentElemental = 2f;
    }

   
    [System.Serializable]
    public class ProjectileVariables

    {
        public FireRotationParent Rotation = FireRotationParent.ParentGameObject;
        [Tooltip("If Null, will not fire")]
        public GameObject ProjectilePrefab = null;
        public ProjectileOffsets Offsets = new ProjectileOffsets();
        public ChargingDamageVars ChargeOptions = new ChargingDamageVars();
        public ProjectileDamageOverrides Overrides = new ProjectileDamageOverrides();
    }
    public enum FireRotationParent
    {
        ParentGameObject,
        FirePoint,
        TowardsMouse2D
    }

    /// <summary>
    /// Applies projectiles to the ability.
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_Projectiles Mod")]

    public class CasterProjectile : AbilityLogic
    {

        [SerializeField]
        ProjectileVariables abilityStartProjectile = new ProjectileVariables();
        [SerializeField]
        ProjectileVariables abilityEndProjectile = new ProjectileVariables();
        [SerializeField]
        ProjectileSlot[] additionalProjectiles = new ProjectileSlot[0];

        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;
            IProjectileCombatUser projectileUser = forUser.MyProjectiles;
            if (projectileUser == null) return false;
            if (projectileUser.GetProjectileFirePoint() == null) return false;
            return true;
        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                ChargeHelper.CheckCharge(skillUser, theSkill);
                CombatHelper.DoFireAndInIProjectile(skillUser, abilityStartProjectile);
                for (int i = 0; i < additionalProjectiles.Length; i++)
                {
                    float duration = theSkill.Duration * additionalProjectiles[i].Delay;
                    ProjectileDelayVars delay = new ProjectileDelayVars(duration, skillUser, additionalProjectiles[i].Vars);
                }
            }
           
        }


        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                Remove(skillUser.MyTransform);
                CombatHelper.DoFireAndInIProjectile(skillUser, abilityEndProjectile);
            }
   

           
        }

       

       
    }


    public static class ChargeHelper
    {
        static Dictionary<IActorHub, Ability> chargedic = new Dictionary<IActorHub, Ability>();

        /// <summary>
        /// returns if we have a charge available to expend
        /// </summary>
        /// <param name="skillUser"></param>
        /// <param name="theSkill"></param>
        /// <returns></returns>
        public static bool CheckCharge(IActorHub skillUser, Ability theSkill)
        {
            if (skillUser.MyAbilities.GetChargedAbility() == theSkill && chargedic.ContainsKey(skillUser) == false)
            {
                skillUser.MyAbilities.GetRuntimeController().OnAbilityUserEnd += EndCharge;
                chargedic.Add(skillUser, theSkill);
                return true;

            }
            return false;
        }

        static void EndCharge(IActorHub skilluser, Ability ability)
        {
            if (skilluser.MyAbilities.GetChargedAbility() == ability && chargedic.ContainsKey(skilluser))
            {
                //do the unique charging stuff
                //also make it null
                skilluser.MyAbilities.GetRuntimeController().SetChargeAmount(0);
                skilluser.MyAbilities.SetChargedAbility(null);
                chargedic.Remove(skilluser);
            }
        }
    }
}