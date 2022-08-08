
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{


    /// <summary>
    /// Mods the weapon to have an AOE knockback effect
    /// </summary>
    public class Knockback_AOE : MonoBehaviour, IWeaponModification
    {
        public KnockbackAOEVars Vars;
        bool active;
        IActorHub myself;
       
        public void DoModification(AttackValues other)
        {
            if (IsActive() == false) return;
            CombatHelper.DoKnockbackAOE(myself, transform.position, Vars);
           
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
            this.myself = myself;
        }
    }
}