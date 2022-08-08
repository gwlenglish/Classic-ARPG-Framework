using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.States.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class DashVars
    {
        public Vector3 Direction = new Vector3(0, 0, 1);
        public float Distance = 1;
        public float Duration = 1;
        public AnimationCurve Curve = null;
        public MoveType MoveType = MoveType.Transform;
    }
   
    /// <summary>
    /// the dash state
    /// </summary>
    public class DashState : ITick
    {
        DashVars vars;
        Vector3 start;
        Vector3 end;
        float timer = 0;
        Rigidbody rb = null;
        IActorHub owner = null;
        public DashState(IActorHub dasher, DashVars vars)
        {
            owner = dasher;
            this.start = dasher.MyTransform.position;
            this.end = (start + dasher.MyTransform.TransformDirection(vars.Direction) * vars.Distance);
            this.vars = vars;

            switch (vars.MoveType)
            {
                case MoveType.Rigidbody:
                    rb = dasher.MyTransform.GetComponent<Rigidbody>();
                    break;
            }
            AddTicker();
        }
            
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            timer += Time.deltaTime;
            Vector3 lerp = Vector3.Lerp(start, end, vars.Curve.Evaluate(timer / vars.Duration));
            switch (vars.MoveType)
            {
                case MoveType.Transform:
                    //
                    owner.MyTransform.position = lerp;
                    break;
                case MoveType.Rigidbody:
                    //
                    rb.MovePosition(lerp);
                    break;
            }
            
            if (timer >= vars.Duration)
            {
                //done
                RemoveTicker();
            }
        }

        public float GetTickDuration() => Time.deltaTime;


        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

    }

/// <summary>
/// applies a dash effect
/// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_Dash Mod")]

    public class Dash : AbilityLogic
    {
        public DashVars Vars;
        [System.NonSerialized]
        Dictionary<IActorHub, DashState> dictionary = new Dictionary<IActorHub, DashState>();
        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;
            return true;
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                if (dictionary.ContainsKey(skillUser))
                {
                    dictionary.Remove(skillUser);
                }
            }
           
           
        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                if (dictionary.ContainsKey(skillUser) == false)
                {
                    DashState vars = new DashState(skillUser, Vars);
                    dictionary[skillUser] = vars;
                }
            }
          
           
          
        }
    }
}