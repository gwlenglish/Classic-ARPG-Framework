

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.CanvasUI.com
{
    public interface IUseFloatingText
    {
        void DamageResults(CombatResults args);
        Vector3 GetHPBarOffset();
        void CreateUIDamageText(string message, ElementType type, bool isCritical = false);
        void CreateUIDoTText(string message, ElementType type);
        void CreateUIRegenText(string message, ResourceType type);
        void SetActorHub(IActorHub newhub);

    }
}
