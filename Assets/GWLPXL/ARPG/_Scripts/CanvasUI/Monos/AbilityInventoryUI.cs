using GWLPXL.ARPGCore.Abilities.com;


using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
namespace GWLPXL.ARPGCore.CanvasUI.com
{

    public class AbilityInventoryUI : MonoBehaviour, IAbilityInventorySlotUI
    {
        [SerializeField]
        string interactButton = "Fire1";
        [SerializeField]
        TextMeshProUGUI nameText = null;
        [SerializeField]
        TextMeshProUGUI descriptionText = null;
        [SerializeField]
        Image abilityImage = null;
        StringBuilder sb = new StringBuilder();
        IAbilityUser user = null;
        IAbilityDraggableManager draggable = null;
        int myslot = 0;
        Ability inSlot = null;
        [SerializeField]
        RectTransform draggableArea = null;


        public GameObject GetInstance() => this.gameObject;


        public void SetAbilitySlot(IAbilityUser forUser, int slot, IAbilityDraggableManager drag)
        {
            user = forUser;
            myslot = slot;
            draggable = drag;
            UpdateSlot();
        }

        void UpdateLabel()
        {
            sb.Clear();
            if (inSlot != null)
            {
                sb.Append(inSlot.GetName());
                nameText.SetText(sb.ToString());
                sb.Clear();
                sb.Append(inSlot.Data.Description);
                descriptionText.SetText(sb.ToString());
            }
            else
            {
                //empty
                nameText.SetText(sb.ToString());
                descriptionText.SetText(sb.ToString());
            }
        }

        void UpdateGraphic()
        {
            if (inSlot != null)
            {
                abilityImage.sprite = inSlot.Data.Sprite;
            }
            else
            {
                abilityImage.sprite = null;
            }
        }

        public void UpdateSlot()
        {
            inSlot = user.GetRuntimeController().GetCopyAllLearned()[myslot];
            UpdateLabel();
            UpdateGraphic();
            if (inSlot == null)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }

        private void LateUpdate()
        {
            CheckForDraggable();
        }
        private void CheckForDraggable()
        {
            Vector2 localMousePosition = draggableArea.InverseTransformPoint(Input.mousePosition);
            if (draggableArea.rect.Contains(localMousePosition))
            {
                if (Input.GetButtonDown(interactButton))
                {
                    TryCreateDraggable();
                }
            }
        }

        void TryCreateDraggable()
        {
            if (draggable == null) return;
            draggable.CreateDraggable(myslot);
        }

    }
}