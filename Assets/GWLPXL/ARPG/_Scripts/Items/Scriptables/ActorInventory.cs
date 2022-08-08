using System;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.GameEvents.com;

using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
namespace GWLPXL.ARPGCore.Items.com
{
    /// <summary>
    /// The inventory, pretty self explanatory. 
    /// Handles equipped and unequipped too.
    /// </summary>

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Inventory/NEW_ActorInventory")]
    public class ActorInventory : ScriptableObject, ISaveJsonConfig
    {
        #region actions to subscribe
        public Action<Item> OnAddItem;
        public Action<Item> OnRemoveItem;
        /// <summary>
        /// used when swapping slots.
        /// </summary>
        public Action<int, ItemStack> OnSlotChanged;
        public Action<int> OnSlotChange;
        public Action<ItemStack> OnStackChanged;
        public Action<Equipment> OnEquip;
        public Action<Equipment> OnUnEquip;
        public Action<EquipmentSlot> OnEquipmentSlotChanged;
        public Action<int> OnGoldChanged;
        #endregion
        [SerializeField]
        protected TextAsset config = null;
        [Header("Events")]
        public EquipmentChangeEvent EquipEvent;
        public EquipmentChangeEvent UnEquipEvent;
        public IInventoryUser MyUser { get; set; }
        [SerializeField]
        InventoryID ID;
        [SerializeField]
        string inventoryName = "Inventory";
        [Tooltip("Make sure the inventory size is matched with the inventory slots in the UI.")]
        public int ItemInventorySize = 25;
        [SerializeField]
        protected int currentCurrency = 100;
        protected List<Item> UniqueItemsInInventory = new List<Item>();//for easy saving
        protected List<ItemStack> UniqueStacks = new List<ItemStack>();//for debugging in editor
        [Header("Starting Stuff")]
        [SerializeField]
        protected Equipment[] startingEquipment;
        [SerializeField]
        protected Item[] startingItems;
        [SerializeField]
        protected int startingGold;


        [System.NonSerialized]
        protected Dictionary<EquipmentSlotsType, EquipmentSlot> runtimeEquipment = new Dictionary<EquipmentSlotsType, EquipmentSlot>();
        [System.NonSerialized]
        protected Dictionary<EquipmentType, EquipmentList> equippedByType = new Dictionary<EquipmentType, EquipmentList>();
        [System.NonSerialized]
        protected Dictionary<int, ItemStack> stackPerSlot = new Dictionary<int, ItemStack>();

        protected int[] slotsarr;
        protected EquipmentType[] typesForDamage = new EquipmentType[] { EquipmentType.Weapon };
        protected EquipmentType[] typesForArmor = new EquipmentType[] { EquipmentType.Armor };
        protected EquipmentList[] sortedLists = new EquipmentList[0];
        public Dictionary<Equipment, EquipmentTrait[]> AppliedTraits => appliedTraits;
        protected Dictionary<Equipment, EquipmentTrait[]> appliedTraits = new Dictionary<Equipment, EquipmentTrait[]>();
        protected IAttributeUser myActorStats;
        //clears out the inventory entirely, needed since SO's carry data over in the editor
        protected bool startingEquiped = false;
        public InventoryID GetID() => ID;
        public void SetID(InventoryID newID) => ID = newID;
        public string GetName() => inventoryName;
        public virtual int GetCurrency() => currentCurrency;
        public virtual InventoryID GetDatabaseID() => ID;
        public virtual void SetDatabaseID(InventoryID newID) => ID = newID;

