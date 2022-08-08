
using System.Collections.Generic;

using UnityEngine;

namespace GWLPXL.ARPGCore.ProgressTree.com
{
    /// <summary>
    /// Basic tree unlock system, root is index 0 and traverses +1 from there. 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/ProgressTree/NEW Progress Tree")]
    public class ProgressTreeHolder : ScriptableObject
    {
        public TextAsset JsonConfig;

        [SerializeField]
        protected ID ID;
        [SerializeField]
        protected bool autoName = false;
        [SerializeField]
        protected bool autoAssignID = false;
        public int PointsAvailable => pointsAvailable;
        [Tooltip("Points that can be spent into the tree")]
        [SerializeField]
        protected int pointsAvailable = 1;
        [Tooltip("The tree template. Runtime values must be changed through code, this is only a template that is copied.")]
        [SerializeField]
        protected TreeData[] theTree = new TreeData[0];

        #region runtime dics
        [System.NonSerialized]
        protected Dictionary<int, bool> tierAvailable = new Dictionary<int, bool>();//quick check to see if tier is available.
        [System.NonSerialized]
        protected Dictionary<int, TreeNodeHolder[]> perTier = new Dictionary<int, TreeNodeHolder[]>();//check if tier is unlocked or what tier a node is on
        [System.NonSerialized]
        protected Dictionary<TreeNodeHolder, RuntimeDataNode> runtimeDic = new Dictionary<TreeNodeHolder, RuntimeDataNode>();//dic that holds the runtime copies, main one
        #endregion

        #region public

        /// <summary>
        /// sets the points available.
        /// </summary>
        /// <param name="newAvailable"></param>
        public virtual void SetPointsAvailable(int newAvailable)
        {
            pointsAvailable = newAvailable;
        }
      
        public virtual TreeNodeHolder GetRuntimeNode(TreeNodeHolder template)
        {
            return GetMyNode(template);
        }
        /// <summary>
        /// Makes available a tier, will refund points if a tier is disabled and points are invested in that tier.
        /// </summary>
        /// <param name="tier"></param>
        /// <param name="isAvailable"></param>
        /// 
        public virtual void SetTierAvailable(int tier, bool isAvailable)
        {
           
            //main function
            bool value = GetTierAvailable(tier);
            //tierAvailable.TryGetValue(tier, out bool value);
            if (isAvailable == false)
            {
                int refundAmount = 0;
                perTier.TryGetValue(tier, out TreeNodeHolder[] nodes);
                for (int j = 0; j < nodes.Length; j++)
                {
                    TreeNodeHolder runtime = GetMyNode(nodes[j]);
                    if (runtime.Invested > 0)
                    {
                        int refund = runtime.Invested;
                        runtime.Divest(refund);
                        refundAmount += refund;
                    }

                    //update those that require me
                    TreeNodeHolder[] requirementFor = AmIARequirement(nodes[j]);
                    for (int i = 0; i < requirementFor.Length; i++)
                    {
                        TreeNodeHolder copy = GetMyNode(requirementFor[i]);
                        copy.SetAvailable(false);
                    }
                }

                perTier[tier] = nodes;
                SetPointsAvailable(pointsAvailable + refundAmount);


            }


            bool oldvalue = value;
            value = isAvailable;
            tierAvailable[tier] = value;

            //ugly recursion, refunds all the nodes to tier 0
            //if the tier was available, but then we made it unavailable, we refund all the way up the root "respec"
            if ((oldvalue == true && isAvailable == false) && tier < perTier.Keys.Count - 1)
            {
                SetTierAvailable(tier + 1, false);
            }


        }

        /// <summary>
        /// to remove points from a particular node in the tree, returns the amount minus any points we couldn't allocate
        /// </summary>
        /// <param name="template"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// 
        public virtual int Divest(TreeNodeHolder template, int amount)
        {
            //check node
            TreeNodeHolder runtime = CheckPreconditions(template);
            if (runtime == null)
            {
                //doesnt exist
                return amount;
            }

            if (runtime.Invested <= 0)
            {
                //nothing to divest
                return amount;
            }
            //check tier
            bool tierAvailable = CheckTierConditions(template);
            if (tierAvailable == false)
            {
                //cant affect tier
                return amount;
            }

            //divest
            int leftovers = runtime.Divest(amount);

            //set availability
            if (runtime.Invested > 0)
            {
                runtime.SetAvailable(true);
            }
            else
            {
                runtime.SetAvailable(false);
                //update requirements if any. 

            }

            //set requirements / refund if nec
            if (runtime.Invested >= 1)
            {
                //enable 
                TreeNodeHolder[] requirementFor = AmIARequirement(template);
                for (int i = 0; i < requirementFor.Length; i++)
                {
                    //refund
                    TreeNodeHolder copy = GetMyNode(requirementFor[i]);
                    copy.SetAvailable(true);
                }
            }
            else
            {
                //disable
                TreeNodeHolder[] requirementFor = AmIARequirement(template);
                for (int i = 0; i < requirementFor.Length; i++)
                {
                    //refund
                    TreeNodeHolder copy = GetMyNode(requirementFor[i]);
                    int invested = copy.Invested;
                    copy.Divest(invested);
                    amount += invested;
                    copy.SetAvailable(false);
                }
            }

            //assign points
            SetPointsAvailable(pointsAvailable + (amount - leftovers));
            return amount - leftovers;

        }

