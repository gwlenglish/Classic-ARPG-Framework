using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{
    public interface IKillTracked
    {
        void UpdateQuest(IQuestUser forUser);
        void SetActorHub(IActorHub hub);

    }
}