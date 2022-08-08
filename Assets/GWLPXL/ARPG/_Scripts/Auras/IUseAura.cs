using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Auras.com
{
    public interface IUseAura
    {
        /// <summary>
        /// get the template
        /// </summary>
        /// <returns></returns>
        AuraController GetAuraControllerTemplate();
        /// <summary>
        /// get the runtime data
        /// </summary>
        /// <returns></returns>
        AuraController GetAuraControllerRuntime();
        /// <summary>
        /// toggle aura
        /// </summary>
        /// <param name="toToggle"></param>
        void ToggleAura(Aura toToggle);
        /// <summary>
        /// togglue aura at slot
        /// </summary>
        /// <param name="atEquippedSlot"></param>
        void ToggleAura(int atEquippedSlot);
        /// <summary>
        /// set runtime data
        /// </summary>
        /// <param name="controller"></param>
        void SetRuntimeAuraController(AuraController controller);
        /// <summary>
        /// set template
        /// </summary>
        /// <param name="newTemplate"></param>
        void SetTemplate(AuraController newTemplate);

        void SetActorHub(IActorHub newHub);
    }
}