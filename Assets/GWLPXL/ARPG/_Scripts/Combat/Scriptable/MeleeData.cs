
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
namespace GWLPXL.ARPGCore.Combat.com
{



    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Combat/MeleeData")]

    public class MeleeData : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        [SerializeField]
        MeleeDataID id = null;
        public MeleeOptions MeleeVars = new MeleeOptions("Default", "Default",false);
        public MeleeDataID GetID() => id;
        public void SetID(MeleeDataID newID) => id = newID;
        public Object GetObject() => this;

        public TextAsset GetTextAsset() => config;


        public void SetTextAsset(TextAsset textAsset) => config = textAsset;
       
    }
}