

using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Abilities.Mods.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class ChainVars
    {
        public float RadiusToNextJump = 1;
        [Tooltip("To how many people can this link chain towards?")]
        public int MaxNumberOfTargetsPerChain = 1;
        [Tooltip("Delay before initiating dmg on next jump")]
        public float DamageDelay = 0;
        [Tooltip("If true, will not chain back to an already damaged target. If false, will chain back.")]
        public bool ChainToNonDamagedTarget = true;
        [Tooltip("2D or 3D checks")]
        public ColliderType ColliderType = ColliderType.Collider;
        public LayerMask LayerMaskToCheck;
        public DamageSourceVars_Actor Damage = new DamageSourceVars_Actor();
        public AdditionalSoTSourceVars SoT = new AdditionalSoTSourceVars();
    }
    [System.Serializable]
    public class ChainDamageVars
    {

        [HideInInspector]
        public int CurrentChain = 0;
        public CombatGroupType[] GroupsToChainTowards = new CombatGroupType[1] { CombatGroupType.Enemy };
        public ChainVars[] Chains = new ChainVars[0];
    }

   
    public class ChainDamageStart : MonoBehaviour, IWeaponModification
    {
        public GameObject TrailPrefab;
        //need a way to track the chains so it doesn't hit something it already hit.
        public ChainDamageVars Vars;
        bool isactive = false;
        IActorHub self = null;


  
      
        public void DoModification(AttackValues other)
        {
            if (enabled == false) return;

            //make some manager that tracks.
            GameObject chainManager = new GameObject();
            ChainTracker tracker = chainManager.AddComponent<ChainTracker>();
            tracker.TrailPrefab = TrailPrefab;
            ChainDamageVars newVars = new ChainDamageVars();
            newVars = Vars;
            tracker.SetOriginalCaster(self);
            ChainDamagLink firstlink = other.Defenders[0].MyTransform.gameObject.AddComponent<ChainDamagLink>();
            firstlink.SetDamageVars(newVars, 0, other.Defenders[0], tracker);
          

        }

        public bool DoChange(Transform other) => false;
       

        public Transform GetTransform() => this.transform;


        public bool IsActive() => isactive;


        public void SetActive(bool isEnabled) => isactive = isEnabled;


        public void SetUser(IActorHub myself)
        {
            self = myself;

        }

       
       

       
       
    }
}