        /// <summary>
        /// to add points to a particular node in the tree, returns amount minus any points we couldn't allocate
        /// </summary>
        /// <param name="template"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual int Invest(TreeNodeHolder template, int amount)
        {
            int capSpending = 0;
            for (int i = 0; i < amount; i++)
            {
                int potential = pointsAvailable - i;
                if (potential > 0)
                {
                    capSpending += 1;
                }
            }
            if (capSpending <= 0)
            {
                //not enough
                return capSpending;
            }
            amount = capSpending;

            //check node
            TreeNodeHolder runtime = CheckPreconditions(template);
            if (runtime == null)
            {
                //doesnt exist
                return amount;
            }



            //check unlock requirements
            if (CheckRequirements(template) == false)
            {
                return amount;
            }


            //check tier
            bool tierAvailable = CheckTierConditions(template);
            if (tierAvailable == false)
            {
                //cant affect tier
                return amount;
            }

            //invest
            int leftovers = runtime.Invest(amount);

            //set availability
            if (runtime.Invested > 0)
            {
                runtime.SetAvailable(true);

            }
            else
            {
                runtime.SetAvailable(false);

            }

            //set requirements / refund if nec
            if (runtime.Invested >= 1)
            {
                //enable 
                TreeNodeHolder[] requirementFor = AmIARequirement(template);
                for (int i = 0; i < requirementFor.Length; i++)
                {
                    //refund
                    TreeNodeHolder copy = GetMyNode(requirementFor[i]);
                    copy.SetAvailable(true);
                }
            }
            else
            {
                //disable
                TreeNodeHolder[] requirementFor = AmIARequirement(template);
                for (int i = 0; i < requirementFor.Length; i++)
                {
                    //refund
                    TreeNodeHolder copy = GetMyNode(requirementFor[i]);
                    int invested = copy.Invested;
                    copy.Divest(invested);
                    amount += invested;
                    copy.SetAvailable(false);
                }
            }

            //assign points
            SetPointsAvailable(pointsAvailable - (amount - leftovers));
            return amount - leftovers;


        }

        /// <summary>
        /// Returns the invested amount of the template node 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual int GetNodeInvestment(TreeNodeHolder template)
        {
            TreeNodeHolder runtime = GetMyNode(template);
            return runtime.Invested;
        }

        /// <summary>
        /// Returns the tier node currently resides on
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual int GetNodeTier(TreeNodeHolder template)
        {
            if (runtimeDic.Count == 0)
            {
                LoadRuntimeDic();
            }
            runtimeDic.TryGetValue(template, out RuntimeDataNode value);
            return value.MyTier;
        }

        /// <summary>
        /// Returns tier is available
        /// </summary>
        /// <param name="tier"></param>
        /// <returns></returns>
        public virtual bool GetTierAvailable(int tier)
        {
            if (tierAvailable.Count == 0 || perTier.Count == 0)
            {
                LoadRuntimeDic();
            }

            tierAvailable.TryGetValue(tier, out bool value);
            return value;

        }

        /// <summary>
        /// Returns node available and node exists
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual bool GetAvailableStatus(TreeNodeHolder template)
        {

            if (runtimeDic.Count == 0)
            {
                LoadRuntimeDic();
            }
            runtimeDic.TryGetValue(template, out RuntimeDataNode value);
            bool requirements = CheckRequirements(template);
            return (value.RuntimeCopy.IsAvailable && requirements == true);


        }

        public virtual bool GetUnlockStatus(TreeNodeHolder template)
        {
            if (runtimeDic.Count == 0)
            {
                LoadRuntimeDic();
            }
            runtimeDic.TryGetValue(template, out RuntimeDataNode value);
            return value.RuntimeCopy.Invested >= 1;
        }
        /// <summary>
        /// Returns the tier available and the node available
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual bool GetAvailability(TreeNodeHolder template)
        {

            //we traverse the tiers to find the node
            int tier = 0;
            foreach (var kvp in perTier)
            {
                for (int i = 0; i < kvp.Value.Length; i++)
                {
                    if (kvp.Value[i] == template)
                    {
                        //we found it.
                        tier = kvp.Key;
                        break;
                    }
                }
            }

            //is this tier even available?
            bool availableTier = GetTierAvailable(tier);

            if (availableTier == false)
            {
                Debug.Log("Tier isn't unlocked");
                return false;
            }

            return availableTier && GetAvailableStatus(template);

        }


