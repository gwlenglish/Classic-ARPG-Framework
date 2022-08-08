

using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.Mods.com
{
  /// <summary>
  /// abstract class for the abilitystatuschange. Inherit this to create more ability mods/changes
  /// </summary>
    public abstract class AbilityStatusChange : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        public abstract void ApplyStatus(IActorHub attacker);

        public abstract void RemoveStatus(IActorHub attacker);

        #region interface
        public Object GetObject()
        {
            return this;
        }

        public TextAsset GetTextAsset()
        {
            return config;
        }
        public void SetTextAsset(TextAsset textAsset)
        {
            config = textAsset;
        }
        #endregion
    }
}
