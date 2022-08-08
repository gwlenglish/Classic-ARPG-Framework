using GWLPXL.ARPGCore.Items.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// debug use only, place in scene with an AffixReaderSO and test it out.
/// </summary>
public class TestAffixReader : MonoBehaviour
{
    public string Phrase;
    public string Noun;
    public int AffixTries = 10;
    public int adverbchance = 101;
    public KeyCode GetNameKey = KeyCode.F1;
    public KeyCode PhraseKey = KeyCode.Space;
    public List<string> Words = new List<string>();
    public KeyCode WordKey = KeyCode.Return;
    public AffixReaderSO AffixReader;
    public bool Preload;
    private void Update()
    {
        if (Input.GetKeyDown(PhraseKey))
        {
            if (Preload)
            {
                AffixReader.GetSortedAffixesFromPreload(Phrase);
            }
            else
            {
                AffixReader.GetSortedAffixes(Phrase);
            }

        }

        if (Input.GetKeyDown(WordKey))
        {
            if (Preload)
            {
                AffixReader.GetSortedAffixesFromPreload(Words);
            }
            else
            {
                AffixReader.GetSortedAffixes(Words);
            }

        }

        if (Input.GetKeyDown(GetNameKey))
        {
            List<string> affixlist = new List<string>();

            for (int i = 0; i < AffixTries; i++)
            {
                int random = Random.Range(0, AffixReader.Affixreader.AffixOrders.Count);
                string rando = AffixReader.GetRandomAffix(random);
                if (affixlist.Contains(rando) == false)
                {
                    affixlist.Add(rando);
                }
            }

            string name = AffixReader.GetNameWithAffixesPreLoaded(affixlist, Noun, adverbchance);
            Debug.Log(name);
        }
    }
}
