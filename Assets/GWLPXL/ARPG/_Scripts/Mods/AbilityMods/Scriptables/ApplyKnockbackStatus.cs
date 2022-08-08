
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// deprecated, use ApplyKnockbackMod, will be removed eventually
    /// </summary>
    public class ApplyKnockbackStatus : AbilityStatusChange
    {
        public KnockBackVars Vars;

        [System.NonSerialized]
        Dictionary<Transform, Knockback> knockbacks = new Dictionary<Transform, Knockback>();

        private void Apply(Transform[] key)
        {
            if (key == null || key.Length == 0) return;

            for (int i = 0; i < key.Length; i++)
            {
                knockbacks.TryGetValue(key[i], out Knockback knockback);
                if (knockback == null)
                {
                    knockback = key[i].gameObject.AddComponent<Knockback>();
                    knockback.Vars = this.Vars;
                    IWeaponModification statusChange = knockback.GetComponent<IWeaponModification>();
                    statusChange.SetActive(true);
                    knockbacks.Add(key[i], knockback);
                }
                else
                {
                    knockback.GetComponent<IWeaponModification>().SetActive(true);
                }
            }
          
        }

        public override void ApplyStatus(IActorHub attacker)
        {
            IMeleeCombatUser meleeT = attacker.MyMelee;
            if (meleeT == null) return;
            Apply(meleeT.GetMeleeTransforms());
        }

      

        private void Remove(Transform[] key)
        {
            if (key == null || key.Length == 0) return;
            for (int i = 0; i < key.Length; i++)
            {
                knockbacks.TryGetValue(key[i], out Knockback knockback);
                if (knockback != null)
                {
                    knockback.GetComponent<IWeaponModification>().SetActive(false);
                }
            }
           
        }

        public override void RemoveStatus(IActorHub attacker)
        {
            IMeleeCombatUser meleeT = attacker.MyMelee;
            if (meleeT == null) return;
            Remove(meleeT.GetMeleeTransforms());
        }
    }
}