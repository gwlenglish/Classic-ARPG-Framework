

using GWLPXL.ARPGCore.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
    [System.Serializable]
    public class DeflectorOnHitVars
    {
        [Tooltip("How long to keep deflection active during the ability.")]
        public float Duration = .5f;
        [Tooltip("Enable to skip damage calculations on success.")]
        public bool PreventDamageOnSuccess = true;
        [Tooltip("Enable to allow the  projectile to hit its original caster, the one who deflected now owns the projectile.")]
        public bool EnableTakeOver = true;
        public EditorPhysicsType PhysicsType = EditorPhysicsType.Unity3D; 
    }

    /// <summary>
    /// Applies a deflect projectiles on hit mod
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusChanges/WeaponBuffs/NEW Deflect Projectiles")]

    public class ApplyDeflectProjectileOnHit : WeaponStatusChanges
    {
        public DeflectorOnHitVars Vars = new DeflectorOnHitVars();


        [System.NonSerialized]
        Dictionary<Transform, WeaponBuffTracker> trackerdic = new Dictionary<Transform, WeaponBuffTracker>();


        public override void Apply(Transform[] weapons, IActorHub forUser)
        {
            Enable(weapons, forUser, trackerdic);
        }

        public override void Remove(Transform[] weapons, IActorHub forUser)
        {
            Disable(weapons, forUser, trackerdic);
        }


       
        protected override IWeaponModification CreateIWeaponMono(Transform forTransform)
        {
            DeflectProjectilesOnHit source = forTransform.gameObject.AddComponent<DeflectProjectilesOnHit>();
            source.Vars = this.Vars;
            return source;
        }
    }
}