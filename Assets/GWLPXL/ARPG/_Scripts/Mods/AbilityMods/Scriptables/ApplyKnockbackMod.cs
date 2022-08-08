

using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// Applies a knockback effect on the user's melee weapons
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_Knockback")]

    public class ApplyKnockbackMod : AbilityLogic
    {
        public KnockBackVars Vars;
        [System.NonSerialized]
        Dictionary<Transform, Knockback> buffed = new Dictionary<Transform, Knockback>();



        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;
            return true;
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                Remove(skillUser.MyTransform);
                ModHelper.RemoveKnockbackMod(skillUser, buffed);
            }

        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                ModHelper.ApplyKnockbackMod(skillUser, Vars, buffed);
            }

        }
    }
}
