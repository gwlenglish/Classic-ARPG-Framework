using GWLPXL.ARPGCore.com;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Quests.com;

namespace GWLPXL.ARPGCore.GameEvents.com
{
   public static class QuestEventManager
    {
        public static void EnemyDeathEvent(IAttributeUser enemy, Quest myQuest, IQuestUser forUser)
        {
            Debug.Log("Death Event called");
            if (enemy == null) return;
            ActorAttributes actor = enemy.GetAttributeTemplate();
            PlayerPersistant[] players = DungeonMaster.Instance.GetPlayerPersist();
            for (int i = 0; i < players.Length; i++)
            {
                QuestLog questLog = players[i].PersistantQuestLog;
                if (questLog != forUser.GetQuestLogRuntime()) continue;

                questLog.QuestStats.KillQuestsTracker.TryGetValue(myQuest, out Dictionary<ActorAttributes, int> value);
                if (value == null)
                {
                    value = new Dictionary<ActorAttributes, int>();
                    value[actor] = 0;
                }
                value.TryGetValue(actor, out int killValue);
                killValue += 1;
                value[actor] = killValue;
                questLog.QuestStats.KillQuestsTracker[myQuest] = value;
                Debug.Log("Actors Killed " + actor.ActorName + " " + killValue);

                if (myQuest != null)
                {
                    myQuest.UpdateQuestProgress(forUser);
                }
                questLog.OnQuestLogUpdated?.Invoke();

            }
        }

        public static void PlayerExploreEvent(ExploreEventVars exploreEvent)
        {
            QuestLog runtime = exploreEvent.ForQuester.GetQuestLogRuntime();
            PlayerPersistant[] players = DungeonMaster.Instance.GetPlayerPersist();
            for (int i = 0; i < players.Length; i++)
            {
                QuestLog questLog = players[i].PersistantQuestLog;
                if (questLog == runtime && questLog != null)
                {
                    questLog.QuestStats.ExploreAreasTracker.TryGetValue(exploreEvent.Quest, out Dictionary<ExploreArea, bool> value);
                    if (value == null)
                    {
                        value = new Dictionary<ExploreArea, bool>();
                    }
                    value.TryGetValue(exploreEvent.Area, out bool discovered);
                    discovered = true;
                    value[exploreEvent.Area] = discovered;
                    questLog.QuestStats.ExploreAreasTracker[exploreEvent.Quest] = value;

                    if (exploreEvent.Quest != null)
                    {
                        exploreEvent.Quest.UpdateQuestProgress(exploreEvent.ForQuester);
                    }
                    break;
                }

            }



        }
    }
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance => instance;
        static EventManager instance;

        protected virtual void Awake()
        {
            instance = this;
        }

        public virtual void PlayerExploreEvent(ExploreEventVars exploreEvent)
        {
            QuestEventManager.PlayerExploreEvent(exploreEvent);
           
  
        }

        public virtual void EnemyDeathEvent(IAttributeUser enemy, Quest myQuest, IQuestUser forUser)
        {
            QuestEventManager.EnemyDeathEvent(enemy, myQuest, forUser);
           
        }
      
        public void TestPlayerDeath(DeathEvent deathEvent)
        {
            Debug.Log(deathEvent.name + " died");
        }

        public void PlayerDeathRestart(DeathEvent deathEvent)
        {
            StartCoroutine(DelayRestart());
        }

        IEnumerator DelayRestart()
        {
            if (DungeonMaster.Instance != null)
            {
                DungeonMaster.Instance.GameOver();
                yield return new WaitForSeconds(3f);
                DungeonMaster.Instance.ReloadScene();
            }

        }
        public void TestDamage(TookDamageEvent damageEvent)
        {
           // Debug.Log(damageEvent.EventVars.DamageAmount + " " + damageEvent.EventVars.EleType);
        }
        public void TestLevelUp(LevelUpEvent leveledEvent)//if a lot happen at once, we need a queue system.
        {
            //Debug.Log(leveledEvent.EventVars.OldLevel + " is Old level");
            ARPGDebugger.DebugMessage(leveledEvent.EventVars.OldLevel + " is Old level", leveledEvent);

            foreach (var kvp in leveledEvent.EventVars.PreviousValues)
            {
                AttributeType type = kvp.Key;
                Attribute[] attributes = kvp.Value;
                for (int i = 0; i < attributes.Length; i++)
                {
                    ARPGDebugger.DebugMessage(type + " " + attributes[i].GetFullDescription() + " is previous", null);
                    //Debug.Log(type + " " + attributes[i].GetFullDescription() + " is previous");
                }
            }

            ARPGDebugger.DebugMessage(leveledEvent.EventVars.NowLevel + " is now level", leveledEvent);

            //Debug.Log(leveledEvent.EventVars.NowLevel + " is now level");

            foreach (var kvp in leveledEvent.EventVars.NowValues)
            {
                AttributeType type = kvp.Key;
                Attribute[] attributes = kvp.Value;
                for (int i = 0; i < attributes.Length; i++)
                {
                    ARPGDebugger.DebugMessage(type + " " + attributes[i].GetFullDescription() + " is now", null);
                    //Debug.Log(type + " " + attributes[i].GetFullDescription() + " is now");
                }
            }

        }
        public void TestEquip(EquipmentChangeEvent equipmentEvent)
        {
            //Debug.Log("Raised equipped");
            //Debug.Log(equipmentEvent.EventVars.Inventory.name);
            //Debug.Log("Equipped: " + equipmentEvent.EventVars.Equipment.GetItemName());
        }
        public void TestUnequip(EquipmentChangeEvent equipmentEvent)
        {
            //Debug.Log("Raised equipped");
            //Debug.Log(equipmentEvent.EventVars.Inventory.name);
            //Debug.Log("UNEquipped: " + equipmentEvent.EventVars.Equipment.GetItemName());
        }

        public void PlayerEquip(EquipmentChangeEvent equipEvent)
        {
            Equipment equipment = equipEvent.EventVars.Equipment;
            bool unEquip = equipEvent.EventVars.UnEquip;
            IAttributeUser statUser = equipEvent.EventVars.StatUser;
           // EquipmentHandler.ModifyStats(equipment, unEquip, statUser);

            EquipmentSlotsType[] slots = equipEvent.EventVars.Slots;
            IInventoryUser invUser = equipEvent.EventVars.InvUser;
          //  EquipmentHandler.ModifyVisuals(slots, unEquip, equipment, invUser);
        }
        public void PlayerUnEquip(EquipmentChangeEvent unEquipEvent)
        {
            Equipment equipment = unEquipEvent.EventVars.Equipment;
            bool unEquip = unEquipEvent.EventVars.UnEquip;
            IAttributeUser statUser = unEquipEvent.EventVars.StatUser;
           // EquipmentHandler.ModifyStats(equipment, unEquip, statUser);

            EquipmentSlotsType[] slots = unEquipEvent.EventVars.Slots;
            IInventoryUser invUser = unEquipEvent.EventVars.InvUser;
           // EquipmentHandler.ModifyVisuals(slots, unEquip, equipment, invUser);
        }
    }
}