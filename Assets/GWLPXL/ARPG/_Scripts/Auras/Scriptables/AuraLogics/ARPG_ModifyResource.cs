
using UnityEngine;
using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// like for damage or health auras
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Auras/Logic/NEW Modify Resource")]
    public class ARPG_ModifyResource : AuraLogic
    {
        [Tooltip("Negative will subtract, positive will add.")]
        public int ModifyAmountPerTick;
        //this enum should change to whatever your current system is using. You are just casting it in by int, so any enum will work.
        public ResourceType Type = ResourceType.Health;

        public override bool DoApplyLogic(ITakeAura onUser)
        {
            if (onUser == null)
                return false;
            onUser.AuraModifyCurrentResource((int)Type, ModifyAmountPerTick);
            return true;
        }

        public override bool DoRemoveLogic(ITakeAura fromUser)
        {
            if (fromUser == null) return false;
            return true;
        }

#if UNITY_EDITOR //auto naming
        private void OnValidate()
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(path, "Modify " + Type.ToString() + "_" + ModifyAmountPerTick.ToString());
        }
#endif

    }
}