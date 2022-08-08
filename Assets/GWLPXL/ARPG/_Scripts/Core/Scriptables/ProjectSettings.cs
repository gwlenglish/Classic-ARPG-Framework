using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.States.com;

namespace GWLPXL.ARPGCore.com
{ 
    public enum EditorPhysicsType
{
    Unity3D = 0,
    Unity2D = 1
}

    [System.Serializable]
    public class Player3DDefaults
    {
        public MovementStates Move3DDefault = null;
        public RuntimeAnimatorController AnimController = null;
    }
    [System.Serializable]
    public class Player2DDefaults
    {
        public RuntimeAnimatorController AnimController = null;
        public GenericAnimate2DSO IdleStates = null;
        public GenericAnimate2DSO WalkStates = null;
        public GenericAnimate2DSO DeathStates = null;
        public Ability2D[] AbilityStates = new Ability2D[0];
    }
    [System.Serializable]
    public class AIActorDefaults
    {
        public AIStateSO[] EnemyDefaults3D = new AIStateSO[0];
        public AIStateSO[] EnemyDefaults2D = new AIStateSO[0];
        public RuntimeAnimatorController AnimController2D = null;
        public RuntimeAnimatorController AnimController = null;

    }
    [System.Serializable]
    public class InspectOptions
    {
        public EquipmentViewOptions Equipment;
        public PlayerActorViewOptions Player;
        public EnemyActorViewOptions Enemy;
    }
    [System.Serializable]
    public class GenerateOptions
    {
        public EquipmentGenerateOptions Equipment;
        public AttribtueGenerateOptions Attributes;
    }
   
   
    [System.Serializable]
    public class PowerCurves
    {
        public string Description;
        public AnimationCurve Curve;
        public PowerCurves(AnimationCurve curve, string description)
        {
            Description = description;
            Curve = curve;
        }
    }
    [System.Serializable]
    public class AttribtueGenerateOptions
    {
        [HideInInspector]
        public bool Generated = false;
        [HideInInspector]
        public ActorAttributes Attributes = null;
        [HideInInspector]
        public int MinILevelCurve = 1;
        [HideInInspector]
        public int MaxILevelCurve = 100;
        [HideInInspector]
        public int Level = 1;
        [HideInInspector]
        public Vector2 ScollableEq = Vector2.zero;
        [HideInInspector]
        public List<PowerCurves> PowerCurves = new List<PowerCurves>();
        [HideInInspector]
        public ActorAttributes[] CurvedAttributes = new ActorAttributes[100];
    }
    [System.Serializable]
    public class EquipmentGenerateOptions
    {

        [HideInInspector]
        public int MinILevelCurve = 1;
        [HideInInspector]
        public int MaxILevelCurve = 100;
        [HideInInspector]
        public int ILevel = 1;
        [HideInInspector]
        public Equipment Equipment = null;
        [HideInInspector]
        public Vector2 ScollableEq = Vector2.zero;
        [HideInInspector]
        public List<PowerCurves> PowerCurves = new List<PowerCurves>();
        [HideInInspector]
        public int[] DamageValues = new int[100];
        [HideInInspector]
        public Equipment[] CurvedEq = new Equipment[100];

        public AnimationCurve GetPowerCurve(string description)
        {
            for (int i = 0; i < PowerCurves.Count; i++)
            {
                if (description == PowerCurves[i].Description)
                {
                    return PowerCurves[i].Curve;
                }
            }
            return null;
        }
        public void AddPowerCurve(PowerCurves newCurve)
        {
            PowerCurves.Add(newCurve);
        }
    }
    [System.Serializable]
    public class PlayerActorViewOptions
    {
        /// <summary>
        /// attributes, inventory, class, abilities, auras
        /// </summary>
        [HideInInspector]
        public string[] PlayerViewOptions = new string[5] { "Attributes", "Inventory", "Class", "Abilities", "Auras" };
        [HideInInspector]
        public int ToolBarSelector = 0;
        public ActorAttributes Attributes;
        public ActorInventory Inventory;
        public ActorClass ActorClass;
        public AbilityController Abilities;
        public AuraController Auras;

    }
    [System.Serializable]
    public class EnemyActorViewOptions
    {
        /// <summary>
        /// attributes, inventory, class, abilities, auras
        /// </summary>
        [HideInInspector]
        public string[] EnemyViewOptions = new string[6] { "Attributes", "Inventory", "Class", "Abilities", "Auras", "Loot Drops" };
        [HideInInspector]
        public int ToolBarSelector = 0;
        public ActorAttributes Attributes;
        public ActorInventory Inventory;
        public ActorClass ActorClass;
        public AbilityController Abilities;
        public AuraController Auras;


    }
    [System.Serializable]
    public class EquipmentViewOptions
    {
        public Equipment Equipment;
        
