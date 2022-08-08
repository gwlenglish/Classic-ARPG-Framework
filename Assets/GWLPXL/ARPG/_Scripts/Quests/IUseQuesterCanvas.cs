using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{
    public interface IUseQuesterCanvas
    {
        void SetPrefabCanvas(GameObject newprefab);
        void ToggleCanvas();
        IQuestUser GetQuester();
        IQuesterCanvas GetQuesterUI();
        bool GetFreezeMover();
    }
}