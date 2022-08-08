using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public interface ICreateUI
    {
        Vector3 GetHPBarOffset();
        void CreateUIDamageText(string message, ElementType type);


    }
}