        public Vector2 ScollableEq;
    }
    [System.Serializable]
    public class PingObjects
    {
        public PingObjectPaths Paths = new PingObjectPaths();
        public PingObject TypesPing;
    }
    public class PingObjectPaths
    {
        public string TypePing = "Assets/GWLPXL/ARPG/_Scripts/Types/TypePing.asset";
    }
    [System.Serializable]
    public class LayerAssign
    {
        [Header("For the ground...")]
        [Tooltip("For use in navigation and raycast detection.")]
        public LayerMask GroundLayer = default;
        [Header("Only objects on this layer can use combat and attack and be attacked.")]
        public LayerMask AttackableLayer = default;
        [Header("Only objects on this layer can be interacted with and NOT ATTACKABLE.")]
        public LayerMask InteractableLayer = default;
    }
    [System.Serializable]
    public class GizmoIconPaths
    {
        [Header("Breakable")]
        public string BreakableBase = "Assets/GWLPXL/ARPG/Gizmos/TestAttackable.png";
        public string BreakableDropLocation = "Assets/GWLPXL/ARPG/Gizmos/TestDropLocation.png";
        [Header("Searchable")]
        public string SearchableBase = "Assets/GWLPXL/ARPG/Gizmos/SearchableIcon.png";
    }

    [System.Serializable]
    public class PrefabPaths
    {
        public string EnemyHPInfoPrefabPath = "Assets/GWLPXL/ARPG/Prefabs/Enemy/EnemyInfo.prefab";
        public string LootPrefabPath = "Assets/GWLPXL/ARPG/Prefabs/Looting/LootPrefab.prefab";

    }

    [System.Serializable]
    public class AnimatorControllerDefaults
    {
        public string PlayerAnimatorPath = "Assets/GWLPXL/ARPG/Data/Animations/Animators/Base/ARPG_Player_Base.controller";
        public string EnemyAnimatorPath = "Assets/GWLPXL/ARPG/Data/Animations/Animators/Base/ARPG_Enemy_Base.controller";
    }

    [System.Serializable]
    public class CanvasDefaults
    {
        public string CanvasPrefabspath = "Assets/GWLPXL/ARPG/Prefabs/UI";

    }
   [System.Serializable]
   public class UnityDefaults
{
    public EditorPhysicsType PhysicsType = EditorPhysicsType.Unity3D;

}
[System.Serializable]
    public class ProjectTemplates
    {
        public UnityDefaults UnityDefaults = new UnityDefaults();
        public CanvasDefaults CanvasPaths = new CanvasDefaults();
        public AnimatorControllerDefaults AnimatorPaths = new AnimatorControllerDefaults();
        public PrefabPaths PrefabPaths = new PrefabPaths();
        public RuntimeAnimatorController BasePlayerAnimatorC = null;
        public RuntimeAnimatorController BaseEnemyAnimatorC = null;
        public Player3DDefaults Player3DDefaults = null;
        public AIActorDefaults ActorDefaults = null;
        public Player2DDefaults Player2DDefaults = null;

    }
    [CreateAssetMenu(menuName ="GWLPXL/ARPG/NEW_ProjectSettings")]
    public class ProjectSettings : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        [Header("Project Defaults")]
        public LayerAssign LayerAssign;
        public ProjectTemplates Templates;
        public GizmoIconPaths GizmoIconPaths;
        [Header("Editor Temporary Holders")]
        public PingObjects PingObjects;
        public InspectOptions InspectObjects;
        public GenerateOptions GeneratedTemp;

        public Object GetObject() => this;

        public TextAsset GetTextAsset() => config;

      
        public void SetTextAsset(TextAsset textAsset) => config = textAsset;


        private void OnValidate()
        {
#if UNITY_EDITOR
            if (Templates.BaseEnemyAnimatorC == null)
            {
                Templates.BaseEnemyAnimatorC = UnityEditor.AssetDatabase.LoadAssetAtPath(Templates.AnimatorPaths.EnemyAnimatorPath, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController; 

            }
            if (Templates.BasePlayerAnimatorC == null)
            {
                Templates.BasePlayerAnimatorC = UnityEditor.AssetDatabase.LoadAssetAtPath(Templates.AnimatorPaths.PlayerAnimatorPath, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
            }
            if (PingObjects.TypesPing == null)
            {
                PingObjects.TypesPing = UnityEditor.AssetDatabase.LoadAssetAtPath(PingObjects.Paths.TypePing, typeof(PingObject)) as PingObject;
            }

#endif
        }

    }
}
