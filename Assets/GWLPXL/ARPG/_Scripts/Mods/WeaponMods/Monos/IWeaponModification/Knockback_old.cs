

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{


    /// <summary>
    ///deprecated, will delete in future versions
    /// </summary>
    /// 
    [System.Obsolete]
    public class Knockback_old : MonoBehaviour, IWeaponModification
    {
        [HideInInspector]
        //public KnockbackVars Vars;
        IActorHub myself;
        bool active;

     

        public void DoModification(AttackValues other)
        {
            //if (IsActive() == false) return;

            //IReceiveKnockback receiver = other.GetInstance().GetComponent<IReceiveKnockback>();
            //if (receiver != null && receiver != myself)
            //{
            //    Vector3 direction = other.GetInstance().transform.position - this.transform.position;
            //    Rigidbody rb = other.GetInstance().GetComponent<Rigidbody>();
            //    KnockbackVars copy = new KnockbackVars(Vars.Force, Vars.Duration, direction.normalized, rb);
            //    ARPGDebugger.DebugMessage(other.GetInstance().name + " received knockback", other.GetInstance());
            //    receiver.DoKnockback(copy);
            //}
        }

        public void SetActive(bool isEnabled)
        {
            active = isEnabled;
        }

        public bool IsActive()
        {
            return active;
        }

        public void SetUser(IActorHub myself)
        {
            this.myself = myself;
        }

        public Transform GetTransform() => this.transform;

        public bool DoChange(Transform other)
        {
            return false;
        }
    }
}
