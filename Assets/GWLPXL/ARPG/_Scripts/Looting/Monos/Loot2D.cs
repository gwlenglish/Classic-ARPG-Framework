using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{
    public interface ILoot
    {
        Transform GetInstance();
        LootOptions GetLootOptions();
        void IniLoot(Item _forItem, float _delay);
       
    }

    public class Loot2D : MonoBehaviour, ITick, IInteract, ILoot
    {
        [SerializeField]
        UnityLootInstanceEvents lootEvents = new UnityLootInstanceEvents();
        public LootOptions LootOptions;
        [SerializeField]
        float interactDistance = 1f;
        [SerializeField]
        Transform MeshHolder;


        bool pickedUp;

        #region send info the UI
        public void AddTicker() => TickManager.Instance.AddTicker(this);

        public LootOptions GetLootOptions() => LootOptions;
       

        public void DoTick()//on first tick, make active. the tick is dependant on the timer
        {
            if (LootOptions.CanPickUp) return;
            LootOptions.CanPickUp = true;

            DungeonMaster.Instance.GetLootCanvas().CreateLootTextUI(this);

            lootEvents.SceneEvents.OnLootActive.Invoke();
        }

        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);


        public float GetTickDuration() => GetDefaultDelay();



        public float GetDefaultDelay()
        {
            return LootOptions.DelayBeforeActive;
        }
        public void IniLoot(Item _forItem, float _delay)
        {
            LootOptions = new LootOptions(_forItem, _delay);
        }


        private void Start()
        {

            GameObject visual = LootOptions.DroppedItem.CreateMeshInstance(MeshHolder);
           

            lootEvents.SceneEvents.OnLootDropped.Invoke(this);
            AddTicker();
        }


        private void OnDestroy()
        {
            DungeonMaster.Instance.GetLootCanvas().RemoveLootText(this);
            RemoveTicker();
        }


        #endregion


        public bool DoInteraction(GameObject obj)
        {
            if (LootOptions.CanPickUp == false) return false;
            if (pickedUp == true) return false;//if we already picked this up, don't pick it up again.
            IActorHub invUser = obj.GetComponent<IActorHub>();
            if (invUser == null)
            {
                ARPGDebugger.DebugMessage("Trying to loot but object doesn't implement IActorHub", obj);
                return false;
            }
            ARPGDebugger.DebugMessage(obj.name + " did interact", obj);

            pickedUp = ItemHandler.PickupItem(LootOptions.DroppedItem, invUser.MyInventory);
            if (pickedUp)
            {
                //add the loot
                this.gameObject.SetActive(false);
                lootEvents.SceneEvents.OnLootPickedUp.Invoke(this);
                Destroy(this.gameObject, .1f);//the ondestroy call will remove the loot from the UI
                DungeonMaster.Instance.GetLootCanvas().RemoveLootText(this);
            }

            return pickedUp;
        }


        public bool IsInRange(GameObject invUser)
        {
            Vector3 diff = (invUser.transform.position - this.transform.position);
            float sqrdmag = diff.sqrMagnitude;
            if (sqrdmag <= interactDistance * interactDistance)//if we are, pick it up
            {

                return true;
            }
            return false;
        }
        public Transform GetInstance() => this.transform;
      

        private void OnTriggerEnter(Collider other)
        {
            if (LootOptions.AutoPickupCurrency && LootOptions.DroppedItem is Currency)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    DoInteraction(player.gameObject);
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
