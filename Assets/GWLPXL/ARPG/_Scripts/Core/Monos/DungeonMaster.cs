using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Saving.com;

using GWLPXL.ARPGCore.Types.com;

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Items.com;
using System.Collections.Generic;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Factions.com;

namespace GWLPXL.ARPGCore.com
{
    [System.Serializable]
    public class LastScene
    {
        public string SceneName;
        public int Position;
    }

    [System.Serializable]
    public class CombatFormulaDefaults
    {
        public PlayerDefault GetPlayerCombatHandler()
        {
            if (playerDefaultCombatHandler == null)
            {
                playerDefaultCombatHandler = ScriptableObject.CreateInstance<PlayerDefault>();
            }
            return playerDefaultCombatHandler;
        }
        public EnemyDefault GetEnemyCombathandler()
        {
            if (enemyDefaultCombatHandler == null)
            {
                enemyDefaultCombatHandler = ScriptableObject.CreateInstance<EnemyDefault>();
            }
            return enemyDefaultCombatHandler;
        }

        public CombatFormulas GetCombatFormulas()
        {
            if (defaultCombatFormulas == null)
            {
                defaultCombatFormulas = ScriptableObject.CreateInstance<CombatFormulas>();
                defaultCombatFormulas.PlayerCombat = GetPlayerCombatHandler();
                defaultCombatFormulas.EnemyCombat = GetEnemyCombathandler();
            }
            return defaultCombatFormulas;
        }

        [SerializeField]
        PlayerDefault playerDefaultCombatHandler = null;
        [SerializeField]
        EnemyDefault enemyDefaultCombatHandler = null;
        [SerializeField]
        CombatFormulas defaultCombatFormulas = null;

        public CombatFormulaDefaults(PlayerDefault newPlayer, EnemyDefault newEnemy, CombatFormulas newFormulas)
        {
            playerDefaultCombatHandler = newPlayer;
            enemyDefaultCombatHandler = newEnemy;
            defaultCombatFormulas = newFormulas;
        }
    }

    /// <summary>
    /// TO DO, seperate out the loading logic into its own manager class
    /// </summary>
    public class DungeonMaster : MonoBehaviour
    {
        public GameObject GetLootPrefab() => lootPrefab;
        public Camera GetMainCamera()
        {
            if (main == null)
            {
                main = Camera.main;
            }
            return main;
        }

        Camera main;
        [SerializeField]
        [Tooltip("Simulates framerate. -1 removes the target frame rate cap, so no frame rate cap.")]
        int targetFramerate = -1;
        public DebugMessages Debug;

        [Tooltip("This is the project's default formulas. " +
            "This contains the formulas for the combat interactions. " +
            "You can inherit and override the formulas to create your own.")]
        public CombatFormulaDefaults CombatFormulas;
        public AffixReaderSO AffixReaderDefault;

        [SerializeField]
        protected GameObject loadingCanvasPrefab = null;
        [SerializeField]
        protected GameObject dungeonCanvasPrefab = null;
        [SerializeField]
        protected GameObject lootCanvasPrefab = null;
        [SerializeField]
        protected GameObject floatingTextCanvasPrefab = null;
        [SerializeField]
        protected GameObject lootPrefab = null;

        public string LoadingSceneName = string.Empty;
        public LastScene Last = null;
        public bool Loading { get; private set; }
        public DungeonVariables Variables { get; set; }

        public static DungeonMaster Instance => _instance;
        private static DungeonMaster _instance;
        public GameObject LoadingCanvas => _instanceCanvas;
        protected GameObject _instanceCanvas = null;



        protected PlayerPersistant[] playerPersistantData = new PlayerPersistant[0];
        protected PlayerSceneReference[] sceneReferences = new PlayerSceneReference[0];
        protected IDungeonUI dungeonUISceneRef = null;
        protected ILootCanvas lootCanvasRef = null;
        protected IFloatTextCanvas floatTextRef = null;
        protected List<GameObject> canvasObjs = new List<GameObject>();
        protected bool sequence = false;
        protected bool gameOver = false;

        [RuntimeInitializeOnLoadMethod]
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(_instance);

                Variables = GetComponent<DungeonVariables>();
            }

            SceneManager.sceneLoaded += CheckDungeonReferences;
            Application.targetFrameRate = targetFramerate;

