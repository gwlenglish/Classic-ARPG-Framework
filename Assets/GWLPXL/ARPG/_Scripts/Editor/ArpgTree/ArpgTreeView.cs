using System;
using System.Collections.Generic;
using System.Linq;
using GWLPXL.ARPG._Scripts.Editor.Data.com;
using GWLPXL.ARPG._Scripts.Editor.FuzzySharp;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GWLPXL.ARPG._Scripts.Editor.ArpgTree.com
{
    public class ArpgTreeView
    {
        public Vector2 scrollPosBind { get; set; }
        public HashSet<ArpgTreeItem> Visited = new HashSet<ArpgTreeItem>();
        public List<ArpgTreeItem> Childs = new List<ArpgTreeItem>();
        public List<ArpgTreeItem> Selected = new List<ArpgTreeItem>();
        
        private string searchString;
        private bool drawSearch;
        public List<ArpgTreeItem> searchItems = new List<ArpgTreeItem>();
        
        IEnumerable<ArpgTreeItem> EnumerateTree()
        {
            return Childs.SelectMany(x => x.EnumerateTree());
        }
        
        public void OnGUI()
        {
            DrawSearchField();
            scrollPosBind = EditorGUILayout.BeginScrollView(scrollPosBind, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
            var outerRect = EditorGUILayout.BeginVertical();
            GUILayout.Space(-1f);

            var drawItem = drawSearch ? searchItems : Childs;
            foreach (var item in drawItem)
                item.DrawItems(0);


            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawSearchField()
        {
            var rect = GUILayoutUtility.GetRect(0.0f, 17f);
            var term = EditorGUI.TextField(rect, searchString, ArpgStyle.ToolbarSearchTextField);
            if (term == searchString) return;
            searchString = term;
            searchItems.Clear();
            if (string.IsNullOrEmpty(searchString))
            {
                // reset search
                drawSearch = false;
            }
            else
            {
                // do search
                drawSearch = true;
                var searched = EnumerateTree().Select(s => new FuzzyContainer()
                {
                    Item = s, Score = Fuzz.PartialRatio(searchString, s.Label)
                })
                    .Where(w => w.Score > 40)
                    .OrderByDescending(o => o.Score)
                    .Select(s => s.Item);
                searchItems.AddRange(searched);
            }
        }

        public void Clear()
        {
            Childs.Clear();
            Selected.Clear();
            Visited.Clear();
        }

        public ArpgTreeItem Add<T>(string name, 
            T data,
            Func<object, Object> func = null,
            Action<ArpgTreeItem> onRightClick = null)
        {
            var item = new ArpgTreeItem();
            item.Tree = this;
            item.Label = name;
            item.DataContainer = data;
            item.DragAndDropObjectFunc = func;
            item.OnRightClick = onRightClick;
            Childs.Add(item);
            return item;
        }
        
        public ArpgTreeItem AddToItemAsChild<T>(
            ArpgTreeItem parent,
            T data, 
            string name,
            Func<object, Object> dndFunc = null,
            Action<ArpgTreeItem> onRightClick = null)
        {
            var child = new ArpgTreeItem();
            child.Tree = this;
            child.Label = name;
            child.DataContainer = data;
            child.DragAndDropObjectFunc = dndFunc;
            child.OnRightClick = onRightClick;
            parent.Childs.Add(child);
            return child;
        }
        public void AddToItemAsChildArray<T>(
            ArpgTreeItem parent,
            T[] toArray,
            Func<T, string> nameFunc,
            Func<object, Object> func = null,
            Action<ArpgTreeItem> onRightClick = null)
        {
            foreach (var obj in toArray)
            {
                var name = nameFunc(obj);
                AddToItemAsChild(parent, obj, name, func, onRightClick);
            }
        }

        public void TriggerSelect(ArpgTreeItem arpgTreeItem, bool multiselect = false)
        {
            if (!multiselect)
            {
                foreach (var old in Selected)
                {
                    old.IsSelected = false;
                }
                Selected.Clear();
            }

            if (multiselect && Selected.Contains(arpgTreeItem))
            {
                Selected.Remove(arpgTreeItem);
                arpgTreeItem.IsSelected = false;
            }
            else
            {
                //default behaviour
                Selected.Add(arpgTreeItem);
                arpgTreeItem.IsSelected = true;
            }

            Visited.Add(arpgTreeItem);
            GUI.FocusControl(null);
        }
    }
}