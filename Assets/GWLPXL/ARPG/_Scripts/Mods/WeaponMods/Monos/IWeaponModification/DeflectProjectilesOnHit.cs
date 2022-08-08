
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    /// <summary>
    /// Detects IProjectile, Reverses Rigibody or Rigidbdoy2D velocity
    /// </summary>
    public class DeflectProjectilesOnHit : MonoBehaviour, IWeaponModification
    {
        public DeflectorOnHitVars Vars = new DeflectorOnHitVars();
        bool enabledDeflector = false;
        IActorHub user = null;

        GenericTimer dtimer = null;
        public Transform GetTransform() => this.transform;

      

        public void SetActive(bool isEnabled)
        {
            enabledDeflector = isEnabled;
         
            if (isEnabled)
            {
                enabledDeflector = true;
                dtimer = new GenericTimer(Vars.Duration);
                dtimer.OnComplete += DisableDeflector;
            }
            else
            {
                if (dtimer != null)
                {
                    dtimer.OnComplete.Invoke();
                }
            }
           
        }

        void DisableDeflector()
        {
            enabledDeflector = false;
            dtimer.OnComplete -= DisableDeflector;
            dtimer = null;

        }
        public bool IsActive()
        {
            return enabledDeflector;
        }

        public void DoModification(AttackValues other)
        {
            return;
        }

        public bool DoChange(Transform other)
        {
            if (enabledDeflector == false) return false;

            return CombatHelper.DoDeflectProjectile(user, other, Vars);
        }


        public void SetUser(IActorHub myself)
        {
            user = myself;  
        }

      

       

      
    }
}