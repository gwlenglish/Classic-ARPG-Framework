using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{

    ///// <summary>
    ///// works, now clean up
    ///// </summary>
    //public class KnockedBack : MonoBehaviour, ITick, IDoT
    //{
    //    public KnockBackVars Vars;
    //    float timer = 0;
    //    float apextimer = 0;
    //    float falltimer = 0;
    //    Rigidbody rb = null;

    //    Vector3 start;
    //    float goaly = 0;
    //    bool active;
    //    bool stophorizontal;
    //    Vector3 previous;
    //    Vector3 lerp;

    //    private void Start()
    //    {
 
    //        rb = Vars.Knocked.MyTransform.GetComponent<Rigidbody>();
    //        if (rb != null)
    //        {
    //            rb.isKinematic = false;
    //        }
  
          
    //        start = Vars.Knocked.MyTransform.position;
    //        goaly = start.y * Vars.UpwardForce;
      
    //        active = true;
    //        lerp = Vars.Knocked.MyTransform.position;
    //        RaycastHit hit;
    //        Physics.SphereCast(previous, 1f, Vars.HitDirection * Vars.Distance * Vars.HorizontalCurve.Evaluate((timer + GetTickDuration()) / Vars.Duration), out hit, Vars.StopLayers);
    //        if (hit.collider != null)
    //        {
    //            stophorizontal = true;
    //            Debug.Log("Stopped");
    //        }

    //        if (stophorizontal)
    //        {
    //            Destroy(this);
    //        }
    //        else
    //        {
    //            AddTicker();
    //            ApplyDoT();
    //        }

       
    //    }

     

    //    public void AddTicker()
    //    {
    //        TickManager.Instance.AddTicker(this);
    //    }

    //    public void DoTick()
    //    {
    //        if (active == false) return;

    //        timer += GetTickDuration();
    //        apextimer += GetTickDuration();

    //        if (stophorizontal == false)
    //        {
    //            Ray ray = new Ray(Vars.Knocked.MyTransform.position, Vars.HitDirection);
    //            RaycastHit hit;
    //            Physics.SphereCast(previous, 1f, Vars.HitDirection * Vars.Distance * Vars.HorizontalCurve.Evaluate((timer + GetTickDuration()) / Vars.Duration), out hit, Vars.StopLayers);
    //            if (hit.collider != null)
    //            {
    //                stophorizontal = true;
    //                Debug.Log("Stopped");
    //            }
                
    //        }
          
    //        if (stophorizontal == false)
    //        {
    //            lerp = Vector3.Lerp(start, start + (Vars.HitDirection * Vars.Distance), Vars.HorizontalCurve.Evaluate(timer / Vars.Duration));
    //        }
    //        else
    //        {
    //            lerp = previous;

    //        }
    //        lerp.y = 0;



    //        float lerpy = start.y;
    //        if (apextimer <= (Vars.Duration * Vars.TimeToApex))
    //        {
    //            //upwards
    //            lerpy = Mathf.Lerp(start.y, goaly, apextimer / Vars.UpwardCurve.Evaluate(Vars.Duration / Vars.TimeToApex));

    //        }
    //        else
    //        {
    //            falltimer += GetTickDuration();
    //            //downwards
    //            lerpy = Mathf.Lerp(goaly, start.y, (falltimer / Vars.FallCurve.Evaluate(1 - Vars.TimeToApex)));
    //        }


    //        Vector3 next = lerp + new Vector3(0, lerpy, 0);
           
    //        switch (Vars.KnockType)
    //        {
    //            case MoveType.Transform:
    //                transform.position = next;
    //                break;
    //            case MoveType.Rigidbody:
    //                rb.MovePosition(next);
    //                break;
    //        }

    //        previous = lerp;
    //        if (timer >= Vars.Duration)
    //        {
    //            active = false;
    //            if (rb != null)
    //            {
    //                rb.isKinematic = true;

    //            }
    //            RemoveDoT();
    //            RemoveTicker();
    //        }
    //    }

    //    public float GetTickDuration() => Time.deltaTime;
      

    //    public void RemoveTicker()
    //    {
    //        TickManager.Instance.RemoveTicker(this);
    //        Destroy(this);
    //    }

    //    public void ApplyDoT()
    //    {
    //        Vars.Knocked.MyStatusEffects.AddDoT(this);
    //    }

    //    public void RemoveDoT()
    //    {
    //        Vars.Knocked.MyStatusEffects.RemoveDot(this);
    //    }

    //    public StatusInflictions[] GetEffects()
    //    {
    //        return new StatusInflictions[1] { StatusInflictions.Knockback };
    //    }
    //}
}