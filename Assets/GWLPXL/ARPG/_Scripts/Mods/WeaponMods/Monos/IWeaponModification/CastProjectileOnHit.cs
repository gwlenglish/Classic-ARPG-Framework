using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;

using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    public class CastProjectileOnHit : MonoBehaviour, IWeaponModification
    {

        public ProjectileVariables Vars = new ProjectileVariables();

        bool active = false;
        IActorHub self = null;

        public void DoModification(AttackValues other)
        {
            if (IsActive() == false) return;

            CombatHelper.DoFireAndInIProjectile(self, Vars);
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