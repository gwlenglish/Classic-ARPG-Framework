

using GWLPXL.ARPGCore.Abilities.com;

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class MeleeDmgVars
    {
        public ActorDamageData DamageOverride = null;
        public MeleeData MeleeOptionsOverride = null;
        public ChargingDamageVars ChargeOptions;
        public WeaponStatusChanges[] Wpnbuffs = new WeaponStatusChanges[0];

        [System.NonSerialized]
        public Dictionary<Transform, IDoDamage> MeleeDDDic = new Dictionary<Transform, IDoDamage>();
        [System.NonSerialized]
        public Dictionary<IMeleeCombatUser, Transform[]> Buffdic = new Dictionary<IMeleeCombatUser, Transform[]>();
    }
   
    /// <summary>
    /// Activates/deactivates the damage on the user's melee weapons
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/NEW_Melee")]
    public class MeleeCombatLogic : AbilityLogic
    {
        public MeleeDmgVars Vars;

        public AbilityRequirement[] Requirements = new AbilityRequirement[0];
       

        public GameObject VFXPrefab;
        public bool ParentToMelee = true;


        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;

            for (int i = 0; i < Requirements.Length; i++)
            {
                if (Requirements[i].HasRequirements(forUser) == false) return false;
            }
            IMeleeCombatUser melee = forUser.MyMelee;
            return melee != null;

        }
        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                //only allow melee users to use this
                ChargeHelper.CheckCharge(skillUser, theSkill);
                CombatHelper.EnableAndBuffMeleeDamageBoxes(skillUser, Vars);

                //create vfx
                IMeleeCombatUser melee = skillUser.MyMelee;
                Transform[] transforms = melee.GetMeleeTransforms();//can be null
                if (VFXPrefab != null && transforms != null)
                {
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        if (transforms[i] == null) continue;
                        GameObject instance = Instantiate(VFXPrefab, transforms[i].position, Quaternion.identity);
                        if (ParentToMelee)
                        {
                            instance.transform.SetParent(transforms[i]);
                        }
                    }

                }
            }
            


        }


        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                Remove(skillUser.MyTransform);
                CombatHelper.DisableMeleeWeapons(skillUser, Vars);
            }
       

        }

       

    }
}