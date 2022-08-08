
using UnityEngine;

namespace GWLPXL.ARPGCore.Classes.com
{


    public class PlayerClass : MonoBehaviour, IClassUser
    {
        [SerializeField]
        ActorClass myClass;

        public ActorClass GetMyClass()
        {
            return myClass;
        }

        public void SetMyClass(ActorClass newClass)
        {
            myClass = newClass;
        }
    }
}