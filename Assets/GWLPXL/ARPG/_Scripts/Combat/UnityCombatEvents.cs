

using GWLPXL.ARPGCore.Abilities.Mods.com;
using GWLPXL.ARPGCore.com;
using UnityEngine.Events;

namespace GWLPXL.ARPGCore.Combat.com
{
    /// <summary>
    /// unity event overrides for combat
    /// </summary>


    [System.Serializable]
    public class UnityOnDamagedEvent : UnityEvent<AttackValues, IReceiveDamage> { }

    [System.Serializable]
    public class UnityOnDamageComponentEnabled : UnityEvent<IDoDamage> { }
    [System.Serializable]
    public class UnityOnDamageComponentDisabled : UnityEvent<IDoDamage> { }
    [System.Serializable]
    public class UnityOnWeaponStatusActive : UnityEvent<IWeaponModification> { }
    [System.Serializable]
    public class UnityOnWeaponStatusInActive : UnityEvent<IWeaponModification> { }
    [System.Serializable]
    public class UnitySoloLaunch : UnityEvent<IActorHub> { }

    [System.Serializable]
    public class UnityEnvironmentDamageEvents
    {
        public UnityOnDamagedEvent OnElementalDamageOther = new UnityOnDamagedEvent();
        public UnityOnDamageComponentEnabled OnDamageComponentEnabled = new UnityOnDamageComponentEnabled();
        public UnityOnDamageComponentDisabled OnDamageComponentDisabled = new UnityOnDamageComponentDisabled();

    }

    [System.Serializable]
    public class UnityWeaponStatusEvents
    {
        public UnityOnDamagedEvent OnPhysicalDamagedOther = new UnityOnDamagedEvent();
        public UnityOnDamagedEvent OnElementalDamageOther = new UnityOnDamagedEvent();
        public UnityOnWeaponStatusActive OnWeaponStatusActive = new UnityOnWeaponStatusActive();
        public UnityOnWeaponStatusInActive OnWeaponStatusInActive = new UnityOnWeaponStatusInActive();
    }
    [System.Serializable]
    public class UnityDamageEvents
    {
        public UnityEvent OnDamagedOther = new UnityEvent();
        public UnityOnDamagedEvent OnPhysicalDamagedOther = new UnityOnDamagedEvent();
        public UnityOnDamagedEvent OnElementalDamageOther = new UnityOnDamagedEvent();
        public UnityOnDamageComponentEnabled OnDamageComponentEnabled = new UnityOnDamageComponentEnabled();
        public UnityOnDamageComponentDisabled OnDamageComponentDisabled = new UnityOnDamageComponentDisabled();
        public UnityEvent OnDamageComponentStart = new UnityEvent();
        public UnityEvent OnDamageComponentEnd = new UnityEvent();
    }
    [System.Serializable]
    public class ActorDamageEvents
    {
        //  public LevelUpEvent GameEvents;//on the actual attributes
        public UnityDamageEvents SceneEvents = new UnityDamageEvents();
    }

    [System.Serializable]
    public class DamageSourceEvents
    {
        public UnityWeaponStatusEvents SceneEvents = new UnityWeaponStatusEvents();
    }
    [System.Serializable]
    public class EnvironmentDamageEvents
    {
        public UnityEnvironmentDamageEvents SceneEvents = new UnityEnvironmentDamageEvents();
    }

    [System.Serializable]
    public class ProjectileWeaponEvents
    {
        public UnityEvent OnFired = new UnityEvent();

    }
    [System.Serializable]
    public class ProjectileEvents
    {
        public ProjectileWeaponEvents SceneEvents = new ProjectileWeaponEvents();
    }
    
}