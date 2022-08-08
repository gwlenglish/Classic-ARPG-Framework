namespace GWLPXL.ARPGCore.Traits.com
{
    [System.Serializable]
    public class EquipmentTraitID
    {
        public string Name;
        public int ID;
        public EquipmentTrait Trait;
        public EquipmentTraitID(string name, int id, EquipmentTrait trait)
        {
            Name = name;
            ID = id;
            Trait = trait;
        }
    }
   

}

