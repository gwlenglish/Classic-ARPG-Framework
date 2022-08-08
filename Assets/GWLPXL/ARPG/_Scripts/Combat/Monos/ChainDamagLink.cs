

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.StatusEffects.com;

using GWLPXL.ARPGCore.DebugHelpers.com;

using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;

namespace GWLPXL.ARPGCore.Combat.com
{


    public class ChainDamagLink : MonoBehaviour, ITick
    {
        ChainDamageVars vars;
        IActorHub dmgtarget;
        IChainDamageTracker chainstart;

        float dt = .02f;
        bool active = false;
        int chainindex = 0;
        public void SetDamageVars(ChainDamageVars newvars, int mychainindex, IActorHub target, IChainDamageTracker start)
        {
            dmgtarget = target;
            vars = newvars;
            chainstart = start;
            chainstart.AddDamageTarget(target);
            chainindex = mychainindex;
        }

        private void Start()
        {
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

            

            ChainVars current = vars.Chains[chainindex];
            AttackValues values = new AttackValues(chainstart.GetCaster(), dmgtarget);
            //do the dmg here
            //do the dmg here


            if (current.Damage.AdditionalDamage.PhysMultipler.PercentOfCasterAttack > 0 )
            {
                int add = current.Damage.AdditionalDamage.PhysMultipler.GetPhysicalDamageAmount(chainstart.GetCaster());
                values.PhysicalAttack.Add(new PhysicalAttackResults(add, "Chain Damage Link"));
                if (add > 0)
                {
                    ARPGDebugger.CombatDebugMessage(ARPGDebugger.GetColorForChain("Chain Damage: ") + ARPGDebugger.GetColorForDamage(add.ToString()) + " on " + dmgtarget.ToString(), this);
                }
            }

            if (current.Damage.AdditionalDamage.ElementMultiplers.Length > 0)
            {
                for (int i = 0; i < current.Damage.AdditionalDamage.ElementMultiplers.Length; i++)
                {
                    int add = current.Damage.AdditionalDamage.ElementMultiplers[i].GetElementDamageAmount(chainstart.GetCaster());
                    values.ElementAttacks.Add(new ElementAttackResults(current.Damage.AdditionalDamage.ElementMultiplers[i].DamageType, add, "Chain Damage Link"));
                    if (add > 0)
                    {
                        ARPGDebugger.CombatDebugMessage(ARPGDebugger.GetColorForChain("Chain Damage: ") + ARPGDebugger.GetColorForDamage(add.ToString()) + " on " + dmgtarget.ToString(), this);
                    }
                }
            }
            
            
  
       


            if (current.SoT.StatusOverTimeOptions.AdditionalDOTs.Length > 0)
            {
                for (int i = 0; i < current.SoT.StatusOverTimeOptions.AdditionalDOTs.Length; i++)
                {
                    SoTHelper.AddDoT(dmgtarget, current.SoT.StatusOverTimeOptions.AdditionalDOTs[i]);
                   // dmgtarget.MyStatusEffects.AddDoT(current.SoT.StatusOverTimeOptions.AdditionalDOTs[i]);
                }
            }

            values.Resolve();
            chainstart.TryDamageLink(vars, transform.position);
       
            Destroy(this);
        }

        public float GetTickDuration()
        {

            return vars.Chains[chainindex].DamageDelay;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

      
    }
}
