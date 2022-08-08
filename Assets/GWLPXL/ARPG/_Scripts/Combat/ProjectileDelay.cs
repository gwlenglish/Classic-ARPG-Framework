
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class ShooterProjectileDelayVars : ITick
    {
        public ShooterProjectileVars Vars;
        public float Duration;
        public IActorHub User;

        public ShooterProjectileDelayVars(float duration, IActorHub user, ShooterProjectileVars prj)
        {
            Duration = duration;
            User = user;
            Vars = prj;
            AddTicker();
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            CombatHelper.DoFireShooterProjectile(User, Vars);
            RemoveTicker();
        }

        public float GetTickDuration()
        {
            return Duration;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }



    [System.Serializable]
    public class ProjectileDelayVars : ITick
    {
        public ProjectileVariables Vars;
        public float Duration;
        public IActorHub User;

        public ProjectileDelayVars(float duration, IActorHub user, ProjectileVariables prj)
        {
            Duration = duration;
            User = user;
            Vars = prj;
            AddTicker();
        }

        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            CombatHelper.DoFireAndInIProjectile(User, Vars);
            RemoveTicker();
        }

        public float GetTickDuration()
        {
            return Duration;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }
    }

    /// <summary>
    /// i prefer the non mono versions above, ShooterProjectileDelayVars and ProjectileDelayVars
    /// </summary>
    public class ProjectileDelay : MonoBehaviour, ITick
    {
        public ProjectileDelayVars Vars;
        bool start;
        private void Start()
        {
            AddTicker();
            start = true;
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
            if (start == false) return;
            CombatHelper.DoFireAndInIProjectile(Vars.User, Vars.Vars);
            Destroy(this);
        }

        public float GetTickDuration() => Vars.Duration;
       

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

       
    }
}