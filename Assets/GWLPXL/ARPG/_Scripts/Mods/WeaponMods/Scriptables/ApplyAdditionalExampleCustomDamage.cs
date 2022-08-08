

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    /// <summary>
    /// Applies additional damage mod to the weapon
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/New Example Custom Damage")]

    public class ApplyAdditionalExampleCustomDamage : WeaponStatusChanges
    {

        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackers = new Dictionary<Transform, WeaponBuffTracker>();
        public override void Apply(Transform[] weapons, IActorHub forUser)
        {
            Enable(weapons, forUser, trackers);   
        }

        public override void Remove(Transform[] weapons, IActorHub forUser)
        {
            Disable(weapons, forUser, trackers);
        }

        protected override IWeaponModification CreateIWeaponMono(Transform forTransform)
        {
            ExampleCustomDamage source = forTransform.gameObject.AddComponent<ExampleCustomDamage>();
            return source;

        }
    }
}