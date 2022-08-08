#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using System;
using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Classes.com
{


    [CustomEditor(typeof(ActorClass))]
    public class ActorClassEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ActorClass actorC = (ActorClass)target;
            if (GUILayout.Button("Add All Attributes"))
            {
                AddAll((actorC));
        
            }

            GUILayout.Space(25);
            if (GUILayout.Button("Save Config"))
            {
                JsconConfig.SaveJson(actorC);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(actorC);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(actorC);

            }

        }

        void AddAll(ActorClass toClass)
        {
            ClassAttributes classStuff = toClass.GetClassAttributes();

            int length = Enum.GetValues(typeof(AccessoryType)).Length;
            classStuff.AllowedAccessories = new AccessoryType[length];
            int i = 0;
            foreach (AccessoryType pieceType in Enum.GetValues(typeof(AccessoryType)))
            {
                classStuff.AllowedAccessories[i] = pieceType;
                Debug.Log(pieceType);
                i++;
            }

            length = Enum.GetValues(typeof(ArmorMaterial)).Length;
            classStuff.AllowedArmors = new ArmorMaterial[length];
            i = 0;
            foreach (ArmorMaterial pieceType in Enum.GetValues(typeof(ArmorMaterial)))
            {
                classStuff.AllowedArmors[i] = pieceType;
                Debug.Log(pieceType);
                i++;
            }

            length = Enum.GetValues(typeof(WeaponType)).Length;
            classStuff.AllowedWeapons = new WeaponType[length];
            i = 0;
            foreach (WeaponType pieceType in Enum.GetValues(typeof(WeaponType)))
            {
                classStuff.AllowedWeapons[i] = pieceType;
                Debug.Log(pieceType);
                i++;
            }

            length = Enum.GetValues(typeof(EquipmentSlotsType)).Length;
            classStuff.AllowedSlots = new EquipmentSlotsType[length];
            i = 0;
            foreach (EquipmentSlotsType pieceType in Enum.GetValues(typeof(EquipmentSlotsType)))
            {
                classStuff.AllowedSlots[i] = pieceType;
                Debug.Log(pieceType);
                i++;
            }
        }
    }
}
#endif
