using GWLPXL.ARPGCore.Factions.com;
using GWLPXL.ARPGCore.Types.com;

using UnityEngine;


namespace GWLPXL.ARPGCore.Quests.com
{
    [System.Serializable]
    public class FactionReward
    {
        public FactionTypes ForFaction;
        public int Amount = 0;
    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Rewards/NEW_Faction_Reward")]
    public class FactionAward : QuestReward
    {
        public FactionReward Reward;
        public override void GetRewardPreview(IQuestGiver giver)
        {
           //not implemented
        }

        public override void GiveRewards(IQuestUser toUser)
        {
            IFactionMember factionM = toUser.GetMyInstance().GetComponent<IFactionMember>();
            if (factionM == null) return;
            if (Reward.Amount > 0)
            {
                factionM.IncreaseRep(Reward.ForFaction, Reward.Amount);
            }
            else if (Reward.Amount < 0)
            {
                factionM.DecreaseRep(Reward.ForFaction, Reward.Amount);
            }
        }
    }
}