
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{


    public class TargetDummy : MonoBehaviour, IReceiveDamage, IScale
    {
        public int Level;
        public IAttributeUser MyStats { get; set; }
        [SerializeField]
        ResourceType healthResource = ResourceType.Health;
        [SerializeField]
        CombatGroupType[] combatGroups = new CombatGroupType[1] { CombatGroupType.Enemy };
        bool canhit;
        IActorHub owner = null;
        IUseFloatingText dungeoncanvas = null;
        bool immortal = true;

        public event Action<CombatResults> OnTakeDamage;
        public event Action<CombatResults> OnDied;
        protected CombatResults last;

        private void Awake()
        {
            MyStats = GetComponent<IAttributeUser>();
            dungeoncanvas = GetComponent<IUseFloatingText>();
        }

        private void Start()
        {
            canhit = true;
            MyStats.GetRuntimeAttributes().LevelUp(Level);
        }
        public void DeathSequence()
        {
            //never die
        }

        public Transform GetInstance()
        {
            return this.transform;
        }

        public bool IsDead()
        {
            return false;
        }

        public void TakeDamage(AttackValues values)
        {
            for (int i = 0; i < values.PhysicalAttack.Count; i++)
            {
                MyStats.GetRuntimeAttributes().ModifyNowResource(this.GetHealthResource(), -values.PhysicalAttack[i].PhysicalDamage);

            }
            for (int i = 0; i < values.ElementAttacks.Count; i++)
            {
                MyStats.GetRuntimeAttributes().ModifyNowResource(this.GetHealthResource(), -values.ElementAttacks[i].Damage);
            }

            last = new CombatResults(values, null);
            OnTakeDamage?.Invoke(last);
        }

        
     

        public int GetScaledLevel()
        {
            return Formulas.GetEnemyLevel(MyStats.GetRuntimeAttributes().MyLevel);
        }

        public int GetUNScaledLevel()
        {
            return Level;
        }

        public bool IsHurt()
        {
            return !canhit;
        }

        public void CheckDeath()
        {
           //target dummy doesn't really die
        }

        public ResourceType GetHealthResource()
        {
            return healthResource;
        }

       
        public CombatGroupType[] GetMyCombatGroup()
        {
            return combatGroups;
        }

        public void SetUNScaledLevel(int unscaled)
        {
            MyStats.GetRuntimeAttributes().LevelUp(unscaled);
        }

        public void SetCharacterThatHitMe(IActorHub user)
        {
           //we dont really care about saving this
        }

        public void SetUser(IActorHub forUser)
        {
            owner = forUser;
        }

        public void SetActorHub(IActorHub newHub) => owner = newHub;

        public void SetInvincible(bool isImmoratal) => immortal = isImmoratal;

     

        public IActorHub GetUser()
        {
            return null;
        }




        //bypasses the iframes

    }
}
