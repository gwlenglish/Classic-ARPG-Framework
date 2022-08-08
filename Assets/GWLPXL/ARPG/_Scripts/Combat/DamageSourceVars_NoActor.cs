using UnityEngine;

/// <summary>
/// 
/// </summary>
/// 
namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class DamageSourceVars_NoActor
    {
        [Header("Damage")]
        public DamageMultipliers_NoActor DamageMultipliers = new DamageMultipliers_NoActor();
    }
}