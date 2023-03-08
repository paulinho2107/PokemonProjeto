using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon 

{
    [SerializeField] Pokemonbase _base;
    [SerializeField] int level;

    public Pokemonbase pBase { get { return _base; } }
    public int Level { get { return level; } }
    public List<Move>Moves { get; set; }
    public int HP { get; set; }

    public void init()
    {

        HP = MaxHp;

        //Movimentos
        Moves = new List<Move>();
        foreach(var move in pBase.LearnableMoves )
        {
            if(move.Level <= Level)
                Moves.Add(new Move(move.Base));
            if (Moves.Count >= 4)
                break;

            

        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((pBase.Attack * Level) / 48f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((pBase.Defense * Level) / 48f) + 5; }
    }

    public int spAttack
    {
        get { return Mathf.FloorToInt((pBase.SpAttack * Level) / 48f) + 5; }
    }

    public int SpDefense
    {
        get { return Mathf.FloorToInt((pBase.SpDefense * Level) / 48f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((pBase.Speed * Level) / 48f) + 5; }
    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((pBase.MaxHp * Level) /22f) + 0; }
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if(Random.value * 100f <= 6.5f)
        {
            critical = 2f;
                Debug.Log("Crítico");
        }
        float type = TypeChart.GetEffective(move.Base.Type, this.pBase.Type1) * TypeChart.GetEffective(move.Base.Type, this.pBase.Type2);
        Debug.Log("Deu dano no type:" + type);
        var damageDetails = new DamageDetails()
        {
            TypeEffect = type,
            Critical = critical,
            Fainted = false,

        };
        float attack = (move.Base.IsSpecial) ? attacker.spAttack : attacker.Attack;
        float defense = (move.Base.IsSpecial) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level )/5f + 2;
        float d = (a * move.Base.Power * ((float)attacker.Attack / Defense))/50f + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;

        if(HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetrandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }




}


public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffect { get; set; }

}





    

