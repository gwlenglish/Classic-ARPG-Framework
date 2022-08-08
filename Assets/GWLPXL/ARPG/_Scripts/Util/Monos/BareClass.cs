
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{
    /// <summary>
    /// used for the bare scene to test the individual components
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BareClass : MonoBehaviour
    {
        Collider coll;
        Camera main;

        protected virtual void Awake()
        {
            main = Camera.main;
            coll = GetComponent<Collider>();
        }


        // Update is called once per frame
        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                RaycastHit hit;
                if (Physics.Raycast(main.ScreenPointToRay(mousePos), out hit))
                {
                    if (coll.bounds.Contains(hit.point))
                    {
                        IMouseClickable click = GetComponent<IMouseClickable>();
                        if (click != null)
                        {
                            click.DoClick();
                        }
                    }
                }
            }
        }
    }
}