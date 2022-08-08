using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{


    public class GrappleHook : MonoBehaviour, IWeaponModification
    {
        public PulLVars Vars;
        bool isactive = false;
        IActorHub hub = null;
        public bool DoChange(Transform other)
        {
            return false;
        }

        public void DoModification(AttackValues other)
        {
            PullTowardsTarget pull = hub.MyTransform.gameObject.AddComponent<PullTowardsTarget>();
            PulLVars vars = new PulLVars(Vars.Duration, other.Defenders[0].MyTransform.position, hub.MyTransform.position, hub);
            vars.Duration = Vars.Duration;
            vars.Curve = Vars.Curve;
            pull.Vars = vars;
        }

        public Transform GetTransform() => this.transform;


        public bool IsActive() => isactive;


        public void SetActive(bool isEnabled) => isactive = isEnabled;


        public void SetUser(IActorHub myself) => hub = myself;
      

       
    }
}