using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{

    [SerializeField] Text nametext;
    [SerializeField] Text leveltext;
    [SerializeField] Hpbar hpBar;
    [SerializeField] Text HpText;
    [SerializeField] int PreviousHP; 

    Pokemon _poke;
    public void SetData(Pokemon pokemon)
    {
        _poke = pokemon;

        nametext.text = pokemon.pBase.Name;
        leveltext.text = "Lvl " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP/ pokemon.MaxHp);
        HpText.text = pokemon.HP.ToString();
    }

    public IEnumerator UpdateHP()
    {
        StartCoroutine(HpCounter2());
        yield return hpBar.SetHpsmooth((float) _poke.HP / _poke.MaxHp);
        yield return HpCounter2();
       // HpText.text = _poke.HP.ToString();

    }

    public void HpCounter1()
    {
        PreviousHP = _poke.HP;
    }

    public IEnumerator HpCounter2()
    {
        int CurrentHP = _poke.HP;
        while (PreviousHP > CurrentHP)
        {
            PreviousHP -= 1;
            HpText.text = PreviousHP.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        PreviousHP = _poke.HP;
    }

}
