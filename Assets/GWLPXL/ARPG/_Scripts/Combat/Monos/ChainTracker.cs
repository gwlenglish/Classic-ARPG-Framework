using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.DebugHelpers.com;


namespace GWLPXL.ARPGCore.Combat.com
{
    public interface IChainDamageTracker
    {
        IActorHub GetCaster();
        void SetOriginalCaster(IActorHub caster);
        void ChainComplete();
        void AddDamageTarget(IActorHub damaged);
        bool HasHitTarget(IActorHub target);
        void TryDamageLink(ChainDamageVars vars, Vector3 atPos);
    }

    public class ChainTracker : MonoBehaviour, IChainDamageTracker
    {
        public GameObject TrailPrefab = null;
        GameObject trailinstance = null;
        List<IActorHub> damagedTargets = new List<IActorHub>();
        IActorHub originalcaster = null;


        int index = 0;
        public void TryDamageLink(ChainDamageVars vars, Vector3 atPos)
        {
            if (index >= vars.Chains.Length - 1) return;

            CombatFormulas form = DungeonMaster.Instance.CombatFormulas.GetCombatFormulas();
            ChainVars current = vars.Chains[index];
            List<IActorHub> next = new List<IActorHub>();
            IActorHub dmg = null;

            switch (current.ColliderType)
            {
                case ColliderType.Collider:
                    Collider[] colls = Physics.OverlapSphere(atPos, current.RadiusToNextJump, current.LayerMaskToCheck);
                    for (int i = 0; i < colls.Length; i++)
                    {
                        Debug.Log("Found " + colls[i].name);
                        dmg = colls[i].GetComponent<IActorHub>();
                        
                        if (dmg != null)// && next.Contains(dmg) == false)
                        {
                            if (current.ChainToNonDamagedTarget == true)
                            {
                                if (HasHitTarget(dmg) == true) continue;
                            }

                            if (form.DetermineAttackable(dmg, originalcaster, false))
                            {
                                IChainTarget chainTarget = dmg.MyTransform.GetComponent<IChainTarget>();
                                if (chainTarget != null)
                                {
                                    next.Add(dmg);

                                }

                            }

                        }
                    }
                    break;
                case ColliderType.Collider2D:
                    Collider2D[] colls2d = Physics2D.OverlapCircleAll(atPos, current.RadiusToNextJump, current.LayerMaskToCheck);
                    for (int i = 0; i < colls2d.Length; i++)
                    {
                        dmg = colls2d[i].GetComponent<IActorHub>();
                        if (dmg != null)// && next.Contains(dmg) == false)
                        {
                            if (current.ChainToNonDamagedTarget == true)
                            {
                                if (HasHitTarget(dmg) == true) continue;
                            }

                            if (form.DetermineAttackable(dmg, originalcaster, false))
                            {
                                IChainTarget chainTarget = dmg.MyTransform.GetComponent<IChainTarget>();
                                if (chainTarget != null)
                                {
                                    next.Add(dmg);

                                }

                            }

                        }
                    }

                    break;
            }

  

            //order next by position
            Dictionary<float, IActorHub> temp = new Dictionary<float, IActorHub>();
            List<float> squrdmags = new List<float>();
            for (int i = 0; i < next.Count; i++)
            {
              float sqrdmag = (next[i].MyTransform.position - atPos).sqrMagnitude;
                temp.Add(sqrdmag, next[i]);
                squrdmags.Add(sqrdmag); ;
            }
            squrdmags.Sort((p1, p2) => p1.CompareTo(p2));

            List<IActorHub> closest = new List<IActorHub>();
            for (int i = 0; i < vars.Chains[vars.CurrentChain].MaxNumberOfTargetsPerChain; i++)
            {
                if (i >= squrdmags.Count)
                {
                    break;
                }
                temp.TryGetValue(squrdmags[i], out IActorHub value);

                if (value != null)
                {
                    closest.Add(value);

             
                }

            }
            index += 1;
            ChainDamageVars newvars = new ChainDamageVars();
            newvars = vars;
            newvars.CurrentChain = index;
            //continue the chain damage lnk//limit the size
            for (int i = 0; i < closest.Count; i++)
            {

                ChainDamagLink dmglink = next[i].MyTransform.gameObject.AddComponent<ChainDamagLink>();
                dmglink.SetDamageVars(newvars, newvars.CurrentChain, next[i], this);
                ARPGDebugger.CombatDebugMessage(ARPGDebugger.GetColorForChain("Chained to ") + next[i].ToString() + " with chain level " + newvars.CurrentChain.ToString(), this);
                
            }
           


        }

        
        public void AddDamageTarget(IActorHub damaged)
        {
            damagedTargets.Add(damaged);
            if (trailinstance == null && TrailPrefab != null)
            {
                trailinstance = Instantiate(TrailPrefab);
            }
            trailinstance.transform.position = damaged.MyTransform.position + Vector3.up * 1f;
        }

        public void ChainComplete()
        {
            //damagedTargets.Clear();
            //Destroy(this.gameObject);
        }

        public bool HasHitTarget(IActorHub target)
        {
            return damagedTargets.Contains(target);
        }

        public void SetOriginalCaster(IActorHub caster)
        {
            originalcaster = caster;

        }

        public IActorHub GetCaster() => originalcaster;

     
    }
}