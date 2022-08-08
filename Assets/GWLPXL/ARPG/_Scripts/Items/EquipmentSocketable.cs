using GWLPXL.ARPGCore.Traits.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{
    /// <summary>
    /// socket items to be placed in equipment sockets
    /// </summary>
    [CreateAssetMenu(menuName ="GWLPXL/ARPG/Socketables/New Equipment Socketable")]

    public class EquipmentSocketable : SocketItem
    {
        public List<EquipmentTrait> EquipmentTraitSocketable = new List<EquipmentTrait>();


    }
}