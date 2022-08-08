

using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    /// <summary>
    /// Not yet implemented. Doesn't seem necessary at the moment. 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Auras/NEW_AuraSet")]
   
    public class ActorAuras : ScriptableObject
    {
        public Aura[] AvailableAuras = new Aura[0];

        public void AddAura(Aura anySkill)
        {
            for (int i = 0; i < AvailableAuras.Length; i++)
            {
                if (anySkill == AvailableAuras[i])
                {
                    //already know
                    return;
                }
            }

            System.Array.Resize(ref AvailableAuras, AvailableAuras.Length + 1);
            AvailableAuras[AvailableAuras.Length - 1] = anySkill;

        }

    }
}