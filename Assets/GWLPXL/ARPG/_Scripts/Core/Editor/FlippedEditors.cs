using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Auras.com;

namespace GWLPXL.ARPGCore.com
{
    [CustomEditor(typeof(PlayerAuraUser))]
    public class PlayerAuraUserEditor : FlippedEditor
    {
        protected override Object GetRuntimeObject()
        {
            PlayerAuraUser inv = target as PlayerAuraUser;
            return inv.GetAuraControllerRuntime();
        }
    }

    [CustomEditor(typeof(PlayerInventory))]
    public class PlayerInventoryEditor : FlippedEditor
    {
        protected override Object GetRuntimeObject()
        {
            PlayerInventory inv = target as PlayerInventory;
            return inv.GetInventoryRuntime();
        }
    }

    [CustomEditor(typeof(PlayerAttributes))]
    public class PlayerAttributesEditor : FlippedEditor
    {
        protected override Object GetRuntimeObject()
        {
            PlayerAttributes att = target as PlayerAttributes;
            return att.GetRuntimeAttributes();
        }
    }

    [CustomEditor(typeof(PlayerAbilityUser))]
    public class PlayerAbilityUserEditor : FlippedEditor
    {
        protected override Object GetRuntimeObject()
        {
            PlayerAbilityUser user = target as PlayerAbilityUser;
            return user.GetRuntimeController();
        }
    }
}
