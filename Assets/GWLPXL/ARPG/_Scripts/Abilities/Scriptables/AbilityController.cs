
using GWLPXL.ARPGCore.DebugHelpers.com;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.PlayerInput.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Abilities/NEW_AbilityController")]

    public class AbilityController : ScriptableObject, ISaveJsonConfig
    {
        //ability and slot
        #region events to subscribe to
        public Action<Ability, int> OnAbilityEquip;
        public Action<Ability, int> OnAbilityUnEquip;
        public Action<Ability, int> OnLearnedAbility;
        public Action<Ability, int> OnForgetAbility;
        public Action<float> OnAbilitySpeedModified;
        public Action<Ability> OnAbilityStart;
        public Action<Ability> OnAbilityEnd;
        public Action<IActorHub, Ability> OnAbilityUserEnd;

        public Action<AbilityCooldownTimer> OnAbilityCooldownStart;
        public Action<AbilityCooldownTimer> OnAbilityCooldownEnd;


        #endregion
        [SerializeField]
        TextAsset config = null;
        [SerializeField]
        AbilityControllerID id = new AbilityControllerID(string.Empty, 0, null);
        public AbilityControllerData Data;
        [Tooltip("Basic Attacks")]
        public Ability[] BasicAttacks = new Ability[0];
        [Tooltip("Default Abilities")]
        public Ability[] StartingAbilities = new Ability[0];
        [Tooltip("2 for left and right mouse click. This is probably a direct correlation with the amount of skills you can have on the hotbar at once.")]
        public int EquippedCap = 2;
        [Tooltip("How many skills you can know at a time.")]
        public int LearnedCap = 100;


        Ability[] equipped = new Ability[0];
        Ability[] learned = new Ability[0];

        [System.NonSerialized]
        Dictionary<int, Ability> appliedAbilitiesByCategory = new Dictionary<int, Ability>();
        [System.NonSerialized]
        Dictionary<Ability, bool> appliedAbilities = new Dictionary<Ability, bool>();

        [System.NonSerialized]
        List<AbilityCooldownTimer> cooldownTimers = new List<AbilityCooldownTimer>();


        [System.NonSerialized]
        List<AbilityDurationTimer> durationTimers = new List<AbilityDurationTimer>();
        [System.NonSerialized]
        List<AbilityDelayTimer> delayTimers = new List<AbilityDelayTimer>();
        [System.NonSerialized]
        List<IInterruptAbilityChecker> interruptors = new List<IInterruptAbilityChecker>();

        #region speed values
        readonly int normlizedSpeed = 1;//dont change
        float abilitySpeedMulti = 1;//changes dynamically when you modify the speed
        float clampedmulti = 0;//changes dynamically when you modify the speed
        float clampmaxspeed = 1.75f;//max value for your multiplier, this is even high as it'll send your character animations crazy
        float clampminspeed = .25f;//min value for multiplier
        #endregion
        /// <summary>
        /// used for charge abilities
        /// </summary>
        float chargeAmount = 0;

        #region public

        public float GetChargedAmount() => chargeAmount;
        public void SetChargeAmount(float newAmount) => chargeAmount = newAmount;
        /// <summary>
        /// used to sync with animator speed
        /// </summary>
        /// <param name="forAbility"></param>
        /// <returns></returns>
        public float GetClampedAbilitySpeedCooldown(Ability forAbility)
        {
            float cooldown = forAbility.Duration;

            if (abilitySpeedMulti < normlizedSpeed)//then we are slower than normal, add to the cooldown
            {
                cooldown = cooldown + (cooldown - Mathf.Abs(cooldown * GetClampedAbilityMulti()));
            }
            else if (abilitySpeedMulti > normlizedSpeed)//then we are faster than normal, minus from the cooldown
            {
                cooldown = cooldown + (cooldown - Mathf.Abs(cooldown * (GetClampedAbilityMulti())));

            }

            cooldown = Mathf.Clamp(cooldown, .02f, cooldown + clampmaxspeed);
            return cooldown;
        }

            
        /// <summary>
        /// Use a float as a percentage, i.e. 0.10f = 10%. Can pass positive and negative numbers.
        /// </summary>
        /// <param name="newMulti"></param>
        public void ModifyAbilityMulti(float newMulti)
        {
            abilitySpeedMulti += newMulti;
            OnAbilitySpeedModified?.Invoke(abilitySpeedMulti);
        }

     
        /// <summary>
        /// used to check for available basic attacks
        /// </summary>
        /// <param name="abilityUser"></param>
        /// <returns></returns>
        public Ability GetBasicAttack(IActorHub abilityUser)
        {
            for (int i = 0; i < BasicAttacks.Length; i++)
            {
                if (BasicAttacks[i].CheckRequirements(abilityUser))
                {
                    return BasicAttacks[i];
                }
            }
            return null;
        }
        public AbilityControllerID GetID() => id;
        /// <summary>
        /// Directly starts a skill, bypassing timers, delays, and any other considerations. 
        /// </summary>
        /// <param name="skillUser"></param>
        /// <param name="ability"></param>
        public void StartAbility(IActorHub skillUser, Ability ability)
        {
            ApplyInterruptOptions(skillUser, ability);

            ability.StartSkill(skillUser);
            OnAbilityStart?.Invoke(ability);

        }

        /// <summary>
        /// Applies classes to monitor interrupt conditions
        /// </summary>
        /// <param name="skillUser"></param>
        /// <param name="ability"></param>
        private void ApplyInterruptOptions(IActorHub skillUser, Ability ability)
        {
            AbilityInterruptOptions intOptions = ability.GetInterruptOptions();
            if (intOptions.CollisionLayers.Length > 0)
            {
                switch (intOptions.PhysicsType)
                {
                    case ARPGCore.com.EditorPhysicsType.Unity2D:
                        AbilityCollisionChecker2D checker2d = skillUser.MyTransform.gameObject.AddComponent<AbilityCollisionChecker2D>();
                        checker2d.CollisionLayers = intOptions.CollisionLayers;
                        interruptors.Add(checker2d);
                        checker2d.ToInterrupt = ability;
                        checker2d.OnInterrupt += InterruptAbility;
                        break;
                    case ARPGCore.com.EditorPhysicsType.Unity3D:
                        AbilityCollisionChecker3D checker3d = skillUser.MyTransform.gameObject.AddComponent<AbilityCollisionChecker3D>();
                        checker3d.CollisionLayers = intOptions.CollisionLayers;
                        checker3d.ToInterrupt = ability;
                        checker3d.OnInterrupt += InterruptAbility;
                        interruptors.Add(checker3d);
                        break;

                }
            }

            if (intOptions.OnCasterDamaged || intOptions.OnCasterDied)
            {
                AbilityInterruptHealthChecker healthchecker = skillUser.MyTransform.gameObject.AddComponent<AbilityInterruptHealthChecker>();
                healthchecker.Options = intOptions;
                healthchecker.ToInterrupt = ability;
                healthchecker.Hub = skillUser;
                healthchecker.OnInterrupt += InterruptAbility;
                interruptors.Add(healthchecker);

            }

            if (intOptions.Abilities.Length > 0)
            {
                AbilityInterruptChecker abilitychecker = skillUser.MyTransform.gameObject.AddComponent<AbilityInterruptChecker>();
                abilitychecker.Interruptors = intOptions.Abilities;
                abilitychecker.ToInterrupt = ability;
                abilitychecker.Hub = skillUser;
                abilitychecker.OnInterrupt += InterruptAbility;
                interruptors.Add(abilitychecker);
            }

            if (intOptions.InterruptEffects.Length > 0)
            {
                AbilityInterruptInflictionChecker inflictionchecker = skillUser.MyTransform.gameObject.AddComponent<AbilityInterruptInflictionChecker>();
                inflictionchecker.ToInterrupt = ability;
                inflictionchecker.OnInterrupt += InterruptAbility;
                inflictionchecker.Options = intOptions;
                inflictionchecker.Hub = skillUser;
                interruptors.Add(inflictionchecker);
            }
        }

        /// <summary>
        /// The main call to use abilities, takes into consideration timers, delays, and abilities already in use.
        /// </summary>
        /// <param name="abilityUser"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool TryCastAbility(IActorHub abilityUser, Ability ability)
        {
            if (ability.CheckRequirements(abilityUser) == false)
            {
                //dont meet requirements
                ARPGDebugger.DebugMessage("dont meet requirements", this);

                return false;
            }
            if (learned.Length == 0)
            {
                InitialziedLearned();
            }

            if (equipped.Length == 0)
            {
                InitalizeEquipped();
            }

            //if (equipped.Contains(ability) == false)//wont work with charging.
            //{
            //    //check if it's a basic skill
            //    Ability basic = GetBasicAttack(abilityUser);
            //    if (basic != ability)//if it's equal, we'll proceed onwards
            //    {
            //        ARPGDebugger.DebugMessage("Cant use an skill that we dont have equipped AND isn't at least a basic attack", this);
            //        return false;
            //    }
            //}

            for (int i = 0; i < delayTimers.Count; i++)
            {
                if (delayTimers[i].Cooldown.Ability.GetCategory() == ability.GetCategory())
                {
                    ARPGDebugger.DebugMessage("delaying, cant' use", this);
                    return false;
                }
            }

            for (int i = 0; i < durationTimers.Count; i++)
            {
                if (durationTimers[i].Cooldown.skill.GetCategory() == ability.GetCategory())
                {
                    ARPGDebugger.DebugMessage("Ability in use, cant use", this);
                    return false;
                }
            }

            for (int i = 0; i < cooldownTimers.Count; i++)
            {
                if (cooldownTimers[i].Vars.skill.GetCategory() == ability.GetCategory())
                {
                    ARPGDebugger.DebugMessage("Ability in in cooldown, cant use", this);
                    return false;
                }
            }

            appliedAbilities.TryGetValue(ability, out bool value);
            //we toggle, default will be false, so the opposite is true, which means we should try applying
            if (value == false)
            {
                appliedAbilitiesByCategory.TryGetValue(ability.GetCategory(), out Ability oldValue);
                if (oldValue != null)
                {
                    ARPGDebugger.DebugMessage("Other skill in use, can't use", this);
                    return false;
                }

                if (durationTimers == null)
                {
                    durationTimers = new List<AbilityDurationTimer>();
                }


                float cooldown = GetClampedAbilitySpeedCooldown(ability);

                AbilityDurationTimer cooldownTimer = new AbilityDurationTimer(new AbilityCooldown(cooldown, this, ability, abilityUser));
                durationTimers.Add(cooldownTimer);

                appliedAbilitiesByCategory[ability.GetCategory()] = ability;
                appliedAbilities[ability] = true;
                ARPGDebugger.DebugMessage("Cooldown timer " + cooldown, this);
                if (ability.AddDelay && ability.Delay > 0)
                {
                    if (delayTimers == null)
                    {
                        delayTimers = new List<AbilityDelayTimer>();
                    }

                    float delayt = GetClampedDelay(ability);
                   
                    //add a delay
                    AbilityDelayTimer newTimer = abilityUser.MyTransform.gameObject.AddComponent<AbilityDelayTimer>();
                    AbilityDelay delay = new AbilityDelay(delayt, this, ability, abilityUser);
                    newTimer.Cooldown = delay;
                    delayTimers.Add(newTimer);

                    ARPGDebugger.DebugMessage("Delay timer " + delayt, this);
                }
                else
                {
                    //do right away
                    StartAbility(abilityUser, ability);
                }


                return true;
            }
            ARPGDebugger.DebugMessage("Ability already in use. Can't cast", this);
            return false;

        }

        public bool IsInCooldown(Ability forAbility)
        {
            for (int i = 0; i < cooldownTimers.Count; i++)
            {
                if (forAbility == cooldownTimers[i].Vars.skill)
                {
                    return true;
                }   
            }
            return false;
        }

        public bool IsUsingAbility()
        {
            if (durationTimers.Count > 0 || delayTimers.Count > 0) return true;
            return false;
        }

        public bool GetAbilityActive(Ability ability)
        {
            if (ability == null) return false;
            appliedAbilities.TryGetValue(ability, out bool active);
            return active;
        }
        public Ability GetEquippedAbility(int atSlot)
        {
            if (equipped.Length == 0)
            {
                InitalizeEquipped();
            }
            if (atSlot < 0 || atSlot > equipped.Length - 1)
            {
                ARPGDebugger.DebugMessage("Trying to get ability at " + atSlot + " but we dont have that many equipped slots", this);
                return null;
            }

            return equipped[atSlot];
        }

        public int GetEquippedSlot(Ability forAbility)
        {
            if (equipped.Length == 0)
            {
                InitalizeEquipped();
            }

            for (int i = 0; i < equipped.Length; i++)
            {
                if (forAbility == equipped[i])
                {
                    return i;
                }
            }
            return -1;//negative 1 means not found
        }

        public void EquipAbility(Ability ability, int atSlot, bool andLearn)
        {
            if (learned.Contains(ability) == false)
            {
                if (andLearn)
                {
                    LearnAbility(ability);
                }
                else
                {
                    ARPGDebugger.DebugMessage("Can't equip ability, need to learn it first.", this);
                    return;
                }
            }

            if (equipped[atSlot] != null)
            {
                LearnAbility(equipped[atSlot]);
                OnAbilityUnEquip?.Invoke(equipped[atSlot], atSlot);
            }

            equipped[atSlot] = ability;
            OnAbilityEquip?.Invoke(ability, atSlot);
        }


        public void EquipAbility(Ability ability, int atSlot)
        {
            if (learned.Contains(ability) == false)
            {
                ARPGDebugger.DebugMessage("Can't equip ability, need to learn it first.", this);
                return;
            }

            if (equipped[atSlot] != null)
            {
                LearnAbility(equipped[atSlot]);
                OnAbilityUnEquip?.Invoke(equipped[atSlot], atSlot);
            }

            equipped[atSlot] = ability;
            OnAbilityEquip?.Invoke(ability, atSlot);
        }

        public int GetInventorySlot(Ability forAbility)
        {
            int slot = -1;
            if (learned.Length == 0)
            {
                InitialziedLearned();
            }

            for (int i = 0; i < learned.Length; i++)
            {
                if (forAbility == learned[i])
                {
                    slot = i;
                    break;
                }
            }
            return slot;
        }
        public bool LearnAbility(Ability ability)
        {
            if (ability == null) return false;

            int emptySlot = -1;
            bool known = false;
            for (int i = 0; i < learned.Length; i++)
            {
                if (ability != null && ability == learned[i])
                {
                    known = true;
                    break;
                }
            }

            if (known == true)
            {
                ARPGDebugger.DebugMessage("Can't learn ability, already know it", this);
                return false;
            }

            for (int i = 0; i < learned.Length; i++)
            {
                if (learned[i] == null)
                {
                    emptySlot = i;
                    break;
                }
            }

            if (emptySlot == -1)
            {
                ARPGDebugger.DebugMessage("Can't learn ability, no empty slots", this);
                return false;
            }

            //learned it
            learned[emptySlot] = ability;
            OnLearnedAbility?.Invoke(learned[emptySlot], emptySlot);
            return true;

        }

        public bool ForgetAbility(Ability ability)
        {
            if (ability == null) return false;

            List<int> indexes = new List<int>();
            for (int i = 0; i < learned.Length; i++)
            {
                if (ability != null && ability == learned[i])
                {
                    indexes.Add(i);
                }
            }

            if (indexes.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < indexes.Count; i++)
            {
                Ability forgetAbility = learned[indexes[i]];                
                OnForgetAbility?.Invoke(forgetAbility, indexes[i]);

                for (int j = 0; j < equipped.Length; j++)
                {
                    if (equipped[j] == forgetAbility)
                    {
                        equipped[j] = null;
                        OnAbilityUnEquip?.Invoke(null, j);
                    }
                }
                learned[indexes[i]] = null;

            }

           
            return true;

        }

        /// <summary>
        /// interrupts specfic ability
        /// </summary>
        /// <param name="ability"></param>
        public void InterruptAbility(Ability ability)
        {
            for (int i = 0; i < delayTimers.Count; i++)
            {
                if (delayTimers[i].Cooldown.Ability == ability)
                {
                    RemoveDelay(delayTimers[i]);
                }
            }


            for (int i = 0; i < durationTimers.Count; i++)
            {
                if (durationTimers[i].Cooldown.skill == ability)
                {
                    RemoveTimer(durationTimers[i].Cooldown.skill, durationTimers[i].Cooldown.User, durationTimers[i]);
                }
            }
        }
        /// <summary>
        /// interrupts all abilities
        /// </summary>
        public void InterruptAllAbilities()
        {

            for (int i = 0; i < delayTimers.Count; i++)
            {
                RemoveDelay(delayTimers[i]);
            }
   

            for (int i = 0; i < durationTimers.Count; i++)
            {
                RemoveTimer(durationTimers[i].Cooldown.skill, durationTimers[i].Cooldown.User, durationTimers[i]);
            }

            delayTimers.Clear();
            durationTimers.Clear();
        }
        /// <summary>
        /// Allows timers to control the duration of abilities. 
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="fromUser"></param>
        /// <param name="timer"></param>
        public void RemoveTimer(Ability ability, IActorHub fromUser, AbilityDurationTimer timer)
        {
            if (durationTimers.Contains(timer))
            {
                durationTimers.Remove(timer);
                RemoveAbility(ability, fromUser);
            }
        }

        public void RemoveCooldown(AbilityCooldownTimer timer)
        {
            if (cooldownTimers.Contains(timer))
            {
                cooldownTimers.Remove(timer);
                OnAbilityCooldownEnd?.Invoke(timer);
            }
        }

        public void RemoveDelay(AbilityDelayTimer timer)
        {
            if (delayTimers.Contains(timer))
            {
                delayTimers.Remove(timer);
                Destroy(timer);

            }
        }
        public AbilityDurationTimer GetTimer(Ability forAbility)
        {
            for (int i = 0; i < durationTimers.Count; i++)
            {
                if (durationTimers[i].Cooldown.skill == forAbility)
                {
                    return durationTimers[i];
                }

            }
            return null;
        }
        public Ability[] GetCopyAllLearned()
        {
            if (learned.Length == 0)
            {
                InitialziedLearned();
            }
            Ability[] copy = learned;
            return copy;


        }
        public Ability[] GetEquippedCopy()
        {
            if (equipped.Length == 0)
            {
                InitalizeEquipped();
            }
            Ability[] copy = new Ability[equipped.Length];
            copy = equipped;
            return copy;
        }


        #endregion

        #region private


        float GetClampedAbilityMulti()
        {
            clampedmulti = Mathf.Clamp(abilitySpeedMulti, clampminspeed, clampmaxspeed);
            return clampedmulti;
        }
        float GetClampedDelay(Ability forAbility)
        {
            //clamping, should really come before. 
            float delay = forAbility.Delay;
            delay *= GetClampedAbilitySpeedCooldown(forAbility);
            return delay;
        }


        void InitalizeEquipped()
        {
            if (learned.Length == 0)
            {
                InitialziedLearned();
            }
            equipped = new Ability[EquippedCap];
            for (int i = 0; i < equipped.Length; i++)
            {
                if (i > StartingAbilities.Length - 1) break;//no more
                equipped[i] = StartingAbilities[i];
            }
        }

      

        void InitialziedLearned()
        {
            learned = new Ability[LearnedCap];
            for (int i = 0; i < StartingAbilities.Length; i++)
            {
                learned[i] = StartingAbilities[i];
               
            }

        }

        void RemoveAbility(Ability buff, IActorHub fromUser)
        {
            appliedAbilitiesByCategory.TryGetValue(buff.GetCategory(), out Ability value);
            if (value != null)
            {
                EndAbility(buff, fromUser, value);

            }


        }

        void RemoveInterruptors()
        {
            for (int i = 0; i < interruptors.Count; i++)
            {
                interruptors[i].Remove();
            }
            interruptors.Clear();
        }
        private void EndAbility(Ability buff, IActorHub fromUser, Ability value)
        {
            RemoveInterruptors();
   
            appliedAbilitiesByCategory[buff.GetCategory()] = null;
            appliedAbilities[buff] = false;

          
            if (value.CoolDownRate > 0)
            {
                AbilityCooldownVars cdvars = new AbilityCooldownVars(value.CoolDownRate, this, value, fromUser);
                AbilityCooldownTimer cdtimer = new AbilityCooldownTimer(cdvars);
                cooldownTimers.Add(cdtimer);
                OnAbilityCooldownStart?.Invoke(cdtimer);
            }
          
            
            value.EndSkill(fromUser);

            OnAbilityEnd?.Invoke(value);
            OnAbilityUserEnd?.Invoke(fromUser, value);
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

        public UnityEngine.Object GetObject()
        {
            return this;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Data.AutoAssignUniqueID && Data.UniqueID != this.GetInstanceID())
            {
                Data.UniqueID = this.GetInstanceID();
            }

            if (Data.AutoName && string.IsNullOrEmpty(Data.Name) == false)
            {
                string path = AssetDatabase.GetAssetPath(this);
                AssetDatabase.RenameAsset(path, "AbilityController_" + Data.Name);
            }

        }

      
#endif
    }
}
