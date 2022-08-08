using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.com
{
   
    [System.Serializable]
    public class AoEWeapoNVars
    {
        public float Radius = 5;
        [Tooltip("From attack forward, 180 is full circle around")]
        [Range(1, 180)]
        public int Angle = 180;
        [Range(.01f, 2f)]
        public float PercentOfWpnDmg = .01f;
        public ElementType Element = ElementType.None;
        public GameObject FXPrefab = null;
        public EditorPhysicsType PhysicsType = EditorPhysicsType.Unity3D;
        public AoEWeapoNVars(float radius, int angle)
        {
            Radius = radius;
            Angle = angle;
        }

    }
}