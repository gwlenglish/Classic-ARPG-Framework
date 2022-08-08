using UnityEngine;
using GWLPXL.ARPGCore.StatusEffects.com;

namespace GWLPXL.ARPGCore.Combat.com
{


    [System.Serializable]
    public class DamageDealerForActor
    {
        public CombatFormulas CombatHandler => combatHandler;
        public string Name => descriptiveName;
        public DamageOptions DamageOptions => damageOptions;
        public DamageMultiplers_Actor DamageMultipliers => damageMultipliers;
        public StatusOverTimeOptions SoTOptions => statusOverTimeOptions;
        public DamageOverTimeMultipliers SoTOverTimeMultipliers => statusOverTimeMultiplers;
        [SerializeField]
        [Tooltip("You can override with your own Combat Formulas or leave empty to use defaults.")]
        CombatFormulas combatHandler = null;
        [SerializeField]
        string descriptiveName = string.Empty;
        [Header("Damage")]
        [SerializeField]
        DamageOptions damageOptions = new DamageOptions(true, true, false);
        [SerializeField]
        DamageMultiplers_Actor damageMultipliers = new DamageMultiplers_Actor();
        [Header("SoTs")]
        [SerializeField]
        StatusOverTimeOptions statusOverTimeOptions = new StatusOverTimeOptions(new ModifyResourceVars[0], 1);
        [SerializeField]
        DamageOverTimeMultipliers statusOverTimeMultiplers = new DamageOverTimeMultipliers();

        public DamageDealerForActor(string name, CombatFormulas handler, DamageOptions options, DamageMultiplers_Actor dmgmults, StatusOverTimeOptions sotsoptions, DamageOverTimeMultipliers sotMultis)
        {
            descriptiveName = name;
            combatHandler = handler;
            damageOptions = options;
            damageMultipliers = dmgmults;
            statusOverTimeOptions = sotsoptions;
            statusOverTimeMultiplers = sotMultis;
        }

       
        public void SetCombatFormulas(CombatFormulas newforms) => combatHandler = newforms;
    }


}