
using UnityEngine;
using GWLPXL.ARPGCore.Leveling.com;
namespace GWLPXL.ARPGCore.Quests.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Rewards/NEW_Experience Reward")]
    public class ExperienceReward : QuestReward
    {

        public int Experience = 100;
        public bool AutoName = true;

        public override void GetRewardPreview(IQuestGiver giver)
        {
//
        }

        public override void GiveRewards(IQuestUser toUser)
        {
            ILevel leveler = toUser.GetMyInstance().GetComponent<ILevel>();
            leveler.EarnXP(Experience);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (AutoName)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, "Experience Reward " + Experience.ToString());
            }
        }
#endif

    }



}