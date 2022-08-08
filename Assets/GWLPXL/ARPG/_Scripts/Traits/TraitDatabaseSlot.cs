namespace GWLPXL.ARPGCore.Traits.com
{

        [System.Serializable]
        public class TraitDatabaseSlot
        {
            public string DescriptiveName;
            public EquipmentTraitID ID;
            public EquipmentTrait Trait;
            public TraitDatabaseSlot(EquipmentTraitID id, EquipmentTrait trait)
            {
                ID = id;
                Trait = trait;
                DescriptiveName = trait.GetTraitName();
                trait.SetTraitID(id);
            }
        }
    

}

