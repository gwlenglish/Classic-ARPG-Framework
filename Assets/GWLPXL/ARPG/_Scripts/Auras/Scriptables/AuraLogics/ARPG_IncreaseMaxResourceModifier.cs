using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Auras/Logic/NEW ARPG Increase Max Resource Modifier")]
    public class ARPG_IncreaseMaxResourceModifier : AuraLogic
    {
        public AttributeModifier[] attributeModifier = new AttributeModifier[0];
        public ResourceType type = ResourceType.Health;
        public override bool DoApplyLogic(ITakeAura onUser)
        {
            if (onUser == null) return false;

            foreach (var modifier in attributeModifier)
            {
                onUser.AuraApplyModifierResource((int)type, modifier);
            }

            return true;
        }

        public override bool DoRemoveLogic(ITakeAura fromUser)
        {
            if (fromUser == null) return false;

            foreach (var modifier in attributeModifier)
            {
                fromUser.AuraRemoveModifierResource((int)type, modifier);
            }
            
            return true;
        }
    }
}