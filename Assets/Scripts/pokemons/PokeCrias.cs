using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PokeCrias : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    private void Start()

    {
        foreach(var pokemon in pokemons)
        {
            pokemon.init();
        }
        
    }


    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}
