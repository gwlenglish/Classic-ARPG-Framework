using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/StatusEffects/New")]
    public class StatusEffectSO : ScriptableObject
    {
        public StatusEffectVars Vars;
    }

    [System.Serializable]
    public class StatusEffectVars
    {
        public string EffectName = "Empty";
        [HideInInspector]
        public int MaxStacks = 1;//these aren't stacking types, will remove this eventually.
        [Tooltip("Setting to 0 or -1 will last forever.")]
        public float Duration = 1;
        [Tooltip("If true, will refresh the timer on application.")]
        public bool RefreshOnApply = true;
        [Tooltip("The amount of refreshes allowed")]
        public int MaxRefreshAmount = 1;
        [Tooltip("Immunity from the effect being applied again. Put 0 for no immunity.")]
        public float ImmunityDuration = .25f;
        public StatusEffectLogic[] Logics = new StatusEffectLogic[0];

    }


    public abstract class StatusEffectLogic : ScriptableObject
    {
        public abstract void DoLogic(IActorHub target);
        public abstract void UnDoLogic(IActorHub target);
    }
}