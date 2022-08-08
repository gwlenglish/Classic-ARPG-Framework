using System;
using System.Collections.Generic;
using System.Linq;
using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.Data.com;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GWLPXL.ARPG._Scripts.Editor.ArpgTree.com
{
    public class ArpgTreeItem
    {
        public string Label;
        public ArpgTreeView Tree;
        public object DataContainer;
        public bool IsExpand;
        public bool IsSelected;
        public Rect TriangleRect;
        public List<ArpgTreeItem> Childs = new List<ArpgTreeItem>();

        public Func<object, Object> DragAndDropObjectFunc;
        public Action<ArpgTreeItem> OnRightClick;

        public void DrawItem(int indent)
        {
            var currentEvent = Event.current;
            
            var rect = GUILayoutUtility.GetRect(0.0f, 30f);
            if (indent == 0)
            {
                EditorGUI.DrawRect(rect, ArpgStyle.ItemRootColor);
            }
            if (IsSelected)
            {
                EditorGUI.DrawRect(rect, ArpgStyle.ItemSelectedColor);
            }

            if (!IsSelected && rect.Contains(currentEvent.mousePosition))
            {
                EditorGUI.DrawRect(rect, ArpgStyle.ItemMouseOverColor);
            }

            // draw triangle
            if (Childs.Count > 0)
            {
                TriangleRect = rect;
                TriangleRect.x += TriangleRect.width - ArpgStyle.ItemTriangleSize;
                TriangleRect.width = ArpgStyle.ItemTriangleSize;
                    
                //triangleRect.x -= this.Style.TrianglePadding;
                //EditorGUI.DrawRect(TriangleRect, Color.black);
                var guiColor = GUI.color;
                GUI.color = Color.clear;
                EditorGUI.DrawTextureTransparent(TriangleRect, IsExpand ? ArpgStyle.ItemTriangleExpandLight : ArpgStyle.ItemTriangleLight);
                GUI.color = guiColor;
            }
            
            //draw separator
            var sepRect = rect;
            sepRect.x += ArpgStyle.BorderPadding;
            sepRect.width -= ArpgStyle.BorderPadding * 2;
            ArpgEditorHelper.DrawHorizontalSeparator(sepRect);


            var labelRect = rect;
            labelRect.xMin += ArpgStyle.ItemOffset + indent * ArpgStyle.ItemIndentOffset;
            GUI.Label(labelRect, Label);
            
            //draw dnd zone
            var dndRect = rect;
            dndRect.width = ArpgStyle.ItemOffset;
            if (DragAndDropObjectFunc != null)
            {
                EditorGUI.DrawRect(dndRect, ArpgStyle.DragAndDropZoneColor);
            }
            
            
            // handle mouse click
            if (currentEvent.type != EventType.MouseDown && currentEvent.type != EventType.MouseUp) return;
            if (!rect.Contains(currentEvent.mousePosition)) return;
            if (currentEvent.button == 0)
            {
                if (currentEvent.type == EventType.MouseUp)
                {
                    if (Childs.Count > 0 && TriangleRect.Contains(currentEvent.mousePosition))
                    {
                        IsExpand = !IsExpand;
                    } 
                    else
                    {
                        if (currentEvent.modifiers == EventModifiers.Control)
                        {
                            Tree.TriggerSelect(this, true);
                        }
                        else if (currentEvent.modifiers == EventModifiers.None)
                        {
                            Tree.TriggerSelect(this);
                        }
                    }
                    Event.current.Use();
                }

                if (currentEvent.type == EventType.MouseDown)
                {
                    if (!dndRect.Contains(currentEvent.mousePosition)) return;
                    if (DragAndDropObjectFunc == null) return;
                    var dnd = DragAndDropObjectFunc(DataContainer);
                    
                    if (dnd == null) return;
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new[] {dnd};
                    DragAndDrop.StartDrag(Label);
                    Event.current.Use();
                }
            }

            // todo thinking about right click events
            if (currentEvent.button == 1 && OnRightClick != null)
            {
                OnRightClick(this);
                Event.current.Use();
            }
        }

        public void DrawItems(int indent)
        {
            DrawItem(indent);
            if (!IsExpand) return;
            for (var index = 0; index < Childs.Count; ++index)
                Childs[index].DrawItems(indent + 1);
        }

        public IEnumerable<ArpgTreeItem> EnumerateTree()
        {
            yield return this;
            foreach (var odinMenuItem2 in Childs.SelectMany(x => x.EnumerateTree()))
                yield return odinMenuItem2;
        }
    }
}