        #region initialiation -- setting actor, equipping starting weapons, etc
        public virtual void InitialSetup()
        {
            stackPerSlot.Clear();
            slotsarr = new int[ItemInventorySize];
            for (int i = 0; i < slotsarr.Length; i++)
            {
                slotsarr[i] = i;
                stackPerSlot.Add(slotsarr[i], null);
            }

           

            UniqueItemsInInventory.Clear();
            runtimeEquipment.Clear();
            ModifyCurrency(startingGold);
            ResetEquipment();

        }
        public virtual void EquipStarting()
        {
            if (startingEquiped == true) return;
            startingEquiped = true;
            for (int i = 0; i < startingEquipment.Length; i++)
            {
                if (startingEquipment[i] == null)
                {
                    GWLPXL.ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Starting Equipment at element " + i + " is null, skipping", this);
                    continue;
                }
                //need to instance runtime copies
                Equipment equipment = Instantiate(startingEquipment[i]);
                equipment.AssignEquipmentTraits(startingEquipment[i].GetStats().GetIlevel());
                AddItemToInventory(equipment);

                Equip(equipment);
            }
            for (int i = 0; i < startingItems.Length; i++)
            {
                if (startingItems[i] == null)
                {
                    GWLPXL.ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Starting Item at element " + i + " is null, skipping", this);
                    continue;
                }
                Item item = Instantiate(startingItems[i]);
                if (item is Equipment)
                {
                    Equipment itemeq = item as Equipment;
                    itemeq.AssignEquipmentTraits(itemeq.GetStats().GetIlevel());
                }
                //need to instance runtime copies
                AddItemToInventory(item);
            }

        }
        public virtual void SetMyUser(IInventoryUser invUser)
        {
            MyUser = invUser;
        }
        public virtual void SetMyActorStats(IAttributeUser myStats)
        {
            myActorStats = myStats;
        }
        #endregion

        #region public splitting and swapping
        //main call to add things to the inventory
        /// <summary>
        /// Swaps the items in slotA to slotB and swaps the items in slotB to slotA
        /// </summary>
        /// <param name="slotA"></param>
        /// <param name="slotB"></param>
        /// <returns></returns>
        public virtual bool SwapStacks(int slotA, int slotB)
        {
            stackPerSlot.TryGetValue(slotA, out ItemStack stackA);
            stackPerSlot.TryGetValue(slotB, out ItemStack stackB);

            stackPerSlot[slotB] = stackA;
            OnSlotChanged?.Invoke(slotB, stackA);
            stackPerSlot[slotA] = stackB;
            OnSlotChanged?.Invoke(slotA, stackB);
            OnSlotChange?.Invoke(slotA);
            OnStackChanged?.Invoke(stackA);

            OnSlotChange?.Invoke(slotB);
            OnStackChanged?.Invoke(stackB);
            return true;
        }
        /// <summary>
        /// removes the amount from the slot and returns if can be placed in the destination slot
        /// </summary>
        /// <param name="splitStackSlot"></param>
        /// <param name="amount"></param>
        /// <param name="destinationSlot"></param>
        /// <returns></returns>
        public virtual bool SplitStack(int splitStackSlot, int amount, int destinationSlot)
        {
            stackPerSlot.TryGetValue(splitStackSlot, out ItemStack stackToSplit);
            if (stackToSplit == null)//can't split a null stack
            {
                return false;
            }

            //if we already have an item in the destination and its different, we can't split to here.
            stackPerSlot.TryGetValue(destinationSlot, out ItemStack destinationStack);
            if (destinationStack != null && destinationStack.Item.GetID() != stackToSplit.Item.GetID())
            {
                ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Can't split the stack into the destination, there's already a different item in that slot", this);
                return false;
            }

            ItemStack splitStack = null;
            splitStack = new ItemStack(stackToSplit.Item, 0, -1);

            for (int i = 0; i < stackToSplit.CurrentStackSize; i++)
            {
                if (amount <= 0)
                {
                    break;
                }

                if (splitStack.CurrentStackSize < splitStack.Item.GetStackingAmount())
                {
                    splitStack.CurrentStackSize += 1;
                    amount -= 1;
                }
            }

            for (int i = 0; i < splitStack.CurrentStackSize; i++)
            {
                AddItemToInventory(splitStack.Item, destinationSlot);
                RemoveItemFromInventory(splitStackSlot);
            }

            return true;
        }

