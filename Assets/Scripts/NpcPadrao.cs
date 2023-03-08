using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcPadrao : MonoBehaviour
{
    [SerializeField] Transform[] caminhos;
    [SerializeField] float TempoDeEspera;
    public Animator anifala;
    public Dialogo dialogo;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(corotinaMovimento());
        anifala = GameObject.FindGameObjectWithTag("GUI/Dialogue").GetComponent<Animator>();
    }
    IEnumerator corotinaMovimento()
    {
        for(int i=0; i < caminhos.Length; i++)
        {
            Debug.Log(i);
            yield return StartCoroutine(MoveNpc(i));
            if(i >= caminhos.Length -1)
            {
                i = -1;
            }
        }

    } 

    IEnumerator MoveNpc(int numCaminho)
    {
        GetComponent<NavMeshAgent>().destination = caminhos[numCaminho].position;
        yield return new WaitForSeconds(TempoDeEspera);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetButtonDown("Fire1") && FindObjectOfType<Gerentedialogo>().isSpeaking == false)
        {
            FindObjectOfType<Gerentedialogo>().awakedialogue(dialogo);
            anifala.SetTrigger("DialogueShow");
            anifala.ResetTrigger("DIalogueHIde");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            anifala.SetTrigger("DIalogueHIde");
            anifala.ResetTrigger("DialogueShow");
            FindObjectOfType<Gerentedialogo>().isSpeaking = false;
        }
    }
}


