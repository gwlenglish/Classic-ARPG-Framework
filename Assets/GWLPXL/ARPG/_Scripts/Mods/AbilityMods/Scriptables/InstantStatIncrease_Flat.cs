

using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class StatModFlatVars
    {
        public StatType Type;
        public int Amount;

        public StatModFlatVars(StatType stat, int amount)
        {
            Type = stat;
            Amount = amount;
        }
    }
    /// <summary>
    /// Applies a stat buff
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/Abilities/NEW_StatIncrease_Flat")]

    public class InstantStatIncrease_Flat : AbilityLogic
    {
        public StatModFlatVars Vars = new StatModFlatVars(StatType.Strength, 10);
        [System.NonSerialized]
        public Dictionary<ActorAttributes, int> Buffed = new Dictionary<ActorAttributes, int>();

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
                ModHelper.RemoveStatFlatMod(skillUser, Vars, Buffed);
            }
            

        }


        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                ModHelper.ApplyStatFlatMod(skillUser, Vars, Buffed);
            }
        
        }
    }
}