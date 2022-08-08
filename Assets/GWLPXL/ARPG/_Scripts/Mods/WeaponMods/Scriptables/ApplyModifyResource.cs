

using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.StatusEffects.com;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    /// <summary>
    /// Applies a ModifyResource mod to the weapon
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/NEW_Drain")]
    
    public class ApplyModifyResource : WeaponStatusChanges
    {
        public ModifyResourceVars Vars;
        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackerdic = new Dictionary<Transform, WeaponBuffTracker>();
        public override void Apply(Transform[] weapon, IActorHub forUser)
        {
            Enable(weapon, forUser, trackerdic);
        }

        public override void Remove(Transform[] weapons, IActorHub forUser)
        {
            Disable(weapons, forUser, trackerdic);
        }

        protected override IWeaponModification CreateIWeaponMono(Transform forTransform)
        {
            Drainer drainer = forTransform.gameObject.AddComponent<Drainer>();
            drainer.Vars = this.Vars;
            return drainer;
        }
    }


}