

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.StatusEffects.com
{
 
    /// <summary>
    /// attach to object with iactorhub to give a regen effect
    /// </summary>
    public class RegenResource : MonoBehaviour
    {
        public EnvironmentSotEvents SotEvents;
        public ModifyResourceVars Vars;
        protected IActorHub user = null;
        protected ModifyResourceDoTState runtime;
      

        protected virtual void Awake()
        {
            user = GetComponent<IActorHub>();
        }

        protected virtual void Start()
        {
            if (user != null && user.MyStatusEffects != null)
            {
                runtime =  SoTHelper.AddDoT(user, Vars);

            }
     
        }

    }
}