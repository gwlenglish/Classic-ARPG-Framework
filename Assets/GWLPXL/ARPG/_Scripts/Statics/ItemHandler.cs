

using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Statics.com
{
   
    public static class ItemHandler
    {
        public static bool PickupItem(Item item, IInventoryUser inv)
        {
            if (inv == null) return false;
            if (inv.GetInventoryRuntime().CanWeAddItem(item) == false) return false;

            if (item is Currency)
            {
                //we wont add to inventory, just to currency
                Currency currency = item as Currency;
                inv.GetInventoryRuntime().ModifyCurrency(currency.Amount);
                return true;//exit out
            }

           
            if (item is QuestItem)
            {
                UpdateQuest(item, inv);
            }

            inv.GetInventoryRuntime().AddItemToInventory(item);

            return true;
        }

        private static void UpdateQuest(Item item, IInventoryUser inv)
        {
            QuestItem keyitem = (QuestItem)item;
            IQuestUser questerUser = inv.GetMyInstance().GetComponent<IQuestUser>();
            keyitem.UpdateQuest(questerUser);
        }

        /// <summary>
        /// will drop the item at user's location if inventory is full. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toUser"></param>
        /// <param name="iLevel"></param>
        /// <param name="lootPrefab"></param>
        public static void AddItemToInventory(Item item, IInventoryUser toUser, int iLevel)
        {

            Item copy = ScriptableObject.Instantiate(item);
            if (copy is Equipment)
            {
                Equipment equipmentCopy = (Equipment)copy;
                equipmentCopy.AssignEquipmentTraits(iLevel);
            }

            ActorInventory inv = toUser.GetInventoryRuntime();
            if (inv.CanWeAddItem(item) == false)
            {
                LootHandler.DropLoot(copy, toUser.GetMyInstance().transform.position, DungeonMaster.Instance.GetLootPrefab());
            }
            else
            {
                inv.AddItemToInventory(item);//throw this on the SO
            }

        }
        public static void DropItem(Item myItem, IInventoryUser invUser, int inventorySlotID)
        {
            invUser.GetInventoryRuntime().RemoveItemFromInventory(inventorySlotID);

            GameObject user = invUser.GetMyInstance().gameObject;

            //not great, but good enough for now
            float offset = Random.Range(1, 2);
            Vector3 randomPoint = user.transform.position + Random.insideUnitSphere * offset;
            LootHandler.DropLoot(myItem, randomPoint, DungeonMaster.Instance.GetLootPrefab());
            if (myItem is QuestItem)
            {
                UpdateQuest(myItem, invUser);
            }

        }

        public static void DetermineAndUseItem(Item myItem, IInventoryUser invUser, int inventorySlotID)
        {
            if (myItem is Equipment)
            {
               
                Equipment equipment = myItem as Equipment;
                invUser.GetInventoryRuntime().RemoveItemFromInventory(inventorySlotID);
                invUser.GetInventoryRuntime().Equip(equipment);

            }
            else if (myItem is Potion)
            {

                Potion potion = myItem as Potion;
                IAttributeUser stats = invUser.GetMyInstance().GetComponent<IAttributeUser>();//self use only
                IUseFloatingText dungeonText = invUser.GetMyInstance().GetComponent<IUseFloatingText>();

                if (stats != null)
                {
                    potion.UsePotion(stats, invUser, inventorySlotID);
                }


                if (dungeonText != null)
                {
                    switch (potion.GetPotionType())
                    {
                        case PotionType.RestoreResource:
                            RestoreResource restore = potion as RestoreResource;
                            dungeonText.CreateUIRegenText(restore.GetRestoreAmount().ToString(), restore.GetResourceType());
                            break;
                    }
                }

            }
        }

    }
}