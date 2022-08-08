using UnityEngine;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class StatusOverTimeOptions
    {
        public bool ApplyDotAtEnter => applySOTAtEnter;
        public bool ApplyDotAtExit => applySOTAtExit;
        public bool ApplyAtEnter => applySOTAtEnter;
        public bool ApplyAtExit => applySOTAtExit;
        public float DamageRate => damageRate;
        public bool FriendlyFIre => friendlyFire;
        public ModifyResourceVars[] AdditionalDOTs => additionalSOTs;
        [Header("DOT Options")]
        [Tooltip("Will tick damage without delay into entering.")]
        [SerializeField]
        bool applySOTAtEnter = true;

        [Tooltip("Will tick damage on exit.")]
        [SerializeField]
        bool applySOTAtExit = false;
        [SerializeField]
        ModifyResourceVars[] additionalSOTs = new ModifyResourceVars[0];
        [Header("Additional Damage Options")]
        [SerializeField]
        [Tooltip("The rate in seconds at which damage and dots are applied/refreshed/stacked while inside the damage box.")]
        float damageRate = 1;

        [SerializeField]
        bool friendlyFire = false;
        public StatusOverTimeOptions(ModifyResourceVars[] newDots, float rate)
        {
            additionalSOTs = newDots;
            damageRate = rate;
        }

    }
}