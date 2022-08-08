
using UnityEngine;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{

    /// <summary>
    /// Link between quester and the canvas UI
    /// </summary>
    public class PlayerQuestCanvasUser : MonoBehaviour, IUseCanvas, IUseQuesterCanvas
    {
        [SerializeField]
        protected bool freezeMover = false;

        [SerializeField]
        protected GameObject questCanvasPrefab = null;
        protected GameObject questCanvasInstance = null;

        protected IQuesterCanvas questCanvas = null;
        protected IActorHub actorhub = null;
       
        public virtual bool GetCanvasEnabled()
        {
            if (questCanvas == null) return false;
            return questCanvas.GetCanvasEnabled();
        }

        public virtual IQuestUser GetQuester()
        {
            return actorhub.MyQuests;
        }

        public virtual IQuesterCanvas GetQuesterUI()
        {
            return questCanvas;
        }

        public virtual void SetUserToCanvas()
        {
            if (questCanvasInstance != null)
            {
                Destroy(questCanvasInstance);
            }
            if (questCanvasPrefab == null) return;

            questCanvasInstance = Instantiate(questCanvasPrefab, transform);
            questCanvas = questCanvasInstance.GetComponent<IQuesterCanvas>();
            questCanvas.SetUser(this);

        }

        public virtual void ToggleCanvas()
        {
            GetQuesterUI().ToggleQuesterUI();
        }

        public virtual bool GetFreezeMover()
        {
            return freezeMover;
        }

        public virtual void SetPrefabCanvas(GameObject newprefab) => questCanvasPrefab = newprefab;

        public virtual void SetActorHub(IActorHub hub) => actorhub = hub;
      
    }
}