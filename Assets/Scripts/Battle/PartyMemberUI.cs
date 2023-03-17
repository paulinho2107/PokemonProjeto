using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text NameText;
    [SerializeField] Text levelText;
    [SerializeField] Hpbar hpBar;

    [SerializeField] Color highlightedColor;
    [SerializeField] Color ActualColor;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        NameText.text = pokemon.pBase.Name;
        levelText.text = $"Lvl{pokemon.Level}";
        hpBar.SetHP((float)pokemon.HP / (float)pokemon.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if(selected)
        {
            NameText.color = highlightedColor;
        }
        else
        {
            NameText.color = Color.black; 
        }
    }
}
