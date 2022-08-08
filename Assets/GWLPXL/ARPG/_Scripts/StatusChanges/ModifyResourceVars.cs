using GWLPXL.ARPGCore.Types.com;

using UnityEngine;
using GWLPXL.ARPGCore.DebugHelpers.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
    #region helper

    [System.Serializable]
    public class ModifyResourceVars
    {
        public ResourceType Type = ResourceType.Health;
        public ElementType ElementDamage = ElementType.None;
        public StatusEffectSO[] StatusEffects = new StatusEffectSO[0];
        [Tooltip("Use Negative for damage, positive for regen")]
        public int AmountPerTick = 1;
        [Tooltip("If 0 or less, will tick forever")]
        public float Duration = 1f;
        [Tooltip("When enabled, will clamp the status effect duration to this duration.")]
        public bool ClampStatusEffectToDuration = false;
        public float TickRate = 1f;
        [Tooltip("How many max stacks at once. AmountPerTick * Stack = total.")]
        [Range(1, 5)]
        public int StackAmount = 1;

        public ModifyResourceVars(ResourceType _type, int _amountPerTick, float tickRate, float _duration, int _maxStack)
        {
            Type = _type;
            AmountPerTick = _amountPerTick;
            Duration = _duration;
            TickRate = tickRate;
            StackAmount = _maxStack;

        }
     
    }

    /// <summary>
    /// 
    /// </summary>
    /// 
    [System.Serializable]
    public class ModifyResourceDoTState : IDoT, ITick
    {
        public float CurrentTimer => timer;
        public ModifyResourceVars Runtime => runtime;
        protected ModifyResourceVars runtime = null;
        protected ModifyResourceVars key = null;

        protected IActorHub target = null;
        protected int currentStacks = 0;
        protected bool applied = false;
        protected float timer = 0;
        protected bool paused = false;
        public ModifyResourceDoTState(IActorHub target, ModifyResourceVars key)
        {
            this.target = target;
            this.key = key;
            runtime = new ModifyResourceVars(key.Type, key.AmountPerTick, key.TickRate, key.Duration, key.StackAmount);
            runtime.ElementDamage = key.ElementDamage;
            runtime.StatusEffects = key.StatusEffects;
            runtime.ClampStatusEffectToDuration = key.ClampStatusEffectToDuration;

            AddTicker();
        }
        /// <summary>
        /// pauses the ticking of the dot but still keeps it applied
        /// </summary>
        /// <param name="isPaused"></param>
        public virtual void SetPause(bool isPaused)
        {
            paused = isPaused;
        }
       
        public virtual void AddTicker()
        {
            TickManager.Instance.AddTicker(this);
        }
        /// <summary>
        /// apply dot. if dot already applied, add another stack of it
        /// </summary>
        public virtual  void ApplyDoT()
        {
            timer = 0;
            currentStacks += 1;
            if (currentStacks > runtime.StackAmount)
            {
                currentStacks = runtime.StackAmount;
            }
        
            applied = true;
            StatusEffectHelper.ApplyStatusEffects(target, runtime.StatusEffects);
        }
        /// <summary>
        /// remove a stack of the dot. If last stack, dot itself will be removed
        /// </summary>
        public virtual void RemoveStack()
        {
            currentStacks -= 1;
            if (currentStacks <= 0)
            {
                RemoveDoT();
            }
        }
        public virtual void DoTick()
        {
            if (applied == false) return;
            Tick();
        }


      

        public virtual float GetTickDuration() => runtime.TickRate;

      

        public virtual void RemoveDoT()
        {
            applied = false;
            RemoveTicker();
            SoTHelper.RemoveDot(target, key);
            //target.MyStatusEffects.RemoveDot(vars);
            if (runtime.ClampStatusEffectToDuration)
            {
                StatusEffectHelper.RemoveStatusEffects(target, runtime.StatusEffects);
            }

        }

        public virtual void RemoveTicker()
        {
            TickManager.Instance.RemoveTicker(this);
        }

        public virtual  void Tick()
        {

            if (paused) return;
            if (applied == false) return;
            if (runtime.Duration > 0)//if we are not infinite
            {
                timer += runtime.TickRate;//tick the timer
                if (timer > runtime.Duration)//if last tick, remove
                {

                    RemoveDoT();
                    return;
                }
            }

            if (runtime.AmountPerTick < 0)
            {
                SoTHelper.ReduceResource(target, runtime.AmountPerTick * currentStacks, runtime.Type, runtime.ElementDamage);
                //target.MyStatusEffects.ReduceResource(vars.AmountPerTick * currentStacks, vars.Type, vars.ElementDamage);
            }
            else if (runtime.AmountPerTick > 0)
            {
                SoTHelper.RegenResource(target, runtime.AmountPerTick * currentStacks, runtime.Type, runtime.ElementDamage);

                //target.MyStatusEffects.RegenResource(vars.AmountPerTick * currentStacks, vars.Type, vars.ElementDamage);
            }


            //ARPGDebugger.DebugMessage(ARPGDebugger.GetColorForSOTs("Modify Resource Duration " + timer), null);
            //ARPGDebugger.DebugMessage(ARPGDebugger.GetColorForSOTs("Modify Resource Stack Amount: " + currentStacks), null);

           
        }
    }

    #endregion
}