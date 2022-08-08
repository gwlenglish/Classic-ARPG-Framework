
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.com;
using UnityEditor;
using UnityEngine.AI;
using GWLPXL.ARPGCore.Movement.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Wearables.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Portals.com;
using GWLPXL.ARPGCore.Shopping.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.ProgressTree.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.Animations.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.DebugHelpers.com;

using System.Text;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.States.com;
using GWLPXL.ARPGCore.Factions.com;
using GWLPXL.ARPGCore.AI.com;

namespace GWLPXL.ARPGCore.Creation.com
{
    /// <summary>
    /// static class and helpers used in the creation of actors via the game database
    /// </summary>
    public static class CreatorHelpers
    {
        public enum TriggerType
        {
            Capsule = 0,
            Box = 1
        }
        public static void Setup2DTriggers(GameObject newPlayerObj)
        {
            //2d
            BoxCollider2D box2d = newPlayerObj.AddComponent<BoxCollider2D>();
            box2d.isTrigger = true;
        }

        public static void Setup3DTriggers(GameObject newPlayerObj, TriggerType type)
        {
            switch (type)
            {
                case TriggerType.Capsule:
                    CapsuleCollider cap = newPlayerObj.AddComponent<CapsuleCollider>();
                    cap.center = new Vector3(0, 1, 0);//just defaults guessing for characters
                    cap.height = 2f;
                    cap.isTrigger = true;
                    break;
                case TriggerType.Box:
                    BoxCollider collider = newPlayerObj.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                    break;
            }
          
        }


