
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// Applies a Generate Resource on hit mod to the weapon
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/NEW Generate Resource")]

    public class ApplyWeaponHitGenerateResourceOnHit : WeaponStatusChanges
    {
        public GenerateRourseOnHitVars Vars = new GenerateRourseOnHitVars();


        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackerdic = new Dictionary<Transform, WeaponBuffTracker>();


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
            GenerateResourceOnHit source = forTransform.gameObject.AddComponent<GenerateResourceOnHit>();
            source.Vars = this.Vars;
            return source;
        }
    }
}