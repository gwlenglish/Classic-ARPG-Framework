using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{
    [System.Serializable]
    public class LootOptions
    {
        public bool CanPickUp { get; set; }
        public Item DroppedItem { get; set; }
        public float DelayBeforeActive = 1f;
        public bool AutoPickupCurrency = true;
        public bool AutoPickupItem = false;
        public LootOptions(Item dropped, float delay)
        {
            DroppedItem = dropped;
            DelayBeforeActive = delay;
        }
    }

    [RequireComponent(typeof(Collider))]
    public class Loot : MonoBehaviour, IInteract, ITick, ILoot
    {
        [SerializeField]
        UnityLootInstanceEvents lootEvents = new UnityLootInstanceEvents();
        public LootOptions LootOptions = null;
        [SerializeField]
        float interactDistance = 1f;
        [SerializeField]
        Transform MeshHolder = null;
        [SerializeField]
        Light MeshLight = null;
        [SerializeField]
        [Tooltip("Depending on angle of the camera and the game, you may want to adjust this collider to be more visible. You typically don't want it exactly centered on the ground if the loot falls on the ground plane.")]
        Vector3 sphereColliderOffset = new Vector3(0, .5f, 0);
        bool pickedUp = false;

        #region send info the UI
        public void AddTicker() => TickManager.Instance.AddTicker(this);

        public Transform GetInstance() => this.transform;

        public LootOptions GetLootOptions() => LootOptions;


        public float GetDefaultDelay()
        {
            return LootOptions.DelayBeforeActive;
        }
        public void IniLoot(Item _forItem, float _delay)
        {
            LootOptions = new LootOptions(_forItem, _delay);
        }


        public void DoTick()//on first tick, make active. the tick is dependant on the timer
        {
            ActivateLoot();
        }

        protected virtual void ActivateLoot()
        {
            if (LootOptions.CanPickUp) return;
            LootOptions.CanPickUp = true;

            DungeonMaster.Instance.GetLootCanvas().CreateLootTextUI(this);

            lootEvents.SceneEvents.OnLootActive.Invoke();
        }

        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);


        public float GetTickDuration() => GetDefaultDelay();
       
     
        
      
     

        protected virtual void Start()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            GetComponent<SphereCollider>().center = sphereColliderOffset;
            GameObject visual = LootOptions.DroppedItem.CreateMeshInstance(MeshHolder);
            if (visual != null)
            {
                Color color = LootOptions.DroppedItem.GetRarityColor();
                MeshLight.color = color;
                MeshLight.enabled = true;
            }
            else
            {
                MeshLight.enabled = false;
            }

            lootEvents.SceneEvents.OnLootDropped.Invoke(this);
            AddTicker();
        }

        protected virtual void OnDestroy()
        {
            DungeonMaster.Instance.GetLootCanvas().RemoveLootText(this);
            RemoveTicker();
        }

       
        #endregion

      
        public bool DoInteraction(GameObject obj)
        {
            return CheckInteraction(obj);
        }

        protected virtual bool CheckInteraction(GameObject obj)
        {
            if (LootOptions.CanPickUp == false) return false;
            if (pickedUp == true) return false;//if we already picked this up, don't pick it up again.
            IActorHub invUser = obj.GetComponent<IActorHub>();
            if (invUser == null) return false;//can't pick it up, don't have an inventory
            ARPGDebugger.DebugMessage(obj.name + " did interact", obj);

            pickedUp = ItemHandler.PickupItem(LootOptions.DroppedItem, invUser.MyInventory);//do the pickup logic
            if (pickedUp)//if successful
            {
                //disable this instance and raise the event
                this.gameObject.SetActive(false);
                lootEvents.SceneEvents.OnLootPickedUp.Invoke(this);
                Destroy(this.gameObject, .1f);//the ondestroy call will remove the loot from the UI
                DungeonMaster.Instance.GetLootCanvas().RemoveLootText(this);
            }

            return pickedUp;
        }

        public bool IsInRange(GameObject invUser)
        {
            return CheckRange(invUser);
        }

        protected virtual bool CheckRange(GameObject invUser)
        {
            Vector3 diff = (invUser.transform.position - this.transform.position);
            float sqrdmag = diff.sqrMagnitude;
            if (sqrdmag <= interactDistance * interactDistance)//if we are, we can pick it up
            {

                return true;
            }
            return false;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            AutoPickupChecks(other);

        }

        protected virtual void AutoPickupChecks(Collider other)
        {
            if (LootOptions.DroppedItem is Currency)
            {
                if (LootOptions.AutoPickupCurrency)
                {
                    Player player = other.GetComponent<Player>();
                    if (player != null)
                    {
                        DoInteraction(player.gameObject);
                    }
                }
            }
            else if (LootOptions.DroppedItem is Item)
            {
                if (LootOptions.AutoPickupItem)
                {
                    Player player = other.GetComponent<Player>();
                    if (player != null)
                    {
                        DoInteraction(player.gameObject);
                    }
                }
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.transform.position, interactDistance);
        }

#endif
    }




}
