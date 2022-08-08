using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.GameEvents.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GWLPXL.ARPGCore.com
{
   

    public class PlayerHealth : MonoBehaviour, IReceiveDamage
    {
        public event Action<CombatResults> OnTakeDamage;
        public event Action<CombatResults> OnDied;
        [SerializeField]
        protected PlayerDefault combatHandler = null;
        [SerializeField]
        protected PlayerHealthEvents healthEvents = new PlayerHealthEvents();
        [SerializeField]
        protected ResourceType healthResource = ResourceType.Health;
        [SerializeField]
        protected float iFrameTime = .25f;
        protected bool isDead = false;
        protected bool canBeAttacked = false;
        protected CombatGroupType[] combatGroups = new CombatGroupType[1] { CombatGroupType.Friendly };
        protected IActorHub owner = null;
        protected IActorHub lastCharacterToHitMe;
        protected bool immortal = false;
        protected CombatResults last;
        #region unity calls
        protected virtual void Awake()
        {
            Setup();
        }
        #endregion

        #region public interfaces
        public bool IsHurt()
        {
            return !canBeAttacked;
        }

        public ResourceType GetHealthResource()
        {
            return healthResource;
        }


        public CombatGroupType[] GetMyCombatGroup()
        {
            return combatGroups;
        }

        public void SetCharacterThatHitMe(IActorHub user)
        {
            lastCharacterToHitMe = user;
        }

        public void SetUser(IActorHub forUser)
        {
            owner = forUser;
        }

        public void SetInvincible(bool isImmoratal) => immortal = isImmoratal;

        public Transform GetInstance()
        {
            return transform;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void DeathSequence()
        {
            if (isDead) return;

            isDead = true;
            OnDied?.Invoke(last);
            NotifyCustomDeathEvent();

        }


        
        public virtual void CheckDeath()
        {
            if (owner.MyStats.GetRuntimeAttributes().GetResourceNowValue(healthResource) <= 0)
            {
                DeathSequence();
            }
        }
        #endregion

        #region protected virtual
        protected virtual void Setup()
        {
            canBeAttacked = true;
            if (combatHandler == null) combatHandler = ScriptableObject.CreateInstance<PlayerDefault>();
        }

       

      

        protected virtual void NotifyCustomDeathEvent()
        {

            healthEvents.SceneEvents.OnDie.Invoke();
            if (healthEvents.GameEvents.DeathEvent == null) return;

            healthEvents.GameEvents.DeathEvent.DiedObj = this.gameObject;
            GameEventHandler.RaiseDeathEvent(healthEvents.GameEvents.DeathEvent);

        }

        protected virtual void NotifyCustomDamageEvent(ElementType type, int eleDmgTaken)
        {
            healthEvents.SceneEvents.OnDamageTaken.Invoke(eleDmgTaken);
            if (healthEvents.GameEvents.TookDamageEvent == null) return;

            healthEvents.GameEvents.TookDamageEvent.EventVars = new DamageEvent(this, eleDmgTaken, type, eleDmgTaken.ToString(), transform.position + Vector3.up * 2f);
            GameEventHandler.RaisePlayerDamageEvent(healthEvents.GameEvents.TookDamageEvent);

        }


       

      

        protected virtual IEnumerator CanBeAttackedCooldown(float duration)
        {
            canBeAttacked = false;
            yield return new WaitForSeconds(duration);
            canBeAttacked = true;
        }

        public void TakeDamage(AttackValues values)
        {
            if (canBeAttacked == false || isDead)
            {
                return;
            }
            CombatResults results = combatHandler.TakeDamageFormula(values, owner);
            if (immortal == false)
            {

                for (int i = 0; i < results.DamageValues.ReportElementalDmg.Count; i++)
                {
                    if (results.DamageValues.ReportElementalDmg[i].Result > 0)//prevent dmg if immortal, but show everything else
                    {
                        owner.MyStats.GetRuntimeAttributes().ModifyNowResource(healthResource, -results.DamageValues.ReportElementalDmg[i].Result);
                    }

                }

                if (results.DamageValues.ReportPhysDmg.Result > 0)
                {
                    owner.MyStats.GetRuntimeAttributes().ModifyNowResource(healthResource, -results.DamageValues.ReportPhysDmg.Result);
                }

            }

            last = results;
            CombatLogger.AddResult(results);
            OnTakeDamage?.Invoke(results);
            NotifyUI(results);

            CheckDeath();
            StartCoroutine(CanBeAttackedCooldown(iFrameTime));//we are invulnerable for a short time
            SetCharacterThatHitMe(values.Attacker);
        }
        protected virtual void NotifyUI(CombatResults results)
        {

          //  if (can == null) return;
          //  dungeoncanvas.DamageResults(results);

        }
        public IActorHub GetUser()
        {
            return owner;
        }


        #endregion

    }



}