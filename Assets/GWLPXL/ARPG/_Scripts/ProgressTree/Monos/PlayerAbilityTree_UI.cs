using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.ProgressTree.com
{
    public class PlayerAbilityTree_UI : MonoBehaviour, IProgressTree
    {
        public bool FreezeDungeon = true;
        [SerializeField]
        protected GameObject mainPanel = null;
        [SerializeField]
        protected ProgressTreeHolder abilityTree = null;

        [SerializeField]
        protected List<AbilityUnlocks> abilityUnlocks = new List<AbilityUnlocks>();
        protected IUseAbilityTreeCanvas myUser = null;

        
        public virtual void EnableUI(bool isEnabled)
        {
            mainPanel.SetActive(isEnabled);
        }

        protected virtual void OnEnable()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;//set screen space camera
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        }
        public virtual void SetUser(IUseAbilityTreeCanvas newUser)
        {

            if (myUser == null)
            {
                //first time
                for (int i = 0; i < abilityUnlocks.Count; i++)
                {
                    abilityUnlocks[i].AbilityUser = newUser.GetUser();
                    TreeNodeHolder runtime = abilityTree.GetRuntimeNode(abilityUnlocks[i].NodeUnlock);
                    runtime.Available += ModifySkillToUser;
                }
            }

            else if (myUser != null && myUser != newUser)
            {
                //replacing user
                for (int i = 0; i < abilityUnlocks.Count; i++)
                {
                    TreeNodeHolder runtime = abilityTree.GetRuntimeNode(abilityUnlocks[i].NodeUnlock);
                    runtime.Available -= ModifySkillToUser;
                }

                for (int i = 0; i < abilityUnlocks.Count; i++)
                {
                    abilityUnlocks[i].AbilityUser = newUser.GetUser();
                    TreeNodeHolder runtime = abilityTree.GetRuntimeNode(abilityUnlocks[i].NodeUnlock);
                    runtime.Available += ModifySkillToUser;
                }

            }


            myUser = newUser;

        }

        protected virtual void OnDestroy()
        {
            for (int i = 0; i < abilityUnlocks.Count; i++)
            {
                TreeNodeHolder runtime = abilityTree.GetRuntimeNode(abilityUnlocks[i].NodeUnlock);
                if (runtime == null) continue;
                runtime.Available -= ModifySkillToUser;
            }
        }


        protected virtual void ModifySkillToUser(TreeNodeHolder nodeKey, bool isAvailable)
        {
            //ARPGDebugger.DebugMessage("Raised event", this);
            for (int i = 0; i < abilityUnlocks.Count; i++)
            {
                TreeNodeHolder runtime = abilityTree.GetRuntimeNode(abilityUnlocks[i].NodeUnlock);
                if (runtime == nodeKey)
                {
                    //ARPGDebugger.DebugMessage("found nodekey", this);

                    if (isAvailable)
                    {
                        //ARPGDebugger.DebugMessage("added skill", this);

                        myUser.GetUser().GetRuntimeController().LearnAbility(abilityUnlocks[i].AbilityToUnlock);
                    }
                    else
                    {
                        //ARPGDebugger.DebugMessage("removed skill", this);
                        myUser.GetUser().GetRuntimeController().ForgetAbility(abilityUnlocks[i].AbilityToUnlock);

                    }

                    break;
                }
            }
        }
        public virtual void ToggleUI()
        {
            EnableUI(!mainPanel.activeInHierarchy);
            if (FreezeDungeon && mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
            else if (FreezeDungeon && !mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);

            }
        }

        public virtual bool GetEnabled()
        {
            return mainPanel.activeInHierarchy;
        }
    }
}