using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Combat.com
{
    public abstract class ActorCombatFormulas : ScriptableObject
    {
        /// <summary>
        /// main take damage formula for the actor, calculates the reduction of phys and elemental damage adn writes to the attack values
        /// </summary>
        /// <param name="values"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public abstract CombatResults TakeDamageFormula(AttackValues values, IActorHub self);

        /// <summary>
        /// get element resist value of the self
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract int GetElementResistValue(IActorHub attacker, IActorHub self, ElementType type);
        /// <summary>
        /// get armor value of the self
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public abstract int GetArmorValue(IActorHub attacker, IActorHub self);
        /// <summary>
        /// get attack value of the self
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract PhysicalAttackResults GetAttackValue(IActorHub user);

    }
}