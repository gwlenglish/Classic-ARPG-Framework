namespace GWLPXL.ARPGCore.Types.com
{
    /// <summary>
    /// reserve 101-200 for ARPG, use others for custom
    /// </summary>
    public enum OtherAttributeType
    {
        None = 0,
        CriticalHitChance = 1,
        CriticalHitDamage = 2,
        /// <summary>
        /// Calculated between 0 - 100, 0 being none and 100 being immune (100% resistance)
        /// </summary>
        KnockBackResistance = 101

    }

}