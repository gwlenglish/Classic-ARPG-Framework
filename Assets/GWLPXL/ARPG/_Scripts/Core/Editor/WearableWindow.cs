#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;


public interface ICreatorWIndow
{
    void SetGameDatabase(GameDatabase database);

}

namespace GWLPXL.ARPGCore.Wearables.com
{
 
    /// <summary>
    /// easy creation of wearables, weapons and projectiles
    /// </summary>
    public class WearableWindow : EditorWindow, ICreatorWIndow
    {
        
        Vector2 scrollPos;
        bool isWeapon;
        string[] weaponTypes = new string[3] { "Melee Weapon", "Projectile Weapon", "Projectile" };
        int weaponTypeSelect;
        public GameObject MeshPrefab;
        public GameObject Projectile;

        int meleeOptionIndex;
        string[] meleeOptionData;
        int projectileOptionIndex;
        string[] projectileOptionData;
        int meldamageOptionIndex;
        int projdamageOptionIndex;
        string[] damageOptions;

        GameDatabase gamedatabase;
        
        public void SetGameDatabase(GameDatabase database)
        {
            gamedatabase = database;

            meleeOptionData = gamedatabase.Melee.GetAllNames();
            projectileOptionData = gamedatabase.Projectiles.GetAllNames();
            damageOptions = gamedatabase.ActorDamageTypes.GetAllNames();
        }
        

        private void OnGUI()
        {

            if (gamedatabase == null) Close();

            EditorGUILayout.BeginVertical("Box");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

            EditorGUILayout.LabelField("Wearable Creator", EditorStyles.boldLabel);
            EditorGUILayout.Space(25);
            gamedatabase.Settings.Templates.UnityDefaults.PhysicsType = (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", gamedatabase.Settings.Templates.UnityDefaults.PhysicsType);
            EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
            EditorGUILayout.LabelField("Is a weapon?");
            EditorGUI.indentLevel++;
            isWeapon = EditorGUILayout.Toggle(isWeapon);
            EditorGUI.indentLevel--;
            if (isWeapon)
            {
                EditorGUILayout.HelpBox("Melee weapons are stationary and require contact in order to damage, for example an Axe. " +
                    "Projectile Weapons shoot objects that do damage, for example a bow.", MessageType.Info);
                weaponTypeSelect = GUILayout.SelectionGrid(weaponTypeSelect, weaponTypes, 2);
  
                EditorGUILayout.Space(25);
                EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                switch (weaponTypeSelect)
                {
                    case 0:
                        EditorGUILayout.HelpBox("Melee Weapons Require a Prefab to work. This should be the model or mesh gameobject, the thing that visually represents your weapon.", MessageType.Info);
                        EditorGUILayout.LabelField("The Melee Prefab");
                        MeshPrefab = EditorGUILayout.ObjectField(MeshPrefab, typeof(GameObject), false) as GameObject;

                        if (MeshPrefab != null)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.LabelField("The Melee Options");
                            EditorGUILayout.HelpBox("These can be created in the game database under Melee. ", MessageType.Info);
                            meleeOptionIndex = GUILayout.SelectionGrid(meleeOptionIndex, meleeOptionData, 4);
                            EditorGUILayout.LabelField("Damage Options");
                            EditorGUILayout.HelpBox("These can be created in the game database under Actor Damage. ", MessageType.Info);
                            meldamageOptionIndex = GUILayout.SelectionGrid(meldamageOptionIndex, damageOptions, 4);
                            EditorGUI.indentLevel--;
                        }
                      
                        break;
                    case 1:
                        EditorGUILayout.HelpBox("Projectile Weapons Require a Prefab to work. This should be the model or mesh gameobject, the thing that visually represents your weapon." +
                            "Projectile Weapons also require a projectile gameobject.", MessageType.Info);

                        EditorGUILayout.LabelField("The Projectile Weapon Prefab");
                        MeshPrefab = EditorGUILayout.ObjectField(MeshPrefab, typeof(GameObject), false) as GameObject;
                        EditorGUILayout.LabelField("The Projectile Itself Prefab");
                        Projectile = EditorGUILayout.ObjectField(Projectile, typeof(GameObject), false) as GameObject;
                        
                        if (MeshPrefab && Projectile != null)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.LabelField("The Projectile Options");
                            EditorGUILayout.HelpBox("These can be created in the game database under Projectile. ", MessageType.Info);
                            projectileOptionIndex = GUILayout.SelectionGrid(projectileOptionIndex, projectileOptionData, 4);
                            EditorGUILayout.LabelField("Damage Options");
                            EditorGUILayout.HelpBox("These can be created in the game database under Actor Damage. ", MessageType.Info);
                            projdamageOptionIndex = GUILayout.SelectionGrid(projdamageOptionIndex, damageOptions, 4);
                            EditorGUI.indentLevel--;
                        }
                     
                        break;
                    case 2:
                        //projectile
                        EditorGUI.indentLevel++;
                        EditorGUILayout.LabelField("The Projectile Itself Prefab");
                        Projectile = EditorGUILayout.ObjectField(Projectile, typeof(GameObject), false) as GameObject;

                        if (Projectile != null)
                        {
                            EditorGUILayout.LabelField("The Projectile Options");
                            EditorGUILayout.HelpBox("These can be created in the game database under Projectile. ", MessageType.Info);
                            projectileOptionIndex = GUILayout.SelectionGrid(projectileOptionIndex, projectileOptionData, 4);
                            EditorGUILayout.LabelField("Damage Options");
                            EditorGUILayout.HelpBox("These can be created in the game database under Actor Damage. ", MessageType.Info);
                            projdamageOptionIndex = GUILayout.SelectionGrid(projdamageOptionIndex, damageOptions, 4);
                            EditorGUI.indentLevel--;
                        }
                 
                        break;

                }
              


                EditorGUI.indentLevel--;
            }
            else
            {
                //not a weapon
                EditorGUILayout.LabelField("The Wearable Prefab");
                MeshPrefab = EditorGUILayout.ObjectField(MeshPrefab, typeof(GameObject), false) as GameObject;
            }

