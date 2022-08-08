using GWLPXL.ARPGCore.Abilities.com;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public class DraggableAbilityVars
    {
        public Ability Ability;
        public IAbilityUser AbilityUser;
        public DraggableAbilityVars(Ability ab, IAbilityUser abUser)
        {
            Ability = ab;
            AbilityUser = abUser;
        }
    }

    public class DraggableAbility : MonoBehaviour, IDraggableAbility
    {
        [SerializeField]
        string cancelButton = "Fire1";
        [SerializeField]
        Image image = null;
        [SerializeField]
        TextMeshProUGUI text = null;

        Ability myAbility = null;
        IAbilityUser myUser = null;
        StringBuilder sb = new StringBuilder();
        public void SetDraggable(Ability ability, IAbilityUser forUser)
        {
            myAbility = ability;
            myUser = forUser;

            UpdateLabel(ability);
            UpdateGraphic(ability);


        }

        void UpdateGraphic(Ability ability)
        {
            if (ability != null)
            {
                image.sprite = ability.Data.Sprite;
            }
            else
            {
                image.sprite = null;
            }
        }
        void UpdateLabel(Ability ability)
        {
            sb.Clear();
            if (ability != null)
            {
                sb.Append(ability.GetName());
            }

            text.SetText(sb.ToString());
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy == false) return;
            if (Input.GetButtonDown(cancelButton))
            {
                myAbility = null;
                myUser = null;
                gameObject.SetActive(false);
            }
        }
        private void LateUpdate()
        {
            Drag();
            
        }
        /// <summary>
        ///use late update instead of tick so we can move the abilities even when paused.

        /// </summary>
        public void Drag()
        {
            if (gameObject.activeInHierarchy == false) return;
            transform.position = Input.mousePosition;
            if (myAbility == null || myUser == null)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        public DraggableAbilityVars GetDraggableAbility() => new DraggableAbilityVars(myAbility, myUser);
  
    }
}