using GWLPXL.ARPGCore.GameEvents.com;


namespace GWLPXL.ARPGCore.Statics.com
{
    //this might not be necessary anymore
    public static class GameEventHandler 
    {
        public static void RaiseExploreEvent(ExploreEvent gameEvent)
        {
            if (gameEvent == null) return;
            gameEvent.Raise();
        }
        public static void RaiseLevelUpEvent(LevelUpEvent gameEvent)
        {
            if (gameEvent == null) return;

            gameEvent.Raise();
        }
        public static void RaiseDeathEvent(DeathEvent gameEvent)
        {
            if (gameEvent == null) return;

            gameEvent.Raise();
        }
        public static void RaiseEquipEvent(EquipmentChangeEvent gameEvent)
        {
            if (gameEvent == null) return;

            gameEvent.Raise();
        }

        public static void RaisePlayerDamageEvent(TookDamageEvent gameEvent)
        {
            if (gameEvent == null) return;

            gameEvent.Raise();
        }

        public static void RaisePlayerDeathEvent(DeathEvent gameEvent)
        {
            if (gameEvent == null) return;

            gameEvent.Raise();
        }
    }
}
