using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWritter : MonoBehaviour
{
    Text uitext;
    string textwrite;
    int characterIndex;
    float timepercharacter;
    float timer;

    public void addwriter(Text uitext, string textwrite, float timepercharacter)
    {
        this.uitext = uitext;
        this.textwrite = textwrite;
        this.timepercharacter = timepercharacter;
        characterIndex = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(uitext != null)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer += timepercharacter;
                characterIndex++;
                uitext.text = textwrite.Substring(0, characterIndex);       
            }
        }
    }
}
