namespace GWLPXL.ARPGCore.ProgressTree.com
{

    [System.Serializable]
    public class DataLeaf
    {
        public int TierLevel;
        public bool TierAvailable;
        public TreeNodeHolder[] NodesOnLevel;

        public DataLeaf(int tier, bool available, TreeNodeHolder[] nodes)
        {
            TierLevel = tier;
            TierAvailable = available;
            NodesOnLevel = nodes;
        }
    }
}