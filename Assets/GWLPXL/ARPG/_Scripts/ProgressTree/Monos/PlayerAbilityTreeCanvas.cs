using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.ProgressTree.com
{
    public interface IUseAbilityTreeCanvas
    {
        void ToggleCanvas();
        IAbilityUser GetUser();
        IProgressTree GetInvUI();
        void SetCanvasPrefab(GameObject newPrefab);
        bool GetFreezeMover();
    }
        

    public class PlayerAbilityTreeCanvas : MonoBehaviour, IUseCanvas, IUseAbilityTreeCanvas
    {

        [SerializeField]
        protected bool freezeMover = true;
        [SerializeField]
        protected GameObject InvCanvasPrefab;

        protected IProgressTree ui = null;
        protected IActorHub actorhub = null;
        protected GameObject newCanvas = null;
       

       

        public virtual void SetUserToCanvas()
        {
            if (InvCanvasPrefab == null) return;
            newCanvas = Instantiate(InvCanvasPrefab.gameObject, transform);
            ui = newCanvas.GetComponent<IProgressTree>();
            ui.SetUser(this);
            ui.EnableUI(false);
            //canvas = instance;
        }

      
        public virtual void ToggleCanvas()
        {
            GetInvUI().ToggleUI();
        }

        public virtual IAbilityUser GetUser()
        {
            return actorhub.MyAbilities;
        }

        public virtual IProgressTree GetInvUI()
        {
            return ui;
        }

        public virtual bool GetCanvasEnabled()
        {
            if (ui == null) return false;
            return ui.GetEnabled();
        }

      

        public virtual void SetCanvasPrefab(GameObject newPrefab) => InvCanvasPrefab = newPrefab;

        public virtual bool GetFreezeMover() => freezeMover;

        public virtual void SetActorHub(IActorHub hub) => actorhub = hub;
       
    }
}
