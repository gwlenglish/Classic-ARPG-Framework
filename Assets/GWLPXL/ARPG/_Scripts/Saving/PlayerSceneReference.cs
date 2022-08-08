

using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Saving.com
{
    [System.Serializable]
    public class PlayerSceneReference
    {
        public Player SceneRef;
        public PlayerPersistant PersistData;
        public PlayerSceneReference(Player player, PlayerPersistant data)
        {
            SceneRef = player;
            PersistData = data;
        }
    }

}