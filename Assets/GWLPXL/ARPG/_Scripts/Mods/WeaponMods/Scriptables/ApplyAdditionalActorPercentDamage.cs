
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
        [Range(1, 100)]
        public int ChanceToApply = 100;
        public DamageSourceVars_Actor Vars = new DamageSourceVars_Actor();


        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackerdic = new Dictionary<Transform, WeaponBuffTracker>();//and something to track to enable/disable
        public override void Apply(Transform[] weapons, IActorHub forUser)
        {
            int chance = Random.Range(0, 101);
            if (chance <= ChanceToApply)
            {
                Enable(weapons, forUser, trackerdic);
            }
         
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