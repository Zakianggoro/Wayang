using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState { START, BUSY, PLAYERTURN, WON, LOST }

public class PlayerBattleState : MonoBehaviour
{
    public PlayerState playerState;

    public GameObject playerPrefab;
    public Transform playerPoint;
    public GameObject spawnParent;
    public HealthBar playerHealthBar;

    public Stat player { get; private set; } 
    public BattleAction playerAction{ get; private set; }
    public int healCount = 0;
    public const int maxHeals = 3;
    public const int ultimateEnergyCost = 100;

    public EnemyBattleState enemyBattleState;

    public int HealCount => healCount;
    public int MaxHeals => maxHeals;
    public int UltimateEnergyCost => ultimateEnergyCost;

    private void Start()
    {
        playerState = PlayerState.START;
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerPoint.position, Quaternion.identity, spawnParent.transform);
        player = playerGO.GetComponent<Stat>();
        playerAction = playerGO.GetComponent<BattleAction>();
        playerHealthBar.Initialize(player);

        if (playerAction == null)
        {
            Debug.LogError("PlayerAction component not found on player prefab.");
        }

        yield return new WaitForSeconds(1f);
        playerState = PlayerState.PLAYERTURN;
        PlayerTurn();
    }

    private void Update()
    {
        if (playerState == PlayerState.PLAYERTURN && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(PlayerAttack());
        }
        else if (playerState == PlayerState.PLAYERTURN && Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(PlayerGuard());
        }
        else if (playerState == PlayerState.PLAYERTURN && Input.GetKeyDown(KeyCode.H))
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
        else if (playerState == PlayerState.PLAYERTURN && Input.GetKeyDown(KeyCode.U))
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

    public IEnumerator PlayerAttack()
    {
        playerState = PlayerState.BUSY;
        if (playerAction != null)
        {
            playerAction.Attack(enemyBattleState.enemyAction, () =>
            {
                bool isDead = enemyBattleState.enemy.TakeDamage(player.atk);
                player.GainEnergy(player.energyGain);
                enemyBattleState.enemyHealthBar.UpdateHealth();
                playerHealthBar.UpdateEnergy();
                Debug.Log("Player Attack");
                if (isDead)
                {
                    playerState = PlayerState.WON;
                    EndBattle();
                }
                else
                {
                    playerState = PlayerState.BUSY;
                    StartCoroutine(enemyBattleState.EnemyTurn());
                }
            });
            yield return new WaitForSeconds(1f);
        }
        else
        {
            Debug.LogError("playerAction is not set.");
        }
    }

    public IEnumerator PlayerGuard()
    {
        playerState = PlayerState.BUSY;
        playerAction.Guard();
        yield return new WaitForSeconds(1f);
        StartCoroutine(enemyBattleState.EnemyTurn());
    }

    public IEnumerator PlayerHeal()
    {
        playerState = PlayerState.BUSY;
        healCount++;
        int healAmount = Mathf.FloorToInt(player.maxHp * Random.Range(0.2f, 0.3f));
        player.Heal(healAmount);
        playerHealthBar.UpdateHealth();
        playerHealthBar.UpdateEnergy();
        Debug.Log($"Player healed for {healAmount} HP. Heal count: {healCount}/{maxHeals}");

        yield return new WaitForSeconds(1f); // Optional delay for healing animation/effects
        StartCoroutine(enemyBattleState.EnemyTurn());
    }

    public IEnumerator PlayerUltimate()
    {
        playerState = PlayerState.BUSY;
        player.currentEnergy -= ultimateEnergyCost;
        int ultimateDamage = player.atk * 3; // Example ultimate damage calculation
        bool isDead = enemyBattleState.enemy.TakeDamage(ultimateDamage);
        enemyBattleState.enemyHealthBar.UpdateHealth();
        playerHealthBar.UpdateEnergy();
        Debug.Log($"Player used Ultimate for {ultimateDamage} damage. Remaining Energy: {player.currentEnergy}/{player.maxEnergy}");

        if (playerAction != null)
        {
            yield return new WaitForSeconds(1f);
            playerAction.Attack(enemyBattleState.enemyAction, () =>
            {
                Debug.Log("Player Ultimate Attack");
                if (isDead)
                {
                    playerState = PlayerState.WON;
                    EndBattle();
                }
                else
                {
                    StartCoroutine(enemyBattleState.EnemyTurn());
                }
            });
        }
        else
        {
            Debug.LogError("playerAction is not set.");
        }
    }

    public void EndBattle()
    {
        if (playerState == PlayerState.WON)
        {
            Debug.Log("WON");
            // End the battle, perhaps load a victory screen or transition
        }
        else if (playerState == PlayerState.LOST)
        {
            Debug.Log("LOST");
            // End the battle, perhaps load a defeat screen or transition
        }
    }

    public void PlayerTurn()
    {
        if (playerState == PlayerState.PLAYERTURN)
        {
            Debug.Log("Player Turn");
            // Enable player input or display player turn UI here if needed
        }
    }
}
