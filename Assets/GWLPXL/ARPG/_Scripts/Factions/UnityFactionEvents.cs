
using GWLPXL.ARPGCore.Types.com;

using UnityEngine.Events;

[System.Serializable]
public class UnityFactionEvent : UnityEvent<FactionTypes, int> { }


[System.Serializable]
public class ActorFactionEvents
{
    //  public LevelUpEvent GameEvents;//on the actual attributes
    public UnityFactionEvents SceneEvents = new UnityFactionEvents();
}
[System.Serializable]
public class PlayerFactionEvents
{
    //  public LevelUpEvent GameEvents;//on the actual attributes
    public UnityFactionEvents SceneEvents = new UnityFactionEvents();
}


[System.Serializable]
public class UnityFactionEvents
{
    public UnityEvent OnAnyRepModified;
    public UnityFactionEvent OnRepIncreased;
    public UnityFactionEvent OnRepDecreased;
}