using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
    [System.Serializable]
    public class GenerateRourseOnHitVars
    {
        public ResourceType Type = ResourceType.None;
        [Range(.01f, 5f)]
        public float Percent = .01f;
    }
}
