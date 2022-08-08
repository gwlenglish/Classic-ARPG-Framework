
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;

using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    /// <summary>
    /// Applies an explosive mod to the weapon
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/NEW Explosive Mod")]

    public class ApplyWeaponExplosiveMod : WeaponStatusChanges
    {
        public ExplosionVars Vars;


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
            ExplosiveMod source = forTransform.gameObject.AddComponent<ExplosiveMod>();
            source.Vars = this.Vars;
            return source;
        }
    }
}