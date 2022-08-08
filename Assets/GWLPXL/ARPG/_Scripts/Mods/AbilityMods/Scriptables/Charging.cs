using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// performs charging logic for the next ability to lead into
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_Charging")]

    public class Charging : AbilityLogic
    {

        public ChargingVars ChargingVars = new ChargingVars(null, ChargeType.None,  2f, 1f);

        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;

            return true;
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                Remove(skillUser.MyTransform);
                if (ChargingVars.AbilityToCharge == null) return;
                skillUser.MyAbilities.SetChargedAbility(ChargingVars.AbilityToCharge);//tells the user that the ability uses the charge.
            }
    
        }

       


        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);

                theSkill.CoolDownRate = 0;//charging should not have any cooldown rates.
                ChargeTimer timer = new ChargeTimer(skillUser, theSkill, new ChargingVars(ChargingVars.AbilityToCharge, ChargingVars.Type, ChargingVars.MaxCharge, ChargingVars.TimeToMaxCharge));
              
            }
           
        }


    }
}


public enum ChargeType
{
    None = 0,
    InstantOnRelease = 1,
    FollowThroughOnRelease = 2
}
[System.Serializable]
public class ChargingVars
{
    [Tooltip("The Ability that we are charging up")]
    public Ability AbilityToCharge;
    public ChargeType Type;
    [Tooltip("1 is default damage, 2 is 200% damage.")]
    public float MaxCharge = 2;
    public float TimeToMaxCharge;
    public ChargingVars(Ability next, ChargeType type, float maxCharge, float timeToMax)
    {
        MaxCharge = maxCharge;
        AbilityToCharge = next;
        Type = type;

        TimeToMaxCharge = timeToMax;
    }
}
public class ChargeTimer : ITick
{
    public ChargingVars Vars => vars;
    public Ability Current => current;
    public AbilityDurationTimer Timer => dtimer;
    IActorHub user;
    protected ChargingVars vars;
    protected Ability current;
    protected AbilityDurationTimer dtimer;
    protected float chargetimer = 0;
    protected bool ended;
    public ChargeTimer(IActorHub user, Ability current, ChargingVars vars)
    {
        this.user = user;
        dtimer = user.MyAbilities.GetRuntimeController().GetTimer(current);
        dtimer.Cooldown.Pause = true;
        this.current = current;
        this.vars = vars;
        user.MyAbilities.GetRuntimeController().SetChargeAmount(0);
        AddTicker();
    }

    public void AddTicker()
    {
        TickManager.Instance.AddTicker(this);
    }

    public void DoTick()
    {
        bool held = user.InputHub.AbilityInputs.GetAbilityInput(current);
        Debug.Log("Holding " + held);
        chargetimer += GetTickDuration();
        float percent = chargetimer / vars.TimeToMaxCharge;//using duration as the max charge length
        percent = Mathf.Clamp(0, vars.MaxCharge, percent);//clamping to max, so can't over charge. May want to reconsider this later
        user.MyAbilities.GetRuntimeController().SetChargeAmount(percent);


        if (held == true)
        {

            //holding
            dtimer.Cooldown.Pause = true;
           
        }
        else
        {


            EndCharge();
        }
    }

    public float GetTickDuration()
    {
        return Time.deltaTime;
    }

    void EndCharge()
    {
        if (ended) return;
        switch (vars.Type)
        {
            case ChargeType.InstantOnRelease:
                //
                user.MyAbilities.GetRuntimeController().InterruptAbility(current);
                dtimer.RemoveTicker();
                user.MyAbilities.TryCastAbility(vars.AbilityToCharge);
                RemoveTicker();
                break;
            case ChargeType.FollowThroughOnRelease:

                dtimer.Cooldown.Pause = false;
                user.MyAbilities.GetRuntimeController().OnAbilityEnd += QueueNext;
                break;
            default:
                //
                dtimer.Cooldown.Pause = false;
                RemoveTicker();
                break;
        }
        ended = true;
    }

    void QueueNext(Ability ability)
    {
        user.MyAbilities.GetRuntimeController().OnAbilityEnd -= QueueNext;
        user.MyAbilities.TryCastAbility(vars.AbilityToCharge);
        RemoveTicker();
    }
    public void RemoveTicker()
    {
        TickManager.Instance.RemoveTicker(this);

       

    }
}
