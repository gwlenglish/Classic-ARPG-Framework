

using UnityEngine;

namespace GWLPXL.ARPGCore.DebugHelpers.com
{
    public interface IGizmoDebug
    {
        void ToggleGizmoDraw(GizmoType type, float _radius, Vector3 origin);
        void ToggleGizmoDraw(GizmoType type, float distance, float _angle, Vector3 origin);

    }
}
