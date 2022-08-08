


using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{

    /// <summary>
    /// Renamed from IWeaponStatusChange to IWeaponModification
    /// </summary>
    public interface IWeaponModification
    {
        /// <summary>
        /// get the transform of the mod
        /// </summary>
        /// <returns></returns>
        Transform GetTransform();
        /// <summary>
        /// enable or disable the mod
        /// </summary>
        /// <param name="isEnabled"></param>
        void SetActive(bool isEnabled);
        /// <summary>
        /// query if the mod is active
        /// </summary>
        /// <returns></returns>
        bool IsActive();
        /// <summary>
        /// peform the mod logic
        /// </summary>
        /// <param name="other"></param>
        void DoModification(AttackValues values);
        /// <summary>
        /// if returns true, will prevent damage calculations, false proceed as usual.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool DoChange(Transform other);
        /// <summary>
        /// set the owner of the mod
        /// </summary>
        /// <param name="myself"></param>
        void SetUser(IActorHub myself);
    }
}