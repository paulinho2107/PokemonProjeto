using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogo dialog;
    public void interact()
    {
        Debug.Log("Interagindo pra carai");
    }
}
