
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{

    /// <summary>
    /// scene overrides for the dungeon static variables. 
    /// </summary>

    public class DungeonVariables : MonoBehaviour
    {
        [Header("Mobs/Enemies")]
        [Tooltip("Affects the Diminishing Returns value in the formulas. What this means, enemies with a higher mob multi will do more dmg. '1' is the normal value.")]
        public float MobLevelMultiplier = 1;
        [Tooltip("Affects any additional XP increases. 1 is the normal value")]
        public float MobXPMultiplier = 1;
        [Header("Items")]
        [Tooltip("Affects the ilevel drops. '20' is set here, but this is what you'll want to change to affect the overall loot drop traits. A higher ilevel means higher numbers in the traits.")]
        public float ItemLevelMultiplier = 20;
        //used to random range for equipment traits
        [Header("Item Drop Variance")]
        [Tooltip("For instnace, a .9f lower would receive 90% of the stat attributes, while a 1.1f would receive 110%. The range increases randomness of traits on drops")]
        public LootTraitVariance LootTraitVariance = new LootTraitVariance(.9f, 1.1f);
        [Header("Shops")]
        public float ShopLevelMultipler = 1;
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (LootTraitVariance.LowerRandomRange > LootTraitVariance.UpperRandomRange)
            {
                LootTraitVariance.LowerRandomRange = LootTraitVariance.UpperRandomRange;
            }
        }



#endif
    }


    #region helper classes



    [System.Serializable]
    public class LootTraitVariance
    {
        public float LowerRandomRange = .9f;
        public float UpperRandomRange = 1.1f;
        public LootTraitVariance(float lower, float upper)
        {
            LowerRandomRange = lower;
            UpperRandomRange = upper;
        }
    }


    #endregion
}
