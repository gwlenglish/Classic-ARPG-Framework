
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;


namespace GWLPXL.ARPGCore.States.com
{
    [System.Serializable]
    public class GenericAnimate2D
    {
        public string AnimatorStateName;
        public GlobalMoveDirection MovementDirection;
        public GlobalFacingDirection FacingDirection;
    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/States/Player/2D/Animation/New")]

    public class GenericAnimate2DSO : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        public GenericAnimate2D[] States = new GenericAnimate2D[0];

        public Object GetObject() => this;


        public TextAsset GetTextAsset() => config;


        public void SetTextAsset(TextAsset textAsset) => config = textAsset;

    }




}