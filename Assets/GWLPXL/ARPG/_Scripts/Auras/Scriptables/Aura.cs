
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;


namespace GWLPXL.ARPGCore.Auras.com
{
    /// <summary>
    /// Contains the Aura Data and creates the IAura instance per ITakeAura per the Apply(ItakeAura onUser) call, and keeps track of its existence and what it is affecting.
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Auras/NEW_Aura")]
    public class Aura : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config;
        [SerializeField]
        AuraID auraID;
        public AuraData AuraData;

        [System.NonSerialized]
        protected Dictionary<ITakeAura, GameObject> auraInstance = new Dictionary<ITakeAura, GameObject>();
        [System.NonSerialized]//use dictionary over list because key lookup for contains is faster, also use this dictionary to determine if a aura has been applied or not
        protected Dictionary<ITakeAura, Dictionary<ITakeAura, bool>> userAffected = new Dictionary<ITakeAura, Dictionary<ITakeAura, bool>>();

        //cache for collider 3d
        protected List<Collider> rangeList = new List<Collider>();
        protected List<Collider2D> rangeList2D = new List<Collider2D>();
        #region public

        public AuraID GetID() => auraID;
        public void SetID(AuraID newID) => auraID = newID;
        public virtual bool HasAuraInstance(ITakeAura user)
        {
            auraInstance.TryGetValue(user, out GameObject instance);
            if (instance == null) return false;
            return true;
        }
        public virtual Dictionary<ITakeAura, Dictionary<ITakeAura, bool>> GetAllOthersApplied()
        {
            Dictionary<ITakeAura, Dictionary<ITakeAura, bool>> _tempCopy = new Dictionary<ITakeAura, Dictionary<ITakeAura, bool>>();
            _tempCopy = userAffected;
            return _tempCopy;
        }

        public virtual IReadOnlyDictionary<ITakeAura, bool> GetOthersApplied(ITakeAura fromUser)
        {
            userAffected.TryGetValue(fromUser, out Dictionary<ITakeAura, bool> value);
            IReadOnlyDictionary<Aura, bool> readOnly = (IReadOnlyDictionary<Aura, bool>)value;
            return value;
        }

        public virtual int GetCategory() => (int)AuraData.auraCategory;
        public virtual bool HasAOE() => AuraData.AOE && AuraData.AreaRadius > 0;
        public virtual bool HasPulse() => AuraData.Pulses & AuraData.PulseRate > 0;
        /// <summary>
        /// Applies an instance of the Aura only if an Instance doesn't already exist.
        /// </summary>
        public virtual void Apply(ITakeAura onUser)
        {
            //apply the logic
            if (CheckApplyConditions(onUser))
            {
                userAffected[onUser] = new Dictionary<ITakeAura, bool>();
                SetLifeTimeObjs(onUser);
                AppliedAuraLogic(onUser);


            }

        }
        /// <summary>
        /// Removes an instance of the Aura only if an Instance already exists
        /// </summary>
        public virtual void Remove(ITakeAura fromUser)
        {
            if (CheckRemoveConditions(fromUser))
            {
                RemoveLifetimeObjs(fromUser);
                RemovedAuraLogic(fromUser);

                userAffected.Remove(fromUser);
            }
        }

        public virtual void TryDoAOE(ITakeAura fromUser)
        {
            if (HasAOE() == false) return;
            DoAOE(fromUser);
        }
        public virtual void TryDoPulse(ITakeAura fromUser)
        {
            if (HasPulse() == false) return;
            DoPulse(fromUser);
        }

        #endregion

        #region protected
        protected virtual void SetLifeTimeObjs(ITakeAura onUser)
        {

            //creates instance of Aura
            GameObject auraHolder = new GameObject();
            Transform parent = onUser.GetGameObjectInstance().transform;
            auraHolder.transform.position = parent.transform.position;
            auraHolder.transform.SetParent(parent);
            auraHolder.name = AuraData.AuraName + "_AURA";

            if (AuraData.AuraPrefab != null)
            {
                //this becomes childed to the main holder.
                GameObject objInstance = Instantiate(AuraData.AuraPrefab, auraHolder.transform);
            }
            auraInstance[onUser] = auraHolder;//add it to dictionary

            if (HasPulse())
            {
                //create pulse specific lifetime objs
                AuraTimerPulse auraPulseTimer = auraHolder.AddComponent<AuraTimerPulse>();
                auraPulseTimer.AuraData = this;
                auraPulseTimer.Myuser = onUser;
            }

            if (HasAOE())
            {
                //create aoe specific lifetime objs
                DoAOE(onUser);
                AuraTimerArea auraCheckAreaTimer = auraHolder.AddComponent<AuraTimerArea>();
                auraCheckAreaTimer.AuraData = this;
                auraCheckAreaTimer.MyUser = onUser;

            }


        }

