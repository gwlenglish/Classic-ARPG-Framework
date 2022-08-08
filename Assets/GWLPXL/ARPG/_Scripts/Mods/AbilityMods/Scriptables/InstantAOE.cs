

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using UnityEngine;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using GWLPXL.ARPGCore.Combat.com;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

  
    /// <summary>
    /// instant AOE Weapon dmg effect, using Physics Overlap sphere and the caster as the origin
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/Abilities/New_WeaponDamage AOE")]
    
    public class InstantAOE : AbilityLogic
    {
        public AoEWeapoNVars Vars;
        public GameObject VFX = null;
        public Vector3 LocalOffset = Vector3.zero;
        [Tooltip("Editor helper. Draws a gizmo around the area effected.")]
        public bool ShowDebug;
        bool showing;

        private void Apply(IActorHub obj)
        {
            Vector3 origin = obj.MyTransform.position + obj.MyTransform.TransformDirection(LocalOffset);
            AttackValues values = new AttackValues(obj, null);
            CombatHelper.GetAoEWeaponDmg(values, obj, origin, Vars);

            if (VFX != null)
            {
                GameObject instance = Instantiate(VFX, origin, obj.MyTransform.rotation);//instance controls lifetime
            }

            ApplyGizmo(obj, origin);

        }
        public override bool CheckLogicPreRequisites(IActorHub forUser)
        {
            if (Contains(forUser.MyTransform)) return false;
            return true;
        }

        public override void StartCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform) == false)
            {
                Add(skillUser.MyTransform);
                Apply(skillUser);
            }
    
        }

        public override void EndCastLogic(IActorHub skillUser, Ability theSkill)
        {
            if (Contains(skillUser.MyTransform))
            {
                Remove(skillUser.MyTransform);
                IGizmoDebug gizmo = skillUser.MyTransform.GetComponent<IGizmoDebug>();
                RemoveGizmo(gizmo);
            }
        
        }

        private void ApplyGizmo(IActorHub obj, Vector3 origin)
        {

#if UNITY_EDITOR
            if (ShowDebug)
            {
                IGizmoDebug gizmo = obj.MyTransform.GetComponent<IGizmoDebug>();
                if (gizmo != null)
                {
                    if (Vars.Angle == 180)
                    {
                        gizmo.ToggleGizmoDraw(GizmoType.Sphere, Vars.Radius, origin);
                    }
                    else
                    {
                        gizmo.ToggleGizmoDraw(GizmoType.Line, Vars.Radius, Vars.Angle, origin);
                    }
                    showing = true;
                }
            }

#endif
        }

 
       

        private void RemoveGizmo(IGizmoDebug gizmo)
        {

#if UNITY_EDITOR
            if (showing)
            {
                if (gizmo != null)
                {
                    gizmo.ToggleGizmoDraw(GizmoType.Sphere, Vars.Radius, Vector3.zero);
                    showing = false;
                }
            }
#endif
        }

  

     
    }
}