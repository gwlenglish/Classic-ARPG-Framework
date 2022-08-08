
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
    public class StatModPercentVars
    {
        public StatType Stat;
        [Range(-5.0f, 5.0f)]
        [Tooltip("1 = 100%, .5 = 50%.")]
        public float ByPercent = .01f;
       
        public StatModPercentVars(StatType stat, float percent)
        {
            ByPercent = percent;
            Stat = stat;
        }
    }

    /// <summary>
    /// Applies a stat buff
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/NEW_StatIncrease_Percent")]

    public class InstantStatIncrease_Percent : AbilityLogic
    {
        public StatModPercentVars Vars = new StatModPercentVars(StatType.Strength, .10f);
        [System.NonSerialized]
        public Dictionary<ActorAttributes, StatIncreased> Buffed = new Dictionary<ActorAttributes, StatIncreased>();



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
                ModHelper.RemoveStatPercentMod(skillUser, Vars, Buffed);
            }
      

        }

 
        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                ModHelper.ApplyStatPercentMod(skillUser, Vars, Buffed);
            }

        }
    }

    #region helpers
    public class StatIncreased
    {
        public StatType Stat;
        public int ByAmount;
        public StatIncreased(StatType type, int amount)
        {
            Stat = type;
            ByAmount = amount;
        }
    }

    #endregion
}
