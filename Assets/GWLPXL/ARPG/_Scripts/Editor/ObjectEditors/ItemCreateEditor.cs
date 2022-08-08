using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class ItemCreateEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            var obj = (CreateItem)target;
            obj.type = (ItemType)EditorGUILayout.EnumPopup("Item type to create: ", obj.type);
            obj.itemName = EditorGUILayout.TextField("Item name: ", obj.itemName);

            switch (obj.type)
            {
                case ItemType.Equipment:
                    obj.eqType = (EquipmentType)EditorGUILayout.EnumPopup("Equipment type to create: ", obj.eqType);
                    break;
                case ItemType.Potions:
                    obj.potType = (PotionType)EditorGUILayout.EnumPopup("Potion type to create: ", obj.potType);
                    break;
                case ItemType.EquipmentSocketable:
                   // obj.socketType = (SocketTypes)EditorGUILayout.EnumPopup("Socket type to create: ", obj.potType);
                    break;

            }
            
            GUILayout.Space(25);
            
            if (GUILayout.Button("Create as New"))
            {
                GUI.FocusControl(null);
                
                Item temp = null;
                
                switch (obj.type)
                {
                    case ItemType.Equipment:
                        switch (obj.eqType)
                        {
                            case EquipmentType.Armor:
                                temp = CreateInstance<Armor>();
                                break;
                            case EquipmentType.Accessory:
                                temp = CreateInstance<Accessory>();
                                break;
                            case EquipmentType.Weapon:
                                temp = CreateInstance<Weapon>();
                                break;
                        }
                        break;
                    case ItemType.Potions:
                        switch (obj.potType)
                        {
                            case PotionType.RestoreResource:
                                temp = CreateInstance<RestoreResource>();
                                break;
                            case PotionType.ModifyStat:
                                temp = CreateInstance<ModifyStat>();
                                break;
                        }
                        break;
                    case ItemType.QuestItem:
                        temp = CreateInstance<QuestItem>();
                        break;
                    case ItemType.EquipmentSocketable:
                        temp = CreateInstance<EquipmentSocketable>();
                        EquipmentSocketable socketable = temp as EquipmentSocketable;
                        socketable.SocketableVariables = new SocketableVars(SocketTypes.Any, obj.itemName);
                        break;
                }
                
                if (temp == null)
                {
                    EditorUtility.DisplayDialog("Type Required", "A name and type is both required.", "Okay");
                    return;
                }
                
                temp.SetGeneratedItemName(obj.itemName);
                var asset = ArpgEditorHelper.TryCreateNew(obj.itemName, temp, AttachedContainer.SavePath);
                AttachedContainer.ReloadTrigger?.Reload();
                obj.itemName = string.Empty;
            }
        }
    }
}