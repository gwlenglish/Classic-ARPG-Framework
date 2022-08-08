namespace GWLPXL.ARPGCore.ProgressTree.com
{
    /// <summary>
    /// Defines a required node and the amount invested into that required node. 
    /// </summary>
    [System.Serializable]
    public class NodeRequirements
    {
        public TreeNodeHolder RequiredNode = null;
        public int RequiredInvestment = 0;

        public  NodeRequirements(TreeNodeHolder requiredNode, int requiredInvest)
        {
            RequiredNode = requiredNode;
            RequiredInvestment = requiredInvest;
        }
    }
}