        #endregion


        #region public equipment specific
        /// <summary>
        /// Return the EquipmentSlot
        /// </summary>
        /// <param name="byslots"></param>
        /// <returns></returns>
        public virtual EquipmentSlot GetEquipmentAtSlot(EquipmentSlotsType byslots)
        {
            runtimeEquipment.TryGetValue(byslots, out EquipmentSlot value);
            return value;

        }
        /// <summary>
        /// Equips the Equipment. Will also auto unequip any necessary equipment
        /// </summary>
        /// <param name="equipment"></param>
        public virtual void Equip(Equipment equipment)
        {
            EquipmentSlotsType[] slot = equipment.GetEquipmentSlot();
            for (int i = 0; i < slot.Length; i++)
            {
                runtimeEquipment.TryGetValue(slot[i], out EquipmentSlot value);
                if (value != null)//it means we have something.
                {
                    if (value.EquipmentInSlots != null)
                    {
                        Equipment oldEquipment = value.EquipmentInSlots;//get it, unequip it
                        UnEquip(oldEquipment);
                    }
                }
                else
                {
                    value = new EquipmentSlot();
                    runtimeEquipment[slot[i]] = value;
                }
                value.EquipmentInSlots = equipment;


                appliedTraits.TryGetValue(equipment, out EquipmentTrait[] allTraits);
                if (allTraits == null)
                {
                    EquipmentHandler.ApplyTraits(equipment, myActorStats);
                    allTraits = equipment.GetStats().GetAllTraits();
                    appliedTraits[equipment] = allTraits;
                    EquipmentHandler.ModifyVisuals(equipment.GetEquipmentSlot(), false, equipment, MyUser);


                    //raise the event
                    if (EquipEvent != null && myActorStats != null)
                    {
                        EquipEvent.EventVars = new EquipmentEventVars(this, myActorStats.GetRuntimeAttributes(), equipment, myActorStats, MyUser, slot, false);
                        GameEventHandler.RaiseEquipEvent(EquipEvent);

                    }
                }

                //remove it from inventory
                EquipmentType type = equipment.GetEquipmentType();
                equippedByType.TryGetValue(type, out EquipmentList equippedType);
                //ARPGDebugger.DebugMessage("Equipment: " + equipment + " User: " + user);
                if (equippedByType == null)
                {
                    List<Equipment> _temp = new List<Equipment>();
                    _temp.Add(equipment);
                    EquipmentList newList = new EquipmentList(_temp);
                    equippedByType.Add(type, newList);
                }
                else
                {
                    if (equippedType != null)
                    {
                        equippedType.AddPiece(equipment);
                    }
                    else
                    {
                        List<Equipment> _temp = new List<Equipment>();
                        equippedType = new EquipmentList(_temp);
                        equippedByType[type] = equippedType;
                    }
                }


                OnEquip?.Invoke(equipment);
                RemoveFirstItemFromInventory(equipment);//hmmmm. works because they dont stack and are all unique
                OnEquipmentSlotChanged?.Invoke(value);
            }




        }

