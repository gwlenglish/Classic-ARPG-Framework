
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
namespace GWLPXL.ARPGCore.Wearables.com
{


    public interface IWearClothing
    {
        void UpdateClothing(ActorInventory inventory);
        void UpdateClothing(IInventoryUser inventory);
        void RemoveClothing(EquipmentSlotsType[] atSlots);
        void WearClothing(Equipment equipment);

        void SetActorHub(IActorHub newHub);

    }
}