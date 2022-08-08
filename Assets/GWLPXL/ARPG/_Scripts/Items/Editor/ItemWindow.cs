using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
using UnityEditor;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Traits.com;

namespace GWLPXL.ARPGCore.Items.com
{


    public class ItemWindow : ARPGDatabaseWindow
    {
        ItemDatabase database;
        ItemType type = ItemType.Equipment;
        string itemName = string.Empty;
        EquipmentType eqType = EquipmentType.Accessory;
        PotionType potType = PotionType.RestoreResource;

   
        public override void SetDatabase(Object database)
        {
            this.database = database as ItemDatabase;
            source = GetDatabase();
        }

        public override void ShowWindow()
        {
          //  EditorWindow.GetWindow(typeof(ItemDatabase));
    
        }

        protected override IDatabase GetDatabase()
        {
            return database as IDatabase;
        }

        protected override void TryDelete(Object focused)
        {
            base.TryDelete(focused);
            temp = null;
        }
        protected override void DrawCustomInspector()
        {
            focused = GetObject();
            if (focused != null)
            {
                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(focused);
                editor.DrawDefaultInspector();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                //draw a new one. 
                type = (ItemType)EditorGUILayout.EnumPopup("Item type to create: ", type);
                itemName = EditorGUILayout.TextField("Item name: :", itemName);

                switch (type)
                {
                    case ItemType.Equipment:
                        eqType = (EquipmentType)EditorGUILayout.EnumPopup("Equipment type to create: ", eqType);
                        break;
                    case ItemType.Potions:
                        potType = (PotionType)EditorGUILayout.EnumPopup("Equipment type to create: ", potType);
                        break;

                }
                EditorGUILayout.EndVertical();
            }
        }
        protected override void NewLayout()
        {
            GUILayout.Space(25);
            Debug.Log("TYPE " + type.ToString());
            if (GUILayout.Button("Create as New"))
            {
                string name = itemName;
                if (string.IsNullOrEmpty(name))
                {
                    EditorUtility.DisplayDialog("Name Required", "A name is required in order to create new", "Okay");
                    temp = null;
                    return;
                }

                switch (type)
                {
                    case ItemType.Equipment:
                        switch (eqType)
                        {
                            case EquipmentType.Armor:
                                temp = ScriptableObject.CreateInstance<Armor>();
                                break;
                            case EquipmentType.Accessory:
                                temp = ScriptableObject.CreateInstance<Accessory>();
                                break;
                            case EquipmentType.Weapon:
                                temp = ScriptableObject.CreateInstance<Weapon>();
                                break;
                        }
                        break;
                    case ItemType.Potions:
                        switch (potType)
                        {
                            case PotionType.RestoreResource:
                                temp = ScriptableObject.CreateInstance<RestoreResource>() as RestoreResource;
                                break;
                            case PotionType.ModifyStat:
                                temp = ScriptableObject.CreateInstance<ModifyStat>() as ModifyStat;
                                break;
                        }
                        break;
                    case ItemType.QuestItem:
                        temp = ScriptableObject.CreateInstance<QuestItem>();
                        break;
                    case ItemType.EquipmentSocketable:
                        temp = ScriptableObject.CreateInstance<EquipmentSocketable>();

                        break;
                }

                if (temp == null)
                {
                    EditorUtility.DisplayDialog("Type Required", "A name and type is both required.", "Okay");
                    temp = null;
                    return;
                }
                Item item = (Item)temp;
                item.SetGeneratedItemName(name);
                Object created = TryCreateNew(name, temp);
                ReloadDatabase();
               

                temp = null;
                itemName = string.Empty;
            }


            if (GUILayout.Button("Close"))
            {
                //close.
                CloseWindow();
            }
        }

      
        protected override void ReloadDatabase()
        {
            DatabaseHandler.ReloadDatabase(database);
            ReloadMessage();
        }

        protected override void MakeBlankCopy()
        {
            temp = null;
        }
    }
}