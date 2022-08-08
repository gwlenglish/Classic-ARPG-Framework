
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

/// <summary>
/// applies an additional damage source to a weapon
/// </summary>
namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    public class AdditionalDamageSource_Actor : MonoBehaviour, IWeaponModification
    {
        public DamageSourceVars_Actor Vars = new DamageSourceVars_Actor();

        bool active = false;
        IActorHub self = null;

        public void DoModification(AttackValues other)
        {
            if (IsActive() == false) return;

            CombatHelper.GetAdditionalDamageSourceActor(other, Vars);

        }

        public bool DoChange(Transform other)
        {
            return false;
        }

        public Transform GetTransform() => this.transform;


        public bool IsActive() => active;

        public void SetActive(bool isEnabled)
        {
            active = isEnabled;
           
        }


        public void SetUser(IActorHub myself)
        {
            self = myself;
       
        }



    }
}