using GWLPXL.ARPGCore.com;

using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/New Cast Projectile On Hit")]
    public class ApplyCastProjectOnHileMod : WeaponStatusChanges
    {
        public ProjectileVariables Vars;

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
            CastProjectileOnHit source = forTransform.gameObject.AddComponent<CastProjectileOnHit>();
            source.Vars = this.Vars;
            return source;
        }
    }
}