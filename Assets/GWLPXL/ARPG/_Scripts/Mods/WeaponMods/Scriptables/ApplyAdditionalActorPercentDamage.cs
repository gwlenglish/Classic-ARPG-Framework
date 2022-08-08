
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    /// <summary>
    /// Applies additional damage mod to the weapon
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/New_Actor Percent Damage")]

    public class ApplyAdditionalActorPercentDamage : WeaponStatusChanges
    {


        public DamageSourceVars_Actor Vars = new DamageSourceVars_Actor();


        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackerdic = new Dictionary<Transform, WeaponBuffTracker>();//and something to track to enable/disable
        public override void Apply(Transform[] weapons, IActorHub forUser)
        {
            Enable(weapons, forUser, trackerdic);
        }

        public override void Remove(Transform[] weapons, IActorHub forUser)
        {
            Disable(weapons, forUser, trackerdic);
        }

        protected override IWeaponModification CreateIWeaponMono(Transform forTransform)
        {
            AdditionalDamageSource_Actor source = forTransform.gameObject.AddComponent<AdditionalDamageSource_Actor>();
            source.Vars = this.Vars;
            return source;
        }
    }
}