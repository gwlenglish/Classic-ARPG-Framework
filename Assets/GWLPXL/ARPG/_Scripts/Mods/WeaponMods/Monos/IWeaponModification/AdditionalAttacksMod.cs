
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{


    /// <summary>
    /// appllies a list of additional damage sources
    /// </summary>
    public class AdditionalAttacksMod : MonoBehaviour, IWeaponModification
    {
        public AdditionalAttacksVars Vars;
        bool isactive = false;
        IActorHub user = null;

        public bool DoChange(Transform other)
        {
            return false;
        }

        public void DoModification(AttackValues other)
        {
            if (IsActive() == false) return;

            for (int i = 0; i < Vars.Vars.Count; i++)
            {
                CombatHelper.GetAdditionalDamageSourceActor(other, Vars.Vars[i]);
            }


        }

        public Transform GetTransform() => this.transform;


        public bool IsActive() => isactive;


        public void SetActive(bool isEnabled) => isactive = isEnabled;


        public void SetUser(IActorHub myself) => user = myself;

    }
}