            EditorGUILayout.Space(25);
            bool create = GUILayout.Button("Create Wearable");
            if (create)
            {
                if (MeshPrefab == null && weaponTypeSelect != 2)
                {
                    Debug.LogWarning("Can't create wearable without Mesh prefab");
                    return;
                }
                else if (Projectile == null && isWeapon && weaponTypeSelect == 1)
                {
                    Debug.LogWarning("Can't create projectile weapon without Mesh prefab");
                    return;
                }
                else if (Projectile == null && isWeapon && weaponTypeSelect == 2)
                {
                    Debug.LogWarning("Can't create projectile without Mesh prefab");
                    return;
                }

                GameObject parent = new GameObject();

                GameObject meshInstance = null;
                Wearable wearable = null;
               
                
                string path = string.Empty;
                bool conditionsMet = true;
                if (isWeapon)
                {

                    switch (weaponTypeSelect)
                    {
                        case 0:
                            //melee
                            meshInstance = PrefabUtility.InstantiatePrefab(MeshPrefab) as GameObject;
                            meshInstance.transform.SetParent(parent.transform);
                            meshInstance.transform.position = parent.transform.position;
                            meshInstance.transform.rotation = parent.transform.rotation;

                            path = EditorUtility.SaveFilePanelInProject("Wearable Prefab Creator", MeshPrefab.name + "_Wearable", "prefab", "Where to save the prefab?");
                            if (path.Length == 0) return;

                            wearable = parent.AddComponent<Wearable>();
                            wearable.SetMeshTransform(meshInstance.transform);

                            CreateMeleeDD(parent, meshInstance);

                            break;
                        case 1:
                            meshInstance = PrefabUtility.InstantiatePrefab(MeshPrefab) as GameObject;
                            meshInstance.transform.SetParent(parent.transform);
                            meshInstance.transform.position = parent.transform.position;
                            meshInstance.transform.rotation = parent.transform.rotation;

                            path = EditorUtility.SaveFilePanelInProject("Wearable Prefab Creator", MeshPrefab.name + "_Wearable", "prefab", "Where to save the prefab?");
                            if (path.Length == 0) return;

                            wearable = parent.AddComponent<Wearable>();
                            wearable.SetMeshTransform(meshInstance.transform);

                            ProjectileWeapon projWpn = parent.AddComponent<ProjectileWeapon>();

                            GameObject firePoint = new GameObject();
                            firePoint.name = "FirePoint";
                            firePoint.transform.SetParent(parent.transform);
                            firePoint.transform.position = parent.transform.position;
                            firePoint.transform.rotation = parent.transform.rotation;
                            projWpn.SetFirePoint(firePoint.transform);


                            Projectile iproj = Projectile.GetComponent<Projectile>();
                            if (iproj == null)
                            {
                                bool setup = EditorUtility.DisplayDialog("Projectile", "Prefab is not set up to be a projectile. Do you want to set it up now?", "Yes", "No");
                                if (setup)
                                {
                                    string projpath = EditorUtility.SaveFilePanelInProject("Projectile Prefab", Projectile.name + "_Projectile", "prefab", "Where to save the Projectile Prefab?");
                                    if (projpath.Length > 0)
                                    {
                                        CreateProjectile(projWpn, projpath);
                                    }
                                    else
                                    {
                                        Debug.LogWarning("Destroying wearable, can't have projectile weapon without a projectile.");
                                        DestroyImmediate(parent);
                                        conditionsMet = false;
                                        return;
                                    }

                                }
                                else
                                {
                                    Debug.LogWarning("Destroying wearable, can't have projectile weapon without a projectile.");
                                    conditionsMet = false;
                                }
                            }
                            else
                            {
                                projWpn.SetProjectilePrefab(iproj.gameObject);
                            }

                            break;
                        case 2:
                            //projectile
                            path = EditorUtility.SaveFilePanelInProject("Projectile Prefab", Projectile.name + "_Projectile", "prefab", "Where to save the Projectile Prefab?");
                            if (path.Length == 0) return;
                            GameObject projectileInstance = SetupProjectileInstance();
                            bool success = PrefabUtility.SaveAsPrefabAsset(projectileInstance, path);
                            if (success)
                            {
                                DestroyImmediate(projectileInstance);
                                return;
                            }
                           
                            break;
                       
                    }

                    if (conditionsMet)
                    {
                        bool saved = PrefabUtility.SaveAsPrefabAsset(parent, path);
                        if (saved)
                        {
                            Debug.Log("Wearable prefab successfully created at " + path);
                            GameObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                            EditorGUIUtility.PingObject(obj);
                        }
                    }
             
                    //
                    //create the parent
                    //center the mesh to the parent
                    //add the relevant scripts
                    //create a new wearable
                }
                else if (path.Length > 0 && isWeapon == false)
                {


                    bool saved = PrefabUtility.SaveAsPrefabAsset(parent, path);
                    if (saved)
                    {
                        Debug.Log("Wearable prefab successfully created at " + path);
                    }

                }

                DestroyImmediate(parent);
            }


            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void CreateMeleeDD(GameObject parent, GameObject meshInstance)
        {

            switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    //3d
                    Rigidbody rb = parent.GetComponent<Rigidbody>();
                    if (rb == null)
                    {
                        rb = parent.AddComponent<Rigidbody>();
                    }
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    MeleeWeaponDD meldd = parent.AddComponent<MeleeWeaponDD>();
                    Collider[] colliders = meshInstance.GetComponentsInChildren<Collider>();
                    if (colliders.Length == 0)
                    {
                        Debug.LogWarning("A collider is required on the mesh object in order to detect damage. Adding a box collider as a default");
                        meshInstance.AddComponent<BoxCollider>();
                    }
                    break;
                case EditorPhysicsType.Unity2D:
                    //2d
                    Rigidbody2D rb2d = parent.GetComponent<Rigidbody2D>();
                    if (rb2d == null)
                    {
                        rb2d = parent.AddComponent<Rigidbody2D>();
                    }
                    rb2d.gravityScale = 0;
                    rb2d.isKinematic = true;
                    MeleeWeaponDD2D meldd2d = parent.AddComponent<MeleeWeaponDD2D>();
                    Collider2D[] colliders2D = meshInstance.GetComponentsInChildren<Collider2D>();
                    if (colliders2D.Length == 0)
                    {
                        Debug.LogWarning("A collider is required on the mesh object in order to detect damage. Adding a box collider as a default");
                        meshInstance.AddComponent<BoxCollider2D>();
                    }
                    break;

            }