        #endregion

        #region protected
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected virtual bool CheckTierConditions(TreeNodeHolder template)
        {
            int tier = GetNodeTier(template);
            return GetTierAvailable(tier);
        }
        /// <summary>
        /// main method to retrieve the runtime copy based on the original scriptable
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        /// 
        protected TreeNodeHolder GetMyNode(TreeNodeHolder template)
        {
            if (runtimeDic.Count == 0)
            {
                LoadRuntimeDic();
            }
            runtimeDic.TryGetValue(template, out RuntimeDataNode value);
            return value.RuntimeCopy;
        }
        /// <summary>
        /// for invest and divest, check conditions and return our node runtime
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected TreeNodeHolder CheckPreconditions(TreeNodeHolder template)
        {
            //check dictionaries
            if (runtimeDic.Count == 0)
            {
                LoadRuntimeDic();
            }

            //check node
            TreeNodeHolder runtime = GetMyNode(template);
            return runtime;
        }
        /// <summary>
        /// Returns an array of nodes that require me.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected TreeNodeHolder[] AmIARequirement(TreeNodeHolder template)
        {
            if (runtimeDic.Count == 0)
            {
                LoadRuntimeDic();
            }
            List<TreeNodeHolder> _temp = new List<TreeNodeHolder>();
            foreach (var kvp in runtimeDic)
            {
                for (int i = 0; i < kvp.Value.Requirements.Length; i++)
                {
                    if (template == kvp.Value.Requirements[i].RequiredNode)
                    {
                        _temp.Add(kvp.Key);
                    }
                }
            }
            return _temp.ToArray();
        }
        /// <summary>
        /// returns the requirement condition
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected bool CheckRequirements(TreeNodeHolder template)
        {
            if (runtimeDic.Count == 0)
            {
                LoadRuntimeDic();
            }
            runtimeDic.TryGetValue(template, out RuntimeDataNode value);
            for (int i = 0; i < value.Requirements.Length; i++)
            {
                TreeNodeHolder requiredNode = GetMyNode(value.Requirements[i].RequiredNode);
                int requiredAmount = value.Requirements[i].RequiredInvestment;

                if (requiredNode.Invested < requiredAmount)
                {
                    //can't invest, don't meet requirements. 
                    return false;

                }
            }
            return value != null;
        }
        #endregion

        #region region that initializes dictionaries

        protected void LoadRuntimeDic()
        {
            for (int i = 0; i < theTree.Length; i++)
            {
                theTree[i].SetTierLevel(i);
                perTier.Add(i, theTree[i].NodesOnLevel);
                tierAvailable.Add(i, theTree[i].TierAvailable);
            }

            for (int i = 0; i < theTree.Length; i++)
            {
                for (int j = 0; j < theTree[i].NodesOnLevel.Length; j++)
                {
                    TreeNodeHolder copy = ScriptableObject.Instantiate(theTree[i].NodesOnLevel[j]);

                    NodeRequirements[] requirements = new NodeRequirements[theTree[i].NodesOnLevel[j].Requirements.Length];
                    for (int k = 0; k < theTree[i].NodesOnLevel[j].Requirements.Length; k++)
                    {
                        NodeRequirements requirement = new NodeRequirements(theTree[i].NodesOnLevel[j].Requirements[k].RequiredNode,
                            theTree[i].NodesOnLevel[j].Requirements[k].RequiredInvestment);

                        requirements[k] = requirement;
                    }
                    //NodeRequirements[] requirements = theTree[i].NodesOnLevel[j].Requirements;
                    RuntimeDataNode newData = new RuntimeDataNode(copy, theTree[i].TierLevel, requirements);
                    runtimeDic.Add(theTree[i].NodesOnLevel[j], newData);
                }
            }
        }

        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// force tiers to index
        /// </summary>
        protected virtual void OnValidate()
        {
            ///forces tier to be linked to index
            for (int i = 0; i < theTree.Length; i++)
            {
                theTree[i].SetTierLevel(i);

            }

            if (autoName && string.IsNullOrEmpty(ID.Name) == false)
            {
                //rename asset
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, this.GetType().Name + "_" + ID.Name);
            }

            if (autoAssignID && ID.UniqueID != this.GetInstanceID())
            {
                ID.UniqueID = this.GetInstanceID();
            }

           
        }
#endif
    }
}