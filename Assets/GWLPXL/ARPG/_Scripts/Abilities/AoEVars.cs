using GWLPXL.ARPGCore.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.com
{
    [System.Serializable]
    public class AoEVars
    {
        public float Radius;
        [Tooltip("From attack forward, 180 is full circle around")]
        [Range(1, 180)]
        public int Angle;
        public EditorPhysicsType Type;
        public AoEVars(float radius, int angle)
        {
            Radius = radius;
            Angle = angle;
        }
    }
}