        bool CheckApplyConditions(ITakeAura onUser)
        {
            if (userAffected.ContainsKey(onUser)) return false;
            auraInstance.TryGetValue(onUser, out GameObject value);
            if (value != null)
            {
                //destroy the lifetime objs, no idea why we still have them.
                Destroy(value);
                auraInstance[onUser] = null;
                Debug.Log("Derstroying Lifetime Objs");
            }
            return true;
           
        }

        bool CheckRemoveConditions(ITakeAura fromUser)
        {
            if (userAffected.ContainsKey(fromUser)) return true;
            return false;
        }

        bool AppliedAuraLogic(ITakeAura onUser)
        {
            //get our groups
            AuraTargetGroup[] mygroup = onUser.GetAuraGroups();
            //check our groups against the allowed groups
            for (int i = 0; i < AuraData.AuraGroups.Length; i++)
            {
                for (int j = 0; j < mygroup.Length; j++)
                {
                    //they are on the group, allow application.
                    if (mygroup[j] == AuraData.AuraGroups[i])
                    {
                        AuraData.AuraLogics[j].DoApplyLogic(onUser);
                       
                        //we only apply once, so now return true
                        return true;
                    }

                }
            }
            return false;
        }

        bool RemovedAuraLogic(ITakeAura fromUser)
        {
            for (int i = 0; i < AuraData.AuraLogics.Length; i++)
            {
                AuraData.AuraLogics[i].DoRemoveLogic(fromUser);
            }
            return true;
        }

        protected virtual void RemoveLifetimeObjs(ITakeAura fromUser)
        {
            //grab our aura instance
            auraInstance.TryGetValue(fromUser, out GameObject value);
            if (value == null)
            {
                Debug.LogError("Trying to remove an Aura that doesn't have an instance. Was it destroyed somehow?");
            }
            //destroy the gameobject, which also houses the liftime timers
            if (value != null)
            {
                Destroy(value);
            }
            //the aura instance is now null
            auraInstance[fromUser] = null;

            userAffected.TryGetValue(fromUser, out Dictionary<ITakeAura, bool> users);
            foreach (var kvp in users)
            {
                RemovedAuraLogic(kvp.Key);
            }
            //clear the dictionary
            users.Clear();
            //we remove because we're using the key contains to check
            userAffected.Remove(fromUser);
        }

