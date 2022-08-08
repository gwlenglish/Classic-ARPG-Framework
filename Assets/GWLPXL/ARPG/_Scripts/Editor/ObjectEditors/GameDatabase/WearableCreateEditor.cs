using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Wearables.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class WearableCreateEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (CreateWearableEditorModel) target;
            var wearableOptions = model.options;
            
            model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType = 
                (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType);
            var physicsType = model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType;
            EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                                    "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
            
            ArpgEditorHelper.DrawHorizontalToggleField(ref wearableOptions.IsWeapon, "Is Weapon?");
            if (wearableOptions.IsWeapon)
            {
                EditorGUILayout.HelpBox("Melee weapons are stationary and require contact in order to damage, for example an Axe. " +
                    "Projectile Weapons shoot objects that do damage, for example a bow.", MessageType.Info);
                ArpgEditorHelper.DrawPopupWithLabel(ref wearableOptions.WeaponTypeSelect, "Weapon Type", model.WeaponTypes);
                EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
                //EditorGUI.indentLevel++;
                switch (wearableOptions.WeaponTypeSelect)
                {
                    case 0:
                        EditorGUILayout.HelpBox("Melee Weapons Require a Prefab to work. This should be the model or mesh gameobject, the thing that visually represents your weapon.", MessageType.Info);
                        
                        ArpgEditorHelper.DrawHorizontalObjectField(ref wearableOptions.MeshPrefab, "The Melee Prefab");

                        if (wearableOptions.MeshPrefab != null)
                        {
                            //EditorGUI.indentLevel++;
                            EditorGUILayout.HelpBox("These can be created in the game database under Melee. ", MessageType.Info);
                            
                            ArpgEditorHelper.DrawPopupWithLabel(ref wearableOptions.MeleeIndex, "The Melee Options", model.MeleeDataOptions);
                            
                            EditorGUILayout.HelpBox("These can be created in the game database under Actor Damage. ", MessageType.Info);
                            ArpgEditorHelper.DrawPopupWithLabel(ref wearableOptions.MeleeDamageTypeIndex, "The Damage Type Options", model.DamageTypeOptions);
                            
                            //EditorGUI.indentLevel--;
                        }
                      
                        break;
                    case 1:
                        EditorGUILayout.HelpBox("Projectile Weapons Require a Prefab to work. This should be the model or mesh gameobject, the thing that visually represents your weapon." +
                            "Projectile Weapons also require a projectile gameobject.", MessageType.Info);
                        ArpgEditorHelper.DrawHorizontalObjectField(ref wearableOptions.MeshPrefab, "The Projectile Weapon Prefab");
                        ArpgEditorHelper.DrawHorizontalObjectField(ref wearableOptions.Projectile, "The Projectile Itself Prefab");
                        
                        if (wearableOptions.MeshPrefab != null && wearableOptions.Projectile != null)
                        {
                            //EditorGUI.indentLevel++;
                            ArpgEditorHelper.DrawPopupWithLabel(ref wearableOptions.ProjectileIndex, "The Projectile Options", model.ProjectileOptions);
                            ArpgEditorHelper.DrawPopupWithLabel(ref wearableOptions.ProjectileDamageTypeIndex, "The Damage Type Options", model.DamageTypeOptions);
                            //EditorGUI.indentLevel--;
                        }
                     
                        break;
                    case 2:
                        //projectile
                        //EditorGUI.indentLevel++;
                        ArpgEditorHelper.DrawHorizontalObjectField(ref wearableOptions.Projectile, "The Projectile Itself Prefab");

                        if (wearableOptions.Projectile != null)
                        {
                            EditorGUILayout.HelpBox("These can be created in the game database under Projectile. ", MessageType.Info);
                            ArpgEditorHelper.DrawPopupWithLabel(ref wearableOptions.ProjectileIndex, "The Projectile Options", model.ProjectileOptions);
                            
                            EditorGUILayout.HelpBox("These can be created in the game database under Actor Damage. ", MessageType.Info);
                            ArpgEditorHelper.DrawPopupWithLabel(ref wearableOptions.ProjectileDamageTypeIndex, "The Damage Type Options", model.DamageTypeOptions);
                            //EditorGUI.indentLevel--;
                        }
                 
                        break;
                }
            }
            else
            {
                ArpgEditorHelper.DrawHorizontalObjectField(ref wearableOptions.MeshPrefab, "The Wearable Prefab");
            }
            
            if (GUILayout.Button("Create Wearable"))
            {
                if (wearableOptions.MeshPrefab == null && wearableOptions.WeaponTypeSelect != 2)
                {
                    Debug.LogWarning("Can't create wearable without Mesh prefab");
                    return;
                }
                else if (wearableOptions.Projectile == null && wearableOptions.IsWeapon && wearableOptions.WeaponTypeSelect == 1)
                {
                    Debug.LogWarning("Can't create projectile weapon without Mesh prefab");
                    return;
                }
                else if (wearableOptions.Projectile == null && wearableOptions.IsWeapon && wearableOptions.WeaponTypeSelect == 2)
                {
                    Debug.LogWarning("Can't create projectile without Mesh prefab");
                    return;
                }

                GameObject parent = new GameObject();

                GameObject meshInstance = null;
                Wearable wearable = null;
               
                
                string path = string.Empty;
                bool conditionsMet = true;
                if (wearableOptions.IsWeapon)
                {
                    switch (wearableOptions.WeaponTypeSelect)
                    {
                        case 0:
                            //melee
                            meshInstance = PrefabUtility.InstantiatePrefab(wearableOptions.MeshPrefab) as GameObject;
                            meshInstance.transform.SetParent(parent.transform);
                            meshInstance.transform.position = parent.transform.position;
                            meshInstance.transform.rotation = parent.transform.rotation;

                            path = EditorUtility.SaveFilePanelInProject("Wearable Prefab Creator", wearableOptions.MeshPrefab.name + "_Wearable", "prefab", "Where to save the prefab?");
                            if (path.Length == 0) return;

                            wearable = parent.AddComponent<Wearable>();
                            wearable.SetMeshTransform(meshInstance.transform);

                            CreateMeleeDD(parent, meshInstance);

                            break;
                        case 1:
                            meshInstance = PrefabUtility.InstantiatePrefab(wearableOptions.MeshPrefab) as GameObject;
                            meshInstance.transform.SetParent(parent.transform);
                            meshInstance.transform.position = parent.transform.position;
                            meshInstance.transform.rotation = parent.transform.rotation;

                            path = EditorUtility.SaveFilePanelInProject("Wearable Prefab Creator", wearableOptions.MeshPrefab.name + "_Wearable", "prefab", "Where to save the prefab?");
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


                            Projectile iproj = wearableOptions.Projectile.GetComponent<Projectile>();
                            if (iproj == null)
                            {
                                bool setup = EditorUtility.DisplayDialog("Projectile", "Prefab is not set up to be a projectile. Do you want to set it up now?", "Yes", "No");
                                if (setup)
                                {
                                    string projpath = EditorUtility.SaveFilePanelInProject("Projectile Prefab", wearableOptions.Projectile.name + "_Projectile", "prefab", "Where to save the Projectile Prefab?");
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
                            path = EditorUtility.SaveFilePanelInProject("Projectile Prefab", wearableOptions.Projectile.name + "_Projectile", "prefab", "Where to save the Projectile Prefab?");
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
                else if (path.Length > 0 && wearableOptions.IsWeapon == false)
                {
                    bool saved = PrefabUtility.SaveAsPrefabAsset(parent, path);
                    if (saved)
                    {
                        Debug.Log("Wearable prefab successfully created at " + path);
                    }
                }
                DestroyImmediate(parent);
            }
            
        }
        
        private void CreateMeleeDD(GameObject parent, GameObject meshInstance)
        {
            var model = (CreateWearableEditorModel) target;

            switch (model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType)
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
                    var rb2d = parent.GetComponent<Rigidbody2D>();
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
            ActorDamageData damageData = model.GameDatabase.ActorDamageTypes.GetDatabaseObjectBySlotIndex(model.options.MeleeDamageTypeIndex) as ActorDamageData;
            imelee.SetDamageData(damageData);
            MeleeData melOptions = model.GameDatabase.Melee.GetDatabaseObjectBySlotIndex(model.options.MeleeIndex) as MeleeData;
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
            var model = (CreateWearableEditorModel) target;
            GameObject projectileInstance = PrefabUtility.InstantiatePrefab(model.options.Projectile) as GameObject;

            switch (model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType)
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
            ActorDamageData projddata = model.GameDatabase.ActorDamageTypes.GetDatabaseObjectBySlotIndex(model.options.ProjectileDamageTypeIndex) as ActorDamageData;
            iprojectile.SetDamageData(projddata);
            ProjectileData projOptions = model.GameDatabase.Projectiles.GetDatabaseObjectBySlotIndex(model.options.ProjectileIndex) as ProjectileData;
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