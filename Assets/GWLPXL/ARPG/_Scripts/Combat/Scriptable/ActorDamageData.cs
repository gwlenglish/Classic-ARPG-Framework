using UnityEngine;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.Statics.com;
namespace GWLPXL.ARPGCore.Combat.com
{
    [CreateAssetMenu(menuName ="GWLPXL/ARPG/Combat/ActorDamageData")]
    public class ActorDamageData : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        [SerializeField]
        ActorDamageID id;
        public ActorDamageID GetID() => id;
        public string GetName() => this.DamageVar.Name;
        public void SetID(ActorDamageID newID) => id = newID;

        public DamageDealerForActor DamageVar = new DamageDealerForActor(
            "Default",
            null,
            new DamageOptions(true, true, false),
            new DamageMultiplers_Actor(),
            new StatusOverTimeOptions(new ModifyResourceVars[0], 1),
            new DamageOverTimeMultipliers());

        public Object GetObject() => this;


        public TextAsset GetTextAsset() => config;


        public void SetTextAsset(TextAsset textAsset) => config = textAsset;
       
    }


}