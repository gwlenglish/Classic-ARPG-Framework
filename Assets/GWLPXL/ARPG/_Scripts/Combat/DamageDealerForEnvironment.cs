using UnityEngine;
using GWLPXL.ARPGCore.StatusEffects.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class DamageDealerForEnvironment
    {
        public CombatFormulas CombatHandler => combatHandler;
        public DamageOverTimeMultipliers DamageMultiplers => damageMultipliers;
        public StatusOverTimeOptions DamageOverTimeOptions => damageOverTimeOptions;
        [SerializeField]
        CombatFormulas combatHandler = null;
        [SerializeField]
        DamageOverTimeMultipliers damageMultipliers = new DamageOverTimeMultipliers();
        [SerializeField]
        StatusOverTimeOptions damageOverTimeOptions = new StatusOverTimeOptions(new ModifyResourceVars[0], 1);
        public DamageDealerForEnvironment(DamageOverTimeMultipliers multis, StatusOverTimeOptions sots)
        {
            damageMultipliers = multis;
            damageOverTimeOptions = sots;
        }
        public void SetCombatFormulas(CombatFormulas newforms) => combatHandler = newforms;

    }
}