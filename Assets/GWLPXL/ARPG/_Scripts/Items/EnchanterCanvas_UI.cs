using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Traits.com;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
   [System.Serializable]
   public class EnchantableItemUI
    {
        public ItemStack ItemStack;
        public GameObject UIObject;
        public RectTransform RectTransform;
        public IEnchantableUIElement Interface;
        public EnchantableItemUI(ItemStack stack, GameObject ob)
        {
            ItemStack = stack;
            UIObject = ob;
            RectTransform = ob.GetComponent<RectTransform>();
            Interface = ob.GetComponentInChildren<IEnchantableUIElement>();
        }


      
    }
    /// <summary>
    /// to do, map rect transforms to dictionary like in socket smith
    /// </summary>
    public class EnchanterCanvas_UI : MonoBehaviour, IEnchanterCanvas
    {
        public bool FreezeDungeon = true;
        public Transform MainPanel = null;
        public EnchantUIEvents Events;
        [Tooltip("Whether to add the Enchants in the Native Traits slots or Random Traits slots.")]
        public bool IsNative = true;
        [Header("Dragging Input")]
        [SerializeField]
        protected string interactButton = "Fire1";
        public GameObject DecisionBoxPreview = null;
        public GameObject SuccessBoxShowcase = null;
        [Header("Enchantables")]
        public GameObject EnchantableUIElementPrefab = null;
        public Transform EnchantableContentParent = null;
        public GameObject DraggableEnchantableInstance = null;
        public GameObject EnchantablePreviewInstance = null;
        [Header("Enchants")]
        public GameObject EnchantUIElementPrefab = null;
        public Transform EnchantContentParent = null;
        public GameObject DraggableEnchantInstance = null;
        public GameObject EnchantPreviewInstance = null;
        protected EnchantingStation station = null;
        protected List<GameObject> enchantableUIElements = new List<GameObject>();
        protected List<GameObject> enchantUIElements = new List<GameObject>();

        protected Dictionary<Item, IEnchantableUIElement> uidic = new Dictionary<Item, IEnchantableUIElement>();
        protected IUseEnchanterCanvas user = null;
        protected IEnchantableUIElement draggableenchantable = null;
        protected IEnchantUIElement draggableEnchant = null;
        protected IEnchantUIElement previewEnchant = null;


        protected IItemViewUI successEnchantable = null;
        protected IItemViewUI decisionPreview = null;
        protected IItemViewUI enchantablePreview = null;
        protected bool draggingEnchantable = false;
        protected bool draggingEnchant = false;
        protected Vector3 homeposition = new Vector3(0, 0, 0);
        protected bool active = false;

        protected Item preview = null;

        protected Item original = null;
        protected int originalslot = -1;

        Dictionary<int, GameObject> slotPerUIDic = new Dictionary<int, GameObject>();
        Dictionary<GameObject, EnchantableItemUI> enchantableitemsdic = new Dictionary<GameObject, EnchantableItemUI>();

        #region unity calls
        protected virtual void Awake()
        {
            SetupUI();

        }
        protected virtual void Start()
        {
            Close();
        }
        protected virtual void LateUpdate()
        {
            if (GetCanvasEnabled() == false) return;

            CheckDraggingOnEnchantable();
            CheckStopDraggingEnchantable();
            DoDraggingEnchantable();

            CheckDraggingOnEnchant();
            CheckStopDraggingEnchant();
            DoDraggingEnchant();
           
        }
        #endregion


        public void Close()
        {
            CloseDown();

        }
        public void Open(IUseEnchanterCanvas user)
        {
            Setup(user);
        }
        public void SetStation(EnchantingStation station)
        {
            if (this.station != null)
            {
                this.station.OnEnchanted -= EnchantSuccess;
            }
            this.station = station;
            this.station.OnEnchanted += EnchantSuccess;


        }
       
        public void Toggle()
        {
            if (MainPanel.gameObject.activeInHierarchy)
            {
                Close();
            }
            else
            {
                Open(user);
            }
        }

        public bool GetCanvasEnabled()
        {
            return MainPanel.gameObject.activeInHierarchy;
        }

        public bool GetFreezeMover() => FreezeDungeon;

     
        public virtual void ClosePreview()
        {
            preview = null;
            original = null;
            decisionPreview.SetItem(null);
            enchantablePreview.SetItem(null);
            previewEnchant.SetEnchant(null);
            DecisionBoxPreview.SetActive(false);
        }
        public virtual void DisplayPreview()
        {
            ItemStack item = enchantablePreview.GetViewStack();
            EquipmentEnchant enchant = previewEnchant.GetEnchant();
            if (item == null || enchant == null)
            {
                //failed
                return;
            }

            if (item.Item is Equipment)
            {
                Equipment eq = item.Item as Equipment;
                original = eq;
                Equipment copy = ScriptableObject.Instantiate(eq);
                preview = copy;
                originalslot = item.SlotID;
                station.Enchant(copy, enchant, IsNative);

                DecisionBoxPreview.SetActive(true);

                decisionPreview.SetItem(copy);//this needs to just display the item.

            }

        }
        public virtual void Enchant()
        {
            ItemStack item = enchantablePreview.GetViewStack();
            EquipmentEnchant trait = previewEnchant.GetEnchant();
            if (item == null || trait == null)
            {
                //failed
                return;
            }

            if (originalslot > -1)
            {
                //switch.
                item.Item = preview;
                originalslot = -1;
            }
            else
            {
                //first time
                station.Enchant(item.Item as Equipment, trait, IsNative);

            }
            station.UserInventory.OnSlotChange?.Invoke(item.SlotID);

            ClosePreview();
            CloseDraggables();
            station.OnEnchanted?.Invoke(item.Item as Equipment);

            successEnchantable.SetItem(item.Item);
            SuccessBoxShowcase.SetActive(true);




        }

        protected virtual void CloseDraggables()
        {
            draggableEnchant.SetEnchant(null);
            draggableenchantable.SetEnchantableItem(-1, null);
        }

        protected virtual void EnchantSuccess(Equipment enchanted)
        {
          
            Events.OnEnchantSuccess?.Invoke(enchanted);
            Events.SceneEvents.OnEnchantSuccess?.Invoke(enchanted);
        }
        protected virtual void SetupUI()
        {
            previewEnchant = EnchantPreviewInstance.GetComponent<IEnchantUIElement>();
            enchantablePreview = EnchantablePreviewInstance.GetComponentInChildren<IItemViewUI>();
            decisionPreview = DecisionBoxPreview.GetComponentInChildren<IItemViewUI>();
            successEnchantable = SuccessBoxShowcase.GetComponentInChildren<IItemViewUI>();
            homeposition = DraggableEnchantableInstance.transform.position;
            draggableenchantable = DraggableEnchantableInstance.GetComponent<IEnchantableUIElement>();
            draggableEnchant = DraggableEnchantInstance.GetComponent<IEnchantUIElement>();
        }
        protected virtual void DoDraggingEnchant()
        {
            if (draggingEnchant == false) return;
            DraggableEnchantInstance.transform.position = Input.mousePosition;
           
        }
        protected virtual void CheckStopDraggingEnchant()
        {
            if (draggableEnchant.GetEnchant() == null) return;
            if (draggingEnchant == false) return;
            if (Input.GetButtonUp(interactButton))
            {

                if (DraggableEnchantInstance.GetComponent<RectTransform>().rect.Overlaps(EnchantPreviewInstance.GetComponent<RectTransform>().rect))
                {
                    EnchantPreviewInstance.GetComponent<IEnchantUIElement>().SetEnchant(draggableEnchant.GetEnchant());
                    Events.SceneEvents.OnPreviewSetEnchant?.Invoke(draggableEnchant.GetEnchant());
                    Events.OnPreviewSetEnchant?.Invoke(draggableEnchant.GetEnchant());
                }

                DraggableEnchantInstance.transform.position = homeposition;
                draggingEnchant = false;//also check if trying to place in slot

            }
        }
        protected virtual void CheckDraggingOnEnchant()
        {
            if (draggingEnchant) return;
            for (int i = 0; i < enchantUIElements.Count; i++)
            {
                RectTransform rect = enchantUIElements[i].GetComponent<RectTransform>();
                Vector2 localMousePosition = rect.InverseTransformPoint(Input.mousePosition);
                if (rect.rect.Contains(localMousePosition))
                {
                    if (Input.GetButtonDown(interactButton))
                    {
                        draggingEnchant = true;
                        TryPlaceInSlot(enchantUIElements[i].GetComponent<IEnchantUIElement>().GetEnchant());
                        break;
                    }
                }
            }


        }
        protected virtual void TryPlaceInSlot(EquipmentEnchant enchant)
        {
            draggableEnchant.SetEnchant(enchant);
            if (enchant == null) return;
            Events.SceneEvents.OnStartDragEnchant?.Invoke(enchant);
            Events.OnStartDragEnchant?.Invoke(enchant);
        }
        
        protected virtual void CheckDraggingOnEnchantable()
        {
            if (draggingEnchantable) return;
            foreach (var kvp in enchantableitemsdic)
            {
                RectTransform rect = kvp.Value.RectTransform;
                Vector2 localMousePosition = rect.InverseTransformPoint(Input.mousePosition);
                if (rect.rect.Contains(localMousePosition))
                {
                    if (Input.GetButtonDown(interactButton))
                    {
                        TryPlaceInSlot(kvp.Value.Interface.GetEnchantable());
                        break;
                    }
                }
            }

           


        }

        protected virtual void DoDraggingEnchantable()
        {
            if (draggingEnchantable)
            {
                DraggableEnchantableInstance.transform.position = Input.mousePosition;
            }
        }

        protected virtual void CheckStopDraggingEnchantable()
        {

            if (draggingEnchantable == false) return;
            if (Input.GetButtonUp(interactButton))
            {
                DraggableEnchantableInstance.transform.position = homeposition;
                draggingEnchantable = false;//also check if trying to place in slot
                if (DraggableEnchantableInstance.GetComponent<RectTransform>().rect.Overlaps(EnchantablePreviewInstance.GetComponent<RectTransform>().rect))
                {
                    //placed
                    enchantablePreview.SetItemStack(draggableenchantable.GetEnchantable());
                    Events.SceneEvents.OnPreviewSetEnchantable?.Invoke(draggableenchantable.GetEnchantable());
                    Events.OnPreviewSetEnchantable?.Invoke(draggableenchantable.GetEnchantable());
                }

            }
        }
       

        protected virtual void TryPlaceInSlot(ItemStack item)
        {
            draggableenchantable.SetEnchantableItem(item.SlotID, station.UserInventory);
            if (item == null) return;
            draggingEnchantable = true;
            Events.SceneEvents.OnStartDragEnchantable?.Invoke(item);
            Events.OnStartDragEnchantable?.Invoke(item);
        }

        protected virtual void CloseDown()
        {
            active = false;
            DecisionBoxPreview.SetActive(false);
            SuccessBoxShowcase.SetActive(false);
            decisionPreview.SetItem(null);
            enchantablePreview.SetItem(null);
            previewEnchant.SetEnchant(null);
      

            uidic.Clear();
            MainPanel.gameObject.SetActive(false);


            for (int i = 0; i < enchantUIElements.Count; i++)
            {
                Destroy(enchantUIElements[i].gameObject);
            }
            foreach (var kvp in enchantableitemsdic)
            {
                RemoveEnchantableItemUI(kvp.Key);
            }

            enchantableUIElements.Clear();
            enchantUIElements.Clear();
            slotPerUIDic.Clear();
            enchantableitemsdic.Clear();
            station = null;

            if (FreezeDungeon)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);
            }
        }



        protected virtual void Setup(IUseEnchanterCanvas user)
        {
            active = true;
            this.user = user;
            this.user.SetShopCanvass(this);
            MainPanel.gameObject.SetActive(true);


            ///redo with item stacks
            ///
            List<ItemStack> stacks = station.UserInventory.GetAllUniqueStacks();
            for (int i = 0; i < stacks.Count; i++)
            {
                CreateUIEnchantableElement(stacks[i]);
            }


            //how to handle currently equipped?

            //currently doesn't handle currently equipped.
            //station.UserInventory.GetEquippedEquipment
            //List<Equipment> equipped = station.GetEquippedEquipment();

            //for (int i = 0; i < equipped.Count; i++)
            //{
            //    Item item = equipped[i];
            //    CreateUIEnchantableElement(item);
            //}



            List<EquipmentEnchant> enchants = station.GetAllEnchants();
            for (int i = 0; i < enchants.Count; i++)
            {
                CreateUIEnchantElement(enchants[i]);
            }

            if (FreezeDungeon)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
        }

       protected virtual void RemoveEnchantableItemUI(GameObject key)
        {
            if (enchantableitemsdic.ContainsKey(key))
            {
                EnchantableItemUI value = enchantableitemsdic[key];
                station.UserInventory.OnSlotChange -= value.Interface.UpdateItem;
                slotPerUIDic.Remove(value.ItemStack.SlotID);
                enchantableUIElements.Remove(key);
                Destroy(key);
            }
        }
        protected virtual void CreateUIEnchantElement(EquipmentEnchant enchant)
        {
            GameObject instance = Instantiate(EnchantUIElementPrefab, EnchantContentParent);
            instance.GetComponent<IEnchantUIElement>().SetEnchant(enchant);
            enchantUIElements.Add(instance);
        }
        protected virtual void CreateUIEnchantableElement(ItemStack itemstack)
        {
            if (slotPerUIDic.ContainsKey(itemstack.SlotID) == false)
            {
                if (itemstack.Item == null)
                {
                    //do nothing.
                    return;
                }


                if (itemstack.Item is Equipment)
                {

                    GameObject instance = Instantiate(EnchantableUIElementPrefab, EnchantableContentParent);
                    EnchantableItemUI newui = new EnchantableItemUI(itemstack, instance);
                    newui.Interface.SetEnchantableItem(itemstack.SlotID, station.UserInventory);
                    enchantableUIElements.Add(instance);
                    enchantableitemsdic[instance] = newui;
                    station.UserInventory.OnSlotChange += newui.Interface.UpdateItem;
                    slotPerUIDic[itemstack.SlotID] = instance;
                }
                



            }
          
        }

        
      
    }
}