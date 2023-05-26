using System.Collections;
using UnityEngine;
using System;

public enum BattleState { Start, PlayerAction, PlayerMove, enemyMove, Busy, PartyScreen }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleDialogueBox dialogueBox;
    [SerializeField] Partyscreen partyScreen;

    public event Action<bool> OnBattleOver;


    BattleState state;
    int currentAction;
    int currentMove;
    [SerializeField] int currentMember;
    int escapeAttempts;

    PokeCrias playerParty;
    Pokemon wildPokemon;


    // Start is called before the first frame update
    public void StartBattle(PokeCrias playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }




    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(wildPokemon);
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        partyScreen.Init();

        dialogueBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogueBox.TypeDialogue($"A wild {enemyUnit.Pokemon.pBase.Name} appeared!!!");
        yield return new WaitForSeconds(1f);
        yield return dialogueBox.TypeDialogue("Choose an Action");
        yield return new WaitForSeconds(0.5f);
        PlayerAction();


    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogueBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    void playerMove()
    {
        state = BattleState.PlayerMove;
        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        enemyHud.HpCounter1();
        var move = playerUnit.Pokemon.Moves[currentMove];
        move.Paulimpas--;
        yield return dialogueBox.TypeDialogue($"{playerUnit.Pokemon.pBase.Name} used {move.Base.Name}!!");
        yield return new WaitForSeconds(1f);

        playerUnit.PlayAttackAnim();
        yield return new WaitForSeconds(0.5f);
        enemyUnit.PlayHitAnim();

        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        yield return new WaitForSeconds(1f);
        if (damageDetails.Fainted)
        {
            yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.pBase.Name} fainted.");
            enemyUnit.FaintAnim();

            yield return new WaitForSeconds(1.5f);
            OnBattleOver(true);


        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.enemyMove;
        playerHud.HpCounter1();
        var move = enemyUnit.Pokemon.GetrandomMove();
        move.Paulimpas--;
        yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.pBase.Name} used {move.Base.Name}!!");

        enemyUnit.PlayAttackAnim();
        yield return new WaitForSeconds(0.1f);
        playerUnit.PlayHitAnim();

        yield return new WaitForSeconds(1f);
        var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        yield return new WaitForSeconds(1f);

        if (damageDetails.Fainted)
        {
            yield return dialogueBox.TypeDialogue($"{playerUnit.Pokemon.pBase.Name} fainted.");
            playerUnit.FaintAnim();

            yield return new WaitForSeconds(1.5f);
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                playerUnit.Setup(nextPokemon);
                playerHud.SetData(nextPokemon);

                dialogueBox.SetMoveNames(nextPokemon.Moves);

                yield return dialogueBox.TypeDialogue("GO" + nextPokemon.pBase.name + "!!");
                yield return new WaitForSeconds(1f);
                yield return dialogueBox.TypeDialogue($"Choose an Action");
                yield return new WaitForSeconds(0.5f);
                PlayerAction();

            }

            else
            {
                OnBattleOver(true);
            }

        }
        else
        {
            PlayerAction();
            dialogueBox.EnableDialogueText(false);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical == 2f)
        {
            yield return dialogueBox.TypeDialogue("A Critical hit!!");
        }
        if (damageDetails.TypeEffect > 1f)
        {
            yield return dialogueBox.TypeDialogue("It's super effective");
        }
        else if (damageDetails.TypeEffect < 1f)
        {
            yield return dialogueBox.TypeDialogue("It's not super effective");
        }
    }


    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelector();
        }

        if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }



    void HandleActionSelector()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 3)
            {
                ++currentAction;
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction < 2)
            {
                currentAction += 2;

            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction > 1)
            {
                currentAction -= 2;
            }
        }
        dialogueBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (currentAction == 2)
            {
                playerMove();
            }
            if (currentAction == 1)
            {
                OpenPartyScreen();
            }
            if (currentAction == 3)
            {
                StartCoroutine(TryToEscape());
            }
            if (currentAction == 0)
            {
                //BAG
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
            {
                ++currentMove;
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
            {
                currentMove += 2;
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        dialogueBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialogueBox.EnableMoveSelector(false);
            dialogueBox.EnableDialogueText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentMember++;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentMember--;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentMember < playerParty.Pokemons.Count - 2)
            {
                
                currentMember += 2;
            }
            else if (currentMember == playerParty.Pokemons.Count - 1)
            {
                currentMember = 1;
            }
            else
            {
                currentMember = 0;
            }
            
        }

        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentMember > 1)
            {
                currentMember -= 2;
            }
            else if(currentMember == 1)
            {
                currentMember = playerParty.Pokemons.Count - 1;
            }
            else if (currentMember == 0)
            {
                currentMember = playerParty.Pokemons.Count - 2;
            }
        }

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);
        partyScreen.updateMemberSelection(currentMember);
    } 
    
    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;
        dialogueBox.EnableDialogueText(true);

        escapeAttempts++;

        int playerSpeed = playerUnit.Pokemon.Speed;
        int enemySpeed = enemyUnit.Pokemon.Speed;

        if(enemySpeed < playerSpeed)
        {
            Debug.Log("Correu");
            yield return dialogueBox.TypeDialogue("Corre caraio corre!!");
            OnBattleOver(true);
        }
        else
        {
            float f = ((playerSpeed * 128) / enemySpeed) + 30 * escapeAttempts;
            Debug.Log($"Primeiro calculo escape: {f}");
            float oddEscape = f % 256;
            Debug.Log($"Segundo calculo escape: {oddEscape}");

            if(UnityEngine.Random.Range(0,255) < oddEscape)
            {
                yield return dialogueBox.TypeDialogue("Corre caraio corre!!");
                    OnBattleOver(true);
            }
            else
            {
                yield return dialogueBox.TypeDialogue("Vish deu não");
                StartCoroutine(EnemyMove());
            }
        }
    }



    
}   
        

        



