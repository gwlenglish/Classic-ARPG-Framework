using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;

using UnityEngine;

[CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New Chance to SOT Mod")]
public class ApplyChanceToSOT : AbilityLogic
{
    [Range(0, 100)]
    public int chanceToApply = 20;
    public AdditionalSoTSourceVars SOTs = new AdditionalSoTSourceVars();

    public override bool CheckLogicPreRequisites(IActorHub forUser)
    {
        return true;
    }

    public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
    {
      
    }

    public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
    {
        int current = Random.Range(0, 101);
        if (current <= chanceToApply)
        {
            CombatHelper.DoAddAdditionalSoT(skillUser, SOTs);
        }

    }

   
}
