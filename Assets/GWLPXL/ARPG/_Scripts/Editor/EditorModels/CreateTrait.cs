using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.com
{
    public class CreateTrait: ScriptableObject
    {
        public TraitType type = TraitType.Stat;
        public string traitName = string.Empty;
    }
}