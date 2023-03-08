using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class Pokemonbase : ScriptableObject
{
    [Header("Informa��o & Assets")]
    [SerializeField] string name;
    [TextArea] [SerializeField] string description;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] Animator worldAnimation;

    [Header("Pokemon Stats")]
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    [SerializeField] List<LearnableMove> learanbleMoves;

    [System.Serializable]
    public class LearnableMove
    {
        [SerializeField] MoveBase movebase;
        [SerializeField] int level;

        public MoveBase Base
        {
            get { return movebase; }

        }

        public int Level
        {
            get { return level; }
        }
    }


    #region
    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description;}
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public Animator WorldAnimation
    {
        get { return worldAnimation; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public PokemonType Type1
    {
        get { return type1; }
    }

    public PokemonType Type2
    {
        get { return type2; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learanbleMoves; }
    }
    #endregion

}


public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Eletric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
}

public class TypeChart
{
    static float[][] chart =
    {       
                            //NOR   FIR   WAT   ELE  GRA    ICE   FIG   POI   GRO   FLY   PSY  BUG    ROC   GHO  DRA
        /*NOR*/ new float[] { 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 0.5f, 0f  , 1f },
        /*FIR*/ new float[] { 1f  , 0.5f, 0.5f, 1f  , 2f  , 2f  , 1f  , 1f  , 1f  , 1f  , 1f  , 2f  , 0.5f, 1f  ,0.5f},
        /*WAT*/ new float[] { 1f  , 2f  , 0.5f, 1f  , 0.5f, 1f  , 1f  , 1f  , 2f  , 1f  , 1f  , 1f  , 2f  , 1f  ,0.5f},
        /*ELE*/ new float[] { 1f  , 1f  , 2f  , 0.5f, 0.5f, 1f  , 1f  , 1f  , 0f  , 2f  , 1f  , 1f  , 1f  , 1f  ,0.5f},
        /*ELE*/ new float[] { 1f  , 1f  , 2f  , 0.5f, 0.5f, 1f  , 1f  , 1f  , 0f  , 2f  , 1f  , 1f  , 1f  , 1f  ,0.5f},
        /*GRA*/ new float[] { 1f  , 0.5f, 2f  , 1f  , 0.5f, 1f  , 1f  , 0.5f, 2f  , 0.5f, 1f  , 0.5f, 2f  , 1f  ,0.5f},
        /*ICE*/ new float[] { 1f  , 1f  , 0.5f, 1f  , 2f  , 0.5f, 1f  , 1f  , 2f  , 2f  , 1f  , 1f  , 1f  , 1f  , 2f },
        /*FIG*/ new float[] { 2f  , 1f  , 1f  , 1f  , 1f  , 2f  , 1f  , 0.5f, 1f  , 0.5f, 0.5f, 0.5f, 2f  , 0f  , 1f },
        /*POI*/ new float[] { 1f  , 1f  , 1f  , 1f  , 2f  , 1f  , 1f  , 0.5f, 0.5f, 1f  , 1f  , 2f  , 0.5f, 0.5f, 1f },
        /*GRO*/ new float[] { 1f  , 2f  , 1f  , 2f  , 0.5f, 1f  , 1f  , 2f  , 1f  , 0f  , 1f  , 0.5f, 2f  , 1f  , 1f },
        /*FLY*/ new float[] { 1f  , 1f  , 1f  , 0.5f, 2f  , 1f  , 2f  , 1f  , 1f  , 1f  , 1f  , 2f  , 0.5f, 1f  , 1f },
        /*PSY*/ new float[] { 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 2f  , 2f  , 1f  , 1f  , 0.5f, 1f  , 1f  , 1f  , 1f },
        /*BUG*/ new float[] { 1f  , 0.5f, 1f  , 1f  , 2f  , 1f  , 0.5f, 2f  , 1f  , 0.5f, 2f  , 1f  , 1f  , 0.5f, 1f },
        /*ROC*/ new float[] { 1f  , 2f  , 1f  , 1f  , 1f  , 2f  , 0.5f, 1f  , 0.5f, 2f  , 1f  , 2f  , 1f  , 1f  , 1f },
        /*GHO*/ new float[] { 0f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 0f  , 1f  , 1f  , 2f  , 1f },
        /*DRA*/ new float[] { 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 1f  , 2f },

    };

    public static float[][] Chart { get => chart; set => chart = value; }

    public static float GetEffective(PokemonType attacktype, PokemonType defensetype)
    {
        if(attacktype == PokemonType.None || defensetype == PokemonType.None)
        {
            return 1;
        }

        int row = (int)attacktype - 1;
        int col = (int)defensetype - 1;

        return Chart[row][col];
    }
}