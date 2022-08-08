namespace GWLPXL.ARPGCore.Types.com
{
    public enum QuestRewardTurnInType
    {
        /// <summary>
        /// Must derive from IAttributeUser
        /// </summary>
        SpecificActor = 0,
        /// <summary>
        /// As soon as quest is complete, rewards are given. 
        /// </summary>
        OnComplete = 1,
        /// <summary>
        /// Must derive from IQuestGiver
        /// </summary>
        AnyQuestGiver = 10
    }


    
}