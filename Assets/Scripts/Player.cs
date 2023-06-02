using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    #region variáveis
    [SerializeField]CharacterController ccPlayer;
    [SerializeField] float velocidade;
    [SerializeField]Animator anim;
    [SerializeField] float[] Timeforencounter = new float[2];
    [SerializeField] bool IsMoving;
    [SerializeField]float timer;
    public LayerMask interactabelLayers;

    public event Action OnEncountered; 

   #endregion 
    // Start is called before the first frame update
    void Start()
    {
        ccPlayer = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        timer = UnityEngine.Random.Range(Timeforencounter[0], Timeforencounter[1]);
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        movimento();
        anima();
    }

    void movimento()
    {
        ccPlayer.SimpleMove(Physics.gravity);
        Vector3 movimento = new Vector3((Input.GetAxisRaw("Horizontal") * velocidade)/2,0,Input.GetAxisRaw("Vertical") * velocidade);
        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        ccPlayer.Move(movimento * Time.deltaTime);
    }
    void anima()
    {
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            anim.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
        }
        if(Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("IsMoving", false);
            IsMoving = false;
        }
             
        else
        {
            anim.SetBool("IsMoving", true);
            IsMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }
    }

    void Interact()
    {
        var facinDir = new Vector3(anim.GetFloat("Horizontal")*1.5f, 0.5f, anim.GetFloat("Vertical")*1.5f);
        var interactPos = transform.position + facinDir;

        Debug.DrawLine(transform.position, interactPos, Color.blue, 1f);

        var colliders = Physics.OverlapSphere(interactPos, 0.3f, interactabelLayers);

        if(colliders != null)
        {
            foreach(var collider in colliders)
            {
                collider.GetComponent<Interactable>()?.interact();
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.tag == "Terrains/LongGrass" && IsMoving == true)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                switch(UnityEngine.Random.Range(1,11))
                {
                    case 11:
                    case 10:
                    case 9:
                    case 8:
                    case 7:
                    case 6:
                        Debug.Log("pokemon merda.jpeg");
                        break;
                    case 5:
                        break;
                    case 4:
                        break;
                    case 3:
                        break;
                    case 2:
                        Debug.Log("Hitmonlee encontrado");
                        OnEncountered();
                        timer = UnityEngine.Random.Range(Timeforencounter[0], Timeforencounter[1]);
                        Debug.Log("achooooou");
                        break;
                    case 1:
                        int pokeRandom = 0;
                        pokeRandom = UnityEngine.Random.Range(1, 3);

                        if (pokeRandom == 1)
                        {
                            Debug.Log("Charmander");
                        }

                        if (pokeRandom == 2)
                        {
                            Debug.Log("tartaruga");
                        }

                        if (pokeRandom == 3)
                        {
                            Debug.Log("bixo de planta");
                        }
                        timer = UnityEngine.Random.Range(Timeforencounter[0], Timeforencounter[1]);
                        break;

                }
            }
        }
    }

}
