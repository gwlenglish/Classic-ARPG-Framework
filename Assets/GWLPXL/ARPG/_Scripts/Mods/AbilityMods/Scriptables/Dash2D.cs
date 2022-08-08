using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.States.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class DashVars2D
    {
        public float Distance = 5;
        public AnimationCurve Curve = null;
        public float Duration = 1;
        [Tooltip("Will override the Duration set and match with ability end.")]
        public bool MatchDurationToAbilityDuration = true;
        [Tooltip("Relative to Facing. So Right will get the caster's face forward direction and rotate it 90 degrees to the right.")]
        public LocalActorDirection DashDirection = LocalActorDirection.Facing;
        [Tooltip("The Angle Axis which the character rotates. For 2D, this will generally be (0, 0, 1). For 3D, (0, 1, 0)")]
        public Vector3 RotateVector = new Vector3(0, 0, 1);
        public MoveType MoveType = MoveType.Rigidbody;
    }
   
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_Dash2D Mod")]

    public class Dash2D : AbilityLogic
    {
        [Header("2D Version of the Dash")]
        public DashVars2D Vars;
        [System.NonSerialized]
        Dictionary<IActorHub, Dash2DState> dictionary = new Dictionary<IActorHub, Dash2DState>();
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
                Add(skillUser.MyTransform);
                if (dictionary.ContainsKey(skillUser) == false)
                {
                    Dash2DState vars = new Dash2DState(skillUser, Vars, theSkill);
                    dictionary[skillUser] = vars;
                }
            }
          
            

        }


    }

    /// <summary>
    /// 2d version of the dash
    /// </summary>
    public class Dash2DState : ITick
    {
        DashVars2D vars;
        Vector3 start;
        Vector3 end;
        float timer = 0;
        Rigidbody2D rb = null;
        IActorHub owner = null;
        Ability ability;

        readonly int rightangle = -90;
        readonly int leftangle = 90;
        readonly int behindangle = 180;

        bool useTimer;
        public Dash2DState(IActorHub dasher, DashVars2D vars, Ability ability)
        {
            this.ability = ability;
            owner = dasher;
            this.start = dasher.MyTransform.position;

            Vector3 dir = new Vector3(0, 0, 0);
            switch (vars.DashDirection)
            {
                case LocalActorDirection.Facing:
                    dir = dasher.MyStateMachine.Get2D().GetFacingVector();
                    break;
                case LocalActorDirection.Right:
                    dir = Quaternion.AngleAxis(rightangle, vars.RotateVector) * dasher.MyStateMachine.Get2D().GetFacingVector();
                    break;
                case LocalActorDirection.Left:
                    dir = Quaternion.AngleAxis(leftangle, vars.RotateVector) * dasher.MyStateMachine.Get2D().GetFacingVector();
                    break;
                case LocalActorDirection.Behind:
                    dir = Quaternion.AngleAxis(behindangle, vars.RotateVector) * dasher.MyStateMachine.Get2D().GetFacingVector();
                    break;
            }

            end = dasher.MyTransform.position + (dir.normalized * vars.Distance);
            this.vars = vars;

            switch (vars.MoveType)
            {
                case MoveType.Rigidbody:
                    rb = dasher.MyTransform.GetComponent<Rigidbody2D>();
                    break;
            }


            if (vars.MatchDurationToAbilityDuration && ability != null)
            {
                dasher.MyAbilities.GetRuntimeController().OnAbilityEnd += AbilityEnd;
            }
            else
            {
                useTimer = true;
            }

            AddTicker();
        }

        void AbilityEnd(Ability ability)
        {
            if (this.ability == ability)
            {
                owner.MyAbilities.GetRuntimeController().OnAbilityEnd -= AbilityEnd;
                RemoveTicker();
            }
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            timer += Time.deltaTime;
            float percent = timer / vars.Duration;
            if (vars.Curve != null)
            {
                percent = vars.Curve.Evaluate(percent);
            }
            Vector3 lerp = Vector3.Lerp(start, end, percent);
            switch (vars.MoveType)
            {
                case MoveType.Transform:
                    owner.MyTransform.position = lerp;
                    break;
                case MoveType.Rigidbody:
                    rb.MovePosition(lerp);
                    break;
            }

            if (timer >= vars.Duration && useTimer)
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
}