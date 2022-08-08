using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.com
{
    public class CreateItem: ScriptableObject
    {
        public ItemType type = ItemType.Equipment;
        public string itemName = string.Empty;
        public EquipmentType eqType = EquipmentType.Accessory;
        public PotionType potType = PotionType.RestoreResource;
        public SocketTypes socketType = SocketTypes.Any;
    }
}