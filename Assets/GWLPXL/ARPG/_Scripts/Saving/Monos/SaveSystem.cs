
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using GWLPXL.ARPGCore.Abilities.com;

namespace GWLPXL.ARPGCore.Saving.com
{
    public enum SaveFileType
    {
        /// <summary>
        /// plain text, easily readable and converted with normal json operations
        /// </summary>
        JsonText = 0,
        /// <summary>
        /// json text obfuscated somewhat behind binary
        /// </summary>
        JsonBinary = 1
    }
    //TO DO, split this god class
    /// <summary>
    /// 
    /// </summary>
    public class SaveSystem : MonoBehaviour, ISaveSystem
    {

        public string SaveFileName { get; set; }
        public bool SaveLoaded { get; set; }
        public bool Saving { get; set; }
        [SerializeField]
        GameDatabase gameDatabase = null;
        [SerializeField]
        string saveSubFolder;
        [SerializeField]
        [Tooltip("JsonText is preferred, but the deprecated json binary is kept if save files are using the older version.")]
        SaveFileType type = SaveFileType.JsonText;

        [Header("Debugging")]
        [SerializeField]
        [Tooltip("Used for debugging only, can see what was the last loaded save file.")]
        SaveFile loadedSaveFile = null;
        #region save functions

        //we can now just instantiate items
        public void LoadGame(string saveFileName)
        {
            if (!Directory.Exists(Application.persistentDataPath + saveSubFolder))
            {
                Directory.CreateDirectory(Application.persistentDataPath + saveSubFolder);
            }

            if (File.Exists(Application.persistentDataPath + saveSubFolder + "/" + saveFileName))
            {

                SaveLoaded = false;
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }

                SaveFile loadedSave = new SaveFile("", "", null, null);

                switch (type)
                {
                    case SaveFileType.JsonBinary:
                        //binary formatter method
                        FileStream file = File.Open(Application.persistentDataPath + saveSubFolder + "/" + saveFileName, FileMode.Open);
                        BinaryFormatter bf = new BinaryFormatter();
                        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), loadedSave);
                        file.Close();
                        break;
                    case SaveFileType.JsonText:
                        //Json method
                        string path = Application.persistentDataPath + saveSubFolder + "/" + saveFileName;
                        StreamReader reader = new StreamReader(path);
                        JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), loadedSave);
                        reader.Close();
                        break;
                }







                SceneSave sceneSave = loadedSave.SceneInfo;
                if (sceneSave != null)
                {
                    if (string.IsNullOrEmpty(sceneSave.Name) == false)
                    {


                        DungeonMaster.Instance.LoadNewDungeonScene(sceneSave.Name, true);
                    }
                    else
                    {
                        ARPGDebugger.DebugMessage("Save file can't find an apporpriate scene to load", this.gameObject);
                        DungeonMaster.Instance.LoadNewDungeonScene(SceneManager.GetActiveScene().name, true);
                    }


                }
                else
                {
                    ARPGDebugger.DebugMessage("Save file can't find an apporpriate scene to load", this.gameObject);
                    DungeonMaster.Instance.LoadNewDungeonScene(SceneManager.GetActiveScene().name, true);
                }


                //load the stuff.
                PlayerSave[] playerSave = loadedSave.PlayerInfo;
                DungeonMaster.Instance.SetPersistant(null);
                PlayerPersistant[] newPlayers = new PlayerPersistant[0];

                for (int i = 0; i < playerSave.Length; i++)
                {
                    PlayerSave currentPlayer = playerSave[i];

                    ActorInventory inv = Instantiate(gameDatabase.Inventories.FindInvByID(currentPlayer.DatabaseIDs.ActorInvID));
                    ActorAttributes actorStats = Instantiate(gameDatabase.Attributes.FindActorStatsByID(currentPlayer.DatabaseIDs.ActorStatsID));
                    ActorClass actorClass = Instantiate(gameDatabase.Classes.FindClassByID(currentPlayer.DatabaseIDs.ActorClassID));
                    AbilityController actorAbilityController = Instantiate(gameDatabase.AbilityControllers.FindAbilityControllerByID(currentPlayer.DatabaseIDs.AbilityControllerID));
                    AuraController actorAuras = Instantiate(gameDatabase.AuraControllers.FindAuraControllerByID(currentPlayer.DatabaseIDs.AuraControllerID));
                    QuestLog questlog = Instantiate(gameDatabase.QuestLog.FindQuestLogByID(currentPlayer.DatabaseIDs.QuestLogID));

                    inv.InitialSetup();
                    #region stat load
                    StatsSave[] stats = currentPlayer.Stats;
                    #endregion
                    #region load potions
                    LoadPotions(inv, currentPlayer);
                    #endregion
                    #region equipmentload

                    LoadEquipment(inv, currentPlayer);

                    #endregion

                    //need to load the skills

                    LoadAbilities(currentPlayer, actorAbilityController);

                    LoadAuras(currentPlayer, actorAuras);
                    //
                    LoadQuestchains(currentPlayer, questlog);
                    LoadQuests(currentPlayer, questlog);
                    LoadQuestKillTracking(currentPlayer, questlog);
                    LoadExploreAreaTracking(currentPlayer, questlog);

                    int savedLevel = currentPlayer.Progression.CurrentLevel;
                    int currentXP = currentPlayer.Progression.CurrentXP;
                    actorStats.LevelUp(savedLevel);
                    actorStats.SetCurrentXP(currentXP);
                    System.Array.Resize(ref newPlayers, newPlayers.Length + 1);
                    newPlayers[newPlayers.Length - 1] = new PlayerPersistant(playerSave[i].PlayerNumber, actorStats, inv, actorAbilityController, actorAuras, actorClass, questlog);

                    SaveSystem saveinfo = DungeonMaster.Instance.GetComponent<SaveSystem>();
                    if (saveinfo != null)
                    {
                        saveinfo.loadedSaveFile = this.loadedSaveFile;//to view it in the singleton
                    }


                }

                SaveLoaded = true;

                DungeonMaster.Instance.SetPersistant(newPlayers);

                //load it

            }

        }

        private void LoadExploreAreaTracking(PlayerSave currentPlayer, QuestLog questlog)
        {
            for (int i = 0; i < currentPlayer.QuestStatus.ExploreTracking.Length; i++)
            {
                SavedExploreTracking loadtracking = currentPlayer.QuestStatus.ExploreTracking[i];
                Quest quest = gameDatabase.Quests.FindQuestByID(loadtracking.QuestID);
                if (quest != null)
                {
                    for (int j = 0; j < loadtracking.ExploredAreas.Length; j++)
                    {
                        bool value = loadtracking.ExploredAreas[j];
                        //need a database...?
                        ExploreQuest explorequest = quest.Logic as ExploreQuest;

                        if (explorequest != null)
                        {
                            Dictionary<ExploreArea, bool> _newexplored = new Dictionary<ExploreArea, bool>();
                            ExploreQuestGoal[] goals = explorequest.Goals;

                            for (int k = 0; k < goals.Length; k++)
                            {
                                ExploreArea explorequestobj = goals[k].Area;
                                _newexplored[explorequestobj] = value;
                            }

                            questlog.QuestStats.ExploreAreasTracker[quest] = _newexplored;

                        }
                    }
                }
            }
        }
        private void LoadQuestKillTracking(PlayerSave currentPlayer, QuestLog questlog)
        {
            for (int i = 0; i < currentPlayer.QuestStatus.KilLTracking.Length; i++)
            {
                SavedKillTracking loadtracking = currentPlayer.QuestStatus.KilLTracking[i];
                Quest quest = gameDatabase.Quests.FindQuestByID(loadtracking.QuestID);
                if (quest != null)
                {
                    for (int j = 0; j < loadtracking.KilLGroups.Length; j++)
                    {
                        TrackedKillGroup killgroup = loadtracking.KilLGroups[j];
                        ActorAttributes actor = gameDatabase.Attributes.FindActorStatsByID(killgroup.ActorAttributeID);
                        int amount = killgroup.KillAmount;
                        if (actor != null)
                        {
                            Dictionary<ActorAttributes, int> newtracking = new Dictionary<ActorAttributes, int>();
                            newtracking[actor] = amount;
                            questlog.QuestStats.KillQuestsTracker[quest] = newtracking;
                        }
                    }
                }
            }
        }
        private void LoadQuestchains(PlayerSave currentPlayer, QuestLog questlog)
        {
            for (int j = 0; j < currentPlayer.QuestStatus.QuestChains.Length; j++)
            {
                Questchain quest = gameDatabase.Questchains.FindQuestByID(currentPlayer.QuestStatus.QuestChains[j].QuestchainID);
                questlog.ForceUpdateChain(quest, (QuestStatusType)currentPlayer.QuestStatus.QuestChains[j].StatusEnum);//potential for many things to go wrong...
            }
        }
        private void LoadQuests(PlayerSave currentPlayer, QuestLog questlog)
        {
            for (int j = 0; j < currentPlayer.QuestStatus.Quests.Length; j++)
            {
                Quest quest = gameDatabase.Quests.FindQuestByID(currentPlayer.QuestStatus.Quests[j].QuestID);
                questlog.ForceUpdateQuest(quest, (QuestStatusType)currentPlayer.QuestStatus.Quests[j].StatusEnum);//potential for many things to go wrong...
            }
        }

        private void LoadAuras(PlayerSave currentPlayer, AuraController actorauras)
        {
            List<AuraSave> skills = new List<AuraSave>();
            for (int j = 0; j < currentPlayer.PassiveSkills.Length; j++)
            {
                skills.Add(currentPlayer.PassiveSkills[j]);
            }

            skills.Sort((p1, p2) => p1.SkillSlot.CompareTo(p2.SkillSlot));//we sort them by their saved index

            for (int j = 0; j < skills.Count; j++)
            {
                Aura aura = gameDatabase.Auras.FindAuraByID(skills[j].DatabaseID);
                if (aura != null)
                {
                    Aura copy = Instantiate(aura);
                }
             
                //  actorauras.AddAura(copy);

            }
        }

        private void LoadAbilities(PlayerSave currentPlayer, AbilityController abilityController)
        {
            List<AbilitySave> abilities = new List<AbilitySave>();
            for (int j = 0; j < currentPlayer.Abilities.Length; j++)
            {
                abilities.Add(currentPlayer.Abilities[j]);
            }

            abilities.Sort((p1, p2) => p1.AbilitySlot.CompareTo(p2.AbilitySlot));//we sort them by their saved index

            for (int j = 0; j < abilities.Count; j++)
            {
                Ability abilityTemplate = Instantiate(gameDatabase.Abilities.FindAbilityByID(abilities[j].DatabaseID));
                abilityController.LearnAbility(abilityTemplate);

            }



        }

        public string GetSaveDirectory()
        {
            if (!Directory.Exists(Application.persistentDataPath + saveSubFolder))
            {
                Directory.CreateDirectory(Application.persistentDataPath + saveSubFolder);
            }
            return Application.persistentDataPath + saveSubFolder;
        }
        //we need the names saved now...
        private void LoadEquipment(ActorInventory inv, PlayerSave playerSave)
        {
            EquipmentSave[] equipment = playerSave.Equipment;
            List<Equipment> _loadedInInventory = new List<Equipment>();
            List<Equipment> _loadedEquipped = new List<Equipment>();
            //load it
            for (int i = 0; i < equipment.Length; i++)
            {
                Equipment template = gameDatabase.Items.FindEquipmentByID(equipment[i].DatabaseID);
                if (template == null)
                {
                    ARPGDebugger.DebugMessage("Cant load equipment from database", this);
                    continue;
                }
                Equipment equipmentInstance = Instantiate(template);

                bool isEquipped = equipment[i].Equipped;
                EquipmentTraitSave[] _traits = equipment[i].Traits;
                List<EquipmentTrait> nativeTraitList = new List<EquipmentTrait>();
                List<EquipmentTrait> randomTraitList = new List<EquipmentTrait>();

                for (int j = 0; j < _traits.Length; j++)
                {
                    EquipmentTraitSave toload = _traits[j];
                    bool isNative = toload.IsNative;
                    TraitType traitType = (TraitType)toload.TraitEnum;
                    int _multi = toload.TraitMulti;
                    EquipmentTrait trait = null;
                    EquipmentTrait archetype = gameDatabase.Traits.FindTraitByID(toload.DatabaseID);
                    trait = ScriptableObject.Instantiate(archetype);

                    if (trait != null)
                    {
                        int savedValue = _traits[j].SavedValue;
                        trait.SetStaticValue(savedValue);
                        if (isNative)
                        {
                            nativeTraitList.Add(trait);
                        }
                        else
                        {
                            randomTraitList.Add(trait);
                        }
                    }
                }

                ///added sockets, keep an eye on it newest edition.
                EquipmentSocketSave[] _sockets = equipment[i].Sockets;
                List<Socket> sockets = new List<Socket>();
                for (int j = 0; j < _sockets.Length; j++)
                {
                    EquipmentSocketSave _save = _sockets[j];
                    Socket socket = LoadSocket(_save);
                    if (socket != null)
                    {
                        sockets.Add(socket);
                    }

                }
                

                EquipmentStats _equipmentStats = equipment[i].Stats;
                _equipmentStats.SetRandomTraits(randomTraitList.ToArray());
                _equipmentStats.SetNativeTraits(nativeTraitList.ToArray());
                _equipmentStats.SetSockets(sockets.ToArray());

                EquipmentType type = (EquipmentType)equipment[i].TypeEnum;
                EquipmentSlotsType[] slots = new EquipmentSlotsType[equipment[i].SlotsEnums.Length];
                if (slots.Length > 0)
                {
                    for (int j = 0; j < slots.Length; j++)
                    {
                        slots[j] = (EquipmentSlotsType)equipment[i].SlotsEnums[j];
                    }
                }


                equipmentInstance.SetEquipmentSlots(slots);

                int iLevel = equipment[i].ILevel;
                _equipmentStats.SetiLevel(iLevel);

                bool canEnchant = equipment[i].CanEnchant;
                equipmentInstance.SetCanEnchant(canEnchant);

                equipmentInstance.SetStats(_equipmentStats);
                ItemID loadedID = equipment[i].ItemID;
                equipmentInstance.SetID(loadedID);


                //this isn't equipped it correctly.
                if (isEquipped)
                {
                    //equip on player
                    _loadedEquipped.Add(equipmentInstance);

                }
                else
                {
                    //add to inventory
                    _loadedInInventory.Add(equipmentInstance);
                }
            }

            for (int i = 0; i < _loadedInInventory.Count; i++)
            {
                inv.AddItemToInventory(_loadedInInventory[i]);
            }
            for (int i = 0; i < _loadedEquipped.Count; i++)
            {
                inv.Equip(_loadedEquipped[i]);
            }

        }

      
        private void LoadPotions(ActorInventory inv, PlayerSave playerSave)
        {
            if (playerSave.Potions != null && playerSave.Potions.Length > 0)
            {
                ItemStackingSave[] potions = playerSave.Potions;
                for (int i = 0; i < potions.Length; i++)
                {
                    int amount = potions[i].Amount;
                    Potion template = gameDatabase.Items.FindPotionByID(potions[i].DatabaseID) as Potion;
                    if (template == null)
                    {
                        Debug.LogError("Cant find potion in database with item ID " + potions[i].DatabaseID);
                        continue;
                    }
                    Potion potionInstance = Instantiate(template);
                    for (int j = 0; j < amount; j++)
                    {
                        inv.AddItemToInventory(potionInstance as Item);
                    }
                }
            }
        }

        Socket LoadSocket(EquipmentSocketSave _save)
        {
            SocketTypes socketype = (SocketTypes)_save.SocketType;
            SocketItem socketable = gameDatabase.Items.FindSocketableByID(_save.ItemID);
            if (socketable == null)
            {
                Debug.LogWarning("No Socketable Item found with ID of " + _save.ItemID + " Can't load.");
                return null;

            }
            EquipmentSocketable eqsocket = socketable as EquipmentSocketable;

            EquipmentSocketable eqsocketcopy = Instantiate(eqsocket);
            EquipmentTraitSave[] toload = _save.SocketedThings;
            List<EquipmentTrait> traits = new List<EquipmentTrait>();
            for (int i = 0; i < toload.Length; i++)
            {
                EquipmentTraitSave eqsave = toload[i];
                EquipmentTrait archetype = gameDatabase.Traits.FindTraitByID(eqsave.DatabaseID);
                if (archetype == null)
                {
                    Debug.LogWarning("No Trait found with ID of " + toload[i].DatabaseID + " Can't load.");
                    return null;
                }
                EquipmentTrait trait = ScriptableObject.Instantiate(archetype);
                int savedvalue = eqsave.SavedValue;
                trait.SetStaticValue(savedvalue);
                traits.Add(trait);
                
            }
            eqsocketcopy.EquipmentTraitSocketable = traits;
            Socket socket = new Socket(eqsocketcopy, socketype);
            return socket;
        }
        //currently saves items: potions, items: equipment, current scene info, stats, resources, databaseIDs, skills: active, skills: passive
        public void SaveGame(PlayerPersistant[] players)
        {
            if (string.IsNullOrEmpty(SaveFileName))
            {
                Debug.LogWarning("Need a name to create a save file");
                return;
            }
            Saving = true;
            if (!IsSaveFile())
            {
                Directory.CreateDirectory(Application.persistentDataPath + saveSubFolder);
            }

            if (!Directory.Exists(Application.persistentDataPath + saveSubFolder))
            {
                Directory.CreateDirectory(Application.persistentDataPath + saveSubFolder);
            }

            PlayerSave[] playerSaves = new PlayerSave[0];
            for (int i = 0; i < players.Length; i++)
            {
                ActorInventory savedInv = players[i].PersistantInventory;
                ActorAttributes savedStats = players[i].PersistantStats;
                AbilityController savedAbilities = players[i].PersistantAbilities;
                AuraController savedAuras = players[i].PersistantAuras;//will need to change to aura database
                ActorClass savedClass = players[i].PersistantClass;
                QuestLog savedQuestlog = players[i].PersistantQuestLog;

                List<AbilitySave> _tempAbilities = new List<AbilitySave>();
                List<AuraSave> _tempAuras = new List<AuraSave>();
                List<SavedQuests> _tempQuests = new List<SavedQuests>();
                List<SavedQuestchains> _tempQuestchains = new List<SavedQuestchains>();


                if (savedAbilities != null)
                {
                    Ability[] saved = savedAbilities.GetCopyAllLearned();
                    for (int j = 0; j < saved.Length; j++)
                    {
                        if (saved[j] == null) continue;//bug fix, 
                        Ability current = saved[j];
                        SaveAbility(_tempAbilities, j, current);
                    }


                }

                if (savedAuras != null)
                {
                    Aura[] savedA = savedAuras.GetCopyAllKnownAuras();
                    for (int j = 0; j < savedA.Length; j++)
                    {
                        if (savedA[j] == null) continue;//
                        Aura current = savedA[j];
                        SaveAura(_tempAuras, j, current);
                    }
                }

                //quests
                SavedQuests[] questarr = SaveQuestStatus(savedQuestlog, _tempQuests);
                SavedQuestchains[] questchainsarr = SaveQuestCHAINStatus(savedQuestlog, _tempQuestchains);
                SavedKillTracking[] killtrackingarr = SaveKilLTracking(savedQuestlog);
                SavedExploreTracking[] exploretracking = SaveExploreTracking(savedQuestlog);
                SavedQuestStatus queststatus = new SavedQuestStatus(questarr, questchainsarr, killtrackingarr, exploretracking);



                AbilitySave[] activeSkills = _tempAbilities.ToArray();
                AuraSave[] passiveSkills = _tempAuras.ToArray();

                #region save items
                List<EquipmentSave> _tempEquip = new List<EquipmentSave>();
                List<ItemStackingSave> _tempPotions = new List<ItemStackingSave>();
                if (savedInv != null)
                {
                    SaveItemsInInventory(savedInv, _tempEquip, _tempPotions);
                }
                #endregion
                EquipmentSave[] equipmentArr = _tempEquip.ToArray();
                ItemStackingSave[] potionsArr = _tempPotions.ToArray();

                #region stats...not used at the moment but meh

                List<StatsSave> _temp = new List<StatsSave>();
                List<ResourceSave> _tempResource = new List<ResourceSave>();
                if (savedStats != null)
                {
                    //Dictionary<AttributeType, Attribute[]> savedAttributes = savedStats.GetAllAttributes();
                    //foreach (var kvp in savedAttributes)
                    //{
                    //    //do a saved attribute
                    //}
                    ////create saved attribute class

                    //Dictionary<StatType, Stat> _stats = savedStats.GetRuntimeStatValues();
                    //foreach (var kvp in _stats)
                    //{
                    //    Stat stat = kvp.Value;
                    //    SaveStat(_temp, stat);
                    //}

                    //Dictionary<ResourceType, Resource> _resources = savedStats.GetRuntimeResourceValues();
                    //foreach (var kvp in _resources)
                    //{
                    //    Resource resource = kvp.Value;
                    //    SaveResource(_tempResource, resource);
                    //}

                }
                #endregion
                StatsSave[] statarr = _temp.ToArray();
                ResourceSave[] reArr = _tempResource.ToArray();



                //progression
                int playerNumber = players[i].PlayerNumber;
                int level = savedStats.MyLevel;
                int currentXP = savedStats.CurrentXP;
                SavedProgression progression = new SavedProgression(level, currentXP);

                int actorClassID = 0;
                int actorStatsID = 0;
                int actorInvID = 0;
                int abilityC = 0;
                int auraC = 0;
                int questl = 0;

                if (savedClass != null)
                {
                    actorClassID = savedClass.GetClassID().ID;

                }
                if (savedStats != null)
                {
                    actorStatsID = savedStats.GetStatsID().ID;
                }
                if (savedInv != null)
                {
                    actorInvID = savedInv.GetDatabaseID().ID;
                }
                if (savedAbilities != null)
                {
                    abilityC = savedAbilities.GetID().ID;
                }
                if (savedAuras != null)
                {
                    auraC = savedAuras.GetID().ID;
                }
                if (savedQuestlog != null)
                {
                    questl = savedQuestlog.GetID().ID;
                }

                MiscSavedDatabaseIDs databaseIds = new MiscSavedDatabaseIDs(actorClassID, actorStatsID, actorInvID, abilityC, auraC, questl);


                PlayerSave playerSave = new PlayerSave(playerNumber, progression, databaseIds, statarr, reArr, equipmentArr, potionsArr, activeSkills, passiveSkills, queststatus);//getting a bit out of hand
                playerSave.ScenePosition = DungeonMaster.Instance.Last.Position;

                System.Array.Resize(ref playerSaves, playerSaves.Length + 1);
                playerSaves[playerSaves.Length - 1] = playerSave;
            }

            #region save scene info
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            string path = SceneManager.GetActiveScene().path;
            string sceneName = SceneManager.GetActiveScene().name;
            #endregion
            SceneSave sceneSave = new SceneSave(buildIndex, path, sceneName);

            //save file
            SaveFile saveFile = new SaveFile(SaveFileName, System.DateTime.Today.ToShortDateString(), playerSaves, sceneSave);

            #region json serialize

            string saveTest = JsonUtility.ToJson(saveFile);

            switch (type)
            {
                case SaveFileType.JsonBinary:
                    //binary method
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Create(Application.persistentDataPath + saveSubFolder + "/" + SaveFileName);
                    this.loadedSaveFile = saveFile;
                    bf.Serialize(file, saveTest);
                    file.Close();
                    break;
                case SaveFileType.JsonText:
                    //plain txt method
                    FileStream stringfile = File.Create(Application.persistentDataPath + saveSubFolder + "/" + SaveFileName);
                    StreamWriter writer = new StreamWriter(stringfile);
                    writer.WriteLine(saveTest);
                    writer.Close();
                    stringfile.Close();
                    break;
            }



            #endregion


            //ARPGDebugger.DebugMessage("Saved");
            Saving = false;
        }

        private static SavedExploreTracking[] SaveExploreTracking(QuestLog savedQuestlog)
        {
            Dictionary<Quest, Dictionary<ExploreArea, bool>> explored = savedQuestlog.QuestStats.ExploreAreasTracker;
            List<SavedExploreTracking> _savedexplores = new List<SavedExploreTracking>();
            List<bool> _explored = new List<bool>();
            foreach (var kvp in explored)
            {
                _explored.Clear();
                int questid = kvp.Key.GetID().ID;

                foreach (var jvp in kvp.Value)
                {
                    //dont have database for explore area, so just going to use index
                    bool exploredvalue = jvp.Value;
                    _explored.Add(exploredvalue);
                }
                bool[] exploredarr = _explored.ToArray();
                SavedExploreTracking newexploretracking = new SavedExploreTracking(questid, exploredarr);
                _savedexplores.Add(newexploretracking);
            }

            SavedExploreTracking[] exploretracking = _savedexplores.ToArray();
            return exploretracking;
        }

        private static SavedKillTracking[] SaveKilLTracking(QuestLog savedQuestlog)
        {
            Dictionary<Quest, Dictionary<ActorAttributes, int>> kills = savedQuestlog.QuestStats.KillQuestsTracker;
            List<SavedKillTracking> _killtrackinglist = new List<SavedKillTracking>();
            List<TrackedKillGroup> _killgrouns = new List<TrackedKillGroup>();
            foreach (var kvp in kills)
            {
                _killgrouns.Clear();
                int questid = kvp.Key.GetID().ID;

                foreach (var jvp in kvp.Value)
                {
                    int actorID = jvp.Key.GetID().ID;
                    int killamount = jvp.Value;
                    TrackedKillGroup newtracking = new TrackedKillGroup(actorID, killamount);
                    _killgrouns.Add(newtracking);
                }
                TrackedKillGroup[] killarr = _killgrouns.ToArray();
                SavedKillTracking newquesttracking = new SavedKillTracking(questid, killarr);
                _killtrackinglist.Add(newquesttracking);
            }
            SavedKillTracking[] killtrackingarr = _killtrackinglist.ToArray();
            return killtrackingarr;
        }

        //to do, implement these saves for the tracking, add to quest status class
        [System.Serializable]
        public class TrackedKillGroup
        {
            public int ActorAttributeID;
            public int KillAmount;
            public TrackedKillGroup(int _actorattributeID, int _amount)
            {
                ActorAttributeID = _actorattributeID;
                KillAmount = _amount;
            }
        }
        [System.Serializable]
        public class SavedKillTracking
        {
            public int QuestID;
            public TrackedKillGroup[] KilLGroups = new TrackedKillGroup[0];
            public SavedKillTracking(int _questid, TrackedKillGroup[] _groups)
            {
                QuestID = _questid;
                KilLGroups = _groups;
            }
        }
        [System.Serializable]
        public class SavedExploreTracking
        {
            public int QuestID;
            public bool[] ExploredAreas;
            public SavedExploreTracking(int _questID, bool[] _exploredareas)
            {
                QuestID = _questID;
                ExploredAreas = _exploredareas;
            }
        }
        private SavedQuestchains[] SaveQuestCHAINStatus(QuestLog questLog, List<SavedQuestchains> questschains)
        {
            if (questLog == null) return questschains.ToArray();
            Questchain[] questsToSave = questLog.GetAllRuntimeQuestChains();
            for (int i = 0; i < questsToSave.Length; i++)
            {
                QuestState chainstate = questLog.GetQuestchainState(questsToSave[i]);
                SavedQuestchains newQuestSave = new SavedQuestchains(questsToSave[i].GetID().ID, (int)chainstate.State);
                questschains.Add(newQuestSave);

            }

            return questschains.ToArray();
        }

        private SavedQuests[] SaveQuestStatus(QuestLog questLog, List<SavedQuests> quests)
        {
            if (questLog == null) return quests.ToArray();
            Quest[] questsToSave = questLog.GetAllRuntimeQuests();
            for (int i = 0; i < questsToSave.Length; i++)
            {
                QuestState chainstate = questLog.GetRuntimeQuestState(questsToSave[i]);
                SavedQuests newQuestSave = new SavedQuests(questsToSave[i].GetID().ID, (int)chainstate.State);
                quests.Add(newQuestSave);

            }

            return quests.ToArray();
        }

        private void SaveItemsInInventory(ActorInventory savedInv, List<EquipmentSave> _tempEquip, List<ItemStackingSave> _tempPotions)
        {
            List<Item> itemsInInventory = savedInv.GetItemsInInventory();
            Dictionary<EquipmentSlotsType, EquipmentSlot> EquippedItems = savedInv.GetEquippedEquipment();
            foreach (var kvp in EquippedItems)
            {
                EquipmentSlot _equip = kvp.Value;
                Equipment _equipment = _equip.EquipmentInSlots;
                if (_equipment != null)
                {
                    SaveEquipmentPiece(_tempEquip, _equipment, true);
                }

            }


            for (int j = 0; j < itemsInInventory.Count; j++)
            {

                if (itemsInInventory[j] is Equipment)//equipments are unique with the traits
                {
                    Equipment equipment = (Equipment)itemsInInventory[j];
                    SaveEquipmentPiece(_tempEquip, equipment, false);

                }
                else
                {
                    //the rest can stack, quest items, potions, etc
                    if (itemsInInventory[j].IsStacking())
                    {
                        SaveStacking(_tempPotions, itemsInInventory[j], savedInv);

                    }
                }
              
            }
        }
        private void SaveAbility(List<AbilitySave> _tempSkills, int j, Ability current)
        {
            int ID = current.Data.UniqueID;
            int index = j;
            AbilitySave newSkillSave = new AbilitySave(ID, index);
            _tempSkills.Add(newSkillSave);
        }
        private void SaveAura(List<AuraSave> _tempSkills, int j, Aura current)
        {
            int ID = current.AuraData.uniqueID;
            int index = j;
            AuraSave newSkillSave = new AuraSave(ID, index);
            _tempSkills.Add(newSkillSave);
        }
        private void SaveResource(List<ResourceSave> _temp, Resource resource)
        {
            int enumValue = (int)resource.Type;
            int current = resource.ResourceNowValue;
            int starting = resource.Level1Value;
            int max = resource.NowValue;
            ResourceSave _newSave = new ResourceSave(enumValue, max, current, starting, resource);
            _temp.Add(_newSave);
        }
        private void SaveStat(List<StatsSave> _temp, Stat stat)
        {
            int enumValue = (int)stat.Type;
            int current = stat.NowValue;
            int starting = stat.Level1Value;
            StatsSave _newSave = new StatsSave(enumValue, current, starting, stat);
            _temp.Add(_newSave);
        }
        private void SaveStacking(List<ItemStackingSave> _tempPotions, Item item, ActorInventory inventory)
        {
            List<ItemStack> stacks = inventory.GetAllItemStacks(item);
            int amount = 0;
            for (int i = 0; i < stacks.Count; i++)
            {
                amount += stacks[i].CurrentStackSize;
            }
            ItemStackingSave save = new ItemStackingSave(item.GetID().ID, amount);
            _tempPotions.Add(save);
        }
        private void SaveEquipmentPiece(List<EquipmentSave> _tempEquip, Equipment equipment, bool isEquipped)
        {

            int type = (int)equipment.GetEquipmentType();
            Rarity rarity = equipment.GetRarity();
            ItemRarityType itemR = rarity.GetItemRarity();
            int itemRarityEnum = (int)itemR;


            EquipmentSlotsType[] _slots = equipment.GetEquipmentSlot();
            int[] slots = new int[_slots.Length];
            for (int i = 0; i < _slots.Length; i++)
            {
                slots[i] = (int)_slots[i];
            }
            EquipmentStats stats = equipment.GetStats();
            int _ilevel = equipment.GetStats().GetIlevel();

            List<EquipmentTraitSave> _trait = new List<EquipmentTraitSave>();
            EquipmentTrait[] native = stats.GetNativeTraits();
            for (int j = 0; j < native.Length; j++)
            {
                EquipmentTrait tosave = native[j];
                EquipmentTraitSave _newSave = GetTraitSave(tosave);
                _trait.Add(_newSave);
            }

            EquipmentTrait[] random = stats.GetRandomTraits();
            for (int j = 0; j < random.Length; j++)
            {
                EquipmentTrait tosave = random[j];
                EquipmentTraitSave _newSave = GetTraitSave(tosave);
                _trait.Add(_newSave);
            }

            List<EquipmentSocketSave> socketSaves = new List<EquipmentSocketSave>();
            List<Socket> sockets = stats.GetSockets();
            for (int i = 0; i < sockets.Count; i++)
            {
                //TO DO, need to save the item ID..
              
                EquipmentSocketable socketedthing = sockets[i].SocketedThing as EquipmentSocketable;
                if (socketedthing == null) continue;//not valid save
                EquipmentSocketable socketable = sockets[i].SocketedThing as EquipmentSocketable;
                if (socketable == null) continue;//not vlaid
                List<EquipmentTrait> sockettrait = socketedthing.EquipmentTraitSocketable;
                if (sockettrait == null || sockettrait.Count == 0) continue;//not valid

                int sockettype = (int)sockets[i].SocketType;
                int itemID = socketedthing.GetID().ID;
                EquipmentTraitSave[] eqsaves = new EquipmentTraitSave[sockettrait.Count];
                for (int k = 0; k < sockettrait.Count; k++)
                {
                    eqsaves[k] = GetTraitSave(sockettrait[i]);
                }
                EquipmentSocketSave socketsave = new EquipmentSocketSave(itemID, sockettype, eqsaves);
                socketSaves.Add(socketsave);

            }
            EquipmentSocketSave[] socketarr = socketSaves.ToArray();
            EquipmentTraitSave[] arr = _trait.ToArray();
            int databaseID = equipment.GetID().ID;
            ItemID id = equipment.GetID();
            bool canenchant = equipment.CanEnchant();
            EquipmentSave newSave = new EquipmentSave(databaseID, isEquipped, _ilevel, type, slots, itemRarityEnum, stats, arr, id, canenchant, socketarr);
            _tempEquip.Add(newSave);
        }

        private EquipmentTraitSave GetTraitSave(EquipmentTrait tosave)
        {
            int subEnum = GetSubEnum(tosave);
            float multi = tosave.GetMyLevelMulti();
            int intMulti = Mathf.RoundToInt(multi * Formulas.Hundred);
            TraitType traitType = tosave.GetTraitType();
            int savedValue = tosave.GetStaticValue();
            int traitID = tosave.GetID().ID;
            EquipmentTraitSave _newSave = new EquipmentTraitSave(traitID, false, intMulti, (int)traitType, subEnum, savedValue);
            return _newSave;
        }

        //skill mod currently not working.
        private int GetSubEnum(EquipmentTrait random)
        {
            int subEnum = 0;
            switch (random.GetTraitType())
            {
                case TraitType.ElementAttack:
                    ElementAttackTraitModifier attack = random as ElementAttackTraitModifier;
                    subEnum = (int)attack.Type;
                    break;
                case TraitType.ElementResist:
                    ElementResistTraitModifier resist = random as ElementResistTraitModifier;
                    subEnum = (int)resist.Type;
                    break;
                case TraitType.Resource:
                    MaxResourceTraitModifier resource = random as MaxResourceTraitModifier;
                    subEnum = (int)resource.Type;
                    break;
                case TraitType.AbilityMod:
                    AbilityModTraitModifier skillMod = random as AbilityModTraitModifier;
                    //subEnum = (int)attack.Type;//this needs to eb something entirely different?...in references teh actual skill, so need to find it at runtime.
                    break;
                case TraitType.Stat:
                    StatTraitModifier stat = random as StatTraitModifier;
                    subEnum = (int)stat.Type;
                    break;

            }

            return subEnum;
        }
        private bool IsSaveFile()
        {
            return Directory.Exists(Application.persistentDataPath + saveSubFolder);
        }

        public void SetSaveFileName(string newName)
        {
            SaveFileName = newName;
        }

        public string GetSaveFileName()
        {
            return SaveFileName;
        }

        public bool IsSaving()
        {
            return Saving;
        }

        #endregion


        #region helpers save stuff. Some save more than necessary for now. Meh.

        [System.Serializable]
        public class SaveFile
        {
            public string SaveFileName;
            public string SaveDateTime;
            public SceneSave SceneInfo;
            public PlayerSave[] PlayerInfo;
            public SaveFile(string name, string date, PlayerSave[] saveInfo, SceneSave _sceneInfo)
            {
                SaveFileName = name;
                SaveDateTime = date;
                PlayerInfo = saveInfo;
                SceneInfo = _sceneInfo;
            }
        }

        [System.Serializable]
        public class SceneSave
        {
            public int BuildIndex;
            public string Path;
            public string Name;

            public SceneSave(int sceneBuildIndex, string _path, string name)
            {
                BuildIndex = sceneBuildIndex;
                Path = _path;
                Name = name;

            }
        }

        [System.Serializable]
        public class SavedQuestStatus
        {
            public SavedQuests[] Quests;
            public SavedQuestchains[] QuestChains;
            public SavedKillTracking[] KilLTracking;
            public SavedExploreTracking[] ExploreTracking;
            public SavedQuestStatus(SavedQuests[] _quests, SavedQuestchains[] _questchains, SavedKillTracking[] _killtracking, SavedExploreTracking[] _exploretracking)
            {
                Quests = _quests;
                QuestChains = _questchains;
                KilLTracking = _killtracking;
                ExploreTracking = _exploretracking;
            }
        }
        [System.Serializable]
        public class PlayerSave
        {
            public int PlayerNumber;
            public int ScenePosition;
            public SavedProgression Progression;
            public MiscSavedDatabaseIDs DatabaseIDs;
            public StatsSave[] Stats;
            public ResourceSave[] Resources;
            public EquipmentSave[] Equipment;
            public ItemStackingSave[] Potions;
            public AbilitySave[] Abilities;
            public AuraSave[] PassiveSkills;
            public SavedQuestStatus QuestStatus;

            public PlayerSave(int playerNumber,
                SavedProgression _progression,
                MiscSavedDatabaseIDs ids,
                StatsSave[] _stats,
                ResourceSave[] _resources,
                EquipmentSave[] _equipment,
                ItemStackingSave[] _potions,
                AbilitySave[] _abilities,
                AuraSave[] _auras,
                SavedQuestStatus _queststatus)
            {
                PlayerNumber = playerNumber;
                Progression = _progression;
                DatabaseIDs = ids;

                Stats = _stats;
                Resources = _resources;
                Equipment = _equipment;

                Potions = _potions;
                Abilities = _abilities;
                PassiveSkills = _auras;
                QuestStatus = _queststatus;

            }

        }

        [System.Serializable]
        public class SavedQuests
        {
            public int QuestID;
            public int StatusEnum;
            public SavedQuests(int questID, int statusEnum)
            {
                QuestID = questID;
                StatusEnum = statusEnum;
            }
        }
        [System.Serializable]
        public class SavedQuestchains
        {
            public int QuestchainID;
            public int StatusEnum;
            public SavedQuestchains(int questID, int statusEnum)
            {
                QuestchainID = questID;
                StatusEnum = statusEnum;
            }
        }

        [System.Serializable]
        public class SavedProgression
        {
            public int CurrentLevel;
            public int CurrentXP;
            public SavedProgression(int currentlevel, int currentxp)
            {
                CurrentLevel = currentlevel;
                CurrentXP = currentxp;
            }
        }
        [System.Serializable]
        public class MiscSavedDatabaseIDs
        {
            public int ActorClassID;
            public int ActorStatsID;
            public int ActorInvID;
            public int AbilityControllerID;
            public int AuraControllerID;
            public int QuestLogID;

            public MiscSavedDatabaseIDs(int classID, int statsID, int invID, int abilityC, int auraC, int questlog)
            {
                ActorClassID = classID;
                ActorStatsID = statsID;
                ActorInvID = invID;
                AbilityControllerID = abilityC;
                AuraControllerID = auraC;
                QuestLogID = questlog;
            }
        }


        [System.Serializable]
        public class ResourceSave
        {
            public Resource Resource;
            public int ResourceEnum;
            public int CurrentValue;
            public int MaxValue;
            public int StartingValue;
            public ResourceSave(int _enum, int maxValue, int currentValue, int baseValue, Resource resource)
            {
                ResourceEnum = _enum;
                CurrentValue = currentValue;
                MaxValue = maxValue;
                StartingValue = baseValue;
                Resource = resource;
            }
        }
        [System.Serializable]
        public class StatsSave
        {
            public Stat Stat;
            public int StatEnum;
            public int CurrentValue;
            public int StartingValue;
            public StatsSave(int _enum, int currentValue, int baseValue, Stat stat)
            {
                StatEnum = _enum;
                CurrentValue = currentValue;
                StartingValue = baseValue;
                Stat = stat;
            }
        }
        [System.Serializable]
        public class EquipmentTraitSave
        {
            public int DatabaseID;
            public bool IsNative;
            public int TraitMulti;
            public int TraitEnum;
            public int SubEnum;
            public int SavedValue;
            public EquipmentTraitSave(int databasebaseID, bool isNative, int traitMulti, int traitEnum, int subEnum, int savedValue)
            {
                DatabaseID = databasebaseID;
                IsNative = isNative;
                TraitMulti = traitMulti;
                TraitEnum = traitEnum;
                SubEnum = subEnum;
                SavedValue = savedValue;

            }
        }

        [System.Serializable]
        public class ItemStackingSave
        {
            public int DatabaseID;
            public int Amount;
            public ItemStackingSave(int databaseID, int amount)
            {
                DatabaseID = databaseID;
                Amount = amount;

            }
        }

        [System.Serializable]
        public class EquipmentSocketSave
        {
            public int ItemID;
            public int SocketType;
            public EquipmentTraitSave[] SocketedThings;
            public EquipmentSocketSave(int itemID, int type, EquipmentTraitSave[] traitsave)
            {
                SocketType = type;
                SocketedThings = traitsave;
            }

        }

        [System.Serializable]
        public class AuraSave
        {
            public int DatabaseID;
            public int SkillSlot;

            public AuraSave(int databaseID, int skillSlot)
            {
                DatabaseID = databaseID;
                SkillSlot = skillSlot;

            }
        }

        [System.Serializable]
        public class AbilitySave
        {
            public int DatabaseID;
            public int AbilitySlot;
            public AbilitySave(int databaseID, int abilitySlot)
            {
                DatabaseID = databaseID;
                AbilitySlot = abilitySlot;

            }
        }

        [System.Serializable]
        public class EquipmentSave
        {
            public int DatabaseID;
            public bool Equipped;
            public bool CanEnchant;
            public int ILevel;
            public int TypeEnum;
            public int[] SlotsEnums;
            public int ItemRarityEnum;
            public ItemID ItemID;
            public EquipmentStats Stats;
            public EquipmentTraitSave[] Traits;
            public EquipmentSocketSave[] Sockets;
            public EquipmentSave(int databaseID, bool isEquipped, int iLevel, int type, int[] slots, int itemRarity, EquipmentStats stats, EquipmentTraitSave[] traits, ItemID itemID, bool canEnchant, EquipmentSocketSave[] sockets)
            {
                DatabaseID = databaseID;
                Equipped = isEquipped;
                ILevel = iLevel;
                TypeEnum = type;
                SlotsEnums = slots;
                ItemRarityEnum = itemRarity;
                Stats = stats;
                Traits = traits;
                ItemID = itemID;
                CanEnchant = canEnchant;
                Sockets = sockets;

            }
        }
    }
}
#endregion
