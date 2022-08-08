
using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using UnityEngine;
using System.Collections.Generic;
namespace GWLPXL.ARPGCore.Combat.com
{



    public interface IDoDamage
    {
        float GetAdditionalChargePercent();
        /// <summary>
        /// If applying any buffs, must be called after applying the buffs. 
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <param name="_actor"></param>
        void EnableDamageComponent(bool isEnabled, IActorHub _actor);
        /// <summary>
        /// Perform the damage logic
        /// </summary>
        /// <param name="damageTarget"></param>
        void DamageLogic(IActorHub damageTarget);
        /// <summary>
        /// set who owns the damage object
        /// </summary>
        /// <param name="newOWner"></param>
        void SetActorOwner(IActorHub newOWner);
        /// <summary>
        /// get the owner of the damage object
        /// </summary>
        /// <returns></returns>
        IActorHub GetActorOwner();
        /// <summary>
        /// get the transform of the damage object
        /// </summary>
        /// <returns></returns>
        Transform GetTransform();
        /// <summary>
        /// get the mods on the damage object
        /// </summary>
        /// <returns></returns>
        IWeaponModification[] GetWeaponMods();
        /// <summary>
        /// get actors we've already damaged
        /// </summary>
        /// <returns></returns>
        List<IActorHub> GetDamagedList();
     
    }
}