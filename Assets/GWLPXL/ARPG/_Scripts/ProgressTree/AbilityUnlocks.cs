using GWLPXL.ARPGCore.Abilities.com;


namespace GWLPXL.ARPGCore.ProgressTree.com
{
    [System.Serializable]
    public class AbilityUnlocks
    {
        public TreeNodeHolder NodeUnlock;
        public Ability AbilityToUnlock;
        public IAbilityUser AbilityUser;
    }
}