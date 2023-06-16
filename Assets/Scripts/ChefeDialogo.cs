using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefeDialogo : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogText;
    [SerializeField] Text nameNPC;
    [SerializeField] int letterPS;
    public Animator animSpeak;

    public event Action OnShowDialog;
    public event Action OnClosedDialog;

    int currentLine = 0;
    Dialogo dialog;
    bool isTyping;

    public static ChefeDialogo Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        animSpeak = GameObject.FindGameObjectWithTag("GIU/Dialogue").GetComponent<Animator>();
    }


}