        /// <summary>
        /// Removes all the traits provided by the equipment from the actor
        /// </summary>
        /// <param name="fromUser"></param>
        public virtual void RemoveAllTraits(IAttributeUser fromUser)
        {
            foreach (var kvp in appliedTraits)
            {
                Equipment equipment = kvp.Key;
                EquipmentHandler.RemoveTraits(equipment, fromUser);
            }
        }
        /// <summary>
        /// Applies all the traits provided by the equipment
        /// </summary>
        /// <param name="onUser"></param>
        public virtual void ApplyAllTraits(IAttributeUser onUser)
        {
            foreach (var kvp in appliedTraits)
            {
                Equipment equipment = kvp.Key;
                EquipmentHandler.ApplyTraits(equipment, onUser);
            }
        }
        /// <summary>
        /// Refreshes the traits, first removes then applies.
        /// </summary>
        /// <param name="onUser"></param>
        public virtual void ReApplyAllTraits(IAttributeUser onUser)
        {
            Debug.Log("Reapplied");
            foreach (var kvp in appliedTraits)
            {
                Equipment equipment = kvp.Key;
                EquipmentHandler.RemoveTraits(equipment, onUser);
                EquipmentHandler.ApplyTraits(equipment, onUser);
            }
        }
        /// <summary>
        /// Unequips a specific Equipment
        /// </summary>
        /// <param name="equipment"></param>
        public virtual void UnEquip(Equipment equipment, bool autoaddtoinventory = true)
        {
            EquipmentSlotsType[] slot = equipment.GetEquipmentSlot();
            for (int i = 0; i < slot.Length; i++)
            {
                runtimeEquipment.TryGetValue(slot[i], out EquipmentSlot value);
                value.EquipmentInSlots = null;
         
                EquipmentType type = equipment.GetEquipmentType();
                equippedByType.TryGetValue(type, out EquipmentList equippedList);

                if (equippedList != null && equippedList.Equipment.Count > 0)
                {
                    equippedList.RemovePiece(equipment);
                }

                OnEquipmentSlotChanged?.Invoke(value);
            }
     

            appliedTraits.TryGetValue(equipment, out EquipmentTrait[] allTraits);//ensures unique trait
            if (allTraits != null)
            {
                EquipmentHandler.RemoveTraits(equipment, myActorStats);
                allTraits = null;
                appliedTraits[equipment] = allTraits;
                OnUnEquip?.Invoke(equipment);
                EquipmentHandler.ModifyVisuals(equipment.GetEquipmentSlot(), true, equipment, MyUser);
                if (autoaddtoinventory)
                {
                    AddItemToInventory(equipment);
                }
         
                //raise the event
                if (UnEquipEvent != null && myActorStats != null)
                {
                    UnEquipEvent.EventVars = new EquipmentEventVars(this, myActorStats.GetRuntimeAttributes(), equipment, myActorStats, MyUser, slot, true);
                    GameEventHandler.RaiseEquipEvent(UnEquipEvent);
                }
            }







        }
        /// <summary>
        /// Returns all EquipmentSlot by EquipmentSlotsType
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<EquipmentSlotsType, EquipmentSlot> GetEquippedEquipment()
        {
            return runtimeEquipment;
        }
        /// <summary>
        /// Returns armor values from equipment added together
        /// </summary>
        /// <returns></returns>
        public virtual int GetArmorFromEquipment()
        {
            int armorMods = 0;
            for (int i = 0; i < typesForArmor.Length; i++)
            {
                EquipmentType key = typesForArmor[i];
                if (equippedByType.ContainsKey(typesForArmor[i]))
                {
                    EquipmentList equipmentList = equippedByType[key];
                    armorMods += equipmentList.GetBaseStatsAdded();
                }

            }

            return armorMods;
        }
        /// <summary>
        /// Returns damage values from equipment added together
        /// </summary>
        /// <returns></returns>
        public virtual int GetDamageFromEquipment()
        {

            int damageMods = 0;
            for (int i = 0; i < typesForDamage.Length; i++)
            {
                EquipmentType key = typesForDamage[i];
                if (equippedByType.ContainsKey(key))
                {
                    EquipmentList equipmentList = equippedByType[key];
                    damageMods += equipmentList.GetBaseStatsAdded();
                }


   
            }

            return damageMods;
        }
        /// <summary>
        /// Returns the Equipment occupying a specific slot
        /// </summary>
        /// <param name="slotToGet"></param>
        /// <returns></returns>
        public virtual Equipment GetEquipmentInSlot(EquipmentSlotsType slotToGet)
        {
            runtimeEquipment.TryGetValue(slotToGet, out EquipmentSlot value);
            if (value == null)
            {
                EquipmentSlot newSlot = new EquipmentSlot();
                newSlot.EquipmentInSlots = null;
                newSlot.slot = slotToGet;
                runtimeEquipment.Add(slotToGet, newSlot);
                return newSlot.EquipmentInSlots;
            }
            return value.EquipmentInSlots;

        }
        /// <summary>
        /// Unequips everything we currently have equipped.
        /// </summary>
        public virtual void UnEquipAll()
        {
            foreach (var kvp in runtimeEquipment)
            {
                if (kvp.Value.EquipmentInSlots == null) continue;
                UnEquip(kvp.Value.EquipmentInSlots);
            }


        }
        #endregion

