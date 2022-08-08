

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    public class PlayerCanvasInputClass : MonoBehaviour, IPlayerCanvasInputToggle, IGameplayInput
    {
        [SerializeField]
        protected bool savingCanvasBlockOtherCanvases = true;
        [SerializeField]
        [Tooltip("Buttons will have preference. To not check buttons, leave the button field empty.")]
        protected PlayerCanvasInput[] canvasToggleInputs = new PlayerCanvasInput[5]
        {
             new PlayerCanvasInput(string.Empty, KeyCode.I, CanvasType.Inventory),
           new PlayerCanvasInput(string.Empty, KeyCode.O, CanvasType.Save),
          new PlayerCanvasInput(string.Empty, KeyCode.S, CanvasType.AbilityTree),
             new PlayerCanvasInput(string.Empty, KeyCode.Q, CanvasType.Quest),
           new PlayerCanvasInput(string.Empty, KeyCode.E, CanvasType.AbilityInventory),
        };



        protected IActorHub hub = null;

        protected bool allowed = true;
      

        public void AllowInput() => allowed = true;


        public void DisableInput() => allowed = false;
        

        //we use update instead of ticks because we want to be able to open and class canvases even when the dungeon is frozen
        protected virtual void Update()
        {
            if (allowed == false) return;
           // if (TickManager.Instance.Paused) return;//if we are paused, i.e. loading, dont allow this
            CheckForCanvasInputs();
        }

        public virtual void CheckForCanvasInputs()
        {
            
            for (int i = 0; i < canvasToggleInputs.Length; i++)
            {
                string inputbutton = canvasToggleInputs[i].CanvasToggleButton;
                if (string.IsNullOrEmpty(inputbutton))
                {
                    //check the keycode
                    if (Input.GetKeyDown(canvasToggleInputs[i].CanvasToggleKeyCod))
                    {
                        ToggleCanvas(canvasToggleInputs[i].ForCanvas);
                    }
                }
                else
                {
                    //not empty, so check both with button as priority
                    if (Input.GetButtonDown(inputbutton) || Input.GetKeyDown(canvasToggleInputs[i].CanvasToggleKeyCod))
                    {
                        ToggleCanvas(canvasToggleInputs[i].ForCanvas);

                    }
                }
               
            }
        }

        public virtual void ToggleCanvas(CanvasType type)
        {
            if (savingCanvasBlockOtherCanvases)
            {

                if (hub.PlayerControlled.CanvasHub.SaveCanvas != null)
                {
                    if (hub.PlayerControlled.CanvasHub.SaveCanvas.GetUI().GetCanvasEnabled() == true)
                    {
                        //block other inputs
         
                        if (type != CanvasType.Save)
                        {
                            ARPGDebugger.DebugMessage("SAVE CANVAS BLOCKING OTHER CANVAS INPUTS", this);
                            return;
                        }
                    }
                }
            }
            switch (type)
            {
                case CanvasType.Inventory:
                    if (hub.PlayerControlled.CanvasHub.InvCanvas != null)
                    {
                        hub.PlayerControlled.CanvasHub.InvCanvas.ToggleCanvas();
                    }
                    break;
                case CanvasType.Save:
                    if (hub.PlayerControlled.CanvasHub.SaveCanvas != null)
                    {
                        hub.PlayerControlled.CanvasHub.SaveCanvas.ToggleCanvas();
                    }
                    break;
                case CanvasType.AbilityTree:
                    if (hub.PlayerControlled.CanvasHub.AbilityTreeCanvas != null)
                    {
                        hub.PlayerControlled.CanvasHub.AbilityTreeCanvas.ToggleCanvas();
                    }
                    break;
                case CanvasType.Quest:
                    if (hub.PlayerControlled.CanvasHub.QuestCanvas != null)
                    {
                        hub.PlayerControlled.CanvasHub.QuestCanvas.ToggleCanvas();
                    }
                    break;
                case CanvasType.AbilityInventory:
                    if (hub.PlayerControlled.CanvasHub.AbilityInventory != null)
                    {
                        hub.PlayerControlled.CanvasHub.AbilityInventory.ToggleCanvas();
                    }
                    break;
            }
        }

        //i hate this method but so it goes...
        public virtual bool HasFreezeMoverCanvasEnabled()
        {
            if (hub.PlayerControlled.CanvasHub.InvCanvas != null)
            {
                if (hub.PlayerControlled.CanvasHub.InvCanvas.GetInvUI().GetCanvasEnabled() == true && hub.PlayerControlled.CanvasHub.InvCanvas.GetFreezeMover() == true) return true;
            }

            if (hub.PlayerControlled.CanvasHub.SaveCanvas != null && hub.PlayerControlled.CanvasHub.SaveCanvas.GetUI() != null)
            {
                if (hub.PlayerControlled.CanvasHub.SaveCanvas.GetUI().GetCanvasEnabled() && hub.PlayerControlled.CanvasHub.SaveCanvas.GetFreezeMover() == true) return true;
            }

            if (hub.PlayerControlled.CanvasHub.AbilityTreeCanvas != null)
            {
                if (hub.PlayerControlled.CanvasHub.AbilityTreeCanvas.GetInvUI().GetEnabled() && hub.PlayerControlled.CanvasHub.AbilityTreeCanvas.GetFreezeMover() == true) return true;
            }

            if (hub.PlayerControlled.CanvasHub.QuestCanvas != null && hub.PlayerControlled.CanvasHub.QuestCanvas.GetQuesterUI() != null)
            {
                if (hub.PlayerControlled.CanvasHub.QuestCanvas.GetQuesterUI().GetCanvasEnabled() && hub.PlayerControlled.CanvasHub.QuestCanvas.GetFreezeMover() == true) return true;
            }

            if (hub.PlayerControlled.CanvasHub.AbilityInventory != null && hub.PlayerControlled.CanvasHub.AbilityInventory.GetAbilityInventoryUI() != null)
            {
                if (hub.PlayerControlled.CanvasHub.AbilityInventory.GetAbilityInventoryUI().GetEnabled() && hub.PlayerControlled.CanvasHub.AbilityInventory.GetFreezeMover() == true) return true;
            }

            if (hub.PlayerControlled.CanvasHub.Shopcanvas != null )
            {
                if (hub.PlayerControlled.CanvasHub.Shopcanvas.GetFreezeMover() == true) return true;//simplified on the ishopcanvasuser itself, whcih is why it looks different.
            }

            if (hub.PlayerControlled.CanvasHub.EnchanterCanvas != null)
            {
                if (hub.PlayerControlled.CanvasHub.EnchanterCanvas.GetFreezeMover() == true) return true;
            }

            if (hub.PlayerControlled.CanvasHub.SocketCanvas != null)
            {
                if (hub.PlayerControlled.CanvasHub.SocketCanvas.GetFreezeMover() == true) return true;
            }
            return false;
        }

        public virtual void SetInputHub(IActorHub actorhub)
        {
            hub = actorhub;
        }
    }
}