
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using UnityEngine;
using GWLPXL.ARPGCore.com;

using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class ShooterProjectileSlot
    {
        [Tooltip("Delay to shoot the projectile normalized to the cooldown timer. E.g. a .5f would mean fire it halfway between the cooldown timer.")]
        [Range(0f, 1f)]
        public float Delay = 0;
        public ShooterProjectileVars Vars;
    }
    [System.Serializable]
    public class ShooterProjectileVars
    {
        public ProjectileVariables ProjectileVars;
        public bool OverrideShooterAmmo = false;
        [Tooltip("Leave 0 to not use.")]
        public int[] ShooterIndices = new int[1] { 0 };
        public WeaponStatusChanges[] weaponBuffs = new WeaponStatusChanges[0];

    }
    /// <summary>
    /// used for projectiles that come from a weapon like a gun or arrow, something not the actor itself
    /// </summary>

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/NEW_ShooterProjectile")]
    public class ShooterProjectile : AbilityLogic
    {
        [SerializeField]
        ShooterProjectileVars abilityStartProjectile;
        [SerializeField]
        ShooterProjectileVars abilityEndProjectile;
        [SerializeField]
        ShooterProjectileSlot[] additionalProjectiles = new ShooterProjectileSlot[0];

        [SerializeField]
        AbilityRequirement[] requirements = new AbilityRequirement[0];
       
        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;

            for (int i = 0; i < requirements.Length; i++)//optional requirements, such as requires longbow or flamethrower
            {
                bool meetsrequirements = requirements[i].HasRequirements(forUser);
                if (meetsrequirements == false) return false;
            }

            //check if we are projectile user with something that can shoot
            IProjectileCombatUser projectileUser = forUser.MyProjectiles;
            if (projectileUser == null) return false;
            return true;
        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if(Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                CombatHelper.DoFireShooterProjectile(skillUser, abilityStartProjectile);
                for (int i = 0; i < additionalProjectiles.Length; i++)
                {
                    float duration = theSkill.Duration * additionalProjectiles[i].Delay;
                    ShooterProjectileDelayVars delay = new ShooterProjectileDelayVars(duration, skillUser, additionalProjectiles[i].Vars);
                }
            }
           
        }

       

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                Remove(skillUser.MyTransform);
                CombatHelper.DoFireShooterProjectile(skillUser, abilityEndProjectile);
            }

            //nothing really, no need to track projectiles they track themselves. 
        }




    }
}