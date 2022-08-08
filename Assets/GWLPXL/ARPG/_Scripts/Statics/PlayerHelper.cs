using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Statics.com
{


    public static class PlayerHelper
    {
        public static void DisableGameplayInputs()
        {
            for (int i = 0; i < DungeonMaster.Instance.GetAllSceneReferences().Length; i++)
            {
                IGameplayInput[] inputs = DungeonMaster.Instance.GetAllSceneReferences()[i].SceneRef.InputSystem.GetComponents<IGameplayInput>();
                for (int j = 0; j < inputs.Length; j++)
                {
                    inputs[j].DisableInput();
                }
            }
        }

        public static void EnableGameplayInputs()
        {
            for (int i = 0; i < DungeonMaster.Instance.GetAllSceneReferences().Length; i++)
            {
                IGameplayInput[] inputs = DungeonMaster.Instance.GetAllSceneReferences()[i].SceneRef.InputSystem.GetComponents<IGameplayInput>();
                for (int j = 0; j < inputs.Length; j++)
                {
                    inputs[j].AllowInput();
                }
            }
        }
    }
}