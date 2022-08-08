using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{


    public class AdditionalLauncher : MonoBehaviour, IWeaponModification, ITick
    {
        public float FireRate = 1;
        public bool Looping = false;
        [SerializeField]
        ProjectileEvents events = new ProjectileEvents();
        [SerializeField]
        GameObject projectilePrefab;

        IActorHub hub = null;
        bool isactive = false;
        bool fired = false;
        private void Start()
        {
            AddTicker();
        }
        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public bool DoChange(Transform other)
        {
            return false;
        }

        public void DoModification(AttackValues values)
        {
            
        }

        public void DoTick()
        {
            if (fired && Looping == false) return;
            fired = true;
            SoloLaunch();
        }

        public float GetTickDuration()
        {
            return FireRate;
        }

        public Transform GetTransform() => this.transform;


        public bool IsActive() => isactive;

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public void SetActive(bool isEnabled) => isactive = isEnabled;
       

        public void SetUser(IActorHub myself) => hub = myself;
       

        public void SoloLaunch()
        {
            FireProjectile().GetComponent<IDoDamage>().EnableDamageComponent(true, hub);

        }

        GameObject FireProjectile()
        {
            GameObject newObj = Instantiate(projectilePrefab.gameObject, this.transform.position, Quaternion.identity);
            newObj.transform.forward = this.transform.forward;
            events.SceneEvents.OnFired.Invoke();
            return newObj;
        }
    }
}
