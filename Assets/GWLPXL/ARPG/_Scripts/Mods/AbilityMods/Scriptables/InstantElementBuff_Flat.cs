
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class ElementBuffFlatVars
    {
        public ElementType Element;
        public int BuffAmount;

        public ElementBuffFlatVars(ElementType type, int buff)
        {
            Element = type;
            BuffAmount = buff;
        }
    }
    /// <summary>
    /// Applies an element buff effect 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/Abilities/NEW_WeaponElementBuff_Flat")]
    
    public class InstantElementBuff_Flat : AbilityLogic
    {
        public ElementBuffFlatVars Vars = new ElementBuffFlatVars(ElementType.Cold, 5);
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
                ModHelper.RemoveElementBuffFlat(skillUser, Vars, Buffed);
            }
         
        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                ModHelper.ApplyElementBuffFlat(skillUser, Vars, Buffed);
            }
          
        }
    }
}