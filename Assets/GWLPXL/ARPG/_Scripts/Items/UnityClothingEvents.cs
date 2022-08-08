using UnityEngine.Events;


namespace GWLPXL.ARPGCore.Items.com
{
 

    [System.Serializable]
    public class UnityClothingEvent : UnityEvent<Equipment> { }

    [System.Serializable]
    public class UnityClothingEvents
    {
        public UnityClothingEvent OnNewClothingEquipped = new UnityClothingEvent();
    }
    [System.Serializable]
    public class PlayerClothingEvents
    {
        public UnityClothingEvents SceneEvents = new UnityClothingEvents();
    }










}