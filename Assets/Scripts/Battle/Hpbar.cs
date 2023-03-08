using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hpbar : MonoBehaviour
{
    [SerializeField] GameObject health;
    [SerializeField] GameObject healthAmounth;
    // Start is called before the first frame update
    void Start()
    {
        //health.transform.localScale = new Vector3(0.5f, 1f);
    }

    //HP que o pokeon vai ter
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHpsmooth(float newHp)
    {
        float curHp = health.transform.localScale.x;
        float changeAmt = curHp - newHp;

        health.transform.localScale = new Vector3(newHp, 1f);
        while(curHp - newHp > Mathf.Epsilon)
        {
            curHp -= changeAmt * Time.deltaTime;
            healthAmounth.transform.localScale = new Vector3(curHp, 1f);
            yield return null;
        }
        healthAmounth.transform.localScale = new Vector3(newHp, 1f);
    }
}
