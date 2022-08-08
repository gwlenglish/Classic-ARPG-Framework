using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    public enum BlockOptions
    {
        None = 0,
        Disable = 1,
        Destroy = 2
    }
    [System.Serializable]
    public class ProjectileOptions
    {
        public LayerMask Blocking => blockingLayers;
        public BlockOptions Block => block;
        public string Name => descriptiveName;
        public string Description => description;
        public float Speed => speed;
        public bool DisableOnTouch => disableOnTouch;
        public bool FriendlyFire => friendlyFire;
        public float LifeTime => lifetime;
        [Header("Options")]
        [SerializeField]
        string descriptiveName = string.Empty;
        [SerializeField]
        string description = string.Empty;
        [SerializeField]
        [Range(0, 200)]
        float speed = 35;
        [SerializeField]
        bool disableOnTouch = true;
        [SerializeField]
        bool friendlyFire = false;
        [SerializeField]
        float lifetime = 10f;
        [SerializeField]
        LayerMask blockingLayers;
        [SerializeField]
        BlockOptions block = BlockOptions.Disable;

        public ProjectileOptions(string name, string desc, float _speed, bool _disableOnTouch, float _lifetime, bool _friendlyfire)
        {
            descriptiveName = name;
            description = desc;
            speed = _speed;
            disableOnTouch = _disableOnTouch;
            friendlyFire = _friendlyfire;
            lifetime = _lifetime;
        }
    }
}