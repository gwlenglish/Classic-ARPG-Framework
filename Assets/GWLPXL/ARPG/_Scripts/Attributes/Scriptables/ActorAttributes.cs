

using System;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using GWLPXL.ARPG._Scripts.Attributes.com;
using GWLPXL.ARPGCore.GameEvents.com;

using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Abilities.com;


namespace GWLPXL.ARPGCore.Attributes.com
{

    /// <summary>
    /// Core class that tracks actor stats with leveling, previously actor stats. 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Attributes/NEW_Attributes")]

    public class ActorAttributes : ScriptableObject, ISaveJsonConfig
    {
        #region actions to subscribe
        public System.Action OnLevelUpBegin;
        public System.Action OnLevelUpEnd;
        public System.Action<int> OnLevelUp;
        public System.Action<int> OnResourceChanged;
        public System.Action<Dictionary<AttributeType, Attribute[]>> OnLevelUpAttributes;

        #endregion
        public Stat[] StatsData => Stats;
        public Resource[] ResourceData => Resources;
        public Other[] OtherData => others;
        public StatResourceLink[] ResourceLinks => resourceLinks;
        public StatCombatLink[] CombatLinks => combatLinks;
        #region helpers
        public AttributeDescriptions Description => new AttributeDescriptions(this);
        public AttributePercents Percents => new AttributePercents(this);
        public AttributeConditions Conditions => new AttributeConditions(this);
        #endregion

        [SerializeField]
        protected TextAsset config;
        [SerializeField]
        protected AttributesID databaseID;
        public LevelUpEvent LevelUpEvent;
        public int CurrentXP => currentXP;
        public int MyLevel => myLevel;
        public int MaxLevel => maxLevel;
        public StatType BaseStat => baseStat;

        [SerializeField]
        protected int myLevel;
        [SerializeField]
        [Tooltip("If true, will restore all resources to max values upon leveling.")]
        protected bool restoreOnLevel = true;
        protected int currentXP;
        public string ActorName;
        [SerializeField]
        protected int maxLevel = 99;
        [SerializeField]
        protected StatType baseStat = StatType.Strength;
        [SerializeField]
        [Header("Resources")]
        protected Resource[] Resources = new Resource[0];
        [Header("Stats")]
        [SerializeField]
        protected Stat[] Stats = new Stat[0];
        [Header("Stat Links")]
        [SerializeField]
        protected StatResourceLink[] resourceLinks = new StatResourceLink[0];
        [SerializeField]
        protected StatCombatLink[] combatLinks = new StatCombatLink[0];
        [Header("Elements")]
        [SerializeField]
        protected ElementAttack[] ElementAttacks = new ElementAttack[0];
        [SerializeField]
        protected ElementResist[] ElementResists = new ElementResist[0];
        [Header("Skills")]
        [SerializeField]
        protected AbilityMod[] abilityMods = new AbilityMod[0];
        [Header("Other")]
        [SerializeField]
        protected Other[] others = new Other[0];


        [System.NonSerialized]
        protected Dictionary<AttributeType, Attribute[]> allAttributes = new Dictionary<AttributeType, Attribute[]>();

        protected bool initialized = false;
        public virtual void SettResources(Resource[] newRes) => Resources = newRes;
        public virtual void SetStats(Stat[] newstats) => Stats = newstats;
        public virtual void SetResourceLinks(StatResourceLink[] newLinks) => resourceLinks = newLinks;
        public virtual void SetCombatLinks(StatCombatLink[] newLinks) => combatLinks = newLinks;
        public virtual void SetOthers(Other[] newOthers) => others = newOthers;

