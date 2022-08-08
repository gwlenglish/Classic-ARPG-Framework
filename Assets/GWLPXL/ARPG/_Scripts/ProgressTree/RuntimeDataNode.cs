namespace GWLPXL.ARPGCore.ProgressTree.com
{
    /// <summary>
    /// What the tree builds in its dictionary to maintain runtime copies and only modify those.
    /// </summary>
    [System.Serializable]
    public class RuntimeDataNode
    {
        public TreeNodeHolder RuntimeCopy { get; private set; }
        public int MyTier { get; private set; }
        public NodeRequirements[] Requirements { get; private set; }

        public RuntimeDataNode(TreeNodeHolder runtimeCopy, int myTier, NodeRequirements[] requirements)
        {
            RuntimeCopy = runtimeCopy;
            MyTier = myTier;
            Requirements = requirements;
        }

    }
}