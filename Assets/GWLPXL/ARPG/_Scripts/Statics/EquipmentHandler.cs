using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Wearables.com;

namespace GWLPXL.ARPGCore.Statics.com
{


    public static class EquipmentHandler
    {
        public static void ModifyVisuals(EquipmentSlotsType[] slot, bool unEquip, Equipment equipment, IInventoryUser MyUser)
        {
            if (MyUser != null)
            {
                IWearClothing clothing = MyUser.GetMyInstance().GetComponent<IWearClothing>();
                if (clothing != null)
                {
                    if (unEquip)
                    {
                        clothing.RemoveClothing(slot);
                    }
                    else
                    {
                        clothing.WearClothing(equipment);
                    }
                }

                IVisualizeStats statsUI = MyUser.GetMyInstance().GetComponent<IVisualizeStats>();
                if (statsUI != null)
                {
                    IAttributeUser myActorStats = MyUser.GetMyInstance().GetComponent<IAttributeUser>();
                    statsUI.DisplayStats(myActorStats);
                }
            }
        }

        public static void ApplyTraits(Equipment equipment, IAttributeUser toModify)
        {
            if (toModify == null) return;
            if (equipment == null) return;
            EquipmentTrait[] traits = equipment.GetStats().GetAllTraits();
            for (int i = 0; i < traits.Length; i++)
            {
                traits[i].ApplyTrait(toModify);
               

            }
        }
        public static void RemoveTraits(Equipment equipment, IAttributeUser toModify)
        {
            if (toModify == null) return;
            if (equipment == null) return;
            EquipmentTrait[] traits = equipment.GetStats().GetAllTraits();
            for (int i = 0; i < traits.Length; i++)
            {
                traits[i].RemoveTrait(toModify);


            }
        }

       
    }
}