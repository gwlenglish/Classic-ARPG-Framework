using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.StatusEffects.com;

using UnityEngine;

public class StatModTicker : ITick
{
    float t;
    ModStatStatus m;
    IActorHub u;
    bool removed;
    public StatModTicker(float duration, IActorHub user, ModStatStatus m)
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
        u.MyStats.GetRuntimeAttributes().RemoveModifierStat(m.Type, m.Modifier);
        removed = true;
        RemoveTicker();
    }

    public float GetTickDuration()
    {
        return t;
    }

    public void RemoveTicker()
    {
        if (removed == false) u.MyStats.GetRuntimeAttributes().RemoveModifierStat(m.Type, m.Modifier);
        TickManager.Instance.RemoveTicker(this);
    }
}

[CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/NEW_Stat_Duration")]
public class StatModDuration : AbilityLogic
{
    [Tooltip("if Duration is 0 or less, will end with ability")]
    public float Duration = 0;

    public ModifyStat Mod;

    public override bool CheckLogicPreRequisites(IActorHub forUser)
    {
        return true;
    }

    public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
    {
        ///does it on duration
        if (Duration > 0)
        {
            //let ticker handle
        }
        else
        {
            Mod.UnDoLogic(skillUser);
            Remove(skillUser.MyTransform);
        }
    }

    public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
    {
        Mod.DoLogic(skillUser);
        if (Duration > 0)
        {
            StatModTicker ticker = new StatModTicker(Duration, skillUser, Mod.Vars);
            ticker.AddTicker();

        }
        else
        {
            Add(skillUser.MyTransform);
        }


    }

    

  
}
