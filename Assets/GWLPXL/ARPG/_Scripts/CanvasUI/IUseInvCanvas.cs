
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IUseInvCanvas
    {
        void SetCanvasPrefab(GameObject newprefab);
        void ToggleCanvas();
        IInventoryUser GetUser();
        IInventoryCanvas GetInvUI();
        void EnableCanvas();
        void DisableCanvas();
        bool GetFreezeMover();
        IActorHub GetActorHub();
    }
}
