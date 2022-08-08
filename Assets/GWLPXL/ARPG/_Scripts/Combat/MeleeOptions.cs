using UnityEngine;
namespace GWLPXL.ARPGCore.Combat.com
{
    [System.Serializable]
    public class MeleeOptions
    {
        public string Name => descriptiveName;
        public string Description => description;
        public bool FriendlyFire => friendlyFire;
        [SerializeField]
        string descriptiveName = string.Empty;
        [SerializeField]
        string description = string.Empty;
        [SerializeField]
        bool friendlyFire = false;

        public MeleeOptions(string name, string description, bool _friendlyfire)
        {
            descriptiveName = name;
            this.description = description;
            friendlyFire = _friendlyfire;
        }
    }


}