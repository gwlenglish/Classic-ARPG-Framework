
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;

using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class ElementalWpnBuffVars
    {
        public ElementType Element = ElementType.Cold;
        [Range(.01f, 5.0f)]
        public float PercentOfWpnDmg = .01f;
        public ElementalWpnBuffVars(ElementType type, float percentwpndmg)
        {
            PercentOfWpnDmg = percentwpndmg;
            Element = type;
        }
    }
    /// <summary>
    /// Applies an element buff effect 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/NEW_WeaponElementBuff_Percent")]

    public class InstantElementBuff_Percent : AbilityLogic
    {
        public ElementalWpnBuffVars Vars = new ElementalWpnBuffVars(ElementType.Cold, .01f);
        [System.NonSerialized]
        public Dictionary<IActorHub, BuffedActor> Buffed = new Dictionary<IActorHub, BuffedActor>();




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
                ModHelper.RemoveElementalWpnDmgBuff(skillUser, Vars, Buffed);
            }
   

        }


        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                ModHelper.ApplyElementalWpnDmgBuff(skillUser, Vars, Buffed);
            }


        }
    }

    //takes weapon damage, adds % more damage as cold, for instance
    [System.Serializable]
    public class BuffedActor
    {
        public ElementType Type = ElementType.Cold;
        public int OriginalValue = 1;
        public int Amount = 1;
        public BuffedActor(ElementType type, int byAmount, int original)
        {
            Type = type;
            Amount = byAmount;
            OriginalValue = original;
        }

    }
}