using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class PulLVars
    {
        public float Duration = 1;
        [HideInInspector]
        public Vector3 Target;
        [HideInInspector]
        public Vector3 Start;
        public AnimationCurve Curve;
        public IActorHub Hub;
        public PulLVars(float duration, Vector3 target, Vector3 start, IActorHub pulled)
        {
            Target = target;
            Duration = duration;
            Start = start;
            Hub = pulled;
        }
    }

    public class PullTowardsTarget : MonoBehaviour, ITick
    {
        public PulLVars Vars;
        float timer = 0;
        bool active;
        Rigidbody rb;

     
        private void Start()
        {
            rb = Vars.Hub.MyTransform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            AddTicker();
            active = true;
           
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (active == false) return;
            timer += GetTickDuration();
            Vector3 lerp = Vector3.Lerp(Vars.Start, Vars.Target, Vars.Curve.Evaluate(timer / Vars.Duration));
            rb.MovePosition(lerp);
           // Vars.Hub.MyTransform.gameObject.GetComponent<Rigidbody>().position = lerp;
            //Vars.Hub.MyTransform.position = lerp;

            if (timer >= Vars.Duration)
            {
                rb.isKinematic = true;
                active = false;
                Destroy(this);
            }
        }

        public float GetTickDuration() => Time.deltaTime;
      

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }
}