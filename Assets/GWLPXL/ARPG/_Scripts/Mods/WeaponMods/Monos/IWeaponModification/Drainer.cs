

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// Adds a ModifyResource SOT mod to a weapon
    /// </summary>
    public class Drainer : MonoBehaviour, IWeaponModification
    {
        public ModifyResourceVars Vars;
        bool active;
        //this can keep track of the stacks.
        IActorHub caster = null;
        public void DoModification(AttackValues other)
        {
            if (IsActive() == false) return;
            for (int i = 0; i < other.Defenders.Count; i++)
            {
                CombatHelper.DoAddDot(other.Defenders[i], Vars);
            }
           

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
            caster = myself;
        }
    }
}