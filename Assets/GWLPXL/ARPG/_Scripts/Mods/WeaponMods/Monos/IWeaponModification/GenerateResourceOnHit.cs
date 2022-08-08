

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{


    // Mods the weapon to generate resource on hit
    public class GenerateResourceOnHit : MonoBehaviour, IWeaponModification
    {
        public GenerateRourseOnHitVars Vars;

        IActorHub owner;
        bool active = false;

        

        public void DoModification(AttackValues other)
        {
            if (IsActive() == false) return;
            CombatHelper.DoGenerateResource(owner, Vars);
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
            owner = myself;
        }
    }
}
