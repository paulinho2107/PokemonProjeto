using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gerentedialogo : MonoBehaviour
{
    public Text nametext;
    public Text dialoguetext;
    public Queue<string> sentences;
    public Animator animfala;
    public bool isSpeaking = false;
    public int fontS;
    // Start is called before the first frame update
    void Start()
    {
        animfala = GameObject.FindGameObjectWithTag("GUI/Dialogue").GetComponent<Animator>();
        sentences = new Queue<string>();
        dialoguetext.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && isSpeaking == true)
        {
            StartCoroutine(DisplayNextSentence());
        }
        
    }

    public void awakedialogue(Dialogo dialogof)
    {
        dialoguetext.text = "";
        StartCoroutine(Startdialogue(dialogof));
    }

    IEnumerator Startdialogue(Dialogo dialogoc)
    {
        nametext.text = dialogoc.name;
        sentences.Clear();
        foreach (string sentence in dialogoc.sentences)
        {
            sentences.Enqueue(sentence);
        }
        yield return new WaitForSeconds(1.5f);
        dialoguetext.fontSize = dialogoc.fontS;
        StartCoroutine(DisplayNextSentence());
    }
    
        IEnumerator DisplayNextSentence()
        {
            if(sentences.Count == 0)
            {
                StartCoroutine(EndDialogue());
                yield return null;
            }
            string sentence = sentences.Dequeue();
            //dialoguetext.text = sentence;
            FindObjectOfType<TextWritter>().addwriter(dialoguetext, sentence, 0.1f);
            isSpeaking = true;
        }

        IEnumerator EndDialogue()
        {
            animfala.SetTrigger("DIalogueHIde");
            animfala.ResetTrigger("DialogueShow");
            isSpeaking = false;
            yield return null;




        }
    
}
