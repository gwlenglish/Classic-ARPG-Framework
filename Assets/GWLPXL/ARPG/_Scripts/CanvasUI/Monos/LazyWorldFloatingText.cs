using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace GWLPXL.ARPGCore.CanvasUI.com
{

   /// <summary>
   /// combine not working
   /// </summary>
    public class LazyWorldFloatingText : MonoBehaviour, IFloatTextCanvas
    {
        class DMGUI
        {
            public IReceiveDamage Target;
            public Vector3 Pos;
            public int DMG;

            public DMGUI(IReceiveDamage t, Vector3 v, int d)
            {
                Target = t;
                Pos = v;
                DMG = d;
            }
        }

        public bool Combine = false;
        public GameObject DamageTextPrefab = default;
        public GameObject DoTTextPrefab = default;
        public GameObject RegenTextPrefab = default;
        List<DMGUI> list = new List<DMGUI>();
        int frame;
        private void FixedUpdate()
        {
            for (int i = 0; i < list.Count; i++)
            {
                DefaultMakeText(DamageTextPrefab, list[i].Pos, list[i].DMG.ToString());

            }
            list.Clear();
            frame = 0;
            if (frame > 0)
            {
                
            }
          
            frame++;
        }
        public void CreateDamagedText(IReceiveDamage damageTaker, Vector3 position, string text, ElementType type, bool isCritical = false)
        {

            if (Combine)
            {
                int dmg = 0;
                int.TryParse(text, out dmg);
                bool found = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Target == damageTaker)
                    {
                        list[i].DMG += dmg;
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    list.Add(new DMGUI(damageTaker, position, dmg));
                }
            }
            else
            {
                DefaultMakeText(DamageTextPrefab, position, text);

            }


        }

      

        public void CreateDoTText(IReceiveDamage damageTaker, string text, Vector3 position, ElementType type, bool isCritical = false)
        {
            DefaultMakeText(DoTTextPrefab, position, text);
        }

        public void CreateNewFloatingText(IReceiveDamage damageTaker, ElementUI variables, Vector3 atPosition, string text, FloatingTextType type, bool isCritical = false)
        {
            switch (type)
            {
                case FloatingTextType.Damage:
                    CreateDamagedText(damageTaker, atPosition, text, variables.Type, isCritical);
                    break;
                case FloatingTextType.DoTs:
                    CreateDoTText(damageTaker, text, atPosition, variables.Type, isCritical);
                    break;
                case FloatingTextType.Regen:
                    CreateRegenText(damageTaker, text, atPosition, ResourceType.Health, isCritical);
                    break;
            }
        }

        public void CreateRegenText(IReceiveDamage damageTaker, string text, Vector3 position, ResourceType type, bool isCritical = false)
        {
            DefaultMakeText(RegenTextPrefab, position, text);
        }

        protected virtual void DefaultMakeText(GameObject textPrefab, Vector3 position, string text)
        {
            if (textPrefab == null)
            {
                //no prefab
                return;
            }
            GameObject newtext = Instantiate(textPrefab);
            newtext.transform.position = position;
            newtext.GetComponent<TextMeshPro>().SetText(text);
            Rigidbody rb = newtext.AddComponent<Rigidbody>();
            rb.AddForce(Vector3.up * Random.Range(10, 15), ForceMode.Impulse);
            Destroy(newtext, 3);
        }

    }
}