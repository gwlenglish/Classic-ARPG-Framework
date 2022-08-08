

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.CanvasUI.com
{
    /// <summary>
    ///     /// class that sends information to the floating text canvas
    /// </summary>
    public class EnemyDungeonCanvasUser : MonoBehaviour, IUseFloatingText
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
        [Tooltip("Adjusts the start position of the floating text.")]
        protected Vector3 floatingTextOffset = new Vector3(0, 2, 0);//change the y value to move the hp bar up and down
        [SerializeField]
        protected bool combine = false;
        protected IActorHub hub;

        #region public interface

      
        public virtual void DamageResults(CombatResults args)
        {
            if (combine)
            {
                int dmg = 0;
                for (int i = 0; i < args.DamageValues.ReportElementalDmg.Count; i++)
                {
                    dmg += args.DamageValues.ReportElementalDmg[i].Result;
                }

                dmg += args.DamageValues.ReportPhysDmg.Result;
               
                CreateUIDamageText(dmg.ToString(), ElementType.None, false);
            }
            else
            {
                for (int i = 0; i < args.DamageValues.ReportElementalDmg.Count; i++)
                {
                   
                    CreateUIDamageText(args.DamageValues.ReportElementalDmg[i].Result.ToString(), args.DamageValues.ReportElementalDmg[i].Type, args.DamageValues.ReportElementalDmg[i].WasCrit);
                }

                CreateUIDamageText(args.DamageValues.ReportPhysDmg.Result.ToString(), ElementType.None, args.DamageValues.ReportPhysDmg.WasCrit);
             
            }
        }
        public virtual Vector3 GetHPBarOffset()
        {
            return floatingTextOffset;
        }

       

        public virtual void CreateUIDamageText(string message, ElementType type, bool isCritical)
        {
            DefaultDamageText(message, type, isCritical);
            
        }

        public virtual void CreateUIRegenText(string message, ResourceType type)
        {
            DefaultRegenText(message, type);
        }


        public virtual void CreateUIDoTText(string message, ElementType type)
        {
            DefaultDoTText(message, type);
        }

      
        public virtual void SetActorHub(IActorHub newhub)
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




