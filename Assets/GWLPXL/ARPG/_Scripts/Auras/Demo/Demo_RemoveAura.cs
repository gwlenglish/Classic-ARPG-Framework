
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// Example of how to remove Auras without an Aura Controller. 
    /// </summary>
    public class Demo_RemoveAura : MonoBehaviour
    {
        public Aura ToRemove;
        public KeyCode KeyToApply;
        public GameObject Enemy;

        // Update is called once per frame
        void Update()
        {
            if (ToRemove == null)
            {
                Debug.LogError("Need an aura to apply", this);
                return;
            }

            if (Input.GetKeyDown(KeyToApply))
            {
                ToRemove.Remove(Enemy.GetComponent<ITakeAura>());
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