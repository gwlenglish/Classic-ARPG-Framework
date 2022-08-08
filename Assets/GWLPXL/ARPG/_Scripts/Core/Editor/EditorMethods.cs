using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Classes.com;
using System.Text;
using GWLPXL.ARPGCore.Combat.com;

namespace GWLPXL.ARPGCore.com
{

    public static class EditorExtensions 
    {
        public static bool IsParsable<T>(this string value) where T : struct
        {
            return System.Enum.TryParse<T>(value, true, out _);
        }
    }

    public class EditorMethods : UnityEditor.Editor
    {
        const string extension = ".cs";

        //public static void Go<T>()
        //{
        //    string enumName = "MyEnum";
        //    string[] enumEntries = { "Foo", "Goo", "Hoo" };
        //    string filePathAndName = AssetDatabase.GetAssetPath()

        //    using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        //    {
        //        streamWriter.WriteLine("public enum " + enumName);
        //        streamWriter.WriteLine("{");
        //        for (int i = 0; i < enumEntries.Length; i++)
        //        {
        //            streamWriter.WriteLine("\t" + enumEntries[i] + ",");
        //        }
        //        streamWriter.WriteLine("}");
        //    }
        //    AssetDatabase.Refresh();
        //}

        public static void CreateLayer(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentNullException("name", "New layer name string is either null or empty.");

            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layerProps = tagManager.FindProperty("layers");
            var propCount = layerProps.arraySize;

            SerializedProperty firstEmptyProp = null;

            for (var i = 23; i < propCount; i++)
            {
                var layerProp = layerProps.GetArrayElementAtIndex(i);

                var stringValue = layerProp.stringValue;

                if (stringValue == name) return;

                if (i < 8 || stringValue != string.Empty) continue;

                if (firstEmptyProp == null)
                    firstEmptyProp = layerProp;
            }

            if (firstEmptyProp == null)
            {
                UnityEngine.Debug.LogError("Maximum limit of " + propCount + " layers exceeded. Layer \"" + name + "\" not created.");
                return;
            }

            firstEmptyProp.stringValue = name;
            tagManager.ApplyModifiedProperties();
        }

        public static void WriteToEnum<T>(string path, string name, ICollection<T> data)
        {
            using (StreamWriter file = File.CreateText(path + name + extension))
            {
                
                file.WriteLine("public enum " + name + " \n{");

                int i = 0;
                foreach (var line in data)
                {
                    string lineRep = line.ToString().Replace(" ", string.Empty);
                    if (!string.IsNullOrEmpty(lineRep))
                    {
                        file.WriteLine(string.Format("\t{0} = {1},",
                            lineRep, i));
                        i++;
                    }
                }

                file.WriteLine("\n}");
            }

            AssetDatabase.ImportAsset(path + name + extension);
        }


       

        public static void OpenDatabaseWindow(IDatabase database)
        {
            switch (database.GetDatabaseEntry())
            {
                case DatabaseID.GameDatabase:
                    GameDatabaseWindow gamewindow = (GameDatabaseWindow)EditorWindow.GetWindow(typeof(GameDatabaseWindow));
                    gamewindow.SetDatabase(database.GetMyObject());
                    gamewindow.Show();
                    break;
                case DatabaseID.Abilities:
                    AbilityWindow window = (AbilityWindow)EditorWindow.GetWindow(typeof(AbilityWindow));
                    window.SetDatabase(database.GetMyObject());
                    window.Show();
                    break;
                case DatabaseID.AbilityControllers:
                    AbilityControllerWindow acwindow = (AbilityControllerWindow)EditorWindow.GetWindow(typeof(AbilityControllerWindow));
                    acwindow.SetDatabase(database.GetMyObject());
                    acwindow.Show();
                    break;
                case DatabaseID.ActorDamageDealers:
                    ActorDamageWindow damagers = (ActorDamageWindow)EditorWindow.GetWindow(typeof(ActorDamageWindow));
                    damagers.SetDatabase(database.GetMyObject());
                    damagers.Show();
                    break;
                case DatabaseID.Attributes:
                    AttributesWindow atwindow = (AttributesWindow)EditorWindow.GetWindow(typeof(AttributesWindow));
                    atwindow.SetDatabase(database.GetMyObject());
                    atwindow.Show();
                    break;
                case DatabaseID.AuraControllers:
                    AuraControllerWindow au = (AuraControllerWindow)EditorWindow.GetWindow(typeof(AuraControllerWindow));
                    au.SetDatabase(database.GetMyObject());
                    au.Show();
                    break;
                case DatabaseID.Auras:
                    AuraWindow aura = (AuraWindow)EditorWindow.GetWindow(typeof(AuraWindow));
                    aura.SetDatabase(database.GetMyObject());
                    aura.Show();
                    break;
                case DatabaseID.Classes:
                    ClassWindow classes = (ClassWindow)EditorWindow.GetWindow(typeof(ClassWindow));
                    classes.SetDatabase(database.GetMyObject());
                    classes.Show();
                    break;
                case DatabaseID.EquipmentTraits:
                    TraitWindow traits = (TraitWindow)EditorWindow.GetWindow(typeof(TraitWindow));
                    traits.SetDatabase(database.GetMyObject());
                    traits.Show();
                    break;
                case DatabaseID.Inventories:
                    InventoryWindow inv = (InventoryWindow)EditorWindow.GetWindow(typeof(InventoryWindow));
                    inv.SetDatabase(database.GetMyObject());
                    inv.Show();
                    break;
                case DatabaseID.Items:
                    ItemWindow item = (ItemWindow)EditorWindow.GetWindow(typeof(ItemWindow));
                    item.SetDatabase(database.GetMyObject());
                    item.Show();
                    break;
                case DatabaseID.LootDrops:
                    LootWindow loot = (LootWindow)EditorWindow.GetWindow(typeof(LootWindow));
                    loot.SetDatabase(database.GetMyObject());
                    loot.Show();
                    break;
                case DatabaseID.Melees:
                    MeleeDataWindow meleedata = (MeleeDataWindow)EditorWindow.GetWindow(typeof(MeleeDataWindow));
                    meleedata.SetDatabase(database.GetMyObject());
                    meleedata.Show();
                    break;
                case DatabaseID.Projectiles:
                    ProjectileDataWindow projectile = (ProjectileDataWindow)EditorWindow.GetWindow(typeof(ProjectileDataWindow));
                    projectile.SetDatabase(database.GetMyObject());
                    projectile.Show();
                    break;
                case DatabaseID.Questchains:
                    QuestChainWindow questchain = (QuestChainWindow)EditorWindow.GetWindow(typeof(QuestChainWindow));
                    questchain.SetDatabase(database.GetMyObject());
                    questchain.Show();
                    break;
                case DatabaseID.QuestLogs:
                    QuestLogWindow log = (QuestLogWindow)EditorWindow.GetWindow(typeof(QuestLogWindow));
                    log.SetDatabase(database.GetMyObject());
                    log.Show();
                    break;
                case DatabaseID.Quests:
                    QuestWindow quests = (QuestWindow)EditorWindow.GetWindow(typeof(QuestWindow));
                    quests.SetDatabase(database.GetMyObject());
                    quests.Show();
                    break;
       
            }


        }

    }
}