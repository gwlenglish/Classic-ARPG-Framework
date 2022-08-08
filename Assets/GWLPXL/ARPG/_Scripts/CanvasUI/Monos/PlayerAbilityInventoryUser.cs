using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{


    public class PlayerAbilityInventoryUser : MonoBehaviour, IUseCanvas, IUseAbilityInventory
    {
        [SerializeField]
        bool freezeMover = true;
        [SerializeField]
        bool allowDraggable = true;
        [SerializeField]
        GameObject abilityInventoryPrefab = null;


        IAbilityInventoryUI ui = null;
        IActorHub actorhub = null;
        

        public void SetUserToCanvas()
        {
            GameObject instance = Instantiate(abilityInventoryPrefab, transform);
            ui = instance.GetComponent<IAbilityInventoryUI>();
            ui.SetUser(actorhub);
            ui.EnableUI(false);
            if (allowDraggable)
            {
                IAbilityDraggableManager draggable = instance.GetComponent<IAbilityDraggableManager>();
               if (draggable != null)
                {
                    GetComponent<IUseActionBarUser>().SetDraggableManager(draggable);
                }
            }
        }

        public IAbilityUser GetUser() => actorhub.MyAbilities;
        public bool GetFreezeMover() => freezeMover;
        public void SetCanvasPrefab(GameObject newprefab) => abilityInventoryPrefab = newprefab;


        public void DisableCanvas() 
        {
            ui.EnableUI(false);
        }

        public void EnableCanvas()
        {
            ui.EnableUI(true);
        }

        public bool GetCanvasEnabled() => ui.GetEnabled();
       


     

     

        public void ToggleCanvas()
        {
            ui.ToggleUI();
        }

        public IAbilityInventoryUI GetAbilityInventoryUI() => ui;

        public void SetActorHub(IActorHub hub) => actorhub = hub;
       
    }
}