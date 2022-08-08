
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
  
    /// <summary>
    /// POCO for override edits in editor
    /// </summary>
    [System.Serializable]
    public class FloatingTextOverride
    {
        public bool UseOverride;
        public ElementUI Override;

    }
    /// <summary>
    /// class that sends information to the floating text canvas
    /// </summary>
    public class PlayerFloatingTextUser : MonoBehaviour, IUseFloatingText
    {
        [Tooltip("Will create custom floating text if enabled")]
        [SerializeField]
        protected FloatingTextOverride overrideDmgText;
        [Tooltip("Will create custom floating text if enabled")]
        [SerializeField]
        protected FloatingTextOverride overrideRegenText;
        [Tooltip("Will create custom floating text if enabled")]
        [SerializeField]
        protected FloatingTextOverride overrideDotText;
        [SerializeField]
        protected Vector3 hPBarOffset = new Vector3(0, 2, 0);//change the y value to move the hp bar up and down
        protected IActorHub hub;

        #region public interface
        public void DamageResults(CombatResults args)
        {
            throw new System.NotImplementedException();
        }
        public void CreateUIDamageText(string message, ElementType type, bool isCritical)
        {
            DefaultDamageText(message, type, isCritical);
        }
        public void CreateUIRegenText(string message, ResourceType type)
        {
            DefaultRegenText(message, type);
        }



        public void CreateUIDoTText(string message, ElementType type)
        {
            DefaultDoTText(message, type);

        }
        public Vector3 GetHPBarOffset()
        {
            return hPBarOffset;
        }
        public void SetActorHub(IActorHub newhub)
        {
            hub = newhub;
        }


       
        #endregion

        #region protected virtual
        protected virtual void DefaultDoTText(string message, ElementType type)
        {
            if (overrideDotText.UseOverride)
            {
                DungeonMaster.Instance.GetFloatTextCanvas().CreateNewFloatingText(hub.MyHealth, overrideDotText.Override, transform.position + GetHPBarOffset(), message, FloatingTextType.DoTs);
            }
            else
            {
                DungeonMaster.Instance.GetFloatTextCanvas().CreateDoTText(hub.MyHealth, message, transform.position + GetHPBarOffset(), type);
            }

        }

        protected virtual void DefaultRegenText(string message, ResourceType type)
        {
            if (overrideRegenText.UseOverride)
            {
                DungeonMaster.Instance.GetFloatTextCanvas().CreateNewFloatingText(hub.MyHealth, overrideRegenText.Override, transform.position + GetHPBarOffset(), message, FloatingTextType.Regen);

            }
            else
            {
                DungeonMaster.Instance.GetFloatTextCanvas().CreateRegenText(hub.MyHealth, message, transform.position + GetHPBarOffset(), type);
            }
        }

        protected virtual void DefaultDamageText(string message, ElementType type, bool isCritical)
        {
            if (overrideDmgText.UseOverride)
            {
                DungeonMaster.Instance.GetFloatTextCanvas().CreateNewFloatingText(hub.MyHealth, overrideDmgText.Override, transform.position + GetHPBarOffset(), message, FloatingTextType.Damage);

            }
            else
            {
                DungeonMaster.Instance.GetFloatTextCanvas().CreateDamagedText(hub.MyHealth, transform.position, message, type, isCritical);

            }
        }

      
        #endregion
    }
}