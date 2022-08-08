


namespace GWLPXL.ARPGCore.Statics.com
{
    /// <summary>
    /// Keep outside editor ifs to prevent build errors. 
    /// </summary>
    public interface ISaveJsonConfig
    {
        void SetTextAsset(UnityEngine.TextAsset textAsset);
        UnityEngine.TextAsset GetTextAsset();
        UnityEngine.Object GetObject();

    }
}
