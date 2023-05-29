
using GWLPXL.ARPGCore.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    /// <summary>
    /// Applies additional SOT the weapon (e.g. damage over time)
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/New SOT Source")]
    public class ApplyAdditionalSOTSource : WeaponStatusChanges
    {
        //needs vars
        [Range(1, 101)]
        public int Chance = 101;
        public AdditionalSoTSourceVars Vars = new AdditionalSoTSourceVars();

        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackerdic = new Dictionary<Transform, WeaponBuffTracker>();//and something to track to enable/disable

        public override void Apply(Transform[] weapons, IActorHub forUser)
        {
            int chance = Random.Range(0, 101);
            if (chance < Chance)
            {
                Enable(weapons, forUser, trackerdic);
            }
         
        }

        public override void Remove(Transform[] weapons, IActorHub forUser)
        {
            Disable(weapons, forUser, trackerdic);
        
        }

        protected override IWeaponModification CreateIWeaponMono(Transform forTransform)//needs the mono that dervies from iweaponstatuschange
        {
            AdditiionalSOTSource added = forTransform.gameObject.AddComponent<AdditiionalSOTSource>();
            added.Vars = this.Vars;
            return added;
        }


    }
}