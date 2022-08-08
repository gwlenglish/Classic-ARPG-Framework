using System;
using System.Collections.Generic;
using GWLPXL.ARPGCore.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class TypeViewerEditorModel: ScriptableObject
    {
        public Dictionary<Type, bool> Foldout = new Dictionary<Type, bool>();
        public PingObject PingObject;
    }
}