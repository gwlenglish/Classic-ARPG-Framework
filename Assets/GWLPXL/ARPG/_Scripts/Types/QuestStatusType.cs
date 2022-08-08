
namespace GWLPXL.ARPGCore.Types.com
{


    public enum QuestStatusType
    {
        /// <summary>
        /// Quest can be accepted by the quester.
        /// </summary>
        Available = 0,
        /// <summary>
        /// Quest is currently active/being done by the user. 
        /// </summary>
        InProgress = 1,
        /// <summary>
        /// User has completed the Quest goals. 
        /// </summary>
        Completed = 10,
        /// <summary>
        /// User has received rewards for the Quest. 
        /// </summary>
        RewardsCollected = 20,
        /// <summary>
        /// Quest can NOT be accepted by the quester.
        /// </summary>
        UnAvailable = 30
    }

 
}