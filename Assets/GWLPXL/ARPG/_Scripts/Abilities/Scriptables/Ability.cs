
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using GWLPXL.ARPGCore.StatusEffects.com;

namespace GWLPXL.ARPGCore.Abilities.com
{
   [System.Serializable]
   public class AbilityInterruptOptions
    {
        [Tooltip("Which collision layers should interrupt the ability?")]
        public LayerMask[] CollisionLayers = new LayerMask[0];
        [Tooltip("If using collision layers, must define the appropriate collision type, 2d or 3d")]
        public EditorPhysicsType PhysicsType = EditorPhysicsType.Unity3D;
        public bool OnCasterDamaged = false;
        public bool OnCasterDied = true;
        [Tooltip("Use the EffectName.")]
        public string[] InterruptEffects = new string[0];
        [Tooltip("Abilities that can interrupt")]
        public Ability[] Abilities = new Ability[0];
        
    }
    /// <summary>
    /// Main class that defines an ability. It contains ability logic, which is the actions performed by the ability.
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/NEW_Ability")]
    public class Ability : ScriptableObject, ISaveJsonConfig
    {
        [Header("Editor")]
        [SerializeField]
        TextAsset config;
        [SerializeField]
        AbilityID id;
        bool autoGenerateID = false;
        [SerializeField]
        bool autoName = false;

        [Header("Ability")]
        public AbilityData Data;
        [Tooltip("Abilities in the same category can not be used simultaneously. Ones with different categories can. ")]
        [SerializeField]
        AbilityCategory category = AbilityCategory.One;
        [Header("Cooldown")]
        [HideInInspector]
        public bool AddCooldown = true;//should always be true, hiding for now until tests are complete
        [Tooltip("After skill end, the time before we can use the skill again.")]
        public float CoolDownRate = 1;
        public float Duration = 1;
        [Tooltip("Enable to allow the ability to repeat itself as long as the button is held.")]
        public bool CanLoop = false;
        [Header("Delay")]
        public bool AddDelay = false;
        [SerializeField]
       [Range(0, .99f)]
       [Tooltip("Percent of the cooldown to delay before firing.")]
        public float Delay = 0;
        [Header("Sight Options")]
        [Range(1, 180)]
        [Tooltip("Starting From the user's forward direction and checking left and right. So 15 would check if target is between -15 and 15 degrees. 180 would be a complete circle, 90 would be half.")]
        public float ForwardSightAngle = 180;
        [Range(.1f, .9f)]
        float buffer = .1f;//need a buffer because dont want to be exactly at the range but at least a bit inside
        [Header("Interrupt Options")]
        [SerializeField]
        AbilityInterruptOptions interruptOptions = new AbilityInterruptOptions();
        bool charged = false;
        public bool GetCharged() => charged;
        public void SetCharged(bool chargedAbility) => charged = chargedAbility;
        public AbilityInterruptOptions GetInterruptOptions() => interruptOptions;
        public AbilityID GetID() => id;
        public void SetID(AbilityID newID) => id = newID;
        #region helper functions
        public bool CheckRequirements(IActorHub forUser)
        {
            for (int i = 0; i < Data.Logics.Length; i++)
            {
                if (Data.Logics[i] == null)
                {
                    ARPGDebugger.DebugMessage(ARPGDebugger.GetColorForError("Logic slot is null, reference became lost. Fix it on this ability " + this.Data.Name), this);
                    continue;
                }
                if (Data.Logics[i].CheckLogicPreRequisites(forUser) == false)
                {
                    return false;
                }
            }
            return true;
        }
        public bool HasSight(IActorHub user, Transform target, EditorPhysicsType type)
        {
            return CombatHelper.HasSight(user, target, type, ForwardSightAngle);

           
        }
        public float GetRangeSquaredWithBuffer()
        {
            return (Data.Range * Data.Range) * (1 - buffer);
        }
        public float GetRangeWithBuffer()
        {
            return Data.Range * (1 - buffer);
        }
        public float GetRange()
        {
            return Data.Range;
        }
        public string GetName()
        {
            return Data.Name;
        }
        public float GetDamageMultiplier()
        {
            return Data.DamageMultiplier;
        }
        public bool StartSkill(IActorHub abilityUser)
        {
            for (int i = 0; i < Data.Logics.Length; i++)
            {
                Data.Logics[i].StartCastLogic(abilityUser, this);
            }
            
            return true;
        }

        public bool EndSkill(IActorHub abilityUser)
        {
            for (int i = 0; i < Data.Logics.Length; i++)
            {
                Data.Logics[i].EndCastLogic(abilityUser, this);

            }

            return true;
        }

        public virtual int GetCategory()
        {
            return (int)category;
        }
        #endregion




        #region interface
        public void SetTextAsset(TextAsset textAsset)
        {
            config = textAsset;
        }

        public TextAsset GetTextAsset()
        {
            return config;
        }

        public Object GetObject()
        {
            return this;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {


            AutoName(Data.Name);

            if (Duration <= 0)
            {
                Duration = .02f;
            }

            if (Duration < Delay)
            {
                Duration = Delay + .01f;
            }

          

        }

        public void AutoName(string newName)
        {
            if (autoName && string.IsNullOrEmpty(newName) == false && this.name != newName)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, Data.Name);
            }
        }


#endif
    }
}