        public AttributesID GetID() => databaseID;
        public virtual Attribute[] GetAttributes(AttributeType byType)
        {
            allAttributes.TryGetValue(byType, out Attribute[] value);
            if (value == null)
            {
                switch (byType)
                {
                    case AttributeType.Stat:
                        value = Stats;
                        break;
                    case AttributeType.Resource:
                        value = Resources;
                        break;
                    case AttributeType.ElementAttack:
                        value = ElementAttacks;
                        break;
                    case AttributeType.ElementResist:
                        value = ElementResists;
                        break;
                    case AttributeType.AbilityMod:
                        value = abilityMods;
                        break;
                    case AttributeType.Other:
                        value = others;
                        break;
                }
                if (value == null) value = new Attribute[0];
                for (int i = 0; i < value.Length; i++)
                {
                    value[i].Level(myLevel, maxLevel);
                }
                allAttributes[byType] = value;
            }
            return value;
        }


        //functions for saving
        public virtual void SetStatsID(AttributesID newID)
        {
            databaseID = newID;
        }
        public virtual AttributesID GetStatsID()
        {
            return databaseID;
        }

        public virtual Dictionary<AttributeType, Attribute[]> GetAllAttributesByType()//mostly for editor
        {
            Dictionary<AttributeType, Attribute[]> _temp = new Dictionary<AttributeType, Attribute[]>();
            foreach (AttributeType pieceType in System.Enum.GetValues(typeof(AttributeType)))
            {
                Attribute[] attributes = GetAttributes(pieceType);
                _temp.Add(pieceType, attributes);
            }
            return _temp;
        }
        public virtual Dictionary<AttributeType, Attribute[]> GetAllAttributes()//for runtime use
        {
            return allAttributes;
        }
        //
        #region level up
        [ContextMenu("Level Up")]
        public virtual void LevelUp(int newLevel)
        {
            OnLevelUpBegin?.Invoke();

            int oldlevel = myLevel;
            myLevel = newLevel;
            if (initialized == false)
            {
                allAttributes[AttributeType.Resource] = GetAttributes(AttributeType.Resource);
                allAttributes[AttributeType.Stat] = GetAttributes(AttributeType.Stat);
                allAttributes[AttributeType.ElementAttack] = GetAttributes(AttributeType.ElementAttack);
                allAttributes[AttributeType.ElementResist] = GetAttributes(AttributeType.ElementResist);
                allAttributes[AttributeType.AbilityMod] = GetAttributes(AttributeType.AbilityMod);
                allAttributes[AttributeType.Other] = GetAttributes(AttributeType.Other);
                initialized = true;
            }

            Dictionary<AttributeType, Attribute[]> previous = SavePreviousValuesToEvent();

            LevelUpAttributes();

            SaveNewValuesToEvent(newLevel, oldlevel, previous);

            if (restoreOnLevel)
            {
                RestoreAllResources();
            }

            Attribute[] resources = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < resources.Length; i++)
            {
                Resource res = resources[i] as Resource;
                OnResourceChanged?.Invoke((int)res.Type);
            }

            OnLevelUp?.Invoke(MyLevel);//passing in new level
            OnLevelUpAttributes?.Invoke(GetAllAttributes());
            OnLevelUpEnd?.Invoke();
        }

        private void LevelUpAttributes()
        {
            foreach (var kvp in allAttributes)
            {
                if (kvp.Value == null) continue;
                for (int i = 0; i < kvp.Value.Length; i++)
                {

                    kvp.Value[i].Level(myLevel, maxLevel);

                }
            }
        }

        private void SaveNewValuesToEvent(int newLevel, int oldlevel, Dictionary<AttributeType, Attribute[]> previous)
        {
            Dictionary<AttributeType, Attribute[]> now = new Dictionary<AttributeType, Attribute[]>();
            if (LevelUpEvent != null)
            {
                foreach (var kvp in allAttributes)
                {
                    now.Add(kvp.Key, kvp.Value);
                }

                LevelUpEvent.EventVars = new LevelUpEventVars(previous, now, oldlevel, newLevel);
                GameEventHandler.RaiseLevelUpEvent(LevelUpEvent);
            }
        }

