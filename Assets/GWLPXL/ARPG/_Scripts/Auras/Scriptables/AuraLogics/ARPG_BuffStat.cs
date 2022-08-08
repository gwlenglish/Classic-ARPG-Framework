
using UnityEngine;
using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// obsolete, use BuffStatModifier
    /// </summary>
    [System.Obsolete]

    public class ARPG_BuffStat : AuraLogic
    {
        [SerializeField]
        int amount;
        //this enum should change to whatever your current system is using. 
        //You are just casting it in by int, so any enum will work.
        [SerializeField]
         StatType type = StatType.Strength;
        public override bool DoApplyLogic(ITakeAura onUser)
        {
            if (onUser == null) return false;

            onUser.AuraBuffSat((int)type, amount);
            return true;
        }

        public override bool DoRemoveLogic(ITakeAura fromUser)
        {
            if (fromUser == null) return false;

            fromUser.AuraBuffSat((int)type, -amount);
            return true;
        }













#if UNITY_EDITOR //auto naming
        private void OnValidate()
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(path, "Increase Max " + type.ToString() + "_" + amount.ToString());
        }
#endif

    }
}