            IMeleeWeapon imelee = parent.GetComponent<IMeleeWeapon>();
            ActorDamageData damageData = gamedatabase.ActorDamageTypes.GetDatabaseObjectBySlotIndex(meldamageOptionIndex) as ActorDamageData;
            imelee.SetDamageData(damageData);
            MeleeData melOptions = gamedatabase.Melee.GetDatabaseObjectBySlotIndex(meleeOptionIndex) as MeleeData;
            imelee.SetMeleeOptionData(melOptions);


        }


        private void CreateProjectile(ProjectileWeapon projWpn, string projpath)
        {
            GameObject projectileInstance = SetupProjectileInstance();

            bool success = PrefabUtility.SaveAsPrefabAsset(projectileInstance, projpath);
            if (success)
            {
                projWpn.SetProjectilePrefab(AssetDatabase.LoadAssetAtPath(projpath, typeof(GameObject)) as GameObject);
            }
            DestroyImmediate(projectileInstance);
        }

        private GameObject SetupProjectileInstance()
        {
            GameObject projectileInstance = PrefabUtility.InstantiatePrefab(Projectile) as GameObject;

            switch (gamedatabase.Settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:
                    //3D
                    Projectile _projwpn = Setup3D(projectileInstance);

                    break;
                case EditorPhysicsType.Unity2D:
                    //2d
                    Projectile2D _projwpn2d = Setup2D(projectileInstance);
                    break;
            }



            IProjectile iprojectile = projectileInstance.GetComponent<IProjectile>();
            ActorDamageData projddata = gamedatabase.ActorDamageTypes.GetDatabaseObjectBySlotIndex(projdamageOptionIndex) as ActorDamageData;
            iprojectile.SetDamageData(projddata);
            ProjectileData projOptions = gamedatabase.Projectiles.GetDatabaseObjectBySlotIndex(projectileOptionIndex) as ProjectileData;
            iprojectile.SetProjectileData(projOptions);

            return projectileInstance;
        }

        private static Projectile Setup3D(GameObject projectileInstance)
        {
            Projectile _projwpn = projectileInstance.AddComponent<Projectile>();
            Rigidbody body = projectileInstance.GetComponent<Rigidbody>();
            if (body == null)
            {
                body = projectileInstance.AddComponent<Rigidbody>();
            }
            Collider collider = projectileInstance.GetComponent<Collider>();
            if (collider == null)
            {
                projectileInstance.AddComponent<BoxCollider>();
            }
            body.useGravity = false;
            body.isKinematic = true;
            return _projwpn;
        }

        private static Projectile2D Setup2D(GameObject projectileInstance)
        {
            Projectile2D _projwpn = projectileInstance.AddComponent<Projectile2D>();
            Rigidbody2D body = projectileInstance.GetComponent<Rigidbody2D>();
            if (body == null)
            {
                body = projectileInstance.AddComponent<Rigidbody2D>();
            }
            Collider2D collider = projectileInstance.GetComponent<Collider2D>();
            if (collider == null)
            {
                projectileInstance.AddComponent<BoxCollider2D>();
            }
            body.gravityScale = 0;
            body.isKinematic = true;
            return _projwpn;
        }
    }

}

#endif