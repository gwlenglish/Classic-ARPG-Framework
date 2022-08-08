
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Wearables.com
{

    /// <summary>
    /// attach to prefabs you want to wear in the clothing system.
    /// Add it to the equipment drop
    /// </summary>
    [System.Serializable]
    public class MappedOffsets
    {
        public ActorInventory InvKey => forUser;
        public Vector3 PositionOffset => positionOffset;
        public Vector3 RotatationOffset => rotationOffset;
        public Vector3 MeshScale => meshScale;
        [SerializeField]
        Vector3 positionOffset = Vector3.zero;
        [SerializeField]
        Vector3 rotationOffset = Vector3.zero;
        [SerializeField]
        Vector3 meshScale = new Vector3(1, 1, 1);


        [SerializeField]
        ActorInventory forUser = null;
        public MappedOffsets(Vector3 pos, Vector3 rot, ActorInventory inv, Vector3 _scale)
        {
            positionOffset = pos;
            rotationOffset = rot;
            forUser = inv;
            meshScale = _scale;
        }
    }

    /// <summary>
    /// Actor Inventory Template is the key that finds the offsets for the wearable equipment.
    /// </summary>
    public class Wearable : MonoBehaviour, IWearable
    {
        [SerializeField]
        MappedOffsets[] Offsets = new MappedOffsets[0];
        [SerializeField]
        Transform meshTransform = null;
        Equipment equipment;

        public void SetMaps(IInventoryUser forUser)
        {


            MappedOffsets offsets = FindOffset(forUser);
            transform.localPosition = offsets.PositionOffset;
            transform.localEulerAngles = offsets.RotatationOffset;
            this.transform.localScale = new Vector3(1, 1, 1);
            if (meshTransform != null)
            {
                meshTransform.localScale = offsets.MeshScale;
            }
        
            

        }

        public Vector3 GetWearingOffset(IInventoryUser forUser)
        {
            return FindOffset(forUser).PositionOffset;
        }
        public Vector3 GetWearingRot(IInventoryUser forUser)
        {
            return FindOffset(forUser).RotatationOffset;
        }
        public Equipment GetWearableEquipment()
        {
            return equipment;
        }

        public void SetWearableEquipment(Equipment forEquipment)
        {
            equipment = forEquipment;
        }

        MappedOffsets FindOffset(IInventoryUser forUser)
        {
            ActorInventory key = forUser.GetInvtemplate();
            for (int i = 0; i < Offsets.Length; i++)
            {
                if (Offsets[i].InvKey == key)
                {
                    
                    return Offsets[i];
                }
            }
            if (Offsets.Length == 0)
            {
                Offsets = new MappedOffsets[1];
                Offsets[0] = new MappedOffsets(Vector3.zero, Vector3.zero, key, Vector3.one);
            }

            ARPGDebugger.DebugMessage("Didn't find an offset for " + key.name + ". Create a wearable for " + forUser.GetMyInstance().name + "Defaulting for now.", forUser.GetMyInstance());
            return Offsets[0];
          
        }

        public void SetMeshTransform(Transform newTransform) => meshTransform = newTransform;
        
    }
}