            //SceneManager.sceneUnloaded += ClearCanvasReferences;

        }


        protected virtual void OnDestroy()
        {
            SceneManager.sceneLoaded -= CheckDungeonReferences;
        }

        #region scene refences
        protected virtual void ClearCanvasReferences(Scene scene)
        {
            for (int i = 0; i < canvasObjs.Count; i++)
            {
                Destroy(canvasObjs[i]);
            }
            canvasObjs.Clear();

            dungeonUISceneRef = null;
            lootCanvasRef = null;
            floatTextRef = null;
        }
        protected virtual void CheckDungeonReferences(Scene scene, LoadSceneMode mode)
        {
            ClearCanvasReferences(scene);
            GetDungeonUISceneRef();
            GetLootCanvas();
            GetFloatTextCanvas();
        }
        public virtual PlayerSceneReference[] GetAllSceneReferences()
        {
            return sceneReferences;
        }

        public virtual void GameOver()
        {
            gameOver = true;
           
        }
        public virtual void ReloadScene()
        {
            string activeScene = SceneManager.GetActiveScene().name;
            LoadNewDungeonScene(activeScene, false);
        }
        protected virtual void AddNewSceneRef(Player forPlayer, PlayerPersistant data)
        {
            bool alreadyadded = false;
            for (int i = 0; i < sceneReferences.Length; i++)
            {
                if (data == sceneReferences[i].PersistData)
                {
                    alreadyadded = true;
                    sceneReferences[i].SceneRef = forPlayer;
                    break;
                }
            }

            if (alreadyadded == false)
            {
                System.Array.Resize(ref sceneReferences, sceneReferences.Length + 1);
                sceneReferences[sceneReferences.Length - 1] = new PlayerSceneReference(forPlayer, data);
            }


        }

        /// <summary>
        /// to do, on get, find, if not find, make
        /// </summary>
        /// <returns></returns>
        public virtual IFloatTextCanvas GetFloatTextCanvas()
        {
            if (floatTextRef == null)
            {
                List<IFloatTextCanvas> floattextcanvas = GWLPXL.ARPGCore.Statics.com.FindInterfaces.FindAll<IFloatTextCanvas>();
                if (floattextcanvas.Count == 0)
                {
                    //make a new one. 
                    GameObject sceneInstance = Instantiate(floatingTextCanvasPrefab);
                    SceneManager.MoveGameObjectToScene(sceneInstance, SceneManager.GetActiveScene());
                    canvasObjs.Add(sceneInstance);
                    SetFloatingTextScene(sceneInstance.GetComponent<IFloatTextCanvas>());
                }
                else
                {
                    floatTextRef = floattextcanvas[0];//use first
                }
               
            }
            return floatTextRef;
        }
        public virtual ILootCanvas GetLootCanvas()
        {
            if (lootCanvasRef == null)
            {
                GameObject sceneInstance = Instantiate(lootCanvasPrefab);
                SceneManager.MoveGameObjectToScene(sceneInstance, SceneManager.GetActiveScene());
                canvasObjs.Add(sceneInstance);
                SetLotoSceneRef(sceneInstance.GetComponent<ILootCanvas>());
                
            }
            return lootCanvasRef;
        }
       

        public virtual IDungeonUI GetDungeonUISceneRef()
        {
            if (dungeonUISceneRef == null)
            {
                List<IDungeonUI> dungeonCanvas = GWLPXL.ARPGCore.Statics.com.FindInterfaces.FindAll<IDungeonUI>();
                if (dungeonCanvas.Count == 0)
                {
                    //make a new one. 
                    GameObject sceneInstance = Instantiate(dungeonCanvasPrefab);
                    SceneManager.MoveGameObjectToScene(sceneInstance, SceneManager.GetActiveScene());
                    canvasObjs.Add(sceneInstance);
                    SetDungeonUIScene(sceneInstance.GetComponent<IDungeonUI>());
                }
                else
                {
                    dungeonUISceneRef = dungeonCanvas[0];//use first
                }

                dungeonUISceneRef.InitalizeDungeon();
            }
  
            return dungeonUISceneRef;
        }
        public virtual void SetLotoSceneRef(ILootCanvas canvas)
        {
            lootCanvasRef = canvas;
        }
        public virtual void SetDungeonUIScene(IDungeonUI sceneReference)
        {
            dungeonUISceneRef = sceneReference;
            if (dungeonUISceneRef != null)
            {
                dungeonUISceneRef.InitalizeDungeon();
            }

        }
        public virtual void SetFloatingTextScene(IFloatTextCanvas scenereference)
        {
            floatTextRef = scenereference;
        }
       
        #endregion

        #region public persistant saving methods
        public PlayerPersistant[] GetPlayerPersist()
        {
            return playerPersistantData;
        }
        public PlayerPersistant GetPlayerPersist(int playerNumber)
        {
            for (int i = 0; i < playerPersistantData.Length; i++)
            {
                if (playerPersistantData[i].PlayerNumber == playerNumber)
                {
                    return playerPersistantData[i];
                }
            }
            return null;
        }
        public void SetPersistant(PlayerPersistant[] newPersist)
        {
            if (newPersist == null)
            {
                for (int i = 0; i < playerPersistantData.Length; i++)
                {
                    playerPersistantData[i] = null;
                }
                playerPersistantData = new PlayerPersistant[0];
                return;
            }

            playerPersistantData = newPersist;

        }
        //call into this to persist stats across scenes
        public bool DefineNewPlayer(Player player, int playerNumber)
        {
            for (int i = 0; i < playerPersistantData.Length; i++)
            {
                if (playerNumber == playerPersistantData[i].PlayerNumber)
                {
                    
                    return false;
                }
            }

            PlayerPersistant currentPersist = null;
            DontDestroyOnLoad(player.gameObject);
            AbilityController actorAbilities = null;
            AuraController actorAuras = null;
            ActorClass myClass = null;
            QuestLog questLog = null;
            ActorAttributes playerStats = Instantiate(player.MyStats.GetAttributeTemplate());
            ActorInventory playerInv = Instantiate(player.MyInventory.GetInvtemplate());

            IClassUser classUser = player.MyClass;
            if (classUser != null)
            {
                myClass = Instantiate(player.MyClass.GetMyClass());
            }

            IAbilityUser abilityUser = player.MyAbilities;
            if (abilityUser != null)
            {
                actorAbilities = Instantiate(abilityUser.GetTemplate());
                abilityUser.SetRuntimeController(actorAbilities);
            }
            IUseAura auraUser = player.MyAuraUser;
            if (auraUser != null)
            {
                actorAuras = Instantiate(auraUser.GetAuraControllerTemplate());
                auraUser.SetRuntimeAuraController(actorAuras);
            }
            IQuestUser questUser = player.MyQuests;
            if (questUser != null)
            {
                questLog = Instantiate(player.MyQuests.GetQuestLogTemplate());
                questUser.SetRuntimeQuestLog(questLog);
            }

            if (playerStats.MyLevel == 0)
            {
                playerStats.SetCurrentXP(0);
                playerStats.LevelUp(1);
            }

            playerInv.InitialSetup();
            playerInv.SetMyUser(player.MyInventory);
            playerInv.SetMyActorStats(player.MyStats);
            playerInv.EquipStarting();
            player.MyInventory.SetRuntimeInventory(playerInv);
            player.MyStats.SetRuntimeAttributes(playerStats);



            currentPersist = new PlayerPersistant(playerNumber, playerStats, playerInv, actorAbilities, actorAuras, myClass, questLog);
            System.Array.Resize(ref playerPersistantData, playerPersistantData.Length + 1);
            playerPersistantData[playerPersistantData.Length - 1] = currentPersist;
           

            AddNewSceneRef(player, currentPersist);
            return true;
        }
        #endregion

        #region load functions
        /// <summary>
        /// need to fix the loading progress bar.
        /// </summary>
        /// <param name="sceneToLoad"></param>
        /// <param name="waitForLoad"></param>
        public virtual void LoadNewDungeonScene(string sceneToLoad, bool waitForLoad)
        {
            if (Loading == false && sequence == false)
            {
                sequence = true;
                Scene currentScene = SceneManager.GetActiveScene();
                string currentSceneName = currentScene.name;
                StartCoroutine(LoadingSequence(sceneToLoad, currentSceneName, waitForLoad, waitForLoad));
                TickManager.Instance.Paused = true;
               
            }

            if (gameOver)
            {
                playerPersistantData = new PlayerPersistant[0];
                for (int i = 0; i < sceneReferences.Length; i++)
                {
                    //if i want to leave dead bodies, do that here
                    Destroy(sceneReferences[i].SceneRef.gameObject);
                }
                sceneReferences = new PlayerSceneReference[0];
                gameOver = false;
            }
            else
            {
                for (int i = 0; i < sceneReferences.Length; i++)
                {
                    sceneReferences[i].SceneRef.gameObject.SetActive(false);
                }
            }

        }

        protected virtual IEnumerator LoadingSequence(string sceneToLoad, string currentSceneName, bool isTrans, bool waitForLoad)
        {
            _instanceCanvas = Instantiate(loadingCanvasPrefab, Instance.transform);
            _instanceCanvas.transform.SetParent(Instance.transform);
            ILoadingCanvas iloading = LoadingCanvas.GetComponent<ILoadingCanvas>();
            iloading.EnableLoading(true);//lower the curtain

            StartCoroutine(Load(LoadingSceneName, currentSceneName, isTrans, waitForLoad));//unload currrent scene, load loading scene
            if (isTrans)
            {
                iloading.EnableLoadingBar(false);
            }
            else
            {
                iloading.EnableLoadingBar(true);

            }
            var fadeInTime = 0f;
            var fadeInDuration = .5f;
            while (fadeInTime < fadeInDuration && iloading != null)
            {
                fadeInTime += Time.deltaTime;
                var percent = fadeInTime / fadeInDuration;
                var lertp = Mathf.Lerp(0, 1, percent);
                iloading.SetTransitionEffects(lertp);
                yield return null;
            }
            yield return new WaitUntil(() => fadeInTime >= fadeInDuration);
            yield return new WaitUntil(() => Loading == false);
            StartCoroutine(Load(sceneToLoad, LoadingSceneName, isTrans, waitForLoad));//unload loading scene, load new scene
            if (isTrans)
            {
                iloading.EnableLoadingBar(false);
            }
            else
            {
                iloading.EnableLoadingBar(true);

            }
          //  iloading.EnableLoadingBar(false);
            fadeInTime = 0f;
            fadeInDuration = 2f;

            while (fadeInTime < fadeInDuration && iloading != null)
            {
                fadeInTime += Time.deltaTime;
                var percent = fadeInTime / fadeInDuration;
                var lertp = Mathf.Lerp(1, 0, percent);
                iloading.SetTransitionEffects(lertp);
                yield return null;
            }
            yield return new WaitUntil(() => fadeInTime >= fadeInDuration);

            for (int i = 0; i < sceneReferences.Length; i++)
            {
                sceneReferences[i].SceneRef.gameObject.SetActive(true);
            }

            yield return new WaitUntil(() => Loading == false);
            Destroy(_instanceCanvas);
            sequence = false;
            TickManager.Instance.Paused = false;
          

        }

       protected virtual IEnumerator Load(string sceneToLoad, string sceneToUnload, bool isTransition, bool waitForLoad)
        {

            Loading = true;
            while (Loading == true)
            {
                yield return null;
                if (waitForLoad)
                {
                    ISaveSystem saveSystem = GetComponent<ISaveSystem>();
                    if (saveSystem != null)
                    {
                        yield return new WaitUntil(() => saveSystem.IsSaving() == false);//wait until we saved it all
                    }
                }

                AsyncOperation newSCene = SceneManager.LoadSceneAsync(sceneToLoad);//, LoadSceneMode.Additive);//start loading the scene.//additive not working at the moment
                newSCene.allowSceneActivation = false;
                newSCene.allowSceneActivation = true;//activate scene
                //yield return null;
                yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneToLoad).isLoaded);//is it all loaded in?
                

                if (SceneManager.GetSceneByName(sceneToUnload).IsValid() && SceneManager.GetActiveScene().name != sceneToLoad)//unload scene
                {
                    AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToUnload);
                    yield return null;

                }

                Scene newScene = SceneManager.GetSceneByName(sceneToLoad);//make this scene current active scene
                SceneManager.SetActiveScene(newScene);

                yield return null;

                Loading = false;

            }


        }
        #endregion
    }


   
}