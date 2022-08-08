
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using GWLPXL.ARPGCore.DebugHelpers.com;
namespace GWLPXL.ARPGCore.Combat.com
{
    

    public class ProjectileCombatant : MonoBehaviour, IProjectileCombatUser
    {
        [SerializeField]
        [Tooltip("Default is good for enemies who don't change weapons. For the player, the fist should be the default so they can always attack.")]
        ProjectileWeapon[] defaultshooters = new ProjectileWeapon[0];
        Transform defaultFirePoint = null;
        [SerializeField]
        EquipmentSlotsType[] shooterSlots = new EquipmentSlotsType[2] { EquipmentSlotsType.RightWpnHand, EquipmentSlotsType.LeftWpnHand };//slots that can carry weapons by index.

        IShootProjectiles[] currentShooters = new IShootProjectiles[0];


        GameObject[] shooterObjs = new GameObject[0];
        private void Awake()
        {
            currentShooters = new IShootProjectiles[GetShooterSlots().Length];
            shooterObjs = new GameObject[GetShooterSlots().Length];
            if (defaultshooters.Length == 0)
            {
                defaultshooters = GetComponentsInChildren<ProjectileWeapon>();
            }

            for (int i = 0; i < defaultshooters.Length; i++)
            {
                SetIShooter(defaultshooters[i], i);
            }
        }
        /// <summary>
        /// returns the transform of the currently equipped shooter at index
        /// </summary>
        /// <param name="atShooterIndex"></param>
        /// <returns></returns>
        public Transform GetProjectileFirePoint(int atShooterIndex)
        {
            if (atShooterIndex < currentShooters.Length - 1)
            {
                return currentShooters[atShooterIndex].GetShootPoint();
            }
            return GetProjectileFirePoint();
        }
        /// <summary>
        /// returns this transform
        /// </summary>
        /// <returns></returns>
        public Transform GetProjectileFirePoint()
        {
            if (defaultFirePoint == null)
            {
                GameObject firepoint = new GameObject();
                firepoint.transform.position = this.transform.position;
                firepoint.transform.rotation = this.transform.rotation;
                firepoint.transform.SetParent(this.transform);
                defaultFirePoint = firepoint.transform;
            }
            return defaultFirePoint;
        }
      

        public IShootProjectiles[] GetShooters()
        {
            return currentShooters;
        }

        public void SetIShooter(IShootProjectiles newshooter, int atIndex)
        {
            if (atIndex > shooterSlots.Length - 1)
            {
                ARPGDebugger.DebugMessage("Trying to set a damage dealer over the amount of weapons the actor can carry. Increase the max if you want to carry more.", this);
                return;
            }
            currentShooters[atIndex] = newshooter;
            shooterObjs[atIndex] = newshooter.GetShootPoint().gameObject;
        }

        public EquipmentSlotsType[] GetShooterSlots()
        {
            return shooterSlots;
        }

        public void ResetToDefaultShooter(int atIndex)
        {
            if (atIndex > currentShooters.Length - 1)
            {
                ARPGDebugger.DebugMessage("Trying to set a damage dealer over the amount of weapons the actor can carry. Increase the max if you want to carry more.", this);
                return;
            }


            if (atIndex > currentShooters.Length - 1)
            {
                //we dont have a default one
                currentShooters[atIndex] = null;
            }
            else
            {
                //we do have default one
                if (defaultshooters[atIndex] == null)
                {
                    currentShooters[atIndex] = null;
                }
                else
                {
                    SetIShooter(defaultshooters[atIndex].GetComponent<IShootProjectiles>(), atIndex);
                }
            }



        }

        public IShootProjectiles GetShooter(int atIndex)
        {
           if (atIndex < currentShooters.Length - 1)
            {
                return currentShooters[atIndex];
            }
            return null;
        }
    }
}