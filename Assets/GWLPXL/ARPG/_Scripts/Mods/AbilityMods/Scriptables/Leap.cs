using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;

using UnityEngine;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Movement.com;

using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.StatusEffects.com;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class LeapChargingVars
    {
        public float MaxLeapDistanceMultiplier = 2;
    }
    [System.Serializable]
    public class LeapVars
    {
        [HideInInspector]
        public LayerMask StopLayers;//hidden because not working as intended at the moment
        [Header("Horizontal Movement")]
        public AnimationCurve HorizontalCurve;
        public float Distance = 1;
        public float Duration = 1;
        [Header("Vertical Movement")]
        public AnimationCurve UpwardCurve;
        public AnimationCurve FallCurve;
        public float UpwardForce = 1;
        [Range(0, 1f)]
        public float TimeToApex = .6f;
        public MoveType MoveType = MoveType.Transform;
        public StatusEffectSO[] StatusEffects = new StatusEffectSO[0];
        public LeapChargingVars ChargingVars;

    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/Mods/New_Leap Mod")]

    public class Leap : AbilityLogic
    {
        public LeapVars MotionVars;
        public Vector3 direction = new Vector3(0, 0, 1);
        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;
            return true;
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            //
            Remove(skillUser.MyTransform);
        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            //knock state
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                ChargeHelper.CheckCharge(skillUser, theSkill);

                CombatHelper.DoLeap(skillUser, skillUser.MyTransform.TransformDirection(direction), MotionVars);
            }

        }

        
    }

    /// <summary>
    /// Known bugs with stairs
    /// </summary>
    public class LeapState : ITick
    {
        LeapVars vars;
        IActorHub knocked;
        Vector3 hitdirection;

        float timer;
        float falltimer;
        float apextimer;
        Vector3 start;
        float goaly = 0;
        bool active;
        bool stophorizontal;
        Vector3 previous;
        Vector3 lerp;
        Rigidbody rb = null;
        bool stopupwards;
        INavMeshMover nav;
        float distance;
        public LeapState(IActorHub knocked, Vector3 hitDirection, LeapVars vars)
        {
            this.vars = vars;
            this.knocked = knocked;
            this.hitdirection = hitDirection;

            float maxd = vars.Distance * vars.ChargingVars.MaxLeapDistanceMultiplier;
            this.distance = Mathf.Lerp(vars.Distance, maxd, knocked.MyAbilities.GetRuntimeController().GetChargedAmount());

            start = knocked.MyTransform.position;
            goaly = knocked.MyTransform.position.y * vars.UpwardForce;

            switch (vars.MoveType)
            {
                case MoveType.Rigidbody:
                    rb = knocked.MyTransform.GetComponent<Rigidbody>();
                    knocked.NavMeshAgent.GetAgent().enabled = false;
                    break;
            }

            nav = knocked.NavMeshAgent;
            lerp = start;
            AddTicker();
        }






        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
            active = true;
        }

        public void DoTick()
        {
            if (active == false) return;

            timer += GetTickDuration();
            apextimer += GetTickDuration();

            #region known issues with uneven ground, ie stairs, failed attempts to fix
            if (stophorizontal == false)
            {
                Ray ray = new Ray(knocked.MyTransform.position, hitdirection);
                RaycastHit hit;
                Physics.SphereCast(previous, .01f, knocked.MyTransform.TransformDirection(hitdirection * vars.Distance * vars.HorizontalCurve.Evaluate((timer + GetTickDuration()) / vars.Duration)), out hit, vars.StopLayers);
                if (hit.collider != null)
                {
                    stophorizontal = true;
                    Debug.Log("Stopped");
                }

            }

            if (stopupwards == false)
            {
                float currenty = knocked.MyTransform.position.y;
                Ray ray = new Ray(knocked.MyTransform.position, hitdirection);
                RaycastHit hit;
                Physics.SphereCast(previous, .01f, knocked.MyTransform.TransformDirection(new Vector3(0, currenty, 0) * vars.Distance * vars.UpwardCurve.Evaluate((timer + GetTickDuration()) / vars.Duration)), out hit, vars.StopLayers);
                if (hit.collider != null)
                {
                    stopupwards = true;
                    Debug.Log("Stopped");
                }
            }
            #endregion

            if (stophorizontal == false)
            {
                lerp = Vector3.Lerp(start, start + (hitdirection * distance), vars.HorizontalCurve.Evaluate(timer / vars.Duration));
            }
            else
            {
                lerp = previous;

            }
            lerp.y = 0;


          

            float lerpy = start.y;

           

            if (apextimer <= (vars.Duration * vars.TimeToApex))
            {
                //upwards
                if (stopupwards == false)
                {
                    lerpy = Mathf.Lerp(start.y, goaly, apextimer / vars.UpwardCurve.Evaluate(vars.Duration / vars.TimeToApex));
                }

            }
            else
            {
                falltimer += GetTickDuration();
                //downwards
                lerpy = Mathf.Lerp(goaly, start.y, (falltimer / vars.FallCurve.Evaluate(1 - vars.TimeToApex)));
            }


            Vector3 next = lerp + new Vector3(0, lerpy, 0);

         

            switch (vars.MoveType)
            {
                case MoveType.Transform:
                    knocked.MyTransform.position = next;
                    break;
                case MoveType.Rigidbody:
                    rb.MovePosition(next);
                    break;
            }

            previous = lerp;
            if (timer >= vars.Duration)
            {
                RemoveTicker();
            }
        }

        public float GetTickDuration() => Time.deltaTime;


        public void RemoveTicker()
        {

            active = false;
            knocked.NavMeshAgent.GetAgent().enabled = true;
            TickManager.Instance.RemoveTicker(this);
        }

       
    }
}