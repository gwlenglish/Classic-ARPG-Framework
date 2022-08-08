
using UnityEngine;

namespace GWLPXL.ARPGCore.Portals.com
{
    public interface IPortal
    {
        string GetSceneName();
        int GetScenePosition();
        Vector3 GetLocation();
    }

}