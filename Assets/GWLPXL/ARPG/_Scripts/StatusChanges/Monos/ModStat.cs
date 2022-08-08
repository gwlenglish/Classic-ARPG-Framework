using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;


using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
    [System.Serializable]
    public class ModifyStatType
    {
        public StatType Type;
        public int ModAmount;
        public float Duration;
        public ModifyStatType(StatType type, int modAmount, float duration)
        {
            Type = type;
            ModAmount = modAmount;
            Duration = duration;
        }
    }

    public class ModStat : MonoBehaviour, ITick
    {
        public ModifyStatType ModStatType;
        float timer = 0;
        bool ini;
        IAttributeUser statUser = null;
        private void OnEnable()
        {
            AddTicker();
        }
        private void OnDisable()
        {
            RemoveTicker();
        }

        private void Start()
        {
            statUser = GetComponent<IAttributeUser>();
            if (statUser == null)
            {
                Destroy(this);
                return;
            }
            statUser.GetRuntimeAttributes().ModifyBaseStatValue(ModStatType.Type, ModStatType.ModAmount);
            ini = true;
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            if (ini == false) return;

            timer += Time.deltaTime;
            if (timer >= ModStatType.Duration)
            {
                //remove and destroy
                statUser.GetRuntimeAttributes().ModifyBaseStatValue(ModStatType.Type, -ModStatType.ModAmount);
                Destroy(this);
                ini = false;
            }
        }

        private void OnDestroy()
        {
            if (ini == true)//havent run out the timer yet but destroying, so remove
            {
                statUser.GetRuntimeAttributes().ModifyBaseStatValue(ModStatType.Type, -ModStatType.ModAmount);
            }
        }
        public float GetTickDuration()
        {
            return Time.deltaTime;
        }

        public void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);

        }
    }
}