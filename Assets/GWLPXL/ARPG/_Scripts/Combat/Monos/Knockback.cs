using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    public enum MoveType
    {
        Transform = 0,
        Rigidbody = 1
    }
    [System.Serializable]
    public class KnockBackVars
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
        [Tooltip("Allow the other stat Knockback Resistance to reduce the force of the knockback")]//to do, also limit duration
        public bool AllowKnockBackResistance = true;
        [Tooltip("The value used to calculate how much knockback resistance is required to be immune to the knockback.")]
        public int KBResistForImmune = 100;

    }

    /// <summary>
    /// class that controls the knockback state, in the future add a condition where an actor can be knockbcak proof
    /// </summary>
    public class KnockbackState : ITick
    {
        KnockBackVars vars;
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
        float knockbackmulti = 1;

        public KnockbackState(IActorHub knocked, Vector3 hitDirection, KnockBackVars vars)
        {
            this.vars = vars;
            this.knocked = knocked;
            this.hitdirection = hitDirection;


            start = knocked.MyTransform.position;
            goaly = knocked.MyTransform.position.y * vars.UpwardForce;

            switch (vars.MoveType)
            {
                case MoveType.Rigidbody:
                    rb = knocked.MyTransform.GetComponent<Rigidbody>();
                    knocked.NavMeshAgent.GetAgent().enabled = false;
                    break;
            }

           
            lerp = start;
            bool allowknock = true;

            if (knocked.MyStats != null && vars.AllowKnockBackResistance)
            {
                int knockback = knocked.MyStats.GetRuntimeAttributes().GetOtherAttributeNowValue(Types.com.OtherAttributeType.KnockBackResistance);
                if (vars.KBResistForImmune == 0) vars.KBResistForImmune = 100;//preventing divide by 0
                float knockednormalized = knockback / vars.KBResistForImmune;
                knockbackmulti = (1 - knockednormalized);//calculate the new force multi

                if (knockbackmulti <= 0)
                {
                    //we are immune, dont allow knockback.
                    allowknock = false;
                }
            }

            if (allowknock)
            {
                AddTicker();
            }

        }

       

        public void AddTicker()
        {
            StatusEffectHelper.ApplyStatusEffects(knocked, vars.StatusEffects);
            TickManager.Instance.AddTicker(this);
            active = true;
        }

        public void DoTick()
        {
            if (active == false) return;

            timer += GetTickDuration();
            apextimer += GetTickDuration();

            if (stophorizontal == false)
            {
                Ray ray = new Ray(knocked.MyTransform.position, hitdirection);
                RaycastHit hit;
                Physics.SphereCast(previous, 1f, hitdirection * vars.Distance * vars.HorizontalCurve.Evaluate((timer + GetTickDuration()) / vars.Duration), out hit, vars.StopLayers);
                if (hit.collider != null)
                {
                    stophorizontal = true;
                    //Debug.Log("Stopped");
                }

            }

            if (stophorizontal == false)
            {
                lerp = Vector3.Lerp(start, start + (hitdirection *vars.Distance), vars.HorizontalCurve.Evaluate(timer / vars.Duration) * knockbackmulti);
            }
            else
            {
                lerp = previous;

            }
            lerp.y = 0;



            float lerpy = start.y;
            if (apextimer <= (vars.Duration * vars.TimeToApex ))
            {
                //upwards
                lerpy = Mathf.Lerp(start.y, goaly, apextimer / vars.UpwardCurve.Evaluate(vars.Duration /vars.TimeToApex));

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
            StatusEffectHelper.RemoveStatusEffects(knocked, vars.StatusEffects);
            active = false;
            //knocked.NavMeshAgent.GetAgent().enabled = true;
            TickManager.Instance.RemoveTicker(this);
        }


    }
    /// <summary>
    ///  Mods the weapon to have an AOE knockback effect
    /// </summary>
    public class Knockback : MonoBehaviour, IWeaponModification
    {
        public KnockBackVars Vars;
        bool isactive = false;
        IActorHub hub = null;
        public bool DoChange(Transform other)
        {
            return false;
        }

        public void DoModification(AttackValues other)
        {
            Vector3 direction = other.Defenders[0].MyTransform.position - hub.MyTransform.position;
            direction.y = 0;
            direction.Normalize();
            KnockbackState state = new KnockbackState(other.Defenders[0], direction, Vars);
        }

        public Transform GetTransform() => this.transform;


        public bool IsActive() => isactive;

        public void SetActive(bool isEnabled) => isactive = isEnabled;


        public void SetUser(IActorHub myself) => hub = myself;
       

       
    }
}