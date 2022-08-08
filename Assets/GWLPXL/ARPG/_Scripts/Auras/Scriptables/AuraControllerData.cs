using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{
    [System.Serializable]
    public class AuraControllerData
    {
        public bool AutoName = false;
        public bool AutoAssignUniqueID = false;
        public string Name = string.Empty;
        [TextArea(3, 5)]
        public string Description = string.Empty;
        public int ID = 0;
        public AuraController ControllerRef = null;
        public AuraControllerData(string name, int id, AuraController reference)
        {
            Name = name;
            ID = id;
            ControllerRef = reference;
        }
          
    }
}