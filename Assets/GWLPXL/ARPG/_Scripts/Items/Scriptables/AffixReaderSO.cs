using GWLPXL.ARPGCore.Statics.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GWLPXL.ARPGCore.Items.com
{

    /// <summary>
    /// SO Container for the AffixReader class
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/NEW_AffixReader")]
    public class AffixReaderSO : ScriptableObject
    {
        public AffixReader Affixreader;

        [System.NonSerialized]
        protected Dictionary<string, int> wordIndexDic = new Dictionary<string, int>();
        [System.NonSerialized]
        protected Dictionary<string, int> nounIndexDic = new Dictionary<string, int>();

        public virtual void ClearDictionary()
        {
            wordIndexDic.Clear();
        }
        public virtual void LoadReader()
        {
            AffixHelper.LoadReader(Affixreader, wordIndexDic);

        }
        public virtual string GetPostNoun(List<string> nouns)
        {
            return AffixHelper.GetPostNounFromPreload(nouns, Affixreader, nounIndexDic);
        }
        public virtual string GetPostNoun(string phrase)
        {
            return AffixHelper.GetPostNounFromPreload(phrase, Affixreader, nounIndexDic);
        }
        public virtual List<string> GetSplitAffixes(string phrase, char delimiter = ' ')
        {
            return AffixHelper.SplitPhrase(phrase, delimiter);
        }
        public virtual string GetRandomAffix(int fromOrder)
        {
            return AffixHelper.GetRandomAffix(fromOrder, Affixreader);
        }
        public virtual string GetNameWithAffixesPreLoaded(List<string> affixes, string noun, int randomadverbchance = 0)
        {
            return AffixHelper.GetNameWithAffixes(affixes, Affixreader, noun, wordIndexDic, randomadverbchance);
        }
        public virtual string GetNameWithAffixes(List<string> affixes, string noun)
        {
            return AffixHelper.GetNameWithAffixes(affixes, Affixreader, noun);
        }
        public virtual string GetSortedAffixes(string phrase)
        {
            return AffixHelper.GetSortedAffixes(phrase, Affixreader);
        }
        public virtual string GetSortedAffixesFromPreload(string phrase)
        {
            return AffixHelper.GetSortedAffixesFromPreload(phrase, Affixreader, wordIndexDic);
        }
        public virtual string GetSortedAffixesFromPreload(List<string> affixesToSort)
        {
            return AffixHelper.GetSortedAffixesFromPreload(affixesToSort, Affixreader, wordIndexDic);
        }
        public virtual string GetSortedAffixes(List<string> affixesToSort)
        {
            return AffixHelper.GetSortedAffixes(affixesToSort, Affixreader);
        }
    }

    [System.Serializable]
    public class AffixOrder
    {
        public string Label;
        public string EditorDescription = string.Empty;
        public List<string> Words = new List<string>();
        public List<string> AdverbModifiers = new List<string>();

    }

    public struct SortedAffix
    {
        public string Affix;
        public int Index;
        public SortedAffix(string affix, int index)
        {
            Affix = affix;
            Index = index;
        }
    }



    [System.Serializable]
    public class AffixReader
    {
        public List<AffixOrder> AffixOrders = new List<AffixOrder>();
        public List<string> PostNounModifiers = new List<string>();

    }





    #region editor

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(AffixReaderSO))]
    public class AffixReaderSOEditor : UnityEditor.Editor
    {
        readonly List<string> labels = new List<string>(10) {
        "Opinion",//unusual, lovely
        "Size",//big, small, tall
        "Physical Quality",//thin, rough
        "Shape",//round, square
        "Age",//young, old
        "Color",//blue, red
        "Origin",//Dutch, Japanese
        "Material",//metal, wood
        "Type",//general-purpose, four-sided, U-shaped
        "Purpose"//cleaning, hammering, cooking
    };

        public override void OnInspectorGUI()
        {
            AffixReaderSO so = (AffixReaderSO)target;
            List<AffixOrder> orders = so.Affixreader.AffixOrders;


            while (orders.Count < labels.Count)
            {
                orders.Add(new AffixOrder());
            }

            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].Label = labels[i];
            }


            //a way to save out the affix orders to json to edit elsewhere, and then to reimport into this.


            base.OnInspectorGUI();


            if (GUILayout.Button("Save to Json"))
            {
                string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save File", "Name", "json", "Choose where to save.");
                if (string.IsNullOrEmpty(path) == false)
                {
                    string json = JsonUtility.ToJson(so.Affixreader, true);
                    using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create))
                    {
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                    }
                }

                UnityEditor.AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Load from Json"))
            {
                string path = UnityEditor.EditorUtility.OpenFilePanel("Open", "Assets", "json");
                if (string.IsNullOrEmpty(path) == false)
                {
                    string json = string.Empty;
                    using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(fs))
                        {
                            json = reader.ReadToEnd();
                        }
                    }

                    so.Affixreader = JsonUtility.FromJson<AffixReader>(json);
                }
            }
        }
    }
#endif
    #endregion
}


