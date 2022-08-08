using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptySpace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            Color newcolor = images[i].color;
            newcolor.a = 0;
            images[i].color = newcolor;
        }

        Text[] texts = GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = string.Empty;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
