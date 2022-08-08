
using GWLPXL.ARPGCore.Abilities.com;
using UnityEngine.Events;
/// <summary>
/// Used to extend unity event
/// </summary>
namespace GWLPXL.ARPGCore.com
{
    [System.Serializable]
    public class UnityAbilityTriggerEvent : UnityEvent<Ability> { }


    [System.Serializable]
    public class UnityAbilityEvents
    {
        public UnityAbilityTriggerEvent OnAbilityTriggered;
        public UnityAbilityTriggerEvent OnAbilityEnd;
        public UnityAbilityTriggerEvent OnAbilityLearned;
        public UnityAbilityTriggerEvent OnAbilityForgot;
        public UnityEvent OnAbilityFailedCost;
        public UnityAbilityTriggerEvent OnCostFailedAbility;
        public UnityEvent OnIntendedAbilityChanged;
        public UnityAbilityTriggerEvent OnSetNewIntendedAbility;

    }
    [System.Serializable]
    public class ActorAbilityEvents
    {
        //  public LevelUpEvent GameEvents;//on the actual attributes
        public UnityAbilityEvents SceneEvents = new UnityAbilityEvents();
    }




}