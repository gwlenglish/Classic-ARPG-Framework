
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Wearables.com
{

    public interface IWearable
    {
        void SetMeshTransform(Transform newTransform);
        void SetMaps(IInventoryUser forUser);
        Vector3 GetWearingOffset(IInventoryUser forUser);
        Vector3 GetWearingRot(IInventoryUser forUser);
        Equipment GetWearableEquipment();
        void SetWearableEquipment(Equipment forEquipment);
    }
}