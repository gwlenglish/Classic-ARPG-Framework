
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// Example of applying an Aura without an Aura Controller. 
    /// </summary>
    public class Demo_ApplyAura : MonoBehaviour
    {
        public Aura ToApply;
        public KeyCode KeyToApply;
        public GameObject Enemy;
   
        // Update is called once per frame
        void Update()
        {
            if (ToApply == null)
            {
                Debug.LogError("Need an aura to apply", this);
                return;
            }

            if (UnityEngine.Input.GetKeyDown(KeyToApply))
            {
                ToApply.Apply(Enemy.GetComponent<ITakeAura>());
            }
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Enemy == null) return;

            if (Enemy.GetComponent<ITakeAura>() == null)
            {
                Debug.LogError("Enemy needs to derive from ITAKEAURA", this);
                Enemy = null;
            }
        }
#endif
    }
}