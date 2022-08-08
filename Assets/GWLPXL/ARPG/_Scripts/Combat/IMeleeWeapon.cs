using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Combat.com
{
    public interface IMeleeWeapon
    {
        IDoDamage GetDamageComponent();
        void SetMeleeOptionData(MeleeData newdata);
        void SetDamageData(ActorDamageData newData);
        void SetUser(IActorHub forUser);
        MeleeData GetMeleeOptions();
    }


}