using System.Collections;
using UnityEngine;
using System;

public enum BattleState { Start, PlayerAction, PlayerMove, enemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleDialogueBox dialogueBox;

    public event Action<bool> OnBattleOver;


    BattleState state;
    int currentAction;
    int currentMove;

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
            OnBattleOver(true);
        }
        else
        {
            PlayerAction();
            dialogueBox.EnableDialogueText(false);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical == 2f)
        {
            yield return dialogueBox.TypeDialogue("A Critical hit!!");
        }
        if(damageDetails.TypeEffect > 1f)
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
}
