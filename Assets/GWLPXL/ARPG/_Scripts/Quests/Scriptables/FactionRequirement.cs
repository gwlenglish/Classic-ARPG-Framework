using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Factions.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Requirements/Faction Requirement")]
    public class FactionRequirement : QuestChainRequirement
    {
        public FactionRequirementData Data;
        public override string GetDescription()
        {
            return Data.Description;
        }

        public override bool MeetsRequirement(IActorHub forUser, IQuestGiver questGiver)
        {
            IFactionMember member = forUser.MyFaction;

            IFactionMember questgiver = questGiver.GetInstance().GetComponent<IFactionMember>();
            if (member == null || questgiver == null)
            {
                Debug.LogWarning("Both SHOPPER and SHOP must inherit from Ifactionmember in order to have a faction requirement. One or both are missing.");
                return false;
            }

            if (member.GetFactionRep(questgiver.GetFaction()) >= Data.MinRelationshipValue)
            {
                //we have enough rep
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}