        /// <summary>
        /// Grabs Colliders in range using Physics Overlap Sphere
        /// </summary>
        /// 
        protected virtual List<Collider> GetCollidersInRange(Vector3 centerWorldCoords)
        {
            rangeList.Clear();
            for (int i = 0; i < AuraData.LayersToCheck.Length; i++)
            {
                Collider[] colls = Physics.OverlapSphere(centerWorldCoords, AuraData.AreaRadius, AuraData.LayersToCheck[i]);
                for (int j = 0; j < colls.Length; j++)
                {
                    rangeList.Add(colls[j]);
                }

            }
            return rangeList;
        }
        protected virtual List<Collider2D> GetCollidersInRange2D(Vector3 centerWorldCoords)
        {
            rangeList2D.Clear();
            for (int i = 0; i < AuraData.LayersToCheck.Length; i++)
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(centerWorldCoords, AuraData.AreaRadius, AuraData.LayersToCheck[i]);
                for (int j = 0; j < colls.Length; j++)
                {
                    rangeList2D.Add(colls[j]);
                }

            }
            return rangeList2D;
        }

        /// <summary>
        /// Checks objects within range depending on Collider Type.
        /// </summary>
        /// <param name="fromUser"></param>
        protected virtual void DoAOE(ITakeAura fromUser)
        {

            CheckAffected(fromUser);
 
            //grab all possible candidates in the area
            List<ITakeAura> affected = new List<ITakeAura>();
            userAffected.TryGetValue(fromUser, out Dictionary<ITakeAura, bool> valueDic);
            if (valueDic == null)
            {
                valueDic = new Dictionary<ITakeAura, bool>();
            }

            //Collider or Collider2D
            switch (AuraData.ColliderType)
            {
                case ColliderType.Collider:
                    //for all in range, check for the interface
                    List<Collider> inrange = GetCollidersInRange(fromUser.GetGameObjectInstance().transform.position);
                    for (int i = 0; i < inrange.Count; i++)
                    {
                        ITakeAura itakeAura = inrange[i].GetComponent<ITakeAura>();
                        //if null, self, or alraedy added, move on
                        if (itakeAura == null) continue;
                        if (itakeAura == fromUser) continue;
                        if (valueDic.ContainsKey(itakeAura)) continue;

                        if (AppliedAuraLogic(itakeAura))
                        {
                            affected.Add(itakeAura);
                        }
                    }
                    break;
                case ColliderType.Collider2D:
                    //
                    List<Collider2D> inrange2D = GetCollidersInRange2D(fromUser.GetGameObjectInstance().transform.position);
                    for (int i = 0; i < inrange2D.Count; i++)
                    {
                        ITakeAura itakeAura = inrange2D[i].GetComponent<ITakeAura>();
                        //if null, self, or alraedy added, move on
                        if (itakeAura == null) continue;
                        if (itakeAura == fromUser) continue;
                        if (valueDic.ContainsKey(itakeAura)) continue;

                        if (AppliedAuraLogic(itakeAura))
                        {
                            affected.Add(itakeAura);
                        }
                    }
                    break;

            }


            for (int i = 0; i < affected.Count; i++)
            {
                if (valueDic.ContainsKey(affected[i]) == false)
                {
                    valueDic.Add(affected[i], true);
                }

            }

            userAffected[fromUser] = valueDic;
        }

        /// <summary>
        /// Checks already affected ITakeAura and sees if they are still in range. If not, removes them. 
        /// </summary>
        /// <param name="fromUser"></param>
        protected virtual void CheckAffected(ITakeAura fromUser)
        {
            userAffected.TryGetValue(fromUser, out Dictionary<ITakeAura, bool> valueDic);
            auraInstance.TryGetValue(fromUser, out GameObject valueInstance);

            if (valueDic == null) return;
            if (valueDic.Count == 0) return;
            if (valueInstance == null)
            {
                Debug.LogError("Aura Instance is somehow null");
            }


            List<ITakeAura> _temp = new List<ITakeAura>();
            foreach (var kvp in valueDic)
            {
                Vector3 dir = kvp.Key.GetGameObjectInstance().transform.position - fromUser.GetGameObjectInstance().transform.position;
                float sqrdDst = dir.sqrMagnitude;
                if (sqrdDst > AuraData.AreaRadius * AuraData.AreaRadius)
                {
                    _temp.Add(kvp.Key);
                    continue;
                }

            }

            for (int i = 0; i < _temp.Count; i++)
            {
                valueDic.Remove(_temp[i]);
                RemovedAuraLogic(_temp[i]);

            }
            userAffected[fromUser] = valueDic;

        }
        protected virtual void DoPulse(ITakeAura fromUser)
        {
          
            userAffected.TryGetValue(fromUser, out Dictionary<ITakeAura, bool> valueDic);
            AppliedAuraLogic(fromUser);
            if (valueDic == null || valueDic.Count == 0) return;
            foreach (var kvp in valueDic)
            {
                AppliedAuraLogic(kvp.Key);
            }
        }

        #endregion

        #region interface

        public void SetTextAsset(TextAsset textAsset)
        {
            config = textAsset;
        }

        public TextAsset GetTextAsset()
        {
            return config;
        }

        public Object GetObject()
        {
            return this;
        }

        #endregion
        #region editor auto naming, ID, and checks
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (AuraData == null) return;
            if (AuraData.AutoName && string.IsNullOrEmpty(AuraData.AuraName) == false)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, AuraData.AuraName);
            }
            if (AuraData.AutoAssignID)
            {
                AuraData.uniqueID = this.GetInstanceID();
            }


        }

       

#endif
        #endregion

    }
}