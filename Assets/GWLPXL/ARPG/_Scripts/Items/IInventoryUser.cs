
using UnityEngine;
namespace GWLPXL.ARPGCore.Items.com
{

    public interface IInventoryUser
    {
        void SetInventoryTemplate(ActorInventory newTemplate);
        void SetRuntimeInventory(ActorInventory newInv);
        ActorInventory GetInventoryRuntime();
        ActorInventory GetInvtemplate();
        GameObject GetMyInstance();

    }
}