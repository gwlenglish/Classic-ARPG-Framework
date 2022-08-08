
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{

    /// <summary>
    /// 
    /// connect to saving system?
    /// connect to databases? doesn't seem like i need to.
    /// </summary>
    public class PlayerFactionMember : MonoBehaviour, IFactionMember
    {
        public PlayerFactionEvents FactionEvents;
        [SerializeField]
        protected FactionTypes myFaction = FactionTypes.None;

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