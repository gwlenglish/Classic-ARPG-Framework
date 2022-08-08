using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.com
{
    [System.Serializable]
    public class AbilityCooldown
    {
        public float CooldownRate = 1;
        public float timer = 0;
        public AbilityController Holder = null;
        public Ability skill = null;
        public IActorHub User = null;
        public bool Pause = false;
        public AbilityCooldown(float cooldowntime, AbilityController forSkill, Ability _skill, IActorHub _user)
        {
            CooldownRate = cooldowntime;
            timer = 0;
            Holder = forSkill;
            skill = _skill;
            User = _user;
        }
    }
}