        private Dictionary<AttributeType, Attribute[]> SavePreviousValuesToEvent()
        {
            Dictionary<AttributeType, Attribute[]> previous = new Dictionary<AttributeType, Attribute[]>();
            if (LevelUpEvent != null)
            {
                foreach (var kvp in allAttributes)
                {
                    previous.Add(kvp.Key, kvp.Value);
                }
            }

            return previous;
        }

        public void RestoreAllResources()
        {
            Attribute[] resources = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < resources.Length; i++)
            {
                Resource resource = (Resource)resources[i];
                ModifyNowResource(resource.Type, resource.NowValue);
            }
        }


        #endregion


        #region public resources
        public bool DoWeHaveResources(ResourceType type, int costAmount)
        {
            float current = GetResourceNowValue(type);
            return current >= costAmount;
        }

        #region modifiers

        private ElementAttack FindElementAttack(ElementType type)
        {
            eleAttackDic.TryGetValue(type, out ElementAttack attackvalue);
            if (attackvalue == null)
            {
                Attribute[] value = GetAttributes(AttributeType.ElementAttack);
                for (int i = 0; i < value.Length; i++)
                {
                    ElementAttack attack = (ElementAttack)value[i];
                    if (attack.Type == type)
                    {
                        attackvalue = attack;
                    }
                }

                if (attackvalue == null)
                {
                    attackvalue = new ElementAttack(type);
                    attackvalue.SetBaseValue(0);
                }
            }
            eleAttackDic[type] = attackvalue;
            return attackvalue;
        }
        public virtual void AddModifierElementAttack(ElementType type, AttributeModifier modifier)
        {
            FindElementAttack(type).AddModifier(modifier);
        }
        public virtual bool RemoveModifierElementAttack(ElementType type, AttributeModifier modifier)
        {
            return FindElementAttack(type).RemoveModifier(modifier);
        }
        public virtual bool RemoveSourceModifierElementAttack(ElementType type, object source)
        {
            return FindElementAttack(type).RemoveAllModifiersFromSource(source);
        }

        private ElementResist FindElementResist(ElementType type)
        {
            Attribute[] value = GetAttributes(AttributeType.ElementResist);
            for (int i = 0; i < value.Length; i++)
            {
                ElementResist resist = (ElementResist)value[i];
                if (resist.Type == type)
                {
                    return resist;
                }
            }

            return null;
        }

        public virtual void AddModifierElementResist(ElementType type, AttributeModifier modifier)
        {
            FindElementResist(type)?.AddModifier(modifier);
        }
        public virtual void RemoveModifierElementResist(ElementType type, AttributeModifier modifier)
        {
            FindElementResist(type)?.RemoveModifier(modifier);
        }
        public virtual void RemoveSourceModifierElementResist(ElementType type, object source)
        {
            FindElementResist(type)?.RemoveAllModifiersFromSource(source);
        }

        private Resource FindResource(ResourceType whichOne)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (whichOne == resource.Type)
                {
                    return resource;
                }
            }

            return null;
        }
        public virtual void AddModifierResource(ResourceType whichOne, AttributeModifier modifier)
        {
            var resource = FindResource(whichOne);
            if (resource == null) return;
            resource.AddModifier(modifier);
            OnResourceChanged?.Invoke((int)whichOne);
        }

        public virtual void RemoveModifierResource(ResourceType whichOne, AttributeModifier modifier)
        {
            var resource = FindResource(whichOne);
            if (resource == null) return;
            resource.RemoveModifier(modifier);
            OnResourceChanged?.Invoke((int)whichOne);
        }

        public virtual void RemoveSourceModifierResource(ResourceType whichOne, object source)
        {
            var resource = FindResource(whichOne);
            if (resource == null) return;
            resource.RemoveAllModifiersFromSource(source);
            OnResourceChanged?.Invoke((int)whichOne);
        }

        private AbilityMod FindAbilityMod(Ability ability)
        {
            Attribute[] value = GetAttributes(AttributeType.AbilityMod);
            for (int i = 0; i < value.Length; i++)
            {
                AbilityMod mod = (AbilityMod)value[i];
                if (mod.Ability == ability)
                {
                    return mod;
                }
            }

            return null;
        }
        
        public virtual void AddModifierAbilityMod(Ability ability, AttributeModifier modifier)
        {
            FindAbilityMod(ability)?.AddModifier(modifier);
        }
        public virtual void RemoveModifierAbilityMod(Ability ability, AttributeModifier modifier)
        {
            FindAbilityMod(ability)?.RemoveModifier(modifier);
        }
        public virtual void RemoveSourceModifierAbilityMod(Ability ability, object source)
        {
            FindAbilityMod(ability)?.RemoveAllModifiersFromSource(source);
        }

        private Stat FindStat(StatType whichOne)
        {
            Attribute[] value = GetAttributes(AttributeType.Stat);
            for (int i = 0; i < value.Length; i++)
            {
                Stat stat = (Stat)value[i];
                if (stat.Type == whichOne)
                {
                    return stat;
                }
            }

            return null;
        }

        private void ChangeStat(StatType whichOne, Action<Stat> action)
        {
            var oldValue = GetStatNowValue(whichOne);
            action(FindStat(whichOne));
            var newValue = GetStatNowValue(whichOne);
            ResourceLinkAdditional(whichOne, newValue - oldValue);
        }
        public virtual void AddModifierStat(StatType whichOne, AttributeModifier modifier)
        {
            ChangeStat(whichOne, stat => stat.AddModifier(modifier));
        }
        public virtual void RemoveModifierStat(StatType whichOne, AttributeModifier modifier)
        {
            ChangeStat(whichOne, stat => stat.RemoveModifier(modifier));
        }
        public virtual void RemoveSourceModifierStat(StatType whichOne, object source)
        {
            ChangeStat(whichOne, stat => stat.RemoveAllModifiersFromSource(source));
        }

        #endregion
        
        
        public virtual void SetAttributeMaxBaseValue(ResourceType type, int newValue)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (resource.Type == type)
                {
                    //we found it
                    resource.SetBaseValue(newValue);
                }
            }
        }


        public virtual int GetAttributeMaxValue(ResourceType typeToGet)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (resource.Type == typeToGet)
                {
                    //we found it
                    return resource.NowValue;
                }
            }
            return 0;
        }

        public virtual int GetAttributeNowValue(ResourceType typeToGet)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (resource.Type == typeToGet)
                {
                    //we found it
                    return resource.ResourceNowValue;
                }
            }
            return 0;
        }
        public virtual void ModifyBaseAttribute(ResourceType whichOne, int byHowMuch)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (resource.Type == whichOne)
                {
                    resource.ModifyBaseValue(byHowMuch);
                }
            }
        }


        public virtual int GetResourceNowValue(ResourceType typeToGet)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (typeToGet == resource.Type)
                {
                    //we found it
                    return resource.ResourceNowValue;
                }
            }

            return 0;
        }
        public virtual int GetResourceMaxValue(ResourceType typeToGet)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);
            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (typeToGet == resource.Type)
                {
                    //we found it
                    return resource.NowValue;
                }
            }

            return 0;
        }

        public virtual void ModifyNowResource(ResourceType whichOne, int byHowMuch)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);

            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (whichOne == resource.Type)
                {
                    //we found it
                    resource.ModifyResourceValue(byHowMuch);
                    OnResourceChanged?.Invoke((int)whichOne);

                }
            }
        }
        
        public virtual void ModifyMaxResource(ResourceType whichOne, int byHowMuch)
        {
            Attribute[] value = GetAttributes(AttributeType.Resource);

            for (int i = 0; i < value.Length; i++)
            {
                Resource resource = (Resource)value[i];
                if (whichOne == resource.Type)
                {
                    //we found it
                    resource.ModifyBaseValue(byHowMuch);
                    OnResourceChanged?.Invoke((int)whichOne);
                }
            }

        }

        #endregion

        #region public stats
        public virtual void SetCurrentXP(int current)
        {
            currentXP = current;
        }

        public virtual int GetOtherAttributeBaseValue(OtherAttributeType type)
        {
            Attribute[] value = GetAttributes(AttributeType.Other);
            for (int i = 0; i < value.Length; i++)
            {
                Other other = (Other)value[i];
                if (other.Type == type)
                {
                    return other.BaseValue;
                }
            }
            return 0;
        }
        
        public virtual int GetOtherAttributeNowValue(OtherAttributeType type)
        {
            Attribute[] value = GetAttributes(AttributeType.Other);
            for (int i = 0; i < value.Length; i++)
            {
                Other other = (Other)value[i];
                if (other.Type == type)
                {
                    return other.NowValue;
                }
            }
            return 0;
        }



        public virtual int GetStatForCombat(CombatStatType whichCombatStat)
        {
            int statValue = 1;
            for (int i = 0; i < combatLinks.Length; i++)
            {
                if (whichCombatStat == combatLinks[i].Combat)
                {
                    Attribute[] value = GetAttributes(AttributeType.Stat);
                    for (int j = 0; j < value.Length; j++)
                    {
                        Stat stat = (Stat)value[j];
                        if (stat.Type == combatLinks[i].Stat)
                        {
                            int statPoints = stat.NowValue;
                            int curvedValue = statPoints * combatLinks[i].ValuePerStat;//25 * 1 = 25
                            statValue += curvedValue;
                        }
                    }
                }
            }

            ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("Stat for combat " + statValue + " " + whichCombatStat.ToString(), this);
            return statValue;
        }

        public virtual void ModifyBaseStatValue(StatType whichOne, int byHowMuch)
        {
            int oldValue = GetStatNowValue(whichOne);
            
            int newValue = GetStatBaseValue(whichOne) + byHowMuch;
            SetStatBaseValue(whichOne, newValue);

            int nowValue = GetStatNowValue(whichOne);

            ResourceLinkAdditional(whichOne, nowValue - oldValue);
        }
        


        protected virtual void ResourceLinkAdditional(StatType whichOne, int byHowMuch)
        {
            for (int i = 0; i < resourceLinks.Length; i++)
            {
                StatType link = resourceLinks[i].Stat;
                if (link == whichOne)
                {
                    ResourceType type = resourceLinks[i].Resource;
                    int resourceLinkAddition = byHowMuch * resourceLinks[i].ResourcePerStatValue;
                    ModifyMaxResource(type, resourceLinkAddition);
                }
            }
        }

        public virtual int GetStatNowValue(StatType typeToGet)
        {
            Attribute[] value = GetAttributes(AttributeType.Stat);
            for (int i = 0; i < value.Length; i++)
            {
                Stat stat = (Stat)value[i];
                if (stat.Type == typeToGet)
                {
                    return stat.NowValue;
                }
            }
            return 0;

        }
        
        public virtual int GetStatBaseValue(StatType typeToGet)
        {
            Attribute[] value = GetAttributes(AttributeType.Stat);
            for (int i = 0; i < value.Length; i++)
            {
                Stat stat = (Stat)value[i];
                if (stat.Type == typeToGet)
                {
                    return stat.BaseValue;
                }
            }
            return 0;
        }

        #endregion

        //not yet added in a meaingful way
        #region public skills
        public int GetAbilityMod(Ability ability)
        {
            Attribute[] value = GetAttributes(AttributeType.AbilityMod);

            for (int i = 0; i < value.Length; i++)
            {
                AbilityMod mod = (AbilityMod)value[i];
                if (ability == mod.Ability)
                {
                    return mod.NowValue;
                }
            }

            return 0;//may need to put this at 1...
        }
        public void ModifyAbilityMod(Ability ability, int byAmount)
        {
            Attribute[] value = GetAttributes(AttributeType.AbilityMod);
            for (int i = 0; i < value.Length; i++)
            {
                AbilityMod mod = (AbilityMod)value[i];
                if (mod.Ability == ability)
                {
                    mod.ModifyBaseValue(byAmount);
                }
            }

        }

        #endregion

        #region public elements

        public virtual int GetAllElementAttackValues()
        {
            int attack = 0;
            Attribute[] value = GetAttributes(AttributeType.ElementAttack);
            for (int i = 0; i < value.Length; i++)
            {
                attack += value[i].NowValue;
            }
            return attack;
        }
        public virtual int GetElementResist(ElementType type)
        {
            Attribute[] value = GetAttributes(AttributeType.ElementResist);
            for (int i = 0; i < value.Length; i++)
            {
                ElementResist resist = (ElementResist)value[i];
                if (resist.Type == type)
                {
                    return resist.NowValue;
                }
            }
            return 0;
        }

        Dictionary<ElementType, ElementAttack> eleAttackDic = new Dictionary<ElementType, ElementAttack>();
        public virtual int GetElementAttack(ElementType type)
        {
            eleAttackDic.TryGetValue(type, out ElementAttack attackvalue);
            if (attackvalue == null)
            {
                Attribute[] value = GetAttributes(AttributeType.ElementAttack);
                for (int i = 0; i < value.Length; i++)
                {
                    ElementAttack attack = (ElementAttack)value[i];
                    if (attack.Type == type)
                    {
                        attackvalue = attack;
                    }
                }

                if (attackvalue == null)
                {
                    attackvalue = new ElementAttack(type);
                }
            }
            eleAttackDic[type] = attackvalue;
            return attackvalue.NowValue;
          
        }
        public virtual void ModifyElementAttackBaseValue(ElementType type, int modifyAmount)
        {
            eleAttackDic.TryGetValue(type, out ElementAttack attackvalue);
            if (attackvalue == null)
            {
                Attribute[] value = GetAttributes(AttributeType.ElementAttack);
                for (int i = 0; i < value.Length; i++)
                {
                    ElementAttack attack = (ElementAttack)value[i];
                    if (attack.Type == type)
                    {
                        attackvalue = attack;
                    }
                }

                if (attackvalue == null)
                {
                    attackvalue = new ElementAttack(type);
                    attackvalue.SetBaseValue(0);
                }
            }
            attackvalue.ModifyBaseValue(modifyAmount);
            eleAttackDic[type] = attackvalue;

        }


        public virtual void ModifyElementResistBaseValue(ElementType type, int modifyAmount)
        {
            Attribute[] value = GetAttributes(AttributeType.ElementResist);
            for (int i = 0; i < value.Length; i++)
            {
                ElementResist resist = (ElementResist)value[i];
                if (resist.Type == type)
                {
                    resist.ModifyBaseValue(modifyAmount);
                    break;
                }
            }
        }

       


        #endregion


        protected virtual void SetStatBaseValue(StatType typeToSet, int newValue)
        {
            Attribute[] value = GetAttributes(AttributeType.Stat);
            for (int i = 0; i < value.Length; i++)
            {
                Stat stat = (Stat)value[i];
                if (stat.Type == typeToSet)
                {
                    stat.SetBaseValue(newValue);
                    break;
                }
            }

        }

        //not yet implemented in a meaningful way
        protected virtual AbilityMod GetModByAbility(Ability ability)
        {
            Attribute[] value = GetAttributes(AttributeType.AbilityMod);
            for (int i = 0; i < value.Length; i++)
            {
                AbilityMod mod = (AbilityMod)value[i];
                if (ability == mod.Ability)
                {
                    return mod;
                }
            }

            return null;
        }

        #region interface
        public void SetTextAsset(TextAsset textAsset)
        {
            config = textAsset;
        }

        public TextAsset GetTextAsset()
        {
            return config;
        }

        public UnityEngine.Object GetObject()
        {
            return this;
        }
        #endregion
    }



}