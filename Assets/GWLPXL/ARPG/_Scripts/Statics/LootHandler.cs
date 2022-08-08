using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using UnityEngine;
namespace GWLPXL.ARPGCore.Statics.com
{
    public static class LootHandler
    {
        public static ILoot DropLoot(Item forItem, Vector3 atLocation, GameObject lootPrefab, float delay, Vector3 withRandomOffset)
        {
            if (lootPrefab == null) return null;
            GameObject obj = UnityEngine.Object.Instantiate(lootPrefab, atLocation + withRandomOffset, Quaternion.identity);
            ILoot newdropLoot = obj.GetComponent<ILoot>();
            newdropLoot.IniLoot(forItem, delay);
            return newdropLoot;
        }
        public static ILoot DropLoot(Item forItem, Vector3 atLocation, GameObject lootPrefab, float delay)
        {
            if (lootPrefab == null) return null;

            GameObject newDropLoot = UnityEngine.Object.Instantiate(lootPrefab, atLocation, Quaternion.identity);
            ILoot newloot = newDropLoot.GetComponent<ILoot>();
            newloot.IniLoot(forItem, delay);
            return newloot;
        }
        public static ILoot DropLoot(Item forItem, Vector3 atLocation, GameObject lootPrefab)
        {
            if (lootPrefab == null) return null;
            Vector3 randomUnit = Random.insideUnitSphere;
            GameObject newDropLoot = UnityEngine.Object.Instantiate(lootPrefab, atLocation, Quaternion.identity);
            ILoot newLoot = newDropLoot.GetComponent<ILoot>();
            newLoot.IniLoot(forItem, newLoot.GetLootOptions().DelayBeforeActive);
            return newLoot;
        }
    }
}