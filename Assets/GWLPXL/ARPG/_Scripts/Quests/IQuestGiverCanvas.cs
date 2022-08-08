namespace GWLPXL.ARPGCore.Quests.com
{
    public interface IQuestGiverCanvas
    {
        void SetUser(IQuestGiver giver);
        void EnableCanvas(bool isEnabled);
        void Return();
        void EnableInProgressButton(bool isEnabled);
        void EnableTurnInButton(bool isEnabled);
        void EnableAcceptQuestButtons(bool isEnabled);
        void SetQuestText(string newText);
        void EnableResetQuestchainPanel(bool isEnabled);

    }
}