
using UnityEngine;
namespace GWLPXL.ARPGCore.Attributes.com
{

    public interface IAttributeUser
    {
        Transform GetInstance();
        void SetAttributeTemplate(ActorAttributes newTemplate);
        ActorAttributes GetAttributeTemplate();
        ActorAttributes GetRuntimeAttributes();
        void SetRuntimeAttributes(ActorAttributes newStats);
    }
}