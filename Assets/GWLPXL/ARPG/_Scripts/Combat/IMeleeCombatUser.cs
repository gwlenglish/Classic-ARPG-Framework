
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{

    public interface IMeleeCombatUser
    {
        /// <summary>
        /// e.g. right hand is index 0, left hand is 1
        /// </summary>
        /// <param name="damageDealers"></param>
        void SetMeleeDamageDealer(IDoDamage damager, int atIndex);
        IDoDamage[] GetMeleeDamageBoxes();
        void ResetDefaultDamageDealer(int atIndex);
        Transform[] GetMeleeTransforms();
        EquipmentSlotsType[] GetWpnMeleeSlots();
        void SetActorHub(IActorHub hub);
    }
}
