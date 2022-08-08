using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreateWearableEditorModel: ScriptableObject
    {
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;
        public WearableOptions options = new WearableOptions();
        
        public string[] WeaponTypes = { "Melee Weapon", "Projectile Weapon", "Projectile" };
        
        public string[] MeleeDataOptions;
        public string[] ProjectileOptions;
        public string[] DamageTypeOptions;
        
        public void Setup(GWLPXL.ARPGCore.com.GameDatabase gameDatabase)
        {
            GameDatabase = gameDatabase;

            MeleeDataOptions = GameDatabase.Melee.GetAllNames();
            ProjectileOptions = GameDatabase.Projectiles.GetAllNames();
            DamageTypeOptions = GameDatabase.ActorDamageTypes.GetAllNames();
        }

        public class WearableOptions
        {
            public bool IsWeapon;
            public GameObject MeshPrefab;
            public GameObject Projectile;
            public int WeaponTypeSelect;
            
            public int ProjectileDamageTypeIndex;
            public int ProjectileIndex;
            public int MeleeDamageTypeIndex;
            public int MeleeIndex;
        }
    }
}