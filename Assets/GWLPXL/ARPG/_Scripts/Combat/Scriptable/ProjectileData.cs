using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Combat.com
{

    
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Combat/ProjectileData")]

    public class ProjectileData : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        [SerializeField]
        ProjectileDataID id = null;
        public ProjectileDataID GetID() => id;
        public void SetID(ProjectileDataID newID) => id = newID;

        public ProjectileOptions ProjectileVars = new ProjectileOptions("DEFAULT", "DEFAULT",35, true, 10, false);

        public Object GetObject() => this;


        public TextAsset GetTextAsset() => config;


        public void SetTextAsset(TextAsset textAsset) => config = textAsset;
        
    }
}