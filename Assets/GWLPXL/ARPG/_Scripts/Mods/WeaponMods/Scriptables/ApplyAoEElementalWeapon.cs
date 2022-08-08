
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    /// <summary>
    /// Applies AoE Element damage to the weapon
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/New_AOE_ElementalCleave")]

    public class ApplyAoEElementalWeapon : WeaponStatusChanges
    {
        [Tooltip("Takes actors base ele plus the BaseDamage listed here.")]
        public AoEWeapoNVars Vars;

        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackerdic = new Dictionary<Transform, WeaponBuffTracker>();//and something to track to enable/disable
        public override void Apply(Transform[] weapon, IActorHub forUser)
        {
            Enable(weapon, forUser, trackerdic);
        }

     
        public override void Remove(Transform[] weapon, IActorHub forUser)
        {
            Disable(weapon, forUser, trackerdic);
        }

        protected override IWeaponModification CreateIWeaponMono(Transform forTransform)
        {
            ElementalWeapon_AOE source = forTransform.gameObject.AddComponent<ElementalWeapon_AOE>();
            source.Vars = this.Vars;
            return source;
        }

    }
}
