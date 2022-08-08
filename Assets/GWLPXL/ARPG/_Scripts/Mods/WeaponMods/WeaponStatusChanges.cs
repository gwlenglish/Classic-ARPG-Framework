
using UnityEngine;
using GWLPXL.ARPGCore.Abilities.com;
using System.Collections.Generic;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
   
    /// <summary>
    /// tracker for the buffs
    /// </summary>
    public class WeaponBuffTracker
    {
        public IAbilityUser ForUser = null;
        public WeaponStatusChanges StatusChanges = null;
        public IWeaponModification IWeapon = null;
        public GameObject VFXInstance = null;
        public WeaponBuffTracker(IAbilityUser user, IWeaponModification iweap, WeaponStatusChanges so, GameObject vfxinstance)
        {
            ForUser = user;
            StatusChanges = so;
            VFXInstance = vfxinstance;
            IWeapon = iweap;
        }
        public void EnableVFX()
        {
            if (VFXInstance == null) return;
            VFXInstance.SetActive(true);
        }
        public void DisableVFX()
        {
            if (VFXInstance == null) return;
            VFXInstance.SetActive(false);
        }
    }
    /// <summary>
    /// base class for the weapon mods
    /// </summary>
    public abstract class WeaponStatusChanges : ScriptableObject
    {
        [SerializeField]
        protected string descriptiveName = string.Empty;
        [SerializeField]
        [TextArea(3, 3)]
        protected string description = string.Empty;
        [SerializeField]
        protected bool autoName = false;

        public bool ParentPrefabToDamageSource = false;
        public bool TimeObjectLifetimeWithWeaponStatus = true;
        public GameObject VFXPrefab = null;

        public abstract void Apply(Transform[] weapon, IActorHub forUser);
        public abstract void Remove(Transform[] weapons, IActorHub forUser);
        protected abstract IWeaponModification CreateIWeaponMono(Transform forTransform);

        protected virtual void Enable(Transform[] coat, IActorHub forUser, Dictionary<Transform, WeaponBuffTracker> trackingDic)
        {
           

            for (int i = 0; i < coat.Length; i++)
            {
                Transform coatSkill = coat[i];
                //rewrite this
                if (trackingDic.ContainsKey(coatSkill) == false)
                {
                    //first time, make it and set active
                    WeaponBuffTracker tracker = new WeaponBuffTracker(forUser.MyAbilities, CreateIWeaponMono(coatSkill), this, MakePrefabInstance(coatSkill));
                    tracker.IWeapon.SetUser(forUser);
                    trackingDic[coatSkill] = tracker;
                    ARPGDebugger.DebugMessage("Added new weapon buff + " + this.GetType().Name + " on " + coatSkill.name, this);
                }

                trackingDic[coatSkill].IWeapon.SetActive(true);
                trackingDic[coatSkill].EnableVFX();
                ARPGDebugger.DebugMessage("Enabled weapon buff + " + this.GetType().Name + " on " + coatSkill.name, this);
            }



        }

        protected virtual void Disable(Transform[] coat, IActorHub forUser, Dictionary<Transform, WeaponBuffTracker> trackingDic)
        {
            for (int i = 0; i < coat.Length; i++)
            {
                Transform coatdrain = coat[i];
                if (trackingDic.ContainsKey(coatdrain))
                {
                    WeaponBuffTracker weapon = trackingDic[coatdrain];
                    weapon.IWeapon.SetActive(false);
                    weapon.DisableVFX();
                    Destroy(weapon.IWeapon as MonoBehaviour);
                    weapon = null;
                    trackingDic.Remove(coatdrain);

                    ARPGDebugger.DebugMessage("Disabled weapon buff + " + this.GetType().Name + " on " + coatdrain.name, this);


                }
            }
            
        }

        protected virtual GameObject MakePrefabInstance(Transform coatSkill)
        {
            if (VFXPrefab == null) return null;
            GameObject prefabInstance = Instantiate(VFXPrefab);
            prefabInstance.transform.position = coatSkill.position;
            if (ParentPrefabToDamageSource)
            {
                prefabInstance.transform.SetParent(coatSkill);
            }
            return prefabInstance;

        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (autoName && this.name != descriptiveName)
            {
                //rename logic
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, descriptiveName);
            }


        }

#endif
    }
}