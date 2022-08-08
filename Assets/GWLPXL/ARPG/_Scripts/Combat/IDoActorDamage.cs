namespace GWLPXL.ARPGCore.Combat.com
{
    public interface IDoActorDamage
    {
        void SetDamageData(ActorDamageData newData);
        ActorDamageData GetActorDamageData();
        ActorDamageEvents GetActorDamageEvents();
    }


}