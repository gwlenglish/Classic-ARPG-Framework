using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using GWLPXL.ARPGCore.Items.com;
namespace GWLPXL.ARPGCore.Statics.com
{

    /// <summary>
    /// static class to help with Affix sorting and naming
    /// </summary>
    public static class AffixHelper
    {
        static StringBuilder sb = new StringBuilder();
        static readonly string space = " ";


        /// <summary>
        /// full name with affixes
        /// </summary>
        /// <param name="noun1"></param>
        /// <param name="noun2"></param>
        /// <returns></returns>
        public static string GetNameWithAffixes(List<string> affixes, AffixReader reader, string noun)
        {
            string fullprefix = GetSortedAffixes(affixes, reader);
            sb.Clear();
            sb.Append(fullprefix);
            sb.Append(noun);
            string fullprefixandname = sb.ToString();
            return fullprefixandname;

        }
        /// <summary>
        /// returns first found
        /// </summary>
        /// <param name="words"></param>
        /// <param name="reader"></param>
        /// <param name="preload"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string GetPostNounFromPreload(List<string> words, AffixReader reader, Dictionary<string, int> preload, char delimiter = ' ')
        {
            if (preload.Count == 0)
            {
                LoadPostNouns(reader, preload);
            }

            for (int i = 0; i < words.Count; i++)
            {
                string key = words[i].ToLower();
                if (preload.ContainsKey(key))
                {
                    return words[i];
                }

            }
            return string.Empty;
        }

        /// <summary>
        /// returns first found
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="reader"></param>
        /// <param name="preload"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string GetPostNounFromPreload(string phrase, AffixReader reader, Dictionary<string, int> preload, char delimiter = ' ')
        {
            if (preload.Count == 0)
            {
                LoadPostNouns(reader, preload);
            }

            List<string> words = SplitPhrase(phrase, delimiter);

            return GetPostNounFromPreload(words, reader, preload, delimiter);
        }
        /// <summary>
        /// full name with affixes
        /// </summary>
        /// <param name="affixes"></param>
        /// <param name="reader"></param>
        /// <param name="noun"></param>
        /// <param name="preload"></param>
        /// <returns></returns>
        public static string GetNameWithAffixes(List<string> affixes, AffixReader reader, string noun, Dictionary<string, int> preload, int randomadverbchange = 0)
        {
            string fullprefix = GetSortedAffixesFromPreload(affixes, reader, preload, randomadverbchange);
            sb.Clear();
            sb.Append(fullprefix);
            sb.Append(noun);
            string fullprefixandname = sb.ToString();
            sb.Clear();
            return fullprefixandname;

        }
        public static bool WordEquals(string affix, string word)
        {
            return string.CompareOrdinal(affix.ToLower(), word.ToLower()) == 0;
        }
        /// <summary>
        /// sorts the affixes based on the order found in the reader. Returns as a phrase.
        /// </summary>
        /// <param name="affixesToSort"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string GetSortedAffixes(List<string> affixesToSort, AffixReader reader)
        {
            sb.Clear();
            List<AffixOrder> orders = reader.AffixOrders;
            List<SortedAffix> sortedAffixes = new List<SortedAffix>();
            for (int i = 0; i < affixesToSort.Count; i++)
            {
                string affix = affixesToSort[i];
                for (int j = 0; j < orders.Count; j++)
                {
                    AffixOrder order = orders[j];
                    int index = j;
                    for (int k = 0; k < order.Words.Count; k++)
                    {
                        string word = order.Words[k];
                        if (WordEquals(affix, word))
                        {
                            sortedAffixes.Add(new SortedAffix(affix, index));
                        }

                    }
                }
            }


            sortedAffixes = sortedAffixes.OrderBy(x => x.Index).ToList();
            for (int i = 0; i < sortedAffixes.Count; i++)
            {
                sb.Append(sortedAffixes[i].Affix);
                sb.Append(space);
            }
            sortedAffixes.Clear();
            string stringsorted = sb.ToString();
            UnityEngine.Debug.Log(stringsorted);

            return stringsorted;
        }
        /// <summary>
        /// Splits the phrase, then returns it sorted based on the reader order. You can overload the delimiter
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string GetSortedAffixes(string phrase, AffixReader reader, char delimiter = ' ')
        {
            List<string> split = phrase.Split(delimiter).ToList();
            return GetSortedAffixes(split, reader);
        }
        public static List<string> SplitPhrase(string phrase, char delimiter = ' ')
        {
            List<string> split = phrase.Split(delimiter).ToList();
            return split;
        }
        /// <summary>
        /// Returns a sorted version of the affixes, but uses a preloaded dictionary instead of looping. Mostly just an optimized version of the base.
        /// </summary>
        /// <param name="affixesToSort"></param>
        /// <param name="reader"></param>
        /// <param name="preloadDic"></param>
        /// <returns></returns>
        public static string GetSortedAffixesFromPreload(List<string> affixesToSort, AffixReader reader, Dictionary<string, int> preloadDic, int randomadverbchance = 0)
        {
            sb.Clear();

            if (preloadDic.Count == 0)
            {
                LoadReader(reader, preloadDic);
            }

            List<SortedAffix> sortedAffixes = new List<SortedAffix>();
            for (int i = 0; i < affixesToSort.Count; i++)
            {

                string word = affixesToSort[i];
                string key = word.ToLower();

                if (preloadDic.ContainsKey(key))
                {
                    bool has = sortedAffixes.Any(index => index.Index == preloadDic[key]);
                    if (has)
                    {
                        Debug.LogWarning("Already contains a word at index " + preloadDic[key].ToString() + ". Discarding");
                        continue;
                    }
                    string adverb = string.Empty;
                    if (randomadverbchance > 0)
                    {
                        int randomroll = Random.Range(0, 101);
                        if (randomroll <= randomadverbchance)
                        {
                            //get the adverbs
                            AffixOrder order = reader.AffixOrders[preloadDic[key]];
                            if (order.AdverbModifiers.Count > 0)
                            {
                                int randomadverb = Random.Range(0, order.AdverbModifiers.Count);
                                adverb = order.AdverbModifiers[randomadverb];

                            }

                        }
                    }
                    if (string.IsNullOrEmpty(adverb) == false)
                    {
                        string newword = adverb + space + word;
                        word = newword;
                    }
                    sortedAffixes.Add(new SortedAffix(word, preloadDic[key]));
                }
            }

            sortedAffixes = sortedAffixes.OrderBy(x => x.Index).ToList();
            for (int i = 0; i < sortedAffixes.Count; i++)
            {
                sb.Append(sortedAffixes[i].Affix);
                sb.Append(space);
            }


            string sortedaffixes = sb.ToString();


            return sortedaffixes;
        }
        /// <summary>
        /// Splits the phrase then returns a sorted version based on the reader order. You can overload the delimiter
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="reader"></param>
        /// <param name="preloadDic"></param>
        /// <returns></returns>
        public static string GetSortedAffixesFromPreload(string phrase, AffixReader reader, Dictionary<string, int> preloadDic, char delimiter = ' ')
        {
            List<string> split = phrase.Split(delimiter).ToList();
            return GetSortedAffixesFromPreload(split, reader, preloadDic);
        }

