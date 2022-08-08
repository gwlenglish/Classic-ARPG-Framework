using UnityEngine;
using GWLPXL.ARPGCore.StatusEffects.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class StatusOverTimeWeaponStatusOptions
    {

        public ModifyResourceVars[] AdditionalDOTs => additionalSOTs;

        [SerializeField]
        ModifyResourceVars[] additionalSOTs = new ModifyResourceVars[0];
       

        public StatusOverTimeWeaponStatusOptions(ModifyResourceVars[] newDots)
        {
            additionalSOTs = newDots;
        }

    }
}