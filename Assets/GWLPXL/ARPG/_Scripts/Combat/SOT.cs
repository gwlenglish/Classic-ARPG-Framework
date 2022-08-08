using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.StatusEffects.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    /// <summary>
    /// this is really a SOT, status over time..., can be negative can be beneficial
    /// </summary>
    [System.Serializable]
    public class SOT
    {
       public IActorHub ActorHub { get; set; }
        public IReceiveDamage Attackable { get; set; }
        public IRecieveStatusChanges StatusChange { get; set; }
        public float Duration { get; set; }
        public SOT(IActorHub actorhub)
        {
            ActorHub = actorhub;
            Attackable = actorhub.MyHealth;
            Duration = 0;
            StatusChange = actorhub.MyStatusEffects;
        }
    }
}