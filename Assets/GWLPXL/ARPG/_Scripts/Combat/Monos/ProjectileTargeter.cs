using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    public enum TargetType
    {
        Random = 0,
        Closest = 1,
        Furthest = 2
    }
    [System.Serializable]
    public class ProjectileTargetVars
    {
        public CombatGroupType[] TargetGroups = new CombatGroupType[1] { CombatGroupType.Enemy };
        public float CheckRate = .02f;
        public float FireRate = 1f;
        public EditorPhysicsType Type = EditorPhysicsType.Unity3D;
        public float CheckRadius = 5;
        public TargetType TargetType = TargetType.Closest;
        public int MaxtargetsAtOnce = 1;
        public GameObject ProjectilePrefab = null;
    }
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileTargeter : MonoBehaviour, ITick, IWeaponModification
    {
        [SerializeField]
        ProjectileEvents events = new ProjectileEvents();
        public ProjectileTargetVars Vars = new ProjectileTargetVars();
        public Transform[] FirePoints = new Transform[0];
        float timer = 0;
        float fireTimer = 0;
        Dictionary<float, IActorHub> closestdic = new Dictionary<float, IActorHub>();
        Dictionary<float, IActorHub> furthestDic = new Dictionary<float, IActorHub>();
        List<float> sqrdlist = new List<float>();
        Queue<IActorHub> targetQueue = new Queue<IActorHub>();
        IActorHub owner = null;
        bool isactive;
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        private void Start()
        {
            AddTicker();
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void DoTick()
        {
            timer += GetTickDuration();
            if (timer >= Vars.CheckRate)
            {
                targetQueue.Clear();
                timer = 0;
                List<IActorHub> valid = new List<IActorHub>();
                switch (Vars.Type)
                {
                    case EditorPhysicsType.Unity3D:
                        Collider[] hits = Physics.OverlapSphere(transform.position, Vars.CheckRadius);
             
                        for (int i = 0; i < hits.Length; i++)
                        {
                            IActorHub hub = hits[i].GetComponent<IActorHub>();
           
                            if (hub == null) continue;
                            if (hub == owner) continue;
                            if (DungeonMaster.Instance.CombatFormulas.GetCombatFormulas().DetermineAttackable(hub, owner, false))
                            {
                                valid.Add(hub);
                            }
                    
                        }

                        break;


                }

                if (valid.Count == 0) return;
                List<IActorHub> targets = new List<IActorHub>();
                switch (Vars.TargetType)
                {
                    case TargetType.Random:
                        for (int i = 0; i < Vars.MaxtargetsAtOnce; i++)
                        {
                            if (valid.Count == 0) break;
                            int rando = Random.Range(0, valid.Count - 1);
                            targets.Add(valid[rando]);
                            valid.RemoveAt(rando);
                        }
                        
                        break;
                    case TargetType.Closest:
                        //
                        closestdic.Clear();
                        sqrdlist.Clear();
                        for (int i = 0; i < valid.Count; i++)
                        {
                            Vector3 direction = valid[i].MyTransform.position - this.transform.position;
                            float sqrd = direction.sqrMagnitude;
                            closestdic.Add(sqrd, valid[i]);
                            sqrdlist.Add(sqrd);
                        }
                        sqrdlist.Sort((p1, p2) => p1.CompareTo(p2));
                        for (int i = 0; i < sqrdlist.Count; i++)
                        {
                            if (i >= Vars.MaxtargetsAtOnce)
                            {
                                break;
                            }
                            closestdic.TryGetValue(sqrdlist[i], out IActorHub value);
                            targets.Add(value);
                        }

                        break;

                    case TargetType.Furthest:
                        //
                        furthestDic.Clear();
                        sqrdlist.Clear();

                        for (int i = 0; i < valid.Count; i++)
                        {
                            Vector3 direction = valid[i].MyTransform.position - this.transform.position;
                            float sqrd = direction.sqrMagnitude;
                            furthestDic.Add(sqrd, valid[i]);
                            sqrdlist.Add(sqrd);
                        }
                        sqrdlist.Sort((p2, p1) => p1.CompareTo(p2));

                        for (int i = 0; i < sqrdlist.Count; i++)
                        {
                            if (i >= Vars.MaxtargetsAtOnce)
                            {
                                break;
                            }
                            furthestDic.TryGetValue(sqrdlist[i], out IActorHub value);
                            targets.Add(value);
                        }

                        break;
                        

                }

                for (int i = 0; i < targets.Count; i++)
                {
                    targetQueue.Enqueue(targets[i]);
                }

                
              

            }

            fireTimer += GetTickDuration();
            if (fireTimer >= Vars.FireRate)
            {
                fireTimer = 0;
                int firepointsindex = 0;
                for (int i = 0; i < targetQueue.Count; i++)
                {
                    //fire
                    GameObject instance = FireProjectile(Vars.ProjectilePrefab);
                    Vector3 direction =targetQueue.Dequeue().MyTransform.position - this.transform.position;
                    direction.Normalize();
                    instance.transform.forward = direction;
                    instance.GetComponent<IDoDamage>().EnableDamageComponent(true, owner);

                    if (FirePoints.Length == 0) continue;
                   
                    FirePoints[firepointsindex].forward = instance.transform.forward;
                    firepointsindex++;
                    if (firepointsindex >= targetQueue.Count - 1)
                    {
                        firepointsindex = 0;
                    }

                }
            }
        }

        GameObject FireProjectile(GameObject prefab)
        {
            GameObject newObj = Instantiate(prefab, this.transform.position, Quaternion.identity);
            newObj.transform.forward = this.transform.forward;
            events.SceneEvents.OnFired.Invoke();
            return newObj;
        }

        public float GetTickDuration() => Time.deltaTime;
      

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public Transform GetTransform() => this.transform;


        public void SetActive(bool isEnabled) => isactive = isEnabled;


        public bool IsActive() => isactive;
       

        public void DoModification(AttackValues other)
        {
            //
        }

        public bool DoChange(Transform other)
        {
            return false;
        }

        public void SetUser(IActorHub myself) => owner = myself;
       
    }
}