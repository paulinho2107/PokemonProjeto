using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (isPlayerUnit)
        {
            GetComponent<Image>().sprite = Pokemon.pBase.BackSprite;

        }
        else
        {
            GetComponent<Image>().sprite = Pokemon.pBase.FrontSprite;

        }
        PlayEntreAnimation();

        image.color = originalColor;
    }

    public void PlayEntreAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-1300f, originalPos.y);

        }
        else
        {
            image.transform.localPosition = new Vector3(1300f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 2f);
    }

    public void PlayAttackAnim()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }
        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));

    }

    public void PlayHitAnim()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.grey, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
        
    }

    public void FaintAnim()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 50, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
      
    

}
