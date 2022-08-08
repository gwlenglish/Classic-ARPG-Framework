

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
   
    public class MeleeCombatant : MonoBehaviour, IMeleeCombatUser
    {
        [SerializeField]
        [Tooltip("Default is good for enemies who don't change weapons. For the player, the fist should be the default so they can always attack.")]
        protected GameObject[] defaultMeleeDD = new GameObject[0];
        [SerializeField]
        protected EquipmentSlotsType[] meleeWpnSlots = new EquipmentSlotsType[2] { EquipmentSlotsType.RightWpnHand, EquipmentSlotsType.LeftWpnHand };//slots that can carry weapons by index.
        
        protected IDoDamage[] currentMeleeDD = new IDoDamage[0];
        protected IActorHub hub = null;
        protected List<GameObject> meleeObjs = new List<GameObject>();

        #region unity lifecycle
        protected virtual void Start()
        {
            Setup();

        }
        #endregion

        #region protected
        protected virtual void Setup()
        {
            if (defaultMeleeDD.Length == 0)
            {
                IDoDamage findfirst = hub.MyTransform.GetComponentInChildren<IDoDamage>();
                if (findfirst == null)
                {
                    ARPGDebugger.CombatDebugMessage("No Melee damage boxes set, can't do melee damage without one", hub.MyTransform);
                }
                else
                {
                    defaultMeleeDD = new GameObject[1] { findfirst.GetTransform().gameObject };
                }
            }
            currentMeleeDD = new IDoDamage[GetWpnMeleeSlots().Length];
            if (defaultMeleeDD.Length == 0)
            {
                IDoDamage[] damagers = GetComponentsInChildren<IDoDamage>();
                defaultMeleeDD = new GameObject[damagers.Length];
                for (int i = 0; i < damagers.Length; i++)
                {
                    defaultMeleeDD[i] = damagers[i].GetTransform().gameObject;
                }
                //  defaultMeleeDD = GetComponentsInChildren<IDoDamage>();

            }

            for (int i = 0; i < defaultMeleeDD.Length; i++)
            {
                SetMeleeDamageDealer(defaultMeleeDD[i].GetComponentInChildren<IDoDamage>(), i);
            }
        }
        #endregion

        #region public
        public virtual IDoDamage[] GetMeleeDamageBoxes()
        {
           // if (currentMeleeDD == null || currentMeleeDD.Length == 0) return defaultMeleeDD;
            //ARPGDebugger.DebugMessage(currentMeleeDD.GetTransform().name + " current melee DD", this.gameObject);
            return currentMeleeDD;
        }

        public virtual Transform[] GetMeleeTransforms()
        {
            List<Transform> _temp = new List<Transform>();

            for (int i = 0; i < currentMeleeDD.Length; i++)
            {
                if (currentMeleeDD[i] == null || currentMeleeDD[i].GetTransform() == null)
                {
                    //dont add
                }
                else
                {
                    _temp.Add(currentMeleeDD[i].GetTransform());
                }
                //Debug.Log(currentMeleeDD[i].GetTransform().name + " index " + i);
            }

            return _temp.ToArray();
        }

        

        public virtual void SetMeleeDamageDealer(IDoDamage damager, int atIndex)
        {
            if (atIndex > meleeWpnSlots.Length - 1 || atIndex < 0)
            {
                ARPGDebugger.DebugMessage("Trying to set a damage dealer over the amount of weapons the actor can carry. Increase the max if you want to carry more.", this);
                return;
            }

           
            if (atIndex > currentMeleeDD.Length - 1 || atIndex < 0)
            {
                ARPGDebugger.DebugMessage(ARPGDebugger.SB.Append("Trying to set a damage dealer that the actor doesn't have. Is your damage dealer reference null? does it inherit IDoDamage?"), this);
                return;
            }

            meleeObjs.Clear();
            currentMeleeDD[atIndex] = damager;
            for (int i = 0; i < currentMeleeDD.Length; i++)
            {
                if (currentMeleeDD[i] == null) continue;
                meleeObjs.Add(currentMeleeDD[i].GetTransform().gameObject);
            }

            ARPGDebugger.DebugMessage("Melee dmg dealer set at index " + atIndex + " " + damager.GetTransform().name, this);

        }

        public virtual void ResetDefaultDamageDealer(int atIndex)
        {
            if (atIndex > meleeWpnSlots.Length - 1)
            {
                ARPGDebugger.DebugMessage("Trying to set a damage dealer over the amount of weapons the actor can carry. Increase the max if you want to carry more.", this);
                return;
            }


            if (atIndex > defaultMeleeDD.Length - 1)
            {
                //we dont have a default one
                currentMeleeDD[atIndex] = null;
            }
            else
            {
                //we do have default one
                if (defaultMeleeDD[atIndex] == null)
                {
                    currentMeleeDD[atIndex] = null;
                }
                else
                {
                    currentMeleeDD[atIndex] = defaultMeleeDD[atIndex].GetComponent<IDoDamage>();

                }
            }

          

        }

        public virtual EquipmentSlotsType[] GetWpnMeleeSlots()
        {
            return meleeWpnSlots;
        }

        public virtual void SetActorHub(IActorHub newhub) => hub = newhub;

        #endregion
    }
}