        public static string GetRandomAffix(int fromOrder, AffixReader reader)
        {
            if (fromOrder < 0 || fromOrder > reader.AffixOrders.Count) return null;

            AffixOrder order = reader.AffixOrders[fromOrder];
            if (order.Words == null || order.Words.Count == 0) return null;

            int rando = Random.Range(0, order.Words.Count);
            return order.Words[rando];
        }
        /// <summary>
        /// Keys a dictionary to word.ToLower() and index in the reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="preloadDic"></param>
        public static void LoadReader(AffixReader reader, Dictionary<string, int> preloadDic)
        {
            for (int i = 0; i < reader.AffixOrders.Count; i++)
            {
                AffixOrder order = reader.AffixOrders[i];
                int index = i;
                for (int j = 0; j < order.Words.Count; j++)
                {

                    string word = order.Words[j];

                    if (string.IsNullOrEmpty(word)) continue;

                    string key = word.ToLower();
                    if (preloadDic.ContainsKey(key))
                    {
                        UnityEngine.Debug.LogWarning("Duplicate entries in the reader. The word " + word.ToUpper() + " is found at index " + preloadDic[word].ToString() + " and " + index.ToString() + ". Will only use the first entry found of the word");
                        continue;
                    }
                    else
                    {
                        preloadDic[key] = index;
                    }

                }
            }
        }
        public static void LoadPostNouns(AffixReader reader, Dictionary<string, int> preloadDic)
        {
            for (int i = 0; i < reader.PostNounModifiers.Count; i++)
            {
                string word = reader.PostNounModifiers[i];
                if (string.IsNullOrEmpty(word)) continue;

                string key = word.ToLower();
                if (preloadDic.ContainsKey(key))
                {
                    UnityEngine.Debug.LogWarning("Duplicate entries in the reader. The word " + word.ToUpper() + " is found at index " + preloadDic[word].ToString() + " and " + i.ToString() + ". Will only use the first entry found of the word");
                    continue;
                }
                else
                {
                    preloadDic[key] = i;
                }


            }
        }

    }
}

