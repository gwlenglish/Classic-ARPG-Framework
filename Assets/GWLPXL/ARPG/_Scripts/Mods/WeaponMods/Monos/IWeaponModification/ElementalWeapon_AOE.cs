
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// Elemental Cleave mod for a weapon
    /// </summary>
    public class ElementalWeapon_AOE : MonoBehaviour, IWeaponModification
    {
        public AoEWeapoNVars Vars;
        protected bool active = false;
        protected IActorHub myDamage = null;
        
        public void DoModification(AttackValues results)
        {
            if (IsActive() == false) return;

            CombatHelper.GetElementalCleave(results, results.Attacker, transform.position, Vars);

        }

      

        public bool DoChange(Transform other)
        {
            return false;
        }

        public Transform GetTransform() => this.transform;
    
        public bool IsActive()
        {
            return active;
        }

        public void SetActive(bool isEnabled)
        {
            active = isEnabled;
        }

        public void SetUser(IActorHub myself)
        {
            myDamage = myself;
        }
    }
}