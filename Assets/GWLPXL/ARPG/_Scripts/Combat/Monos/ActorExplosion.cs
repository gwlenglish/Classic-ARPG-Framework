using GWLPXL.ARPGCore.com;

using UnityEngine;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class ExplosionVars
    {
        public ActorDamageData ActorDamage = null;
        public GameObject ExplosionPrefab = null;
        [HideInInspector]
        public IActorHub Owner = null;
        public bool StickToTarget = true;
        public float EndRadius = 3;
        public AnimationCurve ExplosionCurve = null;
        public float Delay = 1;
        public float Duration = .25f;
  
        public ExplosionVars(ActorDamageData damage, IActorHub owner)
        {
            ActorDamage = damage;
            Owner = owner;
        }
    }

    public class ActorExplosion : MonoBehaviour, ITick
    {
        public UnityEvent OnExplodeStart;
        public UnityEvent OnExplodeStop;
        public ActorDamageEvents DamageEvents;
        public ExplosionVars Vars = null;
        bool complete = false;
        float timer = 0;
        bool exploded = false;
        IDoDamage[] dmg = null;
        SphereCollider sphere;
        private void Start()
        {
          
            dmg = GetComponentsInChildren<IDoDamage>();
            IDoActorDamage[] actord = GetComponentsInChildren<IDoActorDamage>();
            for (int i = 0; i < actord.Length; i++)
            {
                actord[i].SetDamageData(Vars.ActorDamage);
            }
          
            AddTicker();
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        void StartExplode()
        {
            exploded = true;
            sphere = gameObject.AddComponent<SphereCollider>();
            sphere.enabled = false;
            sphere.isTrigger = true;
            sphere.enabled = true;
            for (int i = 0; i < dmg.Length; i++)
            {
                dmg[i].EnableDamageComponent(true, Vars.Owner);
            }

            OnExplodeStart.Invoke();
        }
        public void DoTick()
        {
            if (complete) return;
            timer += GetTickDuration();

            if (timer >= Vars.Delay)
            {
                if (exploded == false)
                {
                    StartExplode();
                }

                ExplosionIncrease();

            }

            if (timer > Vars.Delay + Vars.Duration)
            {
                StopExplode();
            }

        }

        private void StopExplode()
        {
            complete = true;
            sphere.enabled = false;
            for (int i = 0; i < dmg.Length; i++)
            {
                dmg[i].EnableDamageComponent(false, Vars.Owner);
            }
        }

        private void ExplosionIncrease()
        {
            float percent = (timer - Vars.Delay) / Vars.Duration;
            if (Vars.ExplosionCurve != null)
            {
                percent = Vars.ExplosionCurve.Evaluate(percent);
            }
            float newRadius = Mathf.Lerp(0, Vars.EndRadius, percent);
            sphere.radius = newRadius;
        }

        

        public float GetTickDuration() => Time.deltaTime;   
       

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

       
    }
}