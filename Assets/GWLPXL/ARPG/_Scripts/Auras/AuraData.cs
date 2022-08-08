using GWLPXL.ARPGCore.Types.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// The Unique Data for an Aura.
    /// </summary>
    [System.Serializable]
    public class AuraData
    {
        public bool AutoName = true;
        public bool AutoAssignID = false;
        public string AuraName = string.Empty;
        [TextArea(3, 5)]
        public string AuraDescription = string.Empty;
        public GameObject AuraPrefab = null;
        [Tooltip("Auras of the same category can not be both applied to the same user at the same time if using the Aura controller." + 
            "For instance, an Aura with category 0 and an aura with category 1 can both be activated at the same time. However " + 
            "An Aura with category 0 will be replaced by another Aura with category 0, when using the Aura Controller.")]
        public AuraCategory auraCategory  = AuraCategory.Zero;
        public int uniqueID = 0;
        public AuraLogic[] AuraLogics;
        [Tooltip("The groups that the aura will effect, including self and AOE. Use 'friendly' for the player objects and allied specific auras, use enemy for others.")]
        public AuraTargetGroup[] AuraGroups = new AuraTargetGroup[0];
        [Header("Pulse")]
        [Tooltip("A pulse is a continuous application of the aura each X second, where X is the Pulse Rate. " +
           "So a pulse rate of 1 means it will apply the aura's effect every 1 second. " +
           "Ensure both the pulse is true and the pulse rate is higher than 0 for it to work.")]
        public bool Pulses = false;
        [Tooltip("The rate in seconds that the aura pulses. ")]
        public float PulseRate = 0;
        [Header("AOE")]
        [Tooltip("AOE stands for Area of Effect. This gives the aura an area of effect so it can affect others." +
    "The AOE uses a physics spherecast, so make sure the layers are checked and the objects to effect " +
    "have a collider and an ITakeAura component." +
    "There must be a layer, AOE must be true, and both Area Radius and Check Area Rate must be greater than 0 " +
    "for the system to use AOE.")]
        public bool AOE = false;
        [Tooltip("Since the check using unity's physics system, we can define the layers to check.")]
        public LayerMask[] LayersToCheck = new LayerMask[0];
        [Tooltip("Do you want to check Collider or Collider2D?")]
        public ColliderType ColliderType = ColliderType.Collider;
        [Tooltip("The check is a sphere cast, so this is a radius where the aura is the center.")]
        public float AreaRadius = 0;
        [Tooltip("How often to check the area for objects that can be affected. This does not apply the effect" +
            "but detects if there are users in the area.")]
        public float CheckAreaRate = .25f;
   


    }
}