using System.Collections;
using UnityEngine;

public enum State { START, BUSY, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleState : MonoBehaviour
{
    public State battleState;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPoint;
    public Transform enemyPoint;

    private BattleAction playerAction;
    private BattleAction enemyAction;
    private Stat player;
    private Stat enemy;

    private int healCount = 0;
    private const int maxHeals = 3;
    private const int ultimateEnergyCost = 100;

    private void Start()
    {
        battleState = State.START;
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerPoint.position, Quaternion.identity);
        player = playerGO.GetComponent<Stat>();
        playerAction = playerGO.GetComponent<BattleAction>();

        if (playerAction == null)
        {
            Debug.LogError("PlayerAction component not found on player prefab.");
        }

        GameObject enemyGO = Instantiate(enemyPrefab, enemyPoint.position, Quaternion.identity);
        enemy = enemyGO.GetComponent<Stat>();
        enemyAction = enemyGO.GetComponent<BattleAction>();

        if (enemyAction == null)
        {
            Debug.LogError("EnemyAction component not found on enemy prefab.");
        }

        yield return new WaitForSeconds(1f);
        battleState = State.PLAYERTURN;
        PlayerTurn();
    }

    private void Update()
    {
        if (battleState == State.PLAYERTURN && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlayerAttack());
        }
        else if (battleState == State.PLAYERTURN && Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(PlayerGuard());
        }
        else if (battleState == State.PLAYERTURN && Input.GetKeyDown(KeyCode.H))
        {
            if (healCount < maxHeals)
            {
                StartCoroutine(PlayerHeal());
                Debug.Log("Heal count " + healCount + "/" + maxHeals);
            }
            else
            {
                Debug.Log("Heal limit reached");
            }
        }
        else if (battleState == State.PLAYERTURN && Input.GetKeyDown(KeyCode.U))
        {
            if (player.currentEnergy >= ultimateEnergyCost)
            {
                StartCoroutine(PlayerUltimate());
            }
            else
            {
                Debug.Log("Not enough energy for Ultimate attack");
            }
        }
    }

    IEnumerator PlayerAttack()
    {
        battleState = State.BUSY;
        bool isDead = enemy.TakeDamage(player.atk);
        player.GainEnergy(player.energyGain);

        if (playerAction != null && enemyAction != null)
        {
            yield return new WaitForSeconds(1f);
            playerAction.Attack(enemyAction, () =>
            {
                Debug.Log("Player Attack");
                if (isDead)
                {
                    battleState = State.WON;
                    EndBattle();
                }
                else
                {
                    battleState = State.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            });
        }
        else
        {
            Debug.LogError("playerAction or enemyAction is not set.");
        }
    }

    IEnumerator PlayerGuard()
    {
        battleState = State.BUSY;
        playerAction.Guard();
        yield return new WaitForSeconds(1f);
        battleState = State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerHeal()
    {
        battleState = State.BUSY;
        healCount++;
        int healAmount = Mathf.FloorToInt(player.maxHp * Random.Range(0.2f, 0.3f));
        player.Heal(healAmount);
        Debug.Log($"Player healed for {healAmount} HP. Heal count: {healCount}/{maxHeals}");

        yield return new WaitForSeconds(1f); // Optional delay for healing animation/effects
        battleState = State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerUltimate()
    {
        battleState = State.BUSY;
        player.currentEnergy -= ultimateEnergyCost;
        int ultimateDamage = player.atk * 3; // Example ultimate damage calculation
        bool isDead = enemy.TakeDamage(ultimateDamage);
        Debug.Log($"Player used Ultimate for {ultimateDamage} damage. Remaining Energy: {player.currentEnergy}/{player.maxEnergy}");

        if (playerAction != null && enemyAction != null)
        {
            yield return new WaitForSeconds(1f);
            playerAction.Attack(enemyAction, () =>
            {
                Debug.Log("Player Ultimate Attack");
                if (isDead)
                {
                    battleState = State.WON;
                    EndBattle();
                }
                else
                {
                    battleState = State.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            });
        }
        else
        {
            Debug.LogError("playerAction or enemyAction is not set.");
        }
    }

    IEnumerator EnemyTurn()
    {
        battleState = State.BUSY;
        Debug.Log("Enemy Turn");

        yield return new WaitForSeconds(1f);
        enemyAction.Attack(playerAction, () =>
        {
            Debug.Log("Enemy Attack");
            bool isDead = playerAction.TakeDamage(enemy.atk);
            player.GainEnergy(player.energyGain);
            Debug.Log("Player HP After Attack");
            Debug.Log(player.currentHp);
            if (isDead)
            {
                battleState = State.LOST;
                EndBattle();
            }
            else
            {
                battleState = State.PLAYERTURN;
            }
        });
    }

    public void EndBattle()
    {
        if (battleState == State.WON)
        {
            Debug.Log("WON");
            // Close Building Index
        }
        else if (battleState == State.LOST)
        {
            Debug.Log("LOST");
            // Close Building Index
        }
    }

    private void PlayerTurn()
    {
        if (battleState == State.PLAYERTURN)
        {
            Debug.Log("Player Turn");
            // Enable player input or display player turn UI here if needed
        }
    }
}