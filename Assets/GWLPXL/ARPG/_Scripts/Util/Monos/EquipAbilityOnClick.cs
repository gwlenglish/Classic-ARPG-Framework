using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.PlayerInput.com;
using GWLPXL.ARPGCore.DebugHelpers.com;

namespace GWLPXL.ARPGCore.Abilities.com
{


    public class EquipAbilityOnClick : MonoBehaviour
    {
        IPlayerMouseInput input;
        IAbilityUser user;
        Ability ability;
        RectTransform rect;
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        public void SetPlayer(IPlayerMouseInput _input, IAbilityUser _user, Ability _ability)
        {
            input = _input;
            user = _user;
            ability = _ability;
        }

        private void LateUpdate()
        {
            Vector2 localMousePosition = rect.InverseTransformPoint(UnityEngine.Input.mousePosition);
            if (rect.rect.Contains(localMousePosition))
            {
                bool left = input.GetMouseButtonOneDown();
                bool right = input.GetMouseButtonTwoDown();
                if (left)
                {
                    ARPGDebugger.DebugMessage("clicked left", this);
                    user.GetRuntimeController().EquipAbility(ability, 0);
                }
                else if (right)
                {
                    ARPGDebugger.DebugMessage("clicked right", this);
                    user.GetRuntimeController().EquipAbility(ability, 1);
                }
            }
        }
      

    }
}