
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{

    //saving will not work without a dungeon singleton
    //

    public class SaveCanvasUser : MonoBehaviour, IUseSaveCanvas, IUseCanvas
    {
        [SerializeField]
        protected bool freezeMover = true;
        [SerializeField]
        protected GameObject SaveCanvasPrefab = null;
  
        protected ISaveCanvas saveC = null;
        protected IActorHub actorhub = null;
        public virtual void SetUserToCanvas()
        {
            if (SaveCanvasPrefab == null) return;

            GameObject newCanvas = Instantiate(SaveCanvasPrefab, transform);
            saveC = newCanvas.GetComponent<ISaveCanvas>();
            if (saveC != null)
            {
                saveC.SetUser(this);
            }


        }

        public virtual void ToggleCanvas()
        {
            saveC.TogglePlayerSaveCanvas();
        }

        public virtual bool GetCanvasEnabled()
        {
            if (saveC == null) return false;
            return saveC.GetCanvasEnabled();
        }


        public virtual void SetCanvasPrefab(GameObject newprefab) => SaveCanvasPrefab = newprefab;

        public virtual ISaveCanvas GetUI() => saveC;

        public virtual bool GetFreezeMover() => freezeMover;

        public virtual void SetActorHub(IActorHub hub) => actorhub = hub;
        
    }
}