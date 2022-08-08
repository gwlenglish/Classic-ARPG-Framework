using GWLPXL.ARPGCore.Abilities.com;

using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.PlayerInput.com
{
    public interface IAbilityController
    {
        void SetActorHub(IActorHub newHub);
    }
    /// <summary>
    /// handles ability inputs
    /// </summary>
    public class PlayerAbilityInputsController : MonoBehaviour, ITick, IAbilityController
    {

        IActorHub owner = null;
        bool hasAbilities = false;

        private void Start()
        {
          
            AddTicker();
        }

        private void OnDestroy()
        {
            RemoveTicker();
        }
        public void AddTicker() => TickManager.Instance.AddTicker(this);
       

        public void DoTick()
        {
           
         
            if (hasAbilities)
            {
                //here we check if we have a skill to use and the player has hit the input
                Ability activeSkill = owner.InputHub.AbilityInputs.GetFirstBasicAttack();
                if (activeSkill == null)//if no basic one, check others
                {
                    activeSkill = owner.InputHub.AbilityInputs.GetFirstAbilityInput();

                }

                if (activeSkill == null)
                {
                    owner.MyAbilities.SetIntendedAbility(null);
                }
                else
                {
                    owner.MyAbilities.SetIntendedAbility(activeSkill);
                    owner.MyAbilities.TryCastAbility(activeSkill);
                }
            }
            
        }

        public float GetTickDuration() => Time.deltaTime;


        public void RemoveTicker() => TickManager.Instance.RemoveTicker(this);

        public void SetActorHub(IActorHub newHub)
        {
            owner = newHub;
            if (owner.InputHub.AbilityInputs != null && owner.MyAbilities != null)
            {
                hasAbilities = true;
            }
        }
       
    }
}