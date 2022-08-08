namespace GWLPXL.ARPGCore.Types.com
{
    public enum QuestStartConditionType
    {
        /// <summary>
        /// As soon as it can, it will start the quest. 
        /// </summary>
        AutoStart = 0,
        /// <summary>
        /// must derive from IAttributeUser
        /// </summary>
        SpecificActor = 1,
        /// <summary>
        /// must also derive IQuestGiver
        /// </summary>
        AnyQuestGiver = 10
    }


    
}