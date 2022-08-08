using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    [System.Serializable]
    public class KnockbackAOEVars
    {
        public AoEWeapoNVars AOE;
        public KnockBackVars Vars;

        public KnockbackAOEVars(AoEWeapoNVars aoe, KnockBackVars vars)
        {
            AOE = aoe;
            Vars = vars;
        }
    }


    /// <summary>
    /// Applies a knockback effect the player's melee weapons
    /// </summary>

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_AOE_Knockback")]

    public class ApplyAoEKnockback : AbilityLogic
    {
        public KnockbackAOEVars Vars;
        [System.NonSerialized]
        Dictionary<Transform, Knockback_AOE> knockbacks = new Dictionary<Transform, Knockback_AOE>();
       

      
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
                ModHelper.RemoveKnockbackAOEMod(skillUser, knockbacks);
            }

        }

      

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform)== false)
            {
                Add(skillUser.MyTransform);
                ModHelper.ApplyKnockbackAOEMod(skillUser, Vars, knockbacks);
            }
       
        }
    }
}