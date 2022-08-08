
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    public interface IProjectileCombatUser
    {
        Transform GetProjectileFirePoint(int atShooterIndex);
        Transform GetProjectileFirePoint();
        IShootProjectiles[] GetShooters();
        IShootProjectiles GetShooter(int atIndex);
        EquipmentSlotsType[] GetShooterSlots();
        void SetIShooter(IShootProjectiles newshooter, int atIndex);
        void ResetToDefaultShooter(int atIndex);

    }
}