        #region public items
        /// <summary>
        /// add the item to a particular slot returns if added was success
        /// </summary>
        /// <param name="item"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public virtual bool AddItemToInventory(Item item, int slot)
        {
            if (stackPerSlot.Count == 0)
            {
                InitialSetup();
            }
            stackPerSlot.TryGetValue(slot, out ItemStack value);
            if (value == null)
            {
                AddUniqueStack(item, slot);
                return true;
            }
            else
            {
                if (value.Item.GetID() == item.GetID())
                {
                    if (value.IsFull)
                    {
                        return false;
                    }
                    int potentialSize = value.CurrentStackSize + 1;
                    if (potentialSize > value.Item.GetStackingAmount())
                    {
                        return false;
                    }

                    value.CurrentStackSize += 1;
                    stackPerSlot[slot] = value;
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// Returns all the item stacks we currently hold in our inventory
        /// </summary>
        /// <param name="forItem"></param>
        /// <returns></returns>
        public virtual List<ItemStack> GetAllItemStacks(Item forItem)
        {
            List<ItemStack> _temp = new List<ItemStack>();
            foreach (var kvp in stackPerSlot)
            {
                if (kvp.Value == null) continue;
                if (forItem == kvp.Value.Item)
                {
                    _temp.Add(kvp.Value);
                }
            }
            return _temp;
        }
        /// <summary>
        /// Returns all UNIQUE stacks
        /// </summary>
        /// <returns></returns>
        public virtual List<ItemStack> GetAllUniqueStacks() => UniqueStacks;
        /// <summary>
        /// Returns all inventory slots and the item stack that occupies each one
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<int, ItemStack> GetItemStacks() => stackPerSlot;
        /// <summary>
        /// Get item stack from a specific inventory slot
        /// </summary>
        /// <param name="slotID"></param>
        /// <returns></returns>
        public virtual ItemStack GetItemStackBySlot(int slotID)
        {
            stackPerSlot.TryGetValue(slotID, out ItemStack value);
            return value;
        }
        /// <summary>
        /// Removes 1 item from the stack slot per call
        /// </summary>
        /// <param name="stackSlot"></param>
        public virtual void RemoveItemFromInventory(int stackSlot)
        {
            ItemStack stack = stackPerSlot[stackSlot];
            if (stack != null)
            {
                if (stack.Item.IsStacking() == false)
                {
                    UniqueItemsInInventory.Remove(stack.Item);
                    UniqueStacks.Remove(stack);
                    OnRemoveItem?.Invoke(stack.Item);
                    stack = null;

                }
                else
                {
                    stack.CurrentStackSize -= 1;
                    OnRemoveItem?.Invoke(stack.Item);

                    if (stack.CurrentStackSize <= 0)
                    {
                        UniqueItemsInInventory.Remove(stack.Item);
                        UniqueStacks.Remove(stack);

                        stack = null;
                    }
                    else
                    {
                        stack.IsFull = false;
                    }
                }
            }

            stackPerSlot[stackSlot] = stack;

        }
        /// <summary>
        /// Returns unique ITEMS in inventory
        /// </summary>
        /// <returns></returns>
        public virtual List<Item> GetItemsInInventory() => UniqueItemsInInventory;
        /// <summary>
        /// Adds item to first available slot, return success
        /// </summary>
        /// <param name="newItem"></param>
        public virtual bool AddItemToInventory(Item newItem)
        {
            if (CanWeAddItem(newItem) == false) return false;

            //get first available free inventory slot
            if (newItem.IsStacking())
            {
                if (TryAddToExistingStack(newItem) == false)
                {
                    AddUniqueStack(newItem);
                }
            }
            else
            {

                AddUniqueStack(newItem);
            }

            return true;

        }
        /// <summary>
        /// Returns the remainder that wasn't added (i.e. the leftovers if any). If 0, all were added.
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual int AddItemsToInventory(Item newItem, int amount)
        {
            int remainder = amount;
            for (int i = 0; i < amount; i++)
            {
                if (AddItemToInventory(newItem))
                {
                    remainder -= 1;
                }
            }

            return remainder;
        }
        /// <summary>
        /// Modifies currency by amount. Negatives will reduce currency, positive will increase.
        /// </summary>
        /// <param name="amount"></param>
        public virtual void ModifyCurrency(int amount)//not implemented yet
        {
            currentCurrency += amount;
            OnGoldChanged?.Invoke(currentCurrency);
        }

        /// <summary>
        /// Returns if there's room for the item to be added to the inventory. 
        /// </summary>
        /// <param name="potentialItem"></param>
        /// <returns></returns>
        public virtual bool CanWeAddItem(Item potentialItem)
        {
            
            if (potentialItem.IsStacking())//if stacking
            {
                foreach (var kvp in stackPerSlot)//we check for existing stack
                {
                    ItemStack stack = kvp.Value;
                    if (stack == null) return true;
                    if (stack.Item.GetID().ID == potentialItem.GetID().ID)
                    {
                        //we found a similar item
                        if (stack.IsFull == false) return true;//if there's a stack that we can put more in, we have room

                    }
                }
                return FindEmptySlot() != -1;//if we find no room in stacks, we find room in the inventory slots
            }
            else
            {
                return FindEmptySlot() != -1;//find an empty inventory slot
            }

        }
        /// <summary>
        /// Removes 1 of the first item it finds in our inventory per call.
        /// </summary>
        /// <param name="item"></param>
        public virtual void RemoveFirstItemFromInventory(Item item)
        {
            if (item == null) return;
            int slot = -1;
            foreach (var kvp in stackPerSlot)
            {
                if (kvp.Value == null) continue;

                if (item == kvp.Value.Item)
                {
                    //found
                    slot = kvp.Key;
                    break;
                }
            }
            if (slot != -1)
            {
                RemoveItemFromInventory(slot);
            }
        }

        public bool RemoveItemFromSlot(Item item, int slot)
        {
            stackPerSlot.TryGetValue(slot, out ItemStack value);
            if (value == null)
            {
                DebugHelpers.com.ARPGDebugger.DebugMessage(ARPGDebugger.GetColorForInventory("Trying to remove " + item.GetGeneratedItemName() +" but none at slot " + slot.ToString()), this);
                return false;
            }

            if (value.Item == null)
            {
                DebugHelpers.com.ARPGDebugger.DebugMessage(ARPGDebugger.GetColorForInventory("Trying to remove " + item.GetGeneratedItemName() + " but none at slot " + slot.ToString()), this);
                return false;
            }
            int current = value.CurrentStackSize;
            int potentialnew = current -= 1;
            if (potentialnew == 0)
            {
                value = new ItemStack(null, 0, value.SlotID);

            }
            else
            {
                value.IsFull = false;
                value.CurrentStackSize = potentialnew;
         
            }
            stackPerSlot[slot] = value;
            OnSlotChanged?.Invoke(slot, value);
            OnSlotChange?.Invoke(slot);
            OnStackChanged?.Invoke(value);
            return true;


        }
        #endregion

        #region protected helpers
        protected virtual bool AddUniqueStack(Item newItem, int atSlot)
        {
            ItemStack newStack = new ItemStack(newItem, 1, atSlot);
            stackPerSlot[atSlot] = newStack;
            UniqueStacks.Add(newStack);
            return true;
        }
        protected virtual void ResetEquipment()
        {
            sortedLists = new EquipmentList[0];
            int length = System.Enum.GetValues(typeof(EquipmentType)).Length;
            sortedLists = new EquipmentList[length];
            int index = 0;
            foreach (EquipmentType pieceType in Enum.GetValues(typeof(EquipmentType)))
            {

                List<Equipment> _temp = new List<Equipment>();
                EquipmentList newList = new EquipmentList(_temp);
                newList.EditorDescription = pieceType.ToString();
                sortedLists[index] = newList;
                index++;
                equippedByType.TryGetValue(pieceType, out EquipmentList value);
                if (value == null)
                {
                    equippedByType.Add(pieceType, newList);
                }
                else
                {
                    equippedByType[pieceType] = newList;
                }
            }

            foreach (EquipmentSlotsType pieceType in Enum.GetValues(typeof(EquipmentSlotsType)))
            {
                if (pieceType == EquipmentSlotsType.None) continue;

                EquipmentSlot newSlot = new EquipmentSlot();
                newSlot.EquipmentInSlots = null;
                newSlot.slot = pieceType;
                runtimeEquipment.TryGetValue(pieceType, out EquipmentSlot value);
                if (value == null)
                {
                    runtimeEquipment.Add(pieceType, newSlot);
                }
                else
                {
                    runtimeEquipment[pieceType] = newSlot;
                }


            }
        }

        protected virtual int FindEmptySlot()
        {
            int availableSlot = -1;
            foreach (var kvp in stackPerSlot)
            {
                ItemStack stack = kvp.Value;
                if (stack == null)
                {
                    availableSlot = kvp.Key;
                    return availableSlot;
                }
            }
            return availableSlot;
        }
        protected virtual void AddUniqueStack(Item newItem)
        {
            int availableSlot = FindEmptySlot();
            if (availableSlot == -1) return;//we can't add

            ItemStack newStack = new ItemStack(newItem, 1, availableSlot);
            stackPerSlot[availableSlot] = newStack;
            UniqueItemsInInventory.Add(newItem);
            UniqueStacks.Add(newStack);

            OnAddItem?.Invoke(newItem);
            OnSlotChange?.Invoke(availableSlot);
            OnSlotChanged?.Invoke(availableSlot, newStack);
            OnStackChanged?.Invoke(newStack);
        }


        protected virtual bool TryAddToExistingStack(Item newItem)
        {
            //we need to find it in the dictionary.

            ItemStack stack = null;
            bool addedToStack = false;

            foreach (var kvp in stackPerSlot)
            {
                stack = kvp.Value;
                if (stack != null)
                {
                    if (newItem.GetID().ID == stack.Item.GetID().ID)
                    {
                        //found a match.
                        if (stack.IsFull == false)
                        {
                            int potentialNewSize = stack.CurrentStackSize + 1;
                            if (potentialNewSize > newItem.GetStackingAmount())
                            {
                                stack.IsFull = true;
                            }
                            else
                            {
                                stack.CurrentStackSize = potentialNewSize;
                                if (stack.CurrentStackSize >= newItem.GetStackingAmount())
                                {
                                    stack.IsFull = true;
                                }
                                addedToStack = true;
                                OnAddItem?.Invoke(newItem);
                                OnSlotChange?.Invoke(stack.SlotID);
                                OnSlotChanged?.Invoke(stack.SlotID, stack);
                                OnStackChanged?.Invoke(stack);
                                break;
                            }
                        }
                    }
                }

            }

           
            return addedToStack;
        }
        #endregion


        #region json save interface
        public void SetTextAsset(TextAsset textAsset)
        {
            config = textAsset;
        }

        public TextAsset GetTextAsset()
        {
            return config;
        }

        public UnityEngine.Object GetObject()
        {
            return this;
        }
        #endregion
    }


}