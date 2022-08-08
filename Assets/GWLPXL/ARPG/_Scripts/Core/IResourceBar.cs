
using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.com
{
    public interface IResourceBar
    {
        void UpdateBar();
        void SetResource(ResourceType type);
        void SetActorHub(IActorHub newHub);
    }
}