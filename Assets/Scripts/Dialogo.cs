using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogo 
{
    [SerializeField]int fontS;
    [SerializeField]string name;
    [SerializeField][TextArea(3, 10)] List<string> sentences;


    public string Name { get { return name; } }
    public int FontS { get { return fontS; } }
    public List<string> Sentences { get { return sentences ; } }

}
