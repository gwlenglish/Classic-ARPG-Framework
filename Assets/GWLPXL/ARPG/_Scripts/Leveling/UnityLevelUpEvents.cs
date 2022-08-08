using UnityEngine.Events;

namespace GWLPXL.ARPGCore.Leveling.com
{
    [System.Serializable]
    public class UnityLevelUpEvent : UnityEvent<int> { }


    [System.Serializable]
    public class UnityLevelingEvents
    {
        public UnityLevelUpEvent OnLevelUp;
        public UnityLevelUpEvent OnXPGain;
    }
   
    [System.Serializable]
    public class PlayerLevelEvents
    {
        //  public LevelUpEvent GameEvents;//on the actual attributes
        public UnityLevelingEvents SceneEvents = new UnityLevelingEvents();
    }


}