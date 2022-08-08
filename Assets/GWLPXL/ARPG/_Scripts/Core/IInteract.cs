
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{
    /// <summary>
    /// Used for non-combat interactions, such as picking up loot. See "Loot" class for an example or any of the OnClick objects in the demo scenes. 
    /// </summary>
    public interface IInteract
    {
        bool DoInteraction(GameObject interactor);
     
        bool IsInRange(GameObject interactor);

    }
}