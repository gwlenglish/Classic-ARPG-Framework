

using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.Wearables.com
{
    /// <summary>
    /// the system that links with the equipment and instances the equipment
    /// recently switched to be able to use dual hands, check for bugs
    /// </summary>
    public class Clothing : MonoBehaviour, IWearClothing
    {
        [SerializeField]
        PlayerClothingEvents clothingEvents = new PlayerClothingEvents();
        [SerializeField]
        protected ClothingPart[] WearableClothing = new ClothingPart[0];

        IActorHub actorHub = null;
        

        public virtual void UpdateClothing(ActorInventory inventory)
        {
            Dictionary<EquipmentSlotsType, EquipmentSlot> startEquipped = inventory.GetEquippedEquipment();
            foreach (var kvp in startEquipped)
            {
                for (int i = 0; i < WearableClothing.Length; i++)
                {
                    if (WearableClothing[i].Slots.slot == kvp.Key)
                    {
                        Equipment equipped = kvp.Value.EquipmentInSlots;
                        if (equipped == null) continue;

                        equipped.CreateWearableInstance(this);
                        continue;
                    }
                }
            }
        }
        public virtual void UpdateClothing(IInventoryUser inventory)
        {
            ActorInventory myInv = inventory.GetInventoryRuntime();
            UpdateClothing(myInv);


        }

        public virtual void RemoveClothing(EquipmentSlotsType[] atSlots)
        {
            for (int i = 0; i < WearableClothing.Length; i++)
            {
                for (int j = 0; j < atSlots.Length; j++)
                {
                    if (atSlots[j] == WearableClothing[i].Slots.slot)
                    {
                        DestroyOldClothes(WearableClothing[i]);

                        if (actorHub.MyMelee == null) continue;


                        //if we destroyed our weapon (unequiped), set the default back on.
                        EquipmentSlotsType[] meleewpnslots = actorHub.MyMelee.GetWpnMeleeSlots();
                        for (int k = 0; k < meleewpnslots.Length; k++)
                        {
                            if (meleewpnslots[k] == atSlots[j])
                            {
                                actorHub.MyMelee.ResetDefaultDamageDealer(k);

                            }
                        }

                    }
                }

            }

            //we need to check if we have nothing equipped, then re-assign the default DD
        }
        public virtual void WearClothing(Equipment equipment)
        {
            if (equipment == null) return;

            EquipmentSlotsType[] equipmentSlot = equipment.GetEquipmentSlot();
            for (int i = 0; i < WearableClothing.Length; i++)
            {
                Transform parent = WearableClothing[i].Parent;
                if (parent == null)
                {
                    ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Trying to wear clothes but don't have a transform set for the Clothing parts", this.transform);
                    continue;
                }
                EquipmentSlotsType slot = WearableClothing[i].Slots.slot;
                if (slot == equipmentSlot[0])
                {
                    //we foudn it
                    ClothingPart clothing = WearableClothing[i];
                    DestroyOldClothes(clothing);

                    if (equipment.GetWearable() != null)
                    {
                        GameObject prefab = equipment.GetWearable().gameObject;
                        GameObject instance = Instantiate(prefab);

                        IWearable wearable = instance.GetComponent<IWearable>();
                        wearable.SetWearableEquipment(equipment);
                        if (wearable.GetWearableEquipment().GetEquipmentType() == EquipmentType.Weapon)
                        {
                            IMeleeWeapon melee = instance.GetComponentInChildren<IMeleeWeapon>();//
                            if (melee != null)
                            {
                                SwitchMeleeWeapon(wearable, melee.GetDamageComponent());
                            }
                            IShootProjectiles shooter = instance.GetComponentInChildren<IShootProjectiles>();
                            if (shooter != null)
                            {
                                SwitchProjectileWeapon(wearable, shooter);
                            }
                            //Debug.Log(instance.name + " DD: " + dd.GetType().Name);
                         
                        }


                        clothing.WearingInstance = instance;
                        clothing.Slots.EquipmentInSlots = wearable.GetWearableEquipment();
                        clothing.WearingInstance.transform.SetParent(clothing.Parent);
                        clothing.WearingInstance.transform.position = clothing.Parent.transform.position;
                        clothing.WearingInstance.transform.rotation = clothing.Parent.transform.rotation;

                        //Vector3 offset = wearable.GetWearingOffset(invuser);
                        //Vector3 rotation = wearable.GetWearingRot(invuser);
                        //Vector3 scale = wearable.GetWearingScale(invuser);

                        clothing.WearingInstance.GetComponent<IWearable>().SetMaps(actorHub.MyInventory);
                        //clothing.WearingInstance.transform.localPosition = offset;
                        //clothing.WearingInstance.transform.localEulerAngles = rotation;
                        //clothing.WearingInstance.GetComponent<IWearable>().GetMeshTransform().localScale = scale;
                        RaiseWearingNewEquipmentEvent(equipment);

                        // WearNewClothes(wearable, clothing);
                    }
                }
            }
        }

        private void RaiseWearingNewEquipmentEvent(Equipment equipment)
        {
            if (clothingEvents == null) return;
            clothingEvents.SceneEvents.OnNewClothingEquipped.Invoke(equipment);
        }

        protected virtual void SwitchProjectileWeapon(IWearable wearable, IShootProjectiles projectileshooter)
        {
            if (projectileshooter == null) return;
            if (actorHub.MyProjectiles == null) return;
            EquipmentSlotsType[] slotTypes = wearable.GetWearableEquipment().GetEquipmentSlot();
            EquipmentSlotsType[] shooterSlots = actorHub.MyProjectiles.GetShooterSlots();
            for (int i = 0; i < shooterSlots.Length; i++)
            {
                EquipmentSlotsType shooterWpnSlot = shooterSlots[i];
                for (int j = 0; j < slotTypes.Length; j++)
                {
                    EquipmentSlotsType slot = slotTypes[j];
                    if (shooterWpnSlot == slot)
                    {
                        ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage(shooterWpnSlot.ToString() + " " + slot.ToString(), this);
                        actorHub.MyProjectiles.SetIShooter(projectileshooter, i);
                    }
                }
            }
        }

        protected virtual void SwitchMeleeWeapon(IWearable wearable, IDoDamage dd)
        {
            if (dd == null) return;
            if (actorHub.MyMelee == null) return;

            EquipmentSlotsType[] slotTypes = wearable.GetWearableEquipment().GetEquipmentSlot();
            EquipmentSlotsType[] meleewpntypes = actorHub.MyMelee.GetWpnMeleeSlots();
            for (int i = 0; i < meleewpntypes.Length; i++)
            {
                EquipmentSlotsType meleeWpnSlot = meleewpntypes[i];
                for (int j = 0; j < slotTypes.Length; j++)
                {
                    EquipmentSlotsType slot = slotTypes[j];
                    if (meleeWpnSlot == slot)
                    {
                        ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage(meleeWpnSlot.ToString() + " " + slot.ToString(), this);
                        actorHub.MyMelee.SetMeleeDamageDealer(dd, i);
                    }
                }
            }
        

        }

        protected virtual void WearNewClothes(IWearable wearable, ClothingPart clothing)
        {

            clothing.Slots.EquipmentInSlots = wearable.GetWearableEquipment();
            clothing.WearingInstance.transform.SetParent(clothing.Parent);
            clothing.WearingInstance.transform.position = clothing.Parent.transform.position;
            clothing.WearingInstance.transform.rotation = clothing.Parent.transform.rotation;
            Vector3 offset = wearable.GetWearingOffset(actorHub.MyInventory);
            Vector3 rotation = wearable.GetWearingRot(actorHub.MyInventory);
            clothing.WearingInstance.transform.localPosition = offset;
            clothing.WearingInstance.transform.localEulerAngles = rotation;
        }

        protected virtual void DestroyOldClothes(ClothingPart clothing)
        {
            if (clothing.WearingInstance == null) return;

            Destroy(clothing.WearingInstance);
            clothing.WearingInstance = null;

        }

        public void SetActorHub(IActorHub newHub)
        {
            actorHub = newHub;
        }
    }

    #region helper classes
    [System.Serializable]
    public class ClothingPart
    {
        public EquipmentSlot Slots;
        public Transform Parent;
        [HideInInspector]
        public GameObject WearingInstance;
    }

    #endregion
}