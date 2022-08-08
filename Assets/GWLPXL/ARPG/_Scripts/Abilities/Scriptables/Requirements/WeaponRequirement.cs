
using UnityEngine;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.com
{
    /// <summary>
    /// example of a requirement. This requires a weapon to be equipped.
    /// </summary>

    [CreateAssetMenu(menuName ="GWLPXL/ARPG/Abilities/Requirements/NEW Weapon Requirement")]
    public class WeaponRequirement : AbilityRequirement
    {
        [SerializeField]
        EquipmentSlotsType[] weaponSlotsToCheck = new EquipmentSlotsType[2] { EquipmentSlotsType.LeftWpnHand, EquipmentSlotsType.RightWpnHand };
        [SerializeField]
        WeaponType[] requiresTypes = new WeaponType[0];
        public override string GetDescription()
        {
            sb.Clear();
            sb.Append("Requires: ");
            for (int i = 0; i < requiresTypes.Length; i++)
            {
                sb.Append(requiresTypes[i].ToString());
                if (i != requiresTypes.Length - 1)
                {
                    sb.Append(" OR ");
                }
            }
            return sb.ToString();
        }

        public override bool HasRequirements(IActorHub forUser)
        {

            IInventoryUser inv = forUser.MyInventory;
            for (int i = 0; i < weaponSlotsToCheck.Length; i++)
            {
                Equipment equipment = inv.GetInventoryRuntime().GetEquipmentInSlot(weaponSlotsToCheck[i]);
                Weapon weapon = equipment as Weapon;
                if (weapon == null) return false;
                WeaponType type = weapon.GetWeaponType();

                if (requiresTypes.Length == 0)
                {
                    return true;//just return true if we didn't set a type, so all types work
                }

                for (int j = 0; j < requiresTypes.Length; j++)
                {
                    if (type == requiresTypes[j])
                    {
                        return true;//we are wearing someting that we require
                    }
                }

            }

            return false;
        }
    }
}