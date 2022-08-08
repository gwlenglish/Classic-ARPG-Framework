


using System.Collections.Generic;
using GWLPXL.ARPGCore.StatusEffects.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Combat.com
{
    /// <summary>
    /// interface for objects that apply Status Over Time (i.e. dots)
    /// </summary>
    public interface IApplySOT
    {
        List<IActorHub> GetSoTAppliedList();
        List<SOT> GetSOTS();
        EnvironmentSotEvents GetSOTEvents();
    }


}