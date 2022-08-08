
using System;
using UnityEngine;

namespace GWLPXL.ARPGCore.ProgressTree.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/ProgressTree/NEW Tree Node Holder")]
    public class TreeNodeHolder : ScriptableObject
    {
        public Action<TreeNodeHolder, bool> Available;

        public TextAsset JsonConfig;
        public ID ID => id;
        [SerializeField]
        protected ID id;
        [SerializeField]
        protected bool autoName = false;
        [SerializeField]
        protected bool autoAssignUniqueID = false;
        public int Invested { get; private set; }
        public bool IsAvailable { get; private set; }
        [Range(0, 255)]
        public int MaxInvestAmount = 5;
        public NodeRequirements[] Requirements = new NodeRequirements[0];
        [TextArea(3,5)]
        public string Description = string.Empty;

        /// <summary>
        /// So people can see what has 'unlocked' or is 'available', even if they haven't reached that tier yet. 
        /// </summary>
        /// <param name="isAvailable"></param>
        public virtual void SetAvailable(bool isAvailable)
        {
            IsAvailable = isAvailable;
            Available?.Invoke(this, IsAvailable);
        }

        /// <summary>
        /// returns amount - divested. Ideally is 0, but will return the remainder if you throw in a bunch.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual int Divest(int amount)
        {
            int divested = 0;
            for (int i = 0; i < amount; i++)
            {
                int potential = Invested - 1;
                if (potential >= 0)
                {
                    divested++;
                }
            }
            Invested -= divested;
            if (Invested > 0)
            {
                SetAvailable(true);
            }
            else
            {
                SetAvailable(false);
            }
            return amount - divested;
        }

        /// <summary>
        /// returns amount - invested. Ideally is 0, but will return the remainder if you throw in too many.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual int Invest(int amount)
        {
            int invested = 0;
            for (int i = 0; i < amount; i++)
            {
                int potential = Invested + 1;
                if (potential <= MaxInvestAmount)
                {
                    invested++;
                }
            }
            Invested += invested;
            if (Invested > 0)
            {
                SetAvailable(true);
            }
            else
            {
                SetAvailable(false);
            }
            return amount - invested;
        }


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            for (int i = 0; i < Requirements.Length; i++)
            {
                if (Requirements[i].RequiredNode == null) continue;
                if (Requirements[i].RequiredInvestment > Requirements[i].RequiredNode.MaxInvestAmount)
                {
                    Debug.LogWarning("Trying to make requirements higher than the max investment allow, I won't allow this", this);
                    Requirements[i].RequiredInvestment = Requirements[i].RequiredNode.MaxInvestAmount;
                }
            }

            if (autoName && string.IsNullOrEmpty(id.Name) == false)
            {
                //rename asset
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, this.GetType().Name + "_" + id.Name);
            }

            if (autoAssignUniqueID && id.UniqueID != this.GetInstanceID())
            {
                id.UniqueID = this.GetInstanceID();
            }
        }
#endif
    }
}