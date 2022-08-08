using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Auras/Logic/NEW ARPG Buff Stat Modifier")]
    public class ARPG_BuffStatModifier : AuraLogic
    {
        public AttributeModifier[] attributeModifier = new AttributeModifier[0];
        public StatType type;
        public override bool DoApplyLogic(ITakeAura onUser)
        {
            if (onUser == null) return false;

            foreach (var modifier in attributeModifier)
            {
                onUser.AuraApplyModifierStat((int)type, modifier);
            }

            return true;
        }

        public override bool DoRemoveLogic(ITakeAura fromUser)
        {
            if (fromUser == null) return false;

            foreach (var modifier in attributeModifier)
            {
                fromUser.AuraRemoveModifierStat((int)type, modifier);
            }
            
            return true;
        }
        
    }
}