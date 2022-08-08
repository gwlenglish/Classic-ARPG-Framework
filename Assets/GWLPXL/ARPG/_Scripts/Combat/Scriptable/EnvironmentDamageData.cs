using GWLPXL.ARPGCore.StatusEffects.com;

using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Combat.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Combat/EnvironmentDamageData")]

    public class EnvironmentDamageData : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        public DamageDealerForEnvironment DamageVars = new DamageDealerForEnvironment
            (
            new DamageOverTimeMultipliers(), 
            new StatusOverTimeOptions(new ModifyResourceVars[0], 1)
            );

        public Object GetObject() => this;


        public TextAsset GetTextAsset() => config;


        public void SetTextAsset(TextAsset textAsset) => config = textAsset;
       
    }
}