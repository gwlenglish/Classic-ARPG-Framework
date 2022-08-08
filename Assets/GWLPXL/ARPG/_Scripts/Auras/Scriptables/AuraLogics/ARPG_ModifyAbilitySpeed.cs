
using UnityEngine;

using GWLPXL.ARPGCore.Abilities.com;


namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// should really be tagged to a stat that conrols the multi for the speed, but this directly influences it without the stat
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Auras/Logic/NEW ARPG Increase Ability Speed")]
    public class ARPG_ModifyAbilitySpeed : AuraLogic
    {
        [SerializeField]
        [Tooltip("0 is normal speed. Positive numbers in an increase in percentage, negative is decrease in percent.")]
        float additionalSpeedMulti = .10f;

        public override bool DoApplyLogic(ITakeAura onUser)
        {
            if (onUser == null) return false;

            IAbilityUser user = onUser.GetGameObjectInstance().GetComponent<IAbilityUser>();
            if (user != null)
            {
                user.ModifyAbilityMulti(additionalSpeedMulti);
            }
            return true;
        }

        public override bool DoRemoveLogic(ITakeAura fromUser)
        {
            if (fromUser == null) return false;

            IAbilityUser user = fromUser.GetGameObjectInstance().GetComponent<IAbilityUser>();
            if (user != null)
            {
                user.ModifyAbilityMulti(-additionalSpeedMulti);
            }
            return true;
        }













#if UNITY_EDITOR //auto naming
        private void OnValidate()
        {
          //  string path = UnityEditor.AssetDatabase.GetAssetPath(this);
           // UnityEditor.AssetDatabase.RenameAsset(path, "Ability Speed Multi " + newSpeedMulti.ToString("D2"));
        }
#endif

    }
}