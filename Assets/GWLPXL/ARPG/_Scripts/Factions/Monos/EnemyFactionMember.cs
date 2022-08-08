using GWLPXL.ARPGCore.com;

using GWLPXL.ARPGCore.Types.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{
    [System.Serializable]
    public class RelationChange
    {
        [Tooltip("The faction to affect")]
        public FactionTypes ForFaction;
        [Tooltip("Negative for losing faction rep, positive for gaining")]
        public int AmountToChange;
    }
    /// <summary>
    /// to do, use a matrix for the rep worth, allows to increase some reps and decrease others.
    /// </summary>
    public class EnemyFactionMember : MonoBehaviour, IFactionMember
    {
        public ActorFactionEvents FactionEvents;
        [SerializeField]
        protected RelationChange[] factionChangesOnDeath = new RelationChange[0];
        [SerializeField]
        protected FactionTypes myFaction = FactionTypes.None;
        protected EnemyHealth hp = null;

        protected virtual void Awake()
        {
            hp = GetComponent<EnemyHealth>();
        }
        protected virtual void OnEnable()
        {
            if (hp != null)
            {
                hp.OnDeath +=ModifyPlayerRep;
            }
        }

        protected virtual void OnDisable()
        {
            if (hp != null)
            {
                hp.OnDeath -= ModifyPlayerRep;
            }
        }

        protected virtual void ModifyPlayerRep()
        {
            FactionManager.Instance.ModifyPlayerRep(factionChangesOnDeath);
           
        }
        public virtual void DecreaseRep(FactionTypes withFaction, int amount)
        {
            FactionManager.Instance.DecreaseFactionRep(GetFaction(), withFaction, amount);
            FactionEvents.SceneEvents.OnRepDecreased.Invoke(withFaction, amount);
            FactionEvents.SceneEvents.OnAnyRepModified.Invoke();
        }

        public virtual FactionTypes GetFaction() => myFaction;

        public virtual int GetFactionRep(FactionTypes withFaction)
        {
            return FactionManager.Instance.GetRepValue(GetFaction(), withFaction);
        }

        public virtual void IncreaseRep(FactionTypes withFaction, int amount)
        {
            FactionManager.Instance.IncreaseFactionRep(GetFaction(), withFaction, amount);
            FactionEvents.SceneEvents.OnRepIncreased.Invoke(withFaction, amount);
            FactionEvents.SceneEvents.OnAnyRepModified.Invoke();
        }
    }
}