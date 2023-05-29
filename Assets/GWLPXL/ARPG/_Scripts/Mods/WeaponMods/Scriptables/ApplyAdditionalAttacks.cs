
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;

using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{


    [System.Serializable]
    public class AdditionalAttacksVars
    {
        public List<DamageSourceVars_Actor> Vars = new List<DamageSourceVars_Actor>();
    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/New Additional Attacks")]
    public class ApplyAdditionalAttacks : WeaponStatusChanges
    {

        public AdditionalAttacksVars Vars;

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
            AdditionalAttacksMod source = forTransform.gameObject.AddComponent<AdditionalAttacksMod>();
            source.Vars = this.Vars;
            return source;
        }


    }
}