
using UnityEngine;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Requirements/NEW Level Requirement")]

    public class LevelRequirement : QuestChainRequirement
    {
        [SerializeField]
        [Tooltip("The level is inclusive.")]
        int requiredMinLevel = 1;
        [SerializeField]
        bool autoName = true;

        public override string GetDescription()
        {
            return "Requires Level " + requiredMinLevel;
        }

        public override bool MeetsRequirement(IActorHub forUser, IQuestGiver questGiver)
        {
            IAttributeUser attributes = forUser.MyStats;
            if (attributes == null) return false;
            if (attributes.GetRuntimeAttributes().MyLevel >= requiredMinLevel)
            {
                return true;
            }
            return false;
            
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (autoName == false) return;
            if (this.name == "Requires Level " + requiredMinLevel.ToString()) return;//not sure abou tthis, testing

            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(path, "Requires Level " + requiredMinLevel.ToString());
        }

#endif

    }
}