
using UnityEngine;
using System.Text;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Abilities.com
{

    /// <summary>
    /// abstract class that defines an ability requirement. Inherit to make your own.
    /// </summary>
    public abstract class AbilityRequirement : ScriptableObject
    {
        protected StringBuilder sb = new StringBuilder();
        public abstract bool HasRequirements(IActorHub forUser);
        public abstract string GetDescription();
    }
}