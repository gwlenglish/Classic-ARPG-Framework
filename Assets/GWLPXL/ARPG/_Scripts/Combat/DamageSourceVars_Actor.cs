using UnityEngine;

/// <summary>
/// 
/// </summary>
/// 
namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class DamageSourceVars_Actor
    {
        [Header("Damage")]
        public DamageMultiplers_Actor AdditionalDamage = new DamageMultiplers_Actor();
        
    }
}