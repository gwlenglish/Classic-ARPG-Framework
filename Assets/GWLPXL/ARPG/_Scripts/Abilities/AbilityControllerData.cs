using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{
    [System.Serializable]
    public class AbilityControllerData
    {
        [Header("Basic Info")]
        public bool AutoName = false;
        public bool AutoAssignUniqueID = false;
        public string Name = string.Empty;
        [TextArea(3, 5)]
        public string Description = string.Empty;
        public int UniqueID = 0;
    }

  
}