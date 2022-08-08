
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

/// <summary>
/// damage source without an actor, typically non-scaleable values
/// </summary>
/// 
namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    public class AdditionalDamageSource_NoActor : MonoBehaviour, IWeaponModification
    {
        public DamageSourceVars_NoActor Vars = new DamageSourceVars_NoActor();
        bool active = false;
        IActorHub self = null;
        public void DoModification(AttackValues results)
        {
            if (IsActive() == false) return;
            CombatHelper.GetAdditionalDamageSource(results, Vars);
        }

        public bool DoChange(Transform other)
        {
            return false;
        }

        public Transform GetTransform() => this.transform;

        public bool IsActive() => active;

        public void SetActive(bool isEnabled) => active = isEnabled;

        public void SetUser(IActorHub myself) => self = myself;
    }
}