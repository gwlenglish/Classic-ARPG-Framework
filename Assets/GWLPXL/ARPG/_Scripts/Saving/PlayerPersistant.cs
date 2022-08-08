using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Classes.com;

using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Items.com;

namespace GWLPXL.ARPGCore.Saving.com
{
    /// <summary>
    /// Eventually need to encapsulate this into just the player script instead of the parts. 
    /// </summary>
    [System.Serializable]
    public class PlayerPersistant
    {
        public int PlayerNumber;

        public ActorAttributes PersistantStats;
        public ActorInventory PersistantInventory;
        public AbilityController PersistantAbilities;
        public AuraController PersistantAuras;
        public ActorClass PersistantClass;
        public QuestLog PersistantQuestLog;
        public PlayerPersistant(int playernumber, ActorAttributes runtimecopy, ActorInventory invruntime, AbilityController abilityController, AuraController auras, ActorClass actorClass, QuestLog questLog)
        {

            PlayerNumber = playernumber;
            PersistantStats = runtimecopy;
            PersistantInventory = invruntime;
            PersistantAbilities = abilityController;
            PersistantClass = actorClass;
            PersistantQuestLog = questLog;
            PersistantAuras = auras;
        }
    }

}