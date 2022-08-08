using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Shopping.com
{
  
    [RequireComponent(typeof(IAttributeUser))]
    [RequireComponent(typeof(IInventoryUser))]
    public class ShopKeeper : MonoBehaviour, IShopKeeper, IInteract
    {

        #region unity events
        [SerializeField]
        ActorShopKeeperEvents shopEvents = new ActorShopKeeperEvents();
        #endregion

        #region fields

        [SerializeField]
        int shopLevel = 1;
        [SerializeField]
        int itemsRollsForShop = 10;
        [SerializeField]
        ItemType[] typesToSell = new ItemType[2] { ItemType.Equipment, ItemType.Potions };
        [SerializeField]
        ShopRequirement[] requirements = new ShopRequirement[0];
        [SerializeField]
        LootDrops storeTable = null;
        [SerializeField]
        GameObject shopKeeperCanvasPrefab = null;
        [SerializeField]
        float interactRange = 1f;
        IActorHub shopperhub = null;
        IShopper shopper = null;
        IUseShopKeeperCanvas shopcanvasuser = null;
        IShopKeeperCanvas canvasInstance = null;
        IScale scaler = null;
        #endregion
        #region unity callbacks
        protected virtual void Awake()
        {
            scaler = GetComponent<IScale>();
        }
        #endregion
        public void SetCanvasPrefab(GameObject newPrefab) => shopKeeperCanvasPrefab = newPrefab;
        public void SetItemsToSell(ItemType[] types) => typesToSell = types;
        public void SetStoreTable(LootDrops newtable) => storeTable = newtable;

        protected virtual IShopper CheckPreConditions(GameObject onObj)
        {
            IActorHub actor = onObj.GetComponent<IActorHub>();
            if (actor.PlayerControlled == null) return null;//if not player, cant use
            IShopper newshopper = actor.PlayerControlled.Shopper;
            if (newshopper == null) return null;//if not shopper, cant use
            IPlayerCanvasHub canvasuser = onObj.GetComponent<IPlayerCanvasHub>();
            if (canvasuser == null || canvasuser.Shopcanvas == null) return null;//if not canvas user, cant use

            shopcanvasuser = canvasuser.Shopcanvas;
            shopper = newshopper;
            shopperhub = actor;
            return shopper;

        }
        public bool DoInteraction(GameObject interactor)
        {
            IShopper newShopper = CheckPreConditions(interactor);
            if (newShopper == null) return false;
            OpenShop(shopper);
            //check if person can shop
            //enter shop
            return true;
        }
        public bool IsInRange(GameObject interactor)
        {
            Vector3 dir = interactor.transform.position - this.transform.position;
            float sqrdMag = dir.sqrMagnitude;
            return (sqrdMag <= (interactRange * interactRange));
        }

        public void OpenShop(IShopper forShopper)
        {
            SetupShop();
            canvasInstance.DisplayShopperCurrency(shopper.GetCurrencyAmount());
            canvasInstance.ToggleUI();
            shopEvents.SceneEvents.OnShopOpen.Invoke();
          
           
        }

        public void SetupShop()
        {
            if (canvasInstance == null)
            {
                canvasInstance = Instantiate(shopKeeperCanvasPrefab).GetComponent<IShopKeeperCanvas>();
                canvasInstance.SetUser(this);
                shopcanvasuser.SetShopCanvass(canvasInstance);
            }
        }

        /// <summary>
        /// rolls and only returns unique items
        /// </summary>
        /// <returns></returns>
        public List<Item> GetShopKeeperItems()
        {
            List<Item> newRoll = new List<Item>();
            List<int> templates = new List<int>();
            int level = shopLevel;
            if (scaler != null)
            {
                level = scaler.GetScaledLevel();
            }
            for (int i = 0; i < itemsRollsForShop; i++)
            {
                Item roll = storeTable.GetRandomDrop(level);
                if (templates.Contains(roll.GetID().ID) == false)
                {
                    //only add unique
                    if (roll is Equipment)
                    {
                        Equipment eq = roll as Equipment;
                        eq.AssignEquipmentTraits(level);
                    }
                    newRoll.Add(roll);
                    templates.Add(roll.GetID().ID);
                }
                else
                {
                    Destroy(roll);
                }
              
            }
            
            return newRoll;
        }

        public bool TryPurchase(Item item)
        {
            if (shopper == null)
            {
                Debug.Log("no shopper set");
                return false;
            }

            for (int i = 0; i < requirements.Length; i++)
            {
                if (requirements[i].MeetsRequirement(shopperhub, this) == false)
                {
                    Debug.Log("Dont meet the requirement " + requirements[i].GetDescription());
                    return false;
                }
            }

            int amount = item.GetPurchaseCost();
            int shopperPurse = shopper.GetCurrencyAmount();

            if (amount > shopperPurse)
            {
                Debug.Log("not enough money");
                return false;
            }

            bool purchased = shopper.PurchaseItem(item, amount);
            if (purchased)
            {
                ForceCloseSellWindow();

                shopper.OpenInventory();//refreshes the inventory
                canvasInstance.DisplayShopperCurrency(shopper.GetCurrencyAmount());

                shopEvents.SceneEvents.OnPurchaseSuccess.Invoke(item);
            }
            else
            {
                shopEvents.SceneEvents.OnPurchaseFail.Invoke(item);
            }
            Debug.Log("Purchase " + purchased);
            return purchased;

        }

        private void ForceCloseSellWindow()
        {
            ISeller seller = shopper.GetInventory().GetMyInstance().GetComponent<ISeller>();
            if (seller != null)
            {
                if (seller.GetCanvasActive())
                {
                    seller.ToggleCanvas();
                }
            }
        }

        public ItemType[] GetItemTypesToSell()
        {
            return typesToSell;
        }

        public IShopper GetShopper()
        {
            return shopper;
        }

        public void CloseShop()
        {
            //
            ForceCloseSellWindow();
            shopEvents.SceneEvents.OnShopClosed.Invoke();
        }

        public void TogglePlayerSelling()
        {
            ISeller seller = shopper.GetInventory().GetMyInstance().GetComponent<ISeller>();
            if (seller != null)
            {
                seller.ToggleCanvas();
            }
        }

        public void SetItemRolls(int newrollamount) => itemsRollsForShop = newrollamount;

        public void SetShopLevel(int newlevel) => shopLevel = newlevel;

        public int GetShopLevel() => shopLevel;

        public Transform GetInstance() => this.transform;
       
    }
}