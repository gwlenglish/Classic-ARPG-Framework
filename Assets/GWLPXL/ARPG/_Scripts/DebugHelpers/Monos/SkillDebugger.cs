
using UnityEngine;

namespace GWLPXL.ARPGCore.DebugHelpers.com
{

    //used to debug skills when necessary in the editor

    public class SkillDebugger : MonoBehaviour, IGizmoDebug
    {
        GizmoType Type = GizmoType.Line;
        bool IsToggled = false;

        float radius = 1;
        float visionDistance = 1;
        float angle = 1;
        Vector3 origin = Vector3.zero;
        public void ToggleGizmoDraw(GizmoType type, float _radius, Vector3 _origin)
        {
            IsToggled = !IsToggled;
            radius = _radius;
            Type = type;
            origin = _origin;

        }
        public void ToggleGizmoDraw(GizmoType type, float distance, float _angle, Vector3 _origin)
        {
            IsToggled = !IsToggled;
            Type = type;
            visionDistance = distance;
            angle = _angle;
            origin = _origin;
        }

    

        private void OnDrawGizmos()
        {
            if (IsToggled)
            {
                switch (Type)
                {
                    case GizmoType.Sphere:
                        Gizmos.DrawWireSphere(origin, radius);
                        break;
                    case GizmoType.Line:
                        Vector3 line = origin + (transform.forward * visionDistance);
                        Vector3 rotatedLine1 = Quaternion.AngleAxis(angle, transform.up) * line;
                        Vector3 rotatedLine2 = Quaternion.AngleAxis(-angle, transform.up) * line;
                        Debug.DrawLine(transform.position, rotatedLine1, Color.blue);
                        Debug.DrawLine(transform.position, rotatedLine2, Color.blue);
                        break;

                }
            }
        }
    }
}
