using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Abilities.com
{
    public interface IAbilityUser
    {
        Ability GetChargedAbility();
        void SetChargedAbility(Ability ability);
        /// <summary>
        /// 1 = 100%, .5 = 50% 2 = 200%, etc.
        /// </summary>
        /// <param name="byAmount"></param>
        void ModifyAbilityMulti(float byAmount);
        /// <summary>
        /// set template, typically only for editor and creation use
        /// </summary>
        /// <param name="newTemplate"></param>
        void SetTemplate(AbilityController newTemplate);
        /// <summary>
        /// get template, used as a unique key for saving and persistence
        /// </summary>
        /// <returns></returns>
        AbilityController GetTemplate();
        /// <summary>
        /// runtime copy of the controller, modified at runtime as to not dirty the template
        /// </summary>
        /// <returns></returns>
        AbilityController GetRuntimeController();
        /// <summary>
        /// sets the runtime controller, this is the one used throughout gameplay
        /// </summary>
        /// <param name="abilityController"></param>
        void SetRuntimeController(AbilityController abilityController);
        /// <summary>
        /// get the transform that this script lives on
        /// </summary>
        /// <returns></returns>
        Transform GetParentTransform();
        /// <summary>
        /// returns true if cast is successful, returns false otherwise
        /// </summary>
        /// <param name="toCast"></param>
        /// <returns></returns>
        bool TryCastAbility(Ability toCast);
        /// <summary>
        /// gets last ability we tried
        /// </summary>
        /// <returns></returns>
        Ability GetLastIntendedAbility();
        /// <summary>
        /// sets the ability we want to try
        /// </summary>
        /// <param name="ability"></param>
        void SetIntendedAbility(Ability ability);
        /// <summary>
        /// sets ability based on slot index in the controller
        /// </summary>
        /// <param name="equippedAbilitySlot"></param>
        void SetIntendedAbility(int equippedAbilitySlot);
        /// <summary>
        /// sets basic attack
        /// </summary>
        void SetIntendedBasicAttack();
        /// <summary>
        /// asks if we currently are using an ability. returns true if we are
        /// </summary>
        /// <returns></returns>
        bool GetInCooldown();
        /// <summary>
        /// returns the owner
        /// </summary>
        /// <returns></returns>
        IActorHub GetActorHub();
/// <summary>
/// sets the owner
/// </summary>
/// <param name="newhub"></param>
        void SetActorHub(IActorHub newhub);

    }
}