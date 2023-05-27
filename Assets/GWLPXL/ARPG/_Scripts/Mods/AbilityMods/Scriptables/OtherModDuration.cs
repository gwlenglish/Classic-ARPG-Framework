using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModTicker : ITick
{
    float t;
    ModifyOTher m;
    IActorHub u;
    bool removed;
    public ModTicker(float duration, IActorHub user, ModifyOTher m)
    {
        removed = false;
        this.u = user;
        this.t = duration;
        this.m = m;
    }
    public void AddTicker()
    {
        TickManager.Instance.AddTicker(this);
    }

    public void DoTick()
    {
        u.MyStats.GetRuntimeAttributes().RemoveModifierOther(m.Type, m.Modifier);
        removed = true;
        RemoveTicker();
    }

    public float GetTickDuration()
    {
        return t;
    }

    public void RemoveTicker()
    {
        if (removed == false) u.MyStats.GetRuntimeAttributes().RemoveModifierOther(m.Type, m.Modifier);
        TickManager.Instance.RemoveTicker(this);
    }
}

[CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/NEW_OtherStatMod_Duration")]
public class OtherModDuration : AbilityLogic
{
    public float Duration = 3;
    public ModifyOtherStat Mod;

    public override bool CheckLogicPreRequisites(IActorHub forUser)
    {
        return true;
    }

    public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
    {
        ///does it on duration
    }

    public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
    {
        Add(skillUser.MyTransform);
        Mod.DoLogic(skillUser);
        ModTicker ticker = new ModTicker(Duration, skillUser, Mod.Vars);
        ticker.AddTicker();
  
       

    }

    

  
}
