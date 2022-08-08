

using UnityEngine;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.com;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/NEW_StatusChangeHolder")]

    public class StatusChangeHolder : AbilityLogic
    {
        public AbilityStatusChange[] StatusChanges = new AbilityStatusChange[0];
        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;

            if (StatusChanges == null || StatusChanges.Length == 0) return false;
            for (int i = 0; i < StatusChanges.Length; i++)
            {
                if (StatusChanges[i] == null) return false;
            }
            return true;
        }
        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                for (int i = 0; i < StatusChanges.Length; i++)
                {
                    StatusChanges[i].ApplyStatus(skillUser);
                }
            }
            
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                Remove(skillUser.MyTransform);
                for (int i = 0; i < StatusChanges.Length; i++)
                {
                    StatusChanges[i].RemoveStatus(skillUser);
                }
            }
            
        }

       
    }
}