using UnityEngine;


namespace GWLPXL.ARPGCore.Abilities.com
{
    /// <summary>
    /// used for a player class or something you want to use abilities
    /// </summary>
    [System.Serializable]
    public class AnimationClamp
    {
        public float MinValue;
        public float MaxValue;
        public AnimationCurve Curve;
        public AnimationClamp(float min, float max)
        {
            MinValue = min;
            MaxValue = max;
        }
        public float GetValue(float atPercent)
        {
            if (Curve == null)
            {
                return Mathf.Lerp(MinValue, MaxValue, atPercent);//linear
            }
            return Curve.Evaluate(Mathf.Lerp(MinValue, MaxValue, atPercent));
        }
    }
}
