using UnityEngine;

namespace GWLPXL.ARPGCore.ProgressTree.com
{

   [System.Serializable]
    public class TreeData 
    {
        //keyed to the index
        public int TierLevel { get; private set; }
        public TreeNodeHolder[] NodesOnLevel => nodesOnLevel;

        public string TierDescription = string.Empty;
        public bool TierAvailable => tierAvailable;
        [SerializeField]
        bool tierAvailable = false;

        [SerializeField]
        TreeNodeHolder[] nodesOnLevel = new TreeNodeHolder[0];
        
        public void SetTierLevel(int tier)
        {
            TierLevel = tier;
        }
        public void SetTierAvailable(bool isAvailable)
        {
            tierAvailable = isAvailable;
        }
        public TreeData(int tier, bool available, TreeNodeHolder[] nodes)
        {
            TierLevel = tier;
            tierAvailable = available;
            nodesOnLevel = nodes;
        }

    }
}