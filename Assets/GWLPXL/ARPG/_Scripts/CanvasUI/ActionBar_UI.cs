using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Util.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public class ActionBar_UI : MonoBehaviour, IActionBarUI
    {
        [SerializeField]
        GameObject mainPanel = null;
        public void EnableUI(bool isEnabled)
        {
            mainPanel.SetActive(isEnabled);
        }

        public bool GetEnabled() => mainPanel.activeInHierarchy;
      

        public void SetUser(IActorHub newUser)
        {
            SetPlayerUI(newUser);
       
        }

        public void SetDraggableAbility(IAbilityDraggableManager dragman)
        {
            IReceiveAbilityDraggable[] draggables = GetComponentsInChildren<IReceiveAbilityDraggable>(true);
            for (int i = 0; i < draggables.Length; i++)
            {
                draggables[i].SetDraggableManager(dragman);
            }
        }

        void SetPlayerUI(IActorHub forUser)
        {
            IActorUI[] uielements = GetComponentsInChildren<IActorUI>(true);
            for (int i = 0; i < uielements.Length; i++)
            {
               
                uielements[i].SetUI(forUser);
            }

            ILabel[] labels = GetComponentsInChildren<ILabel>(true);
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].UpdateLabel();
            }

            IResourceBar[] bars = GetComponentsInChildren<IResourceBar>(true);
            for (int i = 0; i < bars.Length; i++)
            {
                bars[i].UpdateBar();
            }
        }

        public void ToggleUI()
        {
            mainPanel.SetActive(!mainPanel.activeInHierarchy);
        }
    }
}