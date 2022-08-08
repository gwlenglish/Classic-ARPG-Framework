

using UnityEngine;
using UnityEngine.UI;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    /// <summary>
    /// separate concerns, ai control, ui control, ui creation
    /// </summary>
    public interface IDungeonUI
    {
        GraphicRaycaster GetGraphicRaycaster();
      /// <summary>
      /// 0 = inactive
      /// 1 = active;
      /// </summary>
      /// <param name="newState"></param>
        void SetDungeonState(int newState);
        bool IsDungeonActive();
        void InitalizeDungeon();


    }
}