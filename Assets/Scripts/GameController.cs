using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle};

public class GameController : MonoBehaviour
{
    [SerializeField] Player playerCont;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCam;

    GameState state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = GameState.FreeRoam;
        playerCont.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerCont.HandleUpdate();
        }
        else if(state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }

    }

    void StartBattle()
    {
        state = GameState.Battle;
        playerCont.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);
        worldCam.gameObject.SetActive(false);

        var playerParty = playerCont.GetComponent<PokeCrias>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty, wildPokemon);


    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        playerCont.gameObject.SetActive(true);
        worldCam.gameObject.SetActive(true);

    }


}
