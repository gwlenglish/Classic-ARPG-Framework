
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public class PlayerActionBarUser : MonoBehaviour, IUseActionBarUser, IUseCanvas
    {
        [SerializeField]
        GameObject playerActionBarPrefab = null;

        IActionBarUI ui = null;
        IAbilityDraggableManager draggable = null;
        IActorHub actorhub = null;
       
        public bool GetCanvasEnabled() => ui.GetEnabled();
      

        public bool GetFreezeMover() => false;
    

        public void SetUserToCanvas()
        {
            GameObject instance = Instantiate(playerActionBarPrefab, transform);
            ui = instance.GetComponent<IActionBarUI>();
            ui.SetDraggableAbility(draggable);
            ui.SetUser(actorhub);
        }

        public void SetDraggableManager(IAbilityDraggableManager dragman)
        {
            draggable = dragman;
            if (draggable != null && ui != null)
            {
                ui.SetDraggableAbility(dragman);
            }
        }

        public void SetPrefab(GameObject newPrefab) => playerActionBarPrefab = newPrefab;

        public void SetActorHub(IActorHub hub) => actorhub = hub;
       
    }
}