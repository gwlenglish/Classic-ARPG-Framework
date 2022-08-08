using GWLPXL.ARPGCore.Types.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{


    public class NPCFactionMember : MonoBehaviour, IFactionMember
    {
        public ActorFactionEvents FactionEvents;
        [SerializeField]
        FactionTypes myFaction = FactionTypes.None;

        public void DecreaseRep(FactionTypes withFaction, int amount)
        {
            FactionManager.Instance.DecreaseFactionRep(GetFaction(), withFaction, amount);
            FactionEvents.SceneEvents.OnRepDecreased.Invoke(withFaction, amount);
            FactionEvents.SceneEvents.OnAnyRepModified.Invoke();
        }

        public FactionTypes GetFaction() => myFaction;

        public int GetFactionRep(FactionTypes withFaction)
        {
            return FactionManager.Instance.GetRepValue(GetFaction(), withFaction);
        }

        public void IncreaseRep(FactionTypes withFaction, int amount)
        {
            FactionManager.Instance.IncreaseFactionRep(GetFaction(), withFaction, amount);
            FactionEvents.SceneEvents.OnRepIncreased.Invoke(withFaction, amount);
            FactionEvents.SceneEvents.OnAnyRepModified.Invoke();
        }

    }
}