        public static List<GameObject> FindPrefabs(string[] folders)
        {
            string key = "gameobject";
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + key, folders);//specific if you want by putting t:armor or t:equipment, etc.
            List<GameObject> temp = new List<GameObject>();
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                GameObject newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(GameObject)) as GameObject;
                if (newItem != null)
                {
                    temp.Add(newItem);
                    //  Debug.Log("Added");
                }
            }

            return temp;
        }
        public static string GetPathFolder(string pathtosplit)
        {
            string[] splitpath = pathtosplit.Split('/');//split
            splitpath[splitpath.Length - 1] = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < splitpath.Length; i++)
            {
                sb.Append(splitpath[i]);
                if (i != splitpath.Length - 1)
                {
                    sb.Append("/");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// returns child
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="childname"></param>
        /// <returns></returns>
        public static GameObject AssignParentandName(GameObject parent, string childname)
        {
            GameObject child = new GameObject();
            child.name = childname;
            child.transform.SetParent(parent.transform);
            return child;

        }
        public static void AssignAttackableLayer(GameDatabase gamedatabase, GameObject newPlayerObj)
        {
            if (gamedatabase.Settings != null)
            {
                LayerMask mask = gamedatabase.Settings.LayerAssign.AttackableLayer;
                int layer = (int)Mathf.Log(mask.value, 2);
                newPlayerObj.layer = layer;
            }
        }
        public static void AssignInteractbleLayer(GameDatabase gamedatabase, GameObject newPlayerObj)
        {
            if (gamedatabase.Settings != null)
            {
                LayerMask mask = gamedatabase.Settings.LayerAssign.InteractableLayer;
                int layer = (int)Mathf.Log(mask.value, 2);
                newPlayerObj.layer = layer;
            }
        }

    }

    [System.Serializable]
    public class PlayerOptions
    {
       
        //player options
        //so's
        public int Attributes = 0;//indexed to the appropriate database
        public int Inventory = 0;
        public int Class = 0;
        public int AbilityController = 0;
        public int QuestLog = 0;
        public int AuraController = 0;
        public int Mover2DType = 0;
        public int Mover3DType = 0;
        public int Interact2DType = 0;
        public int Interact3DType = 0;
        //bools
        public bool ModifyExisting = false;
        public bool UseBuiltInMoving = true;
        public bool UseBuiltInInteraction = true;
        public bool UseNavMover = true;
        public bool UseBuiltInCanvases = true;
        public bool UseAuras = true;
        public bool ReceiveAuras = true;
        public bool UseQuests = true;
        public bool UseShopping = true;
        public bool UseCombat = true;
        public bool UseLeveling = true;
        public bool CanTakeDamage = true;
        public bool CanTakeStatusEffects = true;
        public bool UseAnimatorController = true;
        public bool UseFactions = true;
        public bool UseEnchanter = true;
        public bool UseSocketSmith = true;
        public GameObject CharacterMesh = null;
        public GameObject PrefabToModify = null;
    }
   
    
    public static class ActorDefinitions
    {
        public static UnityEngine.Object CreateNewPlayerPrefab(string path, PlayerOptions options, GameDatabase gamedatabase)
        {
            if (gamedatabase.Settings == null)
            {
                Debug.LogError("Need a settings object in the database in order to create");
                return null;
            }
            GameObject newPlayerObj = null;
            if (options.ModifyExisting)
            {
                newPlayerObj = GameObject.Instantiate(options.PrefabToModify);
            }
            else
            {
                newPlayerObj = new GameObject();
            }

            EditorPhysicsType type = gamedatabase.Settings.Templates.UnityDefaults.PhysicsType;
            //definition
            Player player = newPlayerObj.AddComponent<Player>();
            ActorEventController eventcontroller = newPlayerObj.AddComponent<ActorEventController>();

            GameObject InputSystem = new GameObject();
            InputSystem.name = "InputSystem";
            InputSystem.transform.SetParent(player.transform);
            player.InputSystem = InputSystem;

            GameObject AbilitySystem = new GameObject();
            AbilitySystem.name = "AbilitySystem";
            AbilitySystem.transform.SetParent(player.transform);
            player.AbilitySystem = AbilitySystem;
            PlayerAbilityInputClass abilityinput = InputSystem.AddComponent<PlayerAbilityInputClass>();
            PlayerAbilityUser playerab = AbilitySystem.AddComponent<PlayerAbilityUser>();
            AbilityController actemplate = gamedatabase.AbilityControllers.GetDatabaseObjectBySlotIndex(options.AbilityController) as AbilityController;//not working for some reason...but comes back correctly
            playerab.SetTemplate(actemplate);



            GameObject actorSystem = new GameObject();
            actorSystem.name = "ActorSystem";
            actorSystem.transform.SetParent(player.transform);
            player.ActorSystem = actorSystem;
            //attributes, inv, class
            ActorAttributes attributestemplate = gamedatabase.Attributes.GetDatabaseObjectBySlotIndex(options.Attributes) as ActorAttributes;
            PlayerAttributes attributes = actorSystem.AddComponent<PlayerAttributes>();
            attributes.SetAttributeTemplate(attributestemplate);

            ActorInventory inventorytemplate = gamedatabase.Inventories.GetDatabaseObjectBySlotIndex(options.Inventory) as ActorInventory;
            PlayerInventory inventory = actorSystem.AddComponent<PlayerInventory>();
            inventory.SetInventoryTemplate(inventorytemplate);

            ActorClass classtemplate = gamedatabase.Classes.GetDatabaseObjectBySlotIndex(options.Class) as ActorClass;
            PlayerClass playerclass = actorSystem.AddComponent<PlayerClass>();
            playerclass.SetMyClass(classtemplate);//different, because we dont have runtime versions since nothing is dynamic on the classes. 

            //leveling
            if (options.UseLeveling)
            {
                PlayerLeveling playerLeveling = actorSystem.AddComponent<PlayerLeveling>();
            }
            Clothing clothing = actorSystem.AddComponent<Clothing>();


            //auras
            GameObject auraSystem = new GameObject();
            auraSystem.name = "AuraSystem";
            auraSystem.transform.SetParent(player.transform);
            player.AuraSystem = auraSystem;
            if (options.UseAuras)
            {
                PlayerAuraInputClass auraInputs = InputSystem.AddComponent<PlayerAuraInputClass>();
                PlayerAuraInputsController inputcontroller = auraSystem.AddComponent<PlayerAuraInputsController>();
                AuraController auraC = gamedatabase.AuraControllers.GetDatabaseObjectBySlotIndex(options.AuraController) as AuraController;
                PlayerAuraUser aurauser = auraSystem.AddComponent<PlayerAuraUser>();
                aurauser.SetTemplate(auraC);
            }
            if (options.ReceiveAuras)
            {
                PlayerAuraReceiver auraRe = auraSystem.AddComponent<PlayerAuraReceiver>();
            }


            GameObject canvasSystem = new GameObject();
            canvasSystem.name = "CanvasSystem";
            player.CanvasSystem = canvasSystem;
            canvasSystem.transform.SetParent(player.transform);
            //built in canvases, always use the dungeon canvas
            if (options.UseBuiltInCanvases)
            {
  
                PlayerCanvasInputClass canvasInputs = InputSystem.AddComponent<PlayerCanvasInputClass>();

                PlayerEnchantingCanvasUser enchantinguser = canvasSystem.AddComponent<PlayerEnchantingCanvasUser>();
                PlayerSocketCanvasUser socketUser = canvasSystem.AddComponent<PlayerSocketCanvasUser>();
                PlayerAbilityTreeCanvas skilltree = canvasSystem.AddComponent<PlayerAbilityTreeCanvas>();

                PlayerInvCanvasUser invUser = canvasSystem.AddComponent<PlayerInvCanvasUser>();

                SaveCanvasUser saveCanvas = canvasSystem.AddComponent<SaveCanvasUser>();

                PlayerQuestCanvasUser questcanvasUser = canvasSystem.AddComponent<PlayerQuestCanvasUser>();

                PlayerFloatingTextUser dungeonCanvas = canvasSystem.AddComponent<PlayerFloatingTextUser>();//no need to set, it's created in the scene. 

                PlayerAbilityInventoryUser abInventory = canvasSystem.AddComponent<PlayerAbilityInventoryUser>();

                PlayerActionBarUser actionBar = canvasSystem.AddComponent<PlayerActionBarUser>();

                List<GameObject> objs = CreatorHelpers.FindPrefabs(new string[1] { gamedatabase.Settings.Templates.CanvasPaths.CanvasPrefabspath });

                IProgressTree treeui;
                IInventoryCanvas invui;
                ISaveCanvas saveui;
                IQuesterCanvas questui;
                IAbilityInventoryUI ui;
                IActionBarUI actionbarui;
                for (int i = 0; i < objs.Count; i++)
                {
                    treeui = objs[i].GetComponent<IProgressTree>();
                    if (treeui != null)
                    {
                        skilltree.SetCanvasPrefab(objs[i]);
                        continue;
                    }
                    invui = objs[i].GetComponent<IInventoryCanvas>();
                    if (invui != null)
                    {
                        invUser.SetCanvasPrefab(objs[i]);
                        continue;
                    }
                    saveui = objs[i].GetComponent<ISaveCanvas>();
                    if (saveui != null)
                    {
                        saveCanvas.SetCanvasPrefab(objs[i]);
                        continue;
                    }
                    questui = objs[i].GetComponent<IQuesterCanvas>();
                    if (questui != null)
                    {
                        questcanvasUser.SetPrefabCanvas(objs[i]);
                        continue;
                    }
                    actionbarui = objs[i].GetComponent<IActionBarUI>();
                    if (actionbarui != null)
                    {
                        actionBar.SetPrefab(objs[i]);
                    }
                    ui = objs[i].GetComponent<IAbilityInventoryUI>();
                    if (ui != null)
                    {
                        abInventory.SetCanvasPrefab(objs[i]);
                        continue;
                    }

                }

            }

            GameObject combatSystem = new GameObject();
            combatSystem.name = "CombatSystem";
            combatSystem.transform.SetParent(player.transform);
            player.CombatSystem = combatSystem;
            //combat
            if (options.UseCombat)
            {
                MeleeCombatant melee = combatSystem.AddComponent<MeleeCombatant>();
                ProjectileCombatant projectile = combatSystem.AddComponent<ProjectileCombatant>();
            }

            if (options.CanTakeDamage)
            {
                PlayerHealth playerHealth = combatSystem.AddComponent<PlayerHealth>();
            }
            PlayerFloatingTextUser floating = combatSystem.AddComponent<PlayerFloatingTextUser>();

            GameObject factionsystem = new GameObject();
            factionsystem.name = "FactionSystem";
            factionsystem.transform.SetParent(player.transform);
            player.FactionSystem = factionsystem;
            if (options.UseFactions)
            {
                PlayerFactionMember faction = factionsystem.AddComponent<PlayerFactionMember>();
            }


            GameObject movementSystem = new GameObject();
            movementSystem.name = "MovementSystem";
            movementSystem.transform.SetParent(player.transform);
            player.MovementSystem = movementSystem;
            

            if (options.UseBuiltInMoving)
            {
                PlayerMoverInputClass moveinput = InputSystem.AddComponent<PlayerMoverInputClass>();

                player.StateMachineSystem = player.gameObject;//just putting it on root for now.
                switch (type)
                {
                    case EditorPhysicsType.Unity3D:
                        ActorStateMachine statemachine = newPlayerObj.AddComponent<ActorStateMachine>();
                        statemachine.ActorHub = player.gameObject;

                        switch (options.Mover3DType)
                        {
                            case 0://nav mover
                                NavMeshAgent agent = newPlayerObj.AddComponent<NavMeshAgent>();
                                PlayerNavMeshMover mover = movementSystem.AddComponent<PlayerNavMeshMover>();
                                mover.PlayerHub = player.gameObject;

                                MouseNavMeshVars vars = new MouseNavMeshVars();
                                vars.Agent = agent;
                                vars.Ground = gamedatabase.Settings.LayerAssign.GroundLayer;
                                vars.Interactable = gamedatabase.Settings.LayerAssign.InteractableLayer;
                                vars.Attackable = gamedatabase.Settings.LayerAssign.AttackableLayer;
                                vars.RayLength = 100;

                                mover.MoveVars = vars;
                                PlayerMouseInputClass mouseInputs = InputSystem.AddComponent<PlayerMouseInputClass>();
                                SceneTraveler sceneTraveler = newPlayerObj.AddComponent<SceneTraveler>();
                                statemachine.MovementStates = gamedatabase.Settings.Templates.Player3DDefaults.Move3DDefault;
                                break;
                        }
                        break;
                    case EditorPhysicsType.Unity2D:
                        ActorStateMachine2D statemachine2d = newPlayerObj.AddComponent<ActorStateMachine2D>();
                        statemachine2d.IdleStates = gamedatabase.Settings.Templates.Player2DDefaults.IdleStates;
                        statemachine2d.WalkStates = gamedatabase.Settings.Templates.Player2DDefaults.WalkStates;
                        statemachine2d.DeathStates = gamedatabase.Settings.Templates.Player2DDefaults.DeathStates;
                        statemachine2d.AbilityStates2D = gamedatabase.Settings.Templates.Player2DDefaults.AbilityStates;
                        statemachine2d.PlayerHub = player.gameObject;
                        break;
                }
               
            }
            else
            {
                PlayerMoverInputClass moveinput = InputSystem.AddComponent<PlayerMoverInputClass>();
                PlayerInteractInputClass interactInput = InputSystem.AddComponent<PlayerInteractInputClass>();
                PlayerAbilityInputsController abilityCo = InputSystem.AddComponent<PlayerAbilityInputsController>();//needs to reconsider the 3d.

            }

            //3d
            switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    CreatorHelpers.Setup3DTriggers(newPlayerObj, CreatorHelpers.TriggerType.Capsule);
                    break;
                case EditorPhysicsType.Unity2D:
                    CreatorHelpers.Setup2DTriggers(newPlayerObj);
                    PlayerAbilityInputsController abilityC = InputSystem.AddComponent<PlayerAbilityInputsController>();//needs to reconsider the 3d.
                    PlayerTopDown2DMover topmover = movementSystem.AddComponent<PlayerTopDown2DMover>();
                    PlayerInteractInputClass interactInput = InputSystem.AddComponent<PlayerInteractInputClass>();

    
                        switch (options.Interact2DType)
                        {
                            case 0:
                                //mouse
                                InteractMouse2D mouse2d = InputSystem.AddComponent<InteractMouse2D>();
                                mouse2d.SetLayerMask(gamedatabase.Settings.LayerAssign.InteractableLayer);
                            mouse2d.ActorHub = player.gameObject;
                                break;
                            case 1:
                                //ontriggerneter
                                OnTriggerEnter2DInteract enter2d = newPlayerObj.AddComponent<OnTriggerEnter2DInteract>();
                                break;
                        }
                    


                   
                    //topdown is default for now.
                    break;
            }
           

            CreatorHelpers.AssignAttackableLayer(gamedatabase, newPlayerObj);


            GameObject questing = new GameObject();
            questing.name = "QuestingSystem";
            questing.transform.SetParent(player.transform);
            player.QuestSystem = questing;
            //quest
            if (options.UseQuests)
            {
                QuestLog log = gamedatabase.QuestLog.GetDatabaseObjectBySlotIndex(options.QuestLog) as QuestLog;
                PlayerQuester quester = questing.AddComponent<PlayerQuester>();
                quester.SetTemplate(log);
                //make a quest log...
            }


            GameObject shopping = new GameObject();
            shopping.name = "ShopSystem";
            shopping.transform.SetParent(player.transform);
            player.ShopSystem = shopping;
            if (options.UseShopping)
            {
                PlayerSeller seller = shopping.AddComponent<PlayerSeller>();
                PlayerShopper shopper = shopping.AddComponent<PlayerShopper>();

                if (options.UseBuiltInCanvases)
                {
                    List<GameObject> objs = CreatorHelpers.FindPrefabs(new string[1] { gamedatabase.Settings.Templates.CanvasPaths.CanvasPrefabspath });
                    for (int i = 0; i < objs.Count; i++)
                    {
                        ISellerCanvasUI sellerui = objs[i].GetComponent<ISellerCanvasUI>();
                        if (sellerui != null)
                        {
                            seller.SetCanvasPrefab(objs[i]);
                        }
                    }

                }
            }


            GameObject status = new GameObject();
            status.name = "StatusSystem";
            status.transform.SetParent(player.transform);
            player.StatusSystem = status;
            if (options.CanTakeStatusEffects)
            {
                PlayerStatusChangeReceiver effects = status.AddComponent<PlayerStatusChangeReceiver>();
            }



            GameObject newMeshHolderObj = new GameObject();
            newMeshHolderObj.transform.SetParent(newPlayerObj.transform);
            newMeshHolderObj.name = "MeshHolder";
            if (options.CharacterMesh != null)
            {
                
                GameObject instance = UnityEngine.GameObject.Instantiate(options.CharacterMesh.gameObject, newMeshHolderObj.transform);
                instance.transform.position = new Vector3(0, 0, 0);//reset mesh to 0, 0, 0 in case it's not already
                switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
                {
                    case EditorPhysicsType.Unity3D:
                        Collider[] coll = instance.GetComponentsInChildren<Collider>();
                        if (coll.Length == 0)
                        {
                            CapsuleCollider collider = instance.AddComponent<CapsuleCollider>();
                            collider.center = new Vector3(0, 1, 0);//just defaults guessing
                            collider.height = 2f;
                            collider.isTrigger = false;
                            Bounds bigBounds = new Bounds();
                            foreach (var r in instance.GetComponentsInChildren<Renderer>())
                            {
                                bigBounds.Encapsulate(r.bounds);
                            }
                            collider.center = bigBounds.center;

                        }
                        break;
                    case EditorPhysicsType.Unity2D:
                        Collider2D[] colls2d = instance.GetComponentsInChildren<Collider2D>();
                        if (colls2d.Length == 0)
                        {
                            BoxCollider2D newbox = instance.AddComponent<BoxCollider2D>();
                            newbox.isTrigger = false;
                           
                        }
                        break;

                      
                }

             
                
                

                if (options.UseAnimatorController)
                {
                    Animator animator = instance.GetComponent<Animator>();
                    if (animator == null)
                    {
                        animator = instance.AddComponent<Animator>();
                    }
                    PlayerAnimations playeranim = animator.gameObject.AddComponent<PlayerAnimations>();
                    player.Animator = instance.gameObject;
                    player.MyAnim = playeranim;
                    RuntimeAnimatorController basecontroller = null;

                    switch (type)
                    {
                        case EditorPhysicsType.Unity2D:
                            basecontroller = gamedatabase.Settings.Templates.Player2DDefaults.AnimController;
                            break;
                        case EditorPhysicsType.Unity3D:
                            basecontroller = gamedatabase.Settings.Templates.Player3DDefaults.AnimController;
                            break;
                    }
      
                    if (basecontroller == null)
                    {
                        Debug.LogWarning("No animator template set in the game database settings. Remember to add your animator controller asset manually");
                    }
                    else
                    {
                        RuntimeAnimatorController copy = UnityEngine.Object.Instantiate(basecontroller) as RuntimeAnimatorController;
                        animator.runtimeAnimatorController = copy;
                        string folder = CreatorHelpers.GetPathFolder(path);
                        string name = options.CharacterMesh.name + "_Player";
                        string extension = ".asset";
                        //split the path to the root
                        // animator.runtimeAnimatorController = newovveride;
                        AssetDatabase.CreateAsset(copy, folder + name + extension);


                    }
                   

                  

                }
                
            }

            PrefabUtility.SaveAsPrefabAsset(newPlayerObj, path);
            UnityEngine.Object.DestroyImmediate(newPlayerObj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Object savedObj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            return savedObj;
        }

      



    }

  


    /// <summary>
    /// enemy options
    /// </summary>
    [System.Serializable]
    public class EnemyOptions
    {
        //enemy options
        //so's//indexed to the appropriate database
        public int Attributes = 0;//req
        public int Inventory = 0;//opt
        public int Class = 0;//opt
        public int AbilityController = 0;//req for combat
        public int AuraController = 0;//req for auras
        public int LootDrops = 0;
        public int Mover2DType = 0;
        public int Mover3DType = 0;
        //bools
        public bool UseBuiltInMoving = true;
        public bool ScaleWithDungeonLevel = true;
        public bool UseNavMover = true;
        public bool UseBuiltinAI = true;
        public bool UseBuiltInHPInfo = true;
        public bool UseAuras = true;
        public bool ReceiveAuras = true;
        public bool UseCombat = true;
        public bool CanTakeDamage = true;
        public bool CanTakeStatusEffects = true;
        public bool UseAnimatorController = true;

        public GameObject CharacterMesh = null;
    }

   
    public static class EnemyDefinition
    {
        //need specific class, inventory and aura controller.
        public static UnityEngine.Object CreateNewEnemyPrefab(string path, EnemyOptions options, GameDatabase gamedatabase)
        {

            GameObject newEnemyObj = new GameObject();
            //definition
            Enemy enemy = newEnemyObj.AddComponent<Enemy>();
            EditorPhysicsType type = gamedatabase.Settings.Templates.UnityDefaults.PhysicsType;
            switch (type)
            {
                case EditorPhysicsType.Unity2D:
                    Rigidbody2D rb2d = newEnemyObj.AddComponent<Rigidbody2D>();
                    rb2d.gravityScale = 0;
                    break;
                case EditorPhysicsType.Unity3D:
                    Rigidbody rb = newEnemyObj.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    break;
            }

            ChainTarget chaintarget = newEnemyObj.AddComponent<ChainTarget>();

            ActorEventController eventcontroller = newEnemyObj.AddComponent<ActorEventController>();
            #region movement
            GameObject movement = new GameObject();
            movement.name = "MovementSystem";
            movement.transform.SetParent(newEnemyObj.transform);
            enemy.MovementSystem = movement;
          switch (type)
            {
                case EditorPhysicsType.Unity3D:
                    //
                    CreatorHelpers.Setup3DTriggers(newEnemyObj, CreatorHelpers.TriggerType.Capsule);

                    if (options.UseBuiltInMoving)
                    {
                        switch (options.Mover3DType)
                        {
                            case 0:
                                NavMeshAgent agent = newEnemyObj.AddComponent<NavMeshAgent>();
                                EnemyNavMeshMover mover = movement.AddComponent<EnemyNavMeshMover>();
                                mover.Agent = agent;
                                break;
                        }
                    }
                   
                    break;
                case EditorPhysicsType.Unity2D:
                    CreatorHelpers.Setup2DTriggers(newEnemyObj);
                    //some basic stuff here.

                    if (options.UseBuiltInMoving)
                    {
                        switch (options.Mover2DType)
                        {
                            case 0:
                                EnemyTopDownMover2D mover2d = movement.AddComponent<EnemyTopDownMover2D>();
                                mover2d.Body = enemy.gameObject.GetComponent<Rigidbody2D>();
                                break;
                        }
                    }
                    break;
            }
            #endregion
            CreatorHelpers.AssignAttackableLayer(gamedatabase, newEnemyObj);

         
            GameObject actor = new GameObject();
            actor.transform.SetParent(newEnemyObj.transform);
            enemy.ActorSystem = actor;
            actor.name = "ActorSystem";
            //attributes, inv, class
            EnemyAttributes attributes = actor.AddComponent<EnemyAttributes>();
            ActorAttributes attributestemplate = gamedatabase.Attributes.GetDatabaseObjectBySlotIndex(options.Attributes) as ActorAttributes;
            attributes.SetAttributeTemplate(attributestemplate);
            ActorClass classtemplate = gamedatabase.Classes.GetDatabaseObjectBySlotIndex(options.Class) as ActorClass;
            EnemyClass enemyClass = actor.AddComponent<EnemyClass>();
            enemyClass.SetMyClass(classtemplate);//different, because we dont have runtime versions since nothing is dynamic on the classes. 
            ActorInventory inventorytemplate = gamedatabase.Inventories.GetDatabaseObjectBySlotIndex(options.Inventory) as ActorInventory;
            EnemyInventory inventory = actor.AddComponent<EnemyInventory>();
            inventory.SetInventoryTemplate(inventorytemplate);

            GameObject ability = new GameObject();
            ability.transform.SetParent(newEnemyObj.transform);
            enemy.AbilitySystem = ability;
            ability.name = "AbilitySystem";
            //combat & abilities
            if (options.UseCombat)
            {

                AbilityUser abilityUser = ability.AddComponent<AbilityUser>();
                AbilityController actemplate = gamedatabase.AbilityControllers.GetDatabaseObjectBySlotIndex(options.AbilityController) as AbilityController;
                abilityUser.SetTemplate(actemplate);
            }

            GameObject aura = new GameObject();
            aura.transform.SetParent(newEnemyObj.transform);
            enemy.AuraSystem = aura;
            aura.name = "AuraSystem";
            if (options.UseAuras)
            {
                AuraController auraC = gamedatabase.AuraControllers.GetDatabaseObjectBySlotIndex(options.AuraController) as AuraController;
                EnemyAuraUser aurauser = aura.AddComponent<EnemyAuraUser>();//dont have enemy aura user yet
                aurauser.SetTemplate(auraC);
            }

            if (options.ReceiveAuras)
            {
                EnemyAuraReceiver auraRe = aura.AddComponent<EnemyAuraReceiver>();
            }

            #region ai
            GameObject ai = new GameObject();
            ai.name = "AISystem";
            ai.transform.SetParent(newEnemyObj.transform);
            enemy.AISystem = ai;
            StateMachineBlackboard bb = ai.AddComponent<StateMachineBlackboard>();
            bb.ActorHub = enemy.gameObject;
            EnemyStateMachine statem = ai.AddComponent<EnemyStateMachine>();
            statem.BlackBoard = bb.gameObject;
            statem.States = new AIStateSO[0];
            switch (type)
            {
                case EditorPhysicsType.Unity3D:
                    statem.States = gamedatabase.Settings.Templates.ActorDefaults.EnemyDefaults3D;
                    break;
                case EditorPhysicsType.Unity2D:
                    statem.States = gamedatabase.Settings.Templates.ActorDefaults.EnemyDefaults2D;
                    break;
            }
            EnemySimpleBrain brain = ai.AddComponent<EnemySimpleBrain>();
            brain.ActorHub = enemy.gameObject;
            brain.DefaultKey = "Aggro";
            brain.AggroKey = "Aggro";
            brain.AggroDuration = 7;
            brain.IdleKey = "Idle";
            brain.DeathKey = "Death";
            switch (type)
            {
                case EditorPhysicsType.Unity2D:
                    State2D state2d = ai.AddComponent<State2D>();
                    state2d.Rb2D = newEnemyObj.GetComponent<Rigidbody2D>();
                    break;
            }
            #endregion

            GameObject combat = new GameObject();
            combat.name = "CombatSystem";
            combat.transform.SetParent(newEnemyObj.transform);
            enemy.CombatSystem = combat;

            if (options.UseCombat)
            {
                MeleeCombatant melee = combat.AddComponent<MeleeCombatant>();
                ProjectileCombatant projectile = combat.AddComponent<ProjectileCombatant>();
            }
            if (options.ScaleWithDungeonLevel)
            {
                EnemyLevelScaler scaler = combat.AddComponent<EnemyLevelScaler>();
            }
            //health
            if (options.CanTakeDamage)
            {
                EnemyHealth enemyhealth = combat.AddComponent<EnemyHealth>();
            }
            //loot drops
            EnemyDrop drop = combat.AddComponent<EnemyDrop>();
            LootDrops drops = gamedatabase.Loot.GetDatabaseObjectBySlotIndex(options.LootDrops) as LootDrops;
            drop.SetLootDrop(drops);
            EnemyXP xp = combat.AddComponent<EnemyXP>();
            EnemyDungeonCanvasUser dungeoncanvas = combat.AddComponent<EnemyDungeonCanvasUser>();




            GameObject factionsys = new GameObject();
            factionsys.name = "FactionSystem";
            factionsys.transform.SetParent(newEnemyObj.transform);
            EnemyFactionMember faction = factionsys.AddComponent<EnemyFactionMember>();
            enemy.FactionSystem = factionsys;
            //misc

            //statuseffects


            GameObject statussys = new GameObject();
            statussys.transform.SetParent(newEnemyObj.transform);
            statussys.name = "StatusSystem";
            enemy.StatusSystem = statussys;

            if (options.CanTakeStatusEffects)
            {
                EnemyStatusChangeReceiver effects = statussys.AddComponent<EnemyStatusChangeReceiver>();
            }

            if (options.CharacterMesh != null)
            {
                GameObject newMeshHolderObj = new GameObject();
                newMeshHolderObj.transform.SetParent(newEnemyObj.transform);
                newMeshHolderObj.name = "MeshHolder";
                GameObject instance = UnityEngine.GameObject.Instantiate(options.CharacterMesh.gameObject, newMeshHolderObj.transform);
                instance.transform.position = new Vector3(0, 0, 0);//reset mesh to 0, 0, 0 in case it's not already
                if (options.UseAnimatorController)
                {
                    Animator animator = instance.GetComponent<Animator>();
                    if (animator == null)
                    {
                        animator = instance.AddComponent<Animator>();
                    }
                    enemy.Animator = animator;
                    EnemyAnimationNavMesh playeranim = animator.gameObject.AddComponent<EnemyAnimationNavMesh>();
                    enemy.MyAnim = playeranim;
                    RuntimeAnimatorController basecontroller = null;

                    switch (type)
                    {
                        case EditorPhysicsType.Unity2D:
                            basecontroller = gamedatabase.Settings.Templates.ActorDefaults.AnimController2D;
                            break;
                        case EditorPhysicsType.Unity3D:
                            basecontroller = gamedatabase.Settings.Templates.ActorDefaults.AnimController;
                            break;
                    }
         
                    if (basecontroller == null)
                    {
                        Debug.LogWarning("No animator controller set the game database settings. Remember to manually set the animator controller", enemy.gameObject);
                    }
                    else
                    {
                        RuntimeAnimatorController copy = UnityEngine.Object.Instantiate(basecontroller) as RuntimeAnimatorController;
                        animator.runtimeAnimatorController = copy;
                        string folder = CreatorHelpers.GetPathFolder(path);
                        string name = options.CharacterMesh.name + "_Enemy";
                        string extension = ".asset";
                        AssetDatabase.CreateAsset(copy, folder + name + extension);
                    }
                  
                    
                   
                }
            }

            GameObject info = new GameObject();
            info.transform.SetParent(enemy.transform);
            enemy.EnemyInfo = info;
            info.name = "EnemyInfo";
            if (options.UseBuiltInHPInfo)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath(gamedatabase.Settings.Templates.PrefabPaths.EnemyHPInfoPrefabPath, typeof(GameObject)) as GameObject;
                GameObject instance = UnityEngine.GameObject.Instantiate(prefab, info.transform);
                IResourceBar[] bars = instance.GetComponentsInChildren<IResourceBar>();
                for (int i = 0; i < bars.Length; i++)
                {
                    bars[i].SetActorHub(enemy);
                }

            }


            PrefabUtility.SaveAsPrefabAsset(newEnemyObj, path);
            UnityEngine.Object.DestroyImmediate(newEnemyObj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Object savedObj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));


            return savedObj;
        }
        ///reorganzie by required and options
        ///sub out options
        //navmesh specific
        //navmesh agent
        //enemy namv mesh mover

        //arpg specific
        //Enemy script
        //Enemy attributes
        //Enemy inventory
        //Enemy class
        //Enemy health
        //Enemy drop
        //Enemy scaler
        //Enemy aura user
        //Enemy aura receiver
        //Enemy dungeon canvas user

        //status effects
        //melee combatant
        //projectile combatant
        //ability user

    }
    [System.Serializable]
    public class ShopkeeperOptions
    {
        public int Attributes = 0;
        public int Inventory = 0;//opt
        public int LootDrops = 0;

        public ItemType[] TypesToSell = new ItemType[2] { ItemType.Potions, ItemType.Equipment };//not in editor yet
        public bool ShopScalesWithDungeonLevel = true;
        public bool UseBuiltInCanvas = true;
        public int ItemRolls = 10;

        public GameObject CharacterMesh = null;

    }
    public static class ShopKeeperDefinition
    {
        public static UnityEngine.Object CreateNewShopKeeperPrefab(string folder, ShopkeeperOptions options, GameDatabase gamedatabase)
        {

            GameObject newShopKeeperObj = new GameObject();

            switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    //
                    CreatorHelpers.Setup3DTriggers(newShopKeeperObj, CreatorHelpers.TriggerType.Capsule);

                   
                    break;
                case EditorPhysicsType.Unity2D:
                    CreatorHelpers.Setup2DTriggers(newShopKeeperObj);


                    break;
            }

            CreatorHelpers.AssignInteractbleLayer(gamedatabase, newShopKeeperObj);

            //definition
            ShopKeeper shop = newShopKeeperObj.AddComponent<ShopKeeper>();
            LootDrops drops = gamedatabase.Loot.GetDatabaseObjectBySlotIndex(options.LootDrops) as LootDrops;
            shop.SetStoreTable(drops);
            shop.SetItemsToSell(options.TypesToSell);
            shop.SetItemRolls(options.ItemRolls);
            //attributes, inv, class
            ActorAttributes attributestemplate = gamedatabase.Attributes.GetDatabaseObjectBySlotIndex(options.Attributes) as ActorAttributes;
            NPCAttributes attributes = newShopKeeperObj.AddComponent<NPCAttributes>();
            attributes.SetAttributeTemplate(attributestemplate);


            ActorInventory inventorytemplate = gamedatabase.Inventories.GetDatabaseObjectBySlotIndex(options.Inventory) as ActorInventory;
            NPCInventory inventory = newShopKeeperObj.AddComponent<NPCInventory>();
            inventory.SetInventoryTemplate(inventorytemplate);


            //statuseffects
            if (options.ShopScalesWithDungeonLevel)
            {
                ShopScaler scaler = newShopKeeperObj.AddComponent<ShopScaler>();
            }

        
            if (options.CharacterMesh != null)
            {
                GameObject newMeshHolderObj = new GameObject();
                newMeshHolderObj.transform.SetParent(newShopKeeperObj.transform);
                newMeshHolderObj.name = "MeshHolder";
                GameObject instance = UnityEngine.GameObject.Instantiate(options.CharacterMesh.gameObject, newMeshHolderObj.transform);
                instance.transform.position = new Vector3(0, 0, 0);//reset mesh to 0, 0, 0 in case it's not already

            }

            if (options.UseBuiltInCanvas)
            {
                List<GameObject> objs = CreatorHelpers.FindPrefabs(new string[1] { gamedatabase.Settings.Templates.CanvasPaths.CanvasPrefabspath });
                for (int i = 0; i < objs.Count; i++)
                {
                    IShopKeeperCanvas shopUI = objs[i].GetComponent<IShopKeeperCanvas>();
                    if (shopUI != null)
                    {
                        shop.SetCanvasPrefab(objs[i]);
                        break;
                    }
                }    
            }

            PrefabUtility.SaveAsPrefabAsset(newShopKeeperObj, folder);
            UnityEngine.Object.DestroyImmediate(newShopKeeperObj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Object savedObj = AssetDatabase.LoadAssetAtPath(folder, typeof(UnityEngine.Object));
            return savedObj;
        }
      

    }

    [System.Serializable]
    public class QuestGiverOptions
    {

        public int Attributes = 0;

        public GameObject CharacterMesh = null;
        public bool UseBuiltInCanvas = true;

    }

    public static class QuestGiverDefinition
    {

        public static UnityEngine.Object CreateQuestGiverPrefab(string folder, QuestGiverOptions options, GameDatabase gamedatabase)
        {

            GameObject newquestgiverobj = new GameObject();

            switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    //
                    CreatorHelpers.Setup3DTriggers(newquestgiverobj, CreatorHelpers.TriggerType.Capsule);


                    break;
                case EditorPhysicsType.Unity2D:
                    CreatorHelpers.Setup2DTriggers(newquestgiverobj);


                    break;
            }


            CreatorHelpers.AssignInteractbleLayer(gamedatabase, newquestgiverobj);



            //definition
            QuestchainGiver questgiver = newquestgiverobj.AddComponent<QuestchainGiver>();
            if (options.UseBuiltInCanvas)
            {
                List<GameObject> objs = CreatorHelpers.FindPrefabs(new string[1] { gamedatabase.Settings.Templates.CanvasPaths.CanvasPrefabspath });
                for (int i = 0; i < objs.Count; i++)
                {
                    IQuestGiverCanvas shopUI = objs[i].GetComponent<IQuestGiverCanvas>();
                    if (shopUI != null)
                    {
                        questgiver.SetCanvasPrefab(objs[i]);
                        break;
                    }
                }
            }

            //attributes, inv, class
            ActorAttributes attributestemplate = gamedatabase.Attributes.GetDatabaseObjectBySlotIndex(options.Attributes) as ActorAttributes;
            NPCAttributes attributes = newquestgiverobj.AddComponent<NPCAttributes>();
            attributes.SetAttributeTemplate(attributestemplate);

           

            if (options.CharacterMesh != null)
            {
                GameObject newMeshHolderObj = new GameObject();
                newMeshHolderObj.transform.SetParent(newquestgiverobj.transform);
                newMeshHolderObj.name = "MeshHolder";
                GameObject instance = UnityEngine.GameObject.Instantiate(options.CharacterMesh.gameObject, newMeshHolderObj.transform);
                instance.transform.position = new Vector3(0, 0, 0);//reset mesh to 0, 0, 0 in case it's not already

            }

            PrefabUtility.SaveAsPrefabAsset(newquestgiverobj, folder);
            UnityEngine.Object.DestroyImmediate(newquestgiverobj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Object savedObj = AssetDatabase.LoadAssetAtPath(folder, typeof(UnityEngine.Object));
            return savedObj;
        }



    }


    [System.Serializable]
    public class BreakableOptions
    {

        public int Attributes = 0;
        public int HPResourceType = 0;
        public int LootDrops = 0;
        public bool UseScaling = true;
        public bool UseMouseOverOutline = true;
        public bool UseGizmoIcons = true;

        public GameObject OriginalMesh = null;
        public GameObject BrokenVersion = null;
        public GameObject HighlightVersion = null;

    }
    public static class BreakableDefinition
    {

        public static UnityEngine.Object CreateBreakablePrefab(string folder, BreakableOptions options, GameDatabase gamedatabase)
        {

            GameObject newbreakableObject = new GameObject();

            switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    //
                    CreatorHelpers.Setup3DTriggers(newbreakableObject, CreatorHelpers.TriggerType.Box);


                    break;
                case EditorPhysicsType.Unity2D:
                    CreatorHelpers.Setup2DTriggers(newbreakableObject);


                    break;
            }


            CreatorHelpers.AssignAttackableLayer(gamedatabase, newbreakableObject);
            GameObject dropLocation = CreatorHelpers.AssignParentandName(newbreakableObject, "DropLocation");
            GameObject parentoforiginal = CreatorHelpers.AssignParentandName(newbreakableObject, "ParentofOriginalMesh");
            GameObject parentofbroken = CreatorHelpers.AssignParentandName(newbreakableObject, "ParentofBrokenVersion");
            GameObject parentofhighlight = CreatorHelpers.AssignParentandName(newbreakableObject, "ParentofHighlightVersion");


           // BoxCollider collider = newbreakableObject.AddComponent<BoxCollider>();
           // collider.isTrigger = true;
            //definition
            Breakable breakable = newbreakableObject.AddComponent<Breakable>();
            BreakableVars vars = new BreakableVars((ResourceType)options.HPResourceType, parentoforiginal.transform, parentofbroken.transform, new CombatGroupType[2] { CombatGroupType.Enemy, CombatGroupType.Neutral });
            breakable.SetVars(vars);

            //attributes, inv, class
            ActorAttributes attributestemplate = gamedatabase.Attributes.GetDatabaseObjectBySlotIndex(options.Attributes) as ActorAttributes;
            NPCAttributes attributes = newbreakableObject.AddComponent<NPCAttributes>();
            attributes.SetAttributeTemplate(attributestemplate);

            LootDrops lootdrops = gamedatabase.Loot.GetDatabaseObjectBySlotIndex(options.LootDrops) as LootDrops;
            BreakableDropLoot loot = newbreakableObject.AddComponent<BreakableDropLoot>();
            DropLootVars dropvars = new DropLootVars(lootdrops, dropLocation.transform, .25f, options.UseScaling);
            loot.SetVars(dropvars);

            BreakableScaler scaler = newbreakableObject.AddComponent<BreakableScaler>();


            if (options.UseMouseOverOutline)
            {
                switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
                {
                    case EditorPhysicsType.Unity3D:
                        OutlineMouseOver moouseover = newbreakableObject.AddComponent<OutlineMouseOver>();
                        OutlineMouseVars mousevars = new OutlineMouseVars(parentoforiginal.transform, parentofhighlight.transform);
                        LayerMask mask = gamedatabase.Settings.LayerAssign.AttackableLayer;
                        mousevars.Layermask = mask;
                        moouseover.SetVars(mousevars);
                        break;
                    case EditorPhysicsType.Unity2D:
                        OutlineMouseOver2D mouseover2D = newbreakableObject.AddComponent<OutlineMouseOver2D>();
                        OutlineMouseVars mousevars2d = new OutlineMouseVars(parentoforiginal.transform, parentofhighlight.transform);
                        LayerMask mask2d = gamedatabase.Settings.LayerAssign.AttackableLayer;
                        mousevars2d.Layermask = mask2d;
                        mouseover2D.SetVars(mousevars2d);
                        break;

                }
              
            }

            if (options.UseGizmoIcons)
            {
                DrawIcon baseicon = newbreakableObject.AddComponent<DrawIcon>();
                DrawIcon dropicon = dropLocation.AddComponent<DrawIcon>();

                string basebreakable = "Assets/GWLPXL/ARPG/Gizmos/TestAttackable.png";
                string droplocationpath = "Assets/GWLPXL/ARPG/Gizmos/TestDropLocation.png";
                if (gamedatabase.Settings != null)
                {
                    basebreakable = gamedatabase.Settings.GizmoIconPaths.BreakableBase;
                    droplocationpath = gamedatabase.Settings.GizmoIconPaths.BreakableDropLocation;
                }
                baseicon.IconPath = basebreakable;
                dropicon.IconPath = droplocationpath;
            }

            if (options.OriginalMesh != null)
            {
                GameObject instance = UnityEngine.GameObject.Instantiate(options.OriginalMesh.gameObject, parentoforiginal.transform);
                instance.transform.localPosition = new Vector3(0, 0, 0);
                instance.transform.localRotation = Quaternion.identity;

            }
            if (options.BrokenVersion != null)
            {
                GameObject instance = UnityEngine.GameObject.Instantiate(options.BrokenVersion.gameObject, parentofbroken.transform);
                instance.transform.localPosition = new Vector3(0, 0, 0);
                instance.transform.localRotation = Quaternion.identity;
            }
            if (options.HighlightVersion != null)
            {
                GameObject instance = UnityEngine.GameObject.Instantiate(options.HighlightVersion.gameObject, parentofhighlight.transform);
                instance.transform.localPosition = new Vector3(0, 0, 0);
                instance.transform.localRotation = Quaternion.identity;
            }
            PrefabUtility.SaveAsPrefabAsset(newbreakableObject, folder);
            UnityEngine.Object.DestroyImmediate(newbreakableObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Object savedObj = AssetDatabase.LoadAssetAtPath(folder, typeof(UnityEngine.Object));
            return savedObj;
        }



    }

    [System.Serializable]
    public class SearchableOptions
    {
        public string Name = string.Empty;
        public int LootDrops = 0;
        public bool UseScaling = true;
        public bool UseMouseOverOutline = true;
        public bool UseGizmoIcons = true;
        public int UnscaledLevel = 1;
        public float SearchDistance = 3f;
        public GameObject OriginalMesh = null;
        public GameObject HighlightVersion = null;

    }

    public static class SearchableDefinition
    {

        public static UnityEngine.Object CreateSearchablePrefab(string folder, SearchableOptions options, GameDatabase gamedatabase)
        {

            GameObject newSearchableObject = new GameObject();
            switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    //
                    CreatorHelpers.Setup3DTriggers(newSearchableObject, CreatorHelpers.TriggerType.Box);


                    break;
                case EditorPhysicsType.Unity2D:
                    CreatorHelpers.Setup2DTriggers(newSearchableObject);


                    break;
            }

            CreatorHelpers.AssignInteractbleLayer(gamedatabase, newSearchableObject);

            GameObject dropLocation = CreatorHelpers.AssignParentandName(newSearchableObject, "DropLocation");
            GameObject parentoforiginal = CreatorHelpers.AssignParentandName(newSearchableObject, "ParentofOriginalMesh");
            GameObject parentofhighlight = CreatorHelpers.AssignParentandName(newSearchableObject, "ParentofHighlightVersion");


            //BoxCollider collider = newSearchableObject.AddComponent<BoxCollider>();
           // collider.isTrigger = true;
            //definition
            Searchable searchable = newSearchableObject.AddComponent<Searchable>();
            SearchableVars vars = new SearchableVars(parentoforiginal.transform, dropLocation.transform, options.SearchDistance);
            searchable.SetVars(vars);
            //loot
            LootDrops lootdrops = gamedatabase.Loot.GetDatabaseObjectBySlotIndex(options.LootDrops) as LootDrops;
            SearchableDropLoot loot = newSearchableObject.AddComponent<SearchableDropLoot>();
            DropLootVars dropvars = new DropLootVars(lootdrops, dropLocation.transform, .25f, options.UseScaling);
            loot.SetVars(dropvars);

            //scaling
            SearchableScaler scaler = newSearchableObject.AddComponent<SearchableScaler>();
            scaler.SetUNScaledLevel(options.UnscaledLevel);

            //mouse interaction

                if (options.UseMouseOverOutline)
                {
                    switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
                    {
                        case EditorPhysicsType.Unity3D:
                            OutlineMouseOver moouseover = newSearchableObject.AddComponent<OutlineMouseOver>();
                            OutlineMouseVars mousevars = new OutlineMouseVars(parentoforiginal.transform, parentofhighlight.transform);
                            LayerMask mask = gamedatabase.Settings.LayerAssign.AttackableLayer;
                            mousevars.Layermask = mask;
                            moouseover.SetVars(mousevars);
                            break;
                        case EditorPhysicsType.Unity2D:
                            OutlineMouseOver2D mouseover2D = newSearchableObject.AddComponent<OutlineMouseOver2D>();
                            OutlineMouseVars mousevars2d = new OutlineMouseVars(parentoforiginal.transform, parentofhighlight.transform);
                            LayerMask mask2d = gamedatabase.Settings.LayerAssign.AttackableLayer;
                            mousevars2d.Layermask = mask2d;
                            mouseover2D.SetVars(mousevars2d);
                            break;

                    }

                }
            

            //gizmos
            if (options.UseGizmoIcons)
            {
                DrawIcon drawicon = newSearchableObject.AddComponent<DrawIcon>();
                string path = "Assets/GWLPXL/ARPG/Gizmos/SearchableIcon.png";
                if (gamedatabase.Settings != null)
                {
                    path = gamedatabase.Settings.GizmoIconPaths.SearchableBase;
                }
                drawicon.IconPath = path;
            }

            if (options.OriginalMesh != null)
            {
                GameObject instance = UnityEngine.GameObject.Instantiate(options.OriginalMesh.gameObject, parentoforiginal.transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;

            }
         
            if (options.HighlightVersion != null)
            {
                GameObject instance = UnityEngine.GameObject.Instantiate(options.HighlightVersion.gameObject, parentofhighlight.transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;

            }
            PrefabUtility.SaveAsPrefabAsset(newSearchableObject, folder);
            UnityEngine.Object.DestroyImmediate(newSearchableObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEngine.Object savedObj = AssetDatabase.LoadAssetAtPath(folder, typeof(UnityEngine.Object));
            return savedObj;
        }



    }

}
#endif