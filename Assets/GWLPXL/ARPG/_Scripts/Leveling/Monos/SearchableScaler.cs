
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Leveling.com
{

    /// <summary>
    /// uses the ilevel multi
    /// </summary>
    public class SearchableScaler : MonoBehaviour, IScale
    {
        [SerializeField]
        int unscaledlevel = 1;
        public int GetScaledLevel()
        {
            return Formulas.GetILevelMulti(unscaledlevel);
        }

        public int GetUNScaledLevel()
        {
            return unscaledlevel;
        }

        public void SetActorHub(IActorHub newHub)
        {
            return;
        }

        public void SetUNScaledLevel(int unscaled) => unscaledlevel = unscaled;
       
    }
}
