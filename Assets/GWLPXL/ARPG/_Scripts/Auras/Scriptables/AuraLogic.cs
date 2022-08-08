
using UnityEngine;


namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// base class for any new aura logic. 
    /// Inherit from this abstract class to create your own logic.
    /// </summary>
    public abstract class AuraLogic : ScriptableObject
    {
        /// <summary>
        /// When Aura is applied. 
        /// </summary>
        /// <param name="onUser"></param>
        /// <returns></returns>
        public abstract bool DoApplyLogic(ITakeAura onUser);
        /// <summary>
        /// When Aura is removed. 
        /// </summary>
        /// <param name="fromUser"></param>
        /// <returns></returns>
        public abstract bool DoRemoveLogic(ITakeAura fromUser);

    }
}