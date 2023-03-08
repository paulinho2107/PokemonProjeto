using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueBox : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] float letterPS;

    [SerializeField] Text dialogText;
    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    [SerializeField] GameObject actionselector;
    [SerializeField] GameObject MoveSelector;
    [SerializeField] GameObject MoveDetails;

    [SerializeField] Color Highlightedcolor;
    [SerializeField] Color Actualcolor;

    [SerializeField] List<Text> moveTexts;
    [SerializeField] List<Text> actionTexts;


    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialogue(string dialog )
    {
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds((float)letterPS);
        }
    }

    public void EnableDialogueText(bool enabled)
    {
        dialogText.enabled = enabled;

    }

    public void EnableActionSelector(bool enabled)
    {
        actionselector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        MoveSelector.SetActive(enabled);
        MoveDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i< actionTexts.Count; i++)
        {
            if(i == selectedAction)
            {
                actionTexts[i].color = Highlightedcolor;
            }
            else
            {
                actionTexts[i].color = Actualcolor;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for(int i = 0; i < moveTexts.Count; i++)
        {
            if(i == selectedMove)
            {
                moveTexts[i].color = Highlightedcolor;

            }
            else
            {
                moveTexts[i].color = Actualcolor;
            }
            ppText.text = $"PP {move.Paulimpas}/{move.Base.PP}";
            typeText.text = move.Base.Type.ToString();
        }
        
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveTexts.Count; i++) 
        {
            if(i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }

}
