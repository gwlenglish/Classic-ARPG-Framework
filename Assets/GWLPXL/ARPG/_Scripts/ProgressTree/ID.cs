using UnityEngine;

namespace GWLPXL.ARPGCore.ProgressTree.com
{
    [System.Serializable]
    public class ID
    {
        public string Name = string.Empty;
        [TextArea(3,5)]
        public string Description = string.Empty;
        public int UniqueID = 0;
    }
}