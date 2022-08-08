
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    public class ProjectileWeapon : MonoBehaviour, IShootProjectiles
    {
        [SerializeField]
        ProjectileEvents events = new ProjectileEvents();
        [SerializeField]
        GameObject projectilePrefab;
        [SerializeField]
        Transform firePoint;

      
        public GameObject FireProjectile()
        {
            GameObject newObj = Instantiate(projectilePrefab.gameObject, firePoint.position, firePoint.rotation);
            events.SceneEvents.OnFired.Invoke();
            return newObj;
        }

        public GameObject FireProjectile(Vector3 offset, Quaternion rot)
        {
            GameObject newObj = Instantiate(projectilePrefab.gameObject, firePoint.position + offset, rot);
            events.SceneEvents.OnFired.Invoke();
            return newObj;
        }

        public GameObject FireProjectile(GameObject newPrefab, Vector3 offset, Quaternion rot)
        {
            GameObject newObj = Instantiate(newPrefab, firePoint.position + offset, rot);
            events.SceneEvents.OnFired.Invoke();
            return newObj;
        }

        public GameObject GetProjectilePrefab() => projectilePrefab;

        public Transform GetShootPoint() => firePoint;

        public void SetFirePoint(Transform newFirePoint) => firePoint = newFirePoint;


        public void SetProjectilePrefab(GameObject newPrefab) => projectilePrefab = newPrefab;
       
    }
}