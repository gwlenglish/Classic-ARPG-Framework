using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.StatusEffects.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Statics.com
{
    [System.Serializable]
    public class StatusTrackerClass
    {

        public StatusEffectVars Key;
        public StatusEffectTimer Timer;
        public StatusTrackerClass(StatusEffectVars target, StatusEffectTimer timre)
        {
            Key = target;
            Timer = timre;
        }
    }
   

    public static class StatusEffectHelper
    {
        public static System.Action<IActorHub, StatusEffectVars> OnEffectAdded;
        public static System.Action<IActorHub, StatusEffectVars> OnEffectRemoved;
        public static System.Action<IActorHub, StatusEffectVars> OnApplyFailImmune;
        static Dictionary<IActorHub, List<StatusTrackerClass>> trackerdic = new Dictionary<IActorHub, List<StatusTrackerClass>>();
        static Dictionary<IActorHub, List<StatusEffectVars>> immuneDic = new Dictionary<IActorHub, List<StatusEffectVars>>();

        public static void RemoveImmunity(IActorHub target, StatusEffectVars vars)
        {
            if (immuneDic.ContainsKey(target) == false)
            {
                Debug.LogWarning("Trying to remove immunity but target is not in dictionary", target.MyTransform);
            }

            List<StatusEffectVars> value = immuneDic[target];
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i] == vars)
                {
                    value.RemoveAt(i);
                }
            }
            immuneDic[target] = value;



        }

        /// <summary>
        /// used to remove slow on the target by changing the move multipler
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vars"></param>
        public static void RemoveSlow(IActorHub target, SlowVars vars)
        {
            vars.Dicvalue.TryGetValue(target, out bool value);
            if (value == true)
            {
                if (target.MyMover != null)
                {
                    target.MyMover.ModifySpeedMultiplier(vars.Savedspeed);
                }
               
                value = false;
                vars.Dicvalue[target] = value;
            }

        }
        /// <summary>
        /// used to inflict slow on the target by changing the move multipler
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vars"></param>
        public static void InflictSlow(IActorHub target, SlowVars vars)
        {

            vars.Dicvalue.TryGetValue(target, out bool value);
            if (value == false)
            {
                vars.Savedspeed = 1 - vars.SlowPercent;
                if (target.MyMover != null)
                {
                    target.MyMover.ModifySpeedMultiplier(-(1 - vars.SlowPercent));
                }
               
                value = true;
                vars.Dicvalue[target] = value;
            }

        }
        /// <summary>
        /// Removes a status effect from the target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="effects"></param>
        public static void RemoveStatusEffects(IActorHub target, StatusEffectSO[] effects)
        {
            if (effects == null || effects.Length == 0) return;

            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] == null) continue;
                RemoveEffect(target, effects[i].Vars);
            }
        }
        /// <summary>
        /// Applies a status effect to the target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="effects"></param>
        public static void ApplyStatusEffects(IActorHub target, StatusEffectSO[] effects)
        {
            if (effects == null || effects.Length == 0) return;

            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] == null) continue;
                ApplyEffect(target, effects[i].Vars);
            }
        }

 /// <summary>
 /// returns all active status effects on user
 /// </summary>
 /// <param name="forUser"></param>
 /// <returns></returns>
        public static List<StatusEffectVars> GetAllActiveEffects(IActorHub forUser)
        {
            List<StatusEffectVars> _temp = new List<StatusEffectVars>();
            if (trackerdic.ContainsKey(forUser))
            {
                List<StatusTrackerClass> trackers = trackerdic[forUser];
                for (int i = 0; i < trackers.Count; i++)
                {
                    _temp.Add(trackers[i].Key);
                }
            }
            return _temp;
        }
        /// <summary>
        /// to do, clean up the params
        /// </summary>
        /// <param name="target"></param>
        /// <param name="effect"></param>
        /// <param name="currentStatusEffects"></param>
        /// <param name="currentEffects"></param>
        public static void ApplyEffect(IActorHub target, StatusEffectVars effect)
        {
            if (immuneDic.ContainsKey(target) == false)
            {
                immuneDic[target] = new List<StatusEffectVars>();
            }
            List<StatusEffectVars> immunities = immuneDic[target];
            for (int i = 0; i < immunities.Count; i++)
            {
                if (immunities[i] == effect)
                {
                    ARPGDebugger.DebugMessage(ARPGDebugger.GetColorForSOTs("Can't apply " + effect.EffectName + ", Immune"), target.MyTransform);
                    OnApplyFailImmune?.Invoke(target, effect);
                    return;
                }
            }

            if (trackerdic.ContainsKey(target) == false)
            {
                trackerdic[target] = new List<StatusTrackerClass>();
            }

            StatusTrackerClass tracker = null;
            List<StatusTrackerClass> list = trackerdic[target];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Key.EffectName == effect.EffectName)
                {
                    //has it
                    tracker = list[i];
                    break;
                }
            }

            if (tracker == null)
            {
                //first time
                StatusTracker newtracker = new StatusTracker(target, effect, 1);
                StatusEffectTimer newtimer = new StatusEffectTimer(newtracker);
                StatusTrackerClass trackerclass = new StatusTrackerClass(effect, newtimer);
                tracker = trackerclass;

                for (int i = 0; i < effect.Logics.Length; i++)
                {
                    effect.Logics[i].DoLogic(target);
                }

                list.Add(tracker);
                OnEffectAdded?.Invoke(target, effect);
            }
            else
            {
                //Debug.Log("Has effect");
                if (effect.RefreshOnApply)
                {
                    //Debug.Log("Try Refresh");
                    if (tracker.Timer.Vars.Stacks < effect.MaxRefreshAmount)
                    {
                        tracker.Timer.Refresh();
                    }
                }
            }
            trackerdic[target] = list;





        }

        public static void RemoveEffect(IActorHub target, StatusEffectVars effect)
        {
            if (trackerdic.ContainsKey(target) == false)
            {
                Debug.LogWarning("Trying to remove effect but not in dictionary " + effect.EffectName);
                return;
            }
            StatusTrackerClass tracker = null;
            List<StatusTrackerClass> list = trackerdic[target];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Key.EffectName == effect.EffectName)
                {
                    tracker = list[i];
                    list.RemoveAt(i);
                    break;
                }
            }

            if (tracker != null)
            {
                for (int i = 0; i < effect.Logics.Length; i++)
                {
                    effect.Logics[i].UnDoLogic(target);
                }

                ARPGDebugger.DebugMessage(ARPGDebugger.GetColorForSOTs("Effect removed: " + effect.EffectName + " on " + target.MyTransform.name), target.MyTransform);
                tracker = null;
                OnEffectRemoved?.Invoke(target, effect);

                if (effect.ImmunityDuration > 0)
                {
                    StatusEffectImmunity newimmunity = new StatusEffectImmunity(target, effect);
                    if (immuneDic.ContainsKey(target) == false)
                    {
                        immuneDic[target] = new List<StatusEffectVars>();
                    }
                    List<StatusEffectVars> value = immuneDic[target];
                    value.Add(effect);
                    immuneDic[target] = value;
                }
                //add immunity timer
            }
            trackerdic[target] = list;
        }

       
       
    }

    [System.Serializable]
    public class StatusTracker
    {
      
        public IActorHub Target;
        public StatusEffectVars Effect;
        public int Stacks;
        public StatusTracker(IActorHub hub, StatusEffectVars vars, int stacks)
        {
            Target = hub;
            Effect = vars;
            Stacks = stacks;
        }
    }

    [System.Serializable]
    public class StatusEffectTimer : ITick
    {
        public StatusTracker Vars;
        float timer = 0;
        public StatusEffectTimer(StatusTracker vars)
        {
            this.Vars = vars;
            timer = 0;
            AddTicker();
        }

        
        public void Refresh()
        {
            ARPGDebugger.DebugMessage("Refreshed Status Effect " + Vars.Effect.EffectName, Vars.Target.MyTransform);
            Vars.Stacks += 1;
            timer = 0;
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            timer += GetTickDuration();
            //Debug.Log(Vars.Stacks + " Stacks");
            ////Debug.Log(timer + " timer");
            //Debug.Log(Vars.Effect.EffectName + " name");
            if (timer >= Vars.Effect.Duration)
            {
                RemoveTicker();
            }

        }

        public float GetTickDuration() => Time.deltaTime;
       


        public void RemoveTicker()
        {
            StatusEffectHelper.RemoveEffect(Vars.Target, Vars.Effect);
            TickManager.Instance.RemoveTicker(this);

        }
    }

    public class StatusEffectImmunity : ITick
    {

        IActorHub target;
        StatusEffectVars immune;
        
        public StatusEffectImmunity(IActorHub target, StatusEffectVars immune)
        {
            this.target = target;
            this.immune = immune;
            AddTicker();
        }
        public void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }

        public void DoTick()
        {
            RemoveTicker();
        }

        public float GetTickDuration() => immune.ImmunityDuration;


        public void RemoveTicker()
        {
            StatusEffectHelper.RemoveImmunity(target, immune);
            TickManager.Instance.RemoveTicker(this);
        }
    }
}