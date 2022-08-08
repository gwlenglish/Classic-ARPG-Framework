using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    public interface IShootProjectiles
    {
        void SetProjectilePrefab(GameObject newPrefab);
        void SetFirePoint(Transform newFirePoint);
        GameObject GetProjectilePrefab();
        Transform GetShootPoint();
        GameObject FireProjectile();
        GameObject FireProjectile(Vector3 offset, Quaternion rot);
        GameObject FireProjectile(GameObject newPrefab, Vector3 offset, Quaternion rot);
    }
}