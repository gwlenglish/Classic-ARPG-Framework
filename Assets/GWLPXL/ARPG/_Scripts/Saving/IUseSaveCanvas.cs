using UnityEngine;

namespace GWLPXL.ARPGCore.Saving.com
{
    public interface IUseSaveCanvas
    {
        void SetCanvasPrefab(GameObject newprefab);
        void ToggleCanvas();
        ISaveCanvas GetUI();
        bool GetFreezeMover();
    }
}