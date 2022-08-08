using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Requirements/NEW Questchain Requirement")]

    public class QuestchainRequired : QuestChainRequirement
    {
        [SerializeField]
        Questchain requiredChain = null;
        [SerializeField]
        QuestStatusType requiredStatus = QuestStatusType.RewardsCollected;
        [SerializeField]
        bool autoName = true;
        public override string GetDescription()
        {
            return "Requires Questchain " + requiredChain.GetQuestName();
        }

        public override bool MeetsRequirement(IActorHub forUser, IQuestGiver questGiver)
        {
            if (forUser == null) return false;
            QuestLog log = forUser.MyQuests.GetQuestLogRuntime();
            QuestState state = log.GetQuestchainState(requiredChain);
            
            if (state.State == requiredStatus)
            {
                return true;
            }
            return false;
            
                
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            if (autoName == false) return;
            if (requiredChain == null) return;
            if (this.name == "Requires Chain " + requiredChain.GetQuestName()) return;//not sure abou tthis, testing

            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(path, "Requires Chain " + requiredChain.GetQuestName());
        }

#endif
    }
}