

using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// Just an example on how to add an additional custom damage source to a melee weapon. For now, this must come before the MeleeCombatLogic in the array.
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_ExampleCustomDamage")]

    public class ApplyCustomDamageMod : AbilityLogic
    {
        [System.NonSerialized]
        Dictionary<Transform, ExampleCustomDamage> buffed = new Dictionary<Transform, ExampleCustomDamage>();


        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;
            return true;
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                IMeleeCombatUser meleeT = skillUser.MyMelee;
                if (meleeT == null)
                {
                    Debug.Log("No melee combat user");
                }
                Transform[] combatT = meleeT.GetMeleeTransforms();
                if (combatT == null || combatT.Length == 0)
                {
                    Debug.Log("No transform to remove from ability");
                }
                for (int i = 0; i < combatT.Length; i++)
                {
                    Debug.Log(combatT[i].name);
                    buffed.TryGetValue(combatT[i], out ExampleCustomDamage value);
                    if (value != null)
                    {
                        Component.Destroy(value);
                        buffed.Remove(combatT[i]);
                    }
                }
            }
           

        }

      

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                Transform[] meleeT = skillUser.MyMelee.GetMeleeTransforms();
                for (int i = 0; i < meleeT.Length; i++)
                {
             
                    buffed.TryGetValue(meleeT[i], out ExampleCustomDamage value);
                    if (value == null)
                    {
                        ExampleCustomDamage generate = meleeT[i].gameObject.AddComponent<ExampleCustomDamage>();
                        IWeaponModification statusChange = generate as IWeaponModification;
                        statusChange.SetActive(true);
                        buffed[meleeT[i]] = generate;
                        Debug.Log("Added custom", meleeT[i]);
                    }
                    else
                    {
                        value.GetComponent<IWeaponModification>().SetActive(true);
                    }
                }
            }
                    

            


        }
    }
}
