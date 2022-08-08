
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;

using UnityEngine;
namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public class PlayerInvCanvasUser : MonoBehaviour, IUseInvCanvas, IUseCanvas
    {

        [SerializeField]
        protected bool freezeMover = true;
        [SerializeField]
        protected GameObject InvCanvasPrefab = null;

        protected IInventoryCanvas canvas = null;
        protected IActorHub actorHub = null;
        protected virtual void Awake()
        {
            

        }
        public virtual IActorHub GetActorHub() => actorHub;
     

        public virtual void SetUserToCanvas()
        {
            if (InvCanvasPrefab == null) return;
            GameObject newCanvas = Instantiate(InvCanvasPrefab.gameObject, transform);
            IInventoryCanvas instance = newCanvas.GetComponent<IInventoryCanvas>();
            instance.SetUser(this);
            canvas = instance;
        }

        //
        public virtual void ToggleCanvas()
        {
            canvas.TogglePlayerInventoryUI();
        }

        public virtual IInventoryUser GetUser()
        {
            return actorHub.MyInventory;
        }

        public virtual IInventoryCanvas GetInvUI()
        {
            return canvas;
        }

        public virtual bool GetCanvasEnabled()
        {
            if (canvas == null) return false;
            return canvas.GetCanvasEnabled();
        }



        public virtual void EnableCanvas()
        {
            if (GetInvUI() == null) return;
            GetInvUI().EnablePlayerInventoryUI(true);
        }

        public virtual void DisableCanvas()
        {
            if (GetInvUI() == null) return;

            GetInvUI().EnablePlayerInventoryUI(false);

        }

        public virtual void SetCanvasPrefab(GameObject newprefab) => InvCanvasPrefab = newprefab;

        public virtual bool GetFreezeMover() => freezeMover;

        public virtual void SetActorHub(IActorHub hub)
        {
            actorHub = hub;
        }
    }
}
