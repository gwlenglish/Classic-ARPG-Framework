using System.Collections.Generic;
using System.Linq;
using GWLPXL.ARPG._Scripts.Editor.ArpgTree.com;
using GWLPXL.ARPG._Scripts.Editor.Data.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ReloadProcessors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.com
{
    public class ArpgEditorWindow : EditorWindow, IReloadGameDatabaseList
    {
        private const string LastDatabaseSavePref = "YM.LastDatabase";
        public static bool OnlyFirstSelectedLabelIsDraw = false;
        
        // TODO MAYBE REMOVE STATIC SAVE DATA HACK
        private static GameDatabase GameDatabase;
        private static ArpgTreeView _arpgTreeView = new ArpgTreeView();

        private ArpgObjectEditors Editors = new ArpgObjectEditors();
        private static List<GameDatabase> GameDatabaseStatic = new List<GameDatabase>();
        
        private static int _selectedDatabase = -1;
        private int SelectedDatabase
        {
            get => _selectedDatabase;
            set
            {
                if (_selectedDatabase == value) return;
                _selectedDatabase = value;
                SetDatabase(GameDatabaseStatic[_selectedDatabase]);
            }
        }

        public float MenuWidth = 210f;
        private Vector2 scrollPosition;
        private ArpgItemDataContainer CreateDatabaseContainer;
        
        
        public List<GameDatabase> GameDatabases { get => GameDatabaseStatic; set => GameDatabaseStatic = value; }
        

        public void SetDatabase(GameDatabase database)
        {
            EditorPrefs.SetString(LastDatabaseSavePref, database.name);
            GameDatabase = database;
            ReloadData();
        }

        public void ReloadData()
        {
            if (GameDatabase == null) return;
            if (_arpgTreeView == null) _arpgTreeView = new ArpgTreeView();
            CheckTreeViewAndClear();
            
            ArpgTreeData.Setup(this, _arpgTreeView, GameDatabase);
        }

        private void CheckTreeViewAndClear()
        {
            if (_arpgTreeView.Visited.Count > 0)
            {
                bool confirmSave = EditorUtility.DisplayDialog("Save Changes?", "Do you want to save before closing?", "Yes, save.", "No.");
                if (confirmSave)
                {
                    SaveAllVisitedItems();
                }
            }
            
            _arpgTreeView.Clear();
        }

        private void SaveAllVisitedItems()
        {
            var visited = _arpgTreeView.Visited;
            var saved = visited
                .Select(s => (ArpgItemDataContainer) s.DataContainer)
                .Where(w => w.EditType == YMObjectEditorType.Modify)
                .Select(s => s.Object)
                .OfType<ISaveJsonConfig>();
            ArpgEditorHelper.SaveAllJsonToDisk(saved.ToArray());
            
            _arpgTreeView.Visited.Clear();
        }

        void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            var menuRect = EditorGUILayout.BeginVertical(GUILayout.Width(MenuWidth), GUILayout.ExpandHeight(true));

            SelectedDatabase = EditorGUILayout.Popup(SelectedDatabase, GameDatabaseStatic.Select(s => s.name).ToArray());
            
            _arpgTreeView.OnGUI();
            
            var rect = menuRect;
            rect.xMin = menuRect.xMax;
            rect.xMax += 4f;
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
            MenuWidth += ArpgEditorHelper.SlideRect(rect).x;
            
            EditorGUILayout.EndVertical();
            
            var rectEditor = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            
            EditorGUI.DrawRect(rectEditor, ArpgStyle.ItemRootColor);


            var toolbarRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(30f), GUILayout.ExpandWidth(true));
            //EditorGUI.DrawRect(toolbarRect, Color.gray);
            
            string label = "";
            if (_arpgTreeView.Selected.Count > 0)
            {
                foreach (var selected in _arpgTreeView.Selected)
                {
                    label += ArpgEditorHelper.GetNameOfArpgObject(((ArpgItemDataContainer)selected.DataContainer).Object) + " | ";
                    if (OnlyFirstSelectedLabelIsDraw)
                        break;
                }

                label = label.Substring(0, label.Length - 2);
            }

            GUILayout.Label(label);

            if (_arpgTreeView.Selected.Count > 0 &&
                GUILayout.Button(new GUIContent("Ping Selected"), ArpgStyle.ToolbarButton))
            {
                var containerAsArpgType = (ArpgItemDataContainer)_arpgTreeView.Selected[0].DataContainer;
                EditorGUIUtility.PingObject(containerAsArpgType.Object);
            }

            if (GameDatabase != null && GUILayout.Button(new GUIContent("Reload DB"), ArpgStyle.ToolbarButton))
            {
                DatabaseHandler.ReloadAll(GameDatabase);
                ReloadData();
            }
            if (GUILayout.Button(new GUIContent("Reload UI"), ArpgStyle.ToolbarButton))
            {
                ReloadData();
            }
            if (GUILayout.Button(new GUIContent("Save All"), ArpgStyle.ToolbarButton))
            {
                SaveAllVisitedItems();
            }

            EditorGUILayout.EndHorizontal();

            //objects
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            if (_arpgTreeView.Selected.Count > 0)
            {
                // delete non exist objects
                for (int i = _arpgTreeView.Selected.Count - 1; i >= 0; i--)
                {
                    var selected = _arpgTreeView.Selected[i];
                    var containerAsArpgType = (ArpgItemDataContainer)selected.DataContainer;
                    if (containerAsArpgType.Object == null)
                    {
                        _arpgTreeView.Selected.Remove(selected);
                        _arpgTreeView.Visited.Remove(selected);
                    }
                }
                
                foreach (var selected in _arpgTreeView.Selected)
                {
                    if (selected.DataContainer == null) continue;
                    var containerAsArpgType = (ArpgItemDataContainer)selected.DataContainer;

                    var editor = Editors.GetCustomEditor(containerAsArpgType);
                    editor.OnInspectorGUI();
                    if (_arpgTreeView.Selected.Last() != selected)
                    {
                        EditorGUILayout.Space(10);
                        
                        var sepRect = GUILayoutUtility.GetRect(0.0f, 3f);
                        sepRect.x += ArpgStyle.BorderPadding;
                        sepRect.width -= ArpgStyle.BorderPadding * 2;
                        ArpgEditorHelper.DrawHorizontalSeparator(sepRect);
                        
                        EditorGUILayout.Space(10);
                    }
                }
            }
            
            // create database menu
            if (GameDatabase == null)
            {
                var editor = Editors.GetCustomEditor(CreateDatabaseContainer);
                editor.OnInspectorGUI();
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }


        private void OnEnable()
        {
            GameDatabaseStatic = ArpgEditorHelper.GetAllGameDatabasesInProject();

            if (GameDatabase == null && GameDatabaseStatic.Any())
            {
                if (EditorPrefs.HasKey(LastDatabaseSavePref))
                {
                    var key = EditorPrefs.GetString(LastDatabaseSavePref);
                    for (int i = 0; i < GameDatabaseStatic.Count; i++)
                    {
                        if (GameDatabaseStatic[i].name == key)
                        {
                            SelectedDatabase = i;
                            break;
                        }
                    }

                    // if last database not exists
                    if (SelectedDatabase == -1)
                        SelectedDatabase = 0;
                }
                else
                {
                    SelectedDatabase = 0;
                }
            }

            CreateDatabaseContainer = new ArpgItemDataContainer(CreateInstance<CreateGameDatabase>(), null,
                new ReloadGameDatabaseList(this), YMObjectEditorType.CreateNew, typeof(GameDatabase));
        }

        private void OnDestroy()
        {
            if (_arpgTreeView.Visited.Count > 0)
            {
                bool confirmSave = EditorUtility.DisplayDialog("Save Changes?", "Do you want to save before closing?", "Yes, save.", "No.");
                if (confirmSave)
                {
                    SaveAllVisitedItems();
                }
            }
        }
    }
}