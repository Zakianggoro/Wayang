using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { START, BUSY, ENEMYTURN, WON, LOST }

public class EnemyBattleState : MonoBehaviour
{
    public EnemyState enemyState;

    public GameObject enemyPrefab;
    public Transform enemyPoint;
    public GameObject spawnParent;
    public HealthBar enemyHealthBar;

    public Stat enemy { get; private set; }
    public BattleAction enemyAction { get; private set; }
    public PlayerBattleState playerBattleState;

    private void Start()
    {
        enemyState = EnemyState.START;
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyPoint.position, Quaternion.identity, spawnParent.transform);
        enemy = enemyGO.GetComponent<Stat>();
        enemyAction = enemyGO.GetComponent<BattleAction>();
        enemyHealthBar.Initialize(enemy);

        if (enemyAction == null)
        {
            Debug.LogError("EnemyAction component not found on enemy prefab.");
        }

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator EnemyTurn()
    {
        enemyState = EnemyState.BUSY;
        Debug.Log("Enemy Turn");

        yield return new WaitForSeconds(1f);

        // Check if the enemy has enough energy to use the ultimate attack
        if (enemy.currentEnergy >= PlayerBattleState.ultimateEnergyCost)
        {
            yield return StartCoroutine(EnemyUltimate());
        }
        else
        {
            // Define actions based on probabilities
            string[] actions = new string[] { "BasicAttack", "BasicAttack", "BasicAttack", "BasicAttack", "BasicAttack", "BasicAttack",
                                          "Guard", "Guard", "Guard",
                                          "Heal" };

            // Choose a random action
            string chosenAction = actions[Random.Range(0, actions.Length)];

            switch (chosenAction)
            {
                case "BasicAttack":
                    yield return StartCoroutine(EnemyBasicAttack());
                    break;
                case "Guard":
                    yield return StartCoroutine(EnemyGuard());
                    break;
                case "Heal":
                    yield return StartCoroutine(EnemyHeal());
                    break;
            }
        }
    }

    public IEnumerator EnemyBasicAttack()
    {
        Debug.Log("Enemy Basic Attack");
        enemyAction.Attack(playerBattleState.playerAction, () =>
        {
            bool isDead = playerBattleState.player.TakeDamage(enemy.atk);
            enemy.GainEnergy(enemy.energyGain); // Enemy gains energy
            playerBattleState.player.GainEnergy(playerBattleState.player.energyGain); // Player also gains energy if applicable
            playerBattleState.playerHealthBar.UpdateHealth();
            playerBattleState.playerHealthBar.UpdateEnergy();
            enemyHealthBar.UpdateEnergy(); // Update enemy energy bar
            Debug.Log("Player HP After Attack");
            Debug.Log(playerBattleState.player.currentHp);
            if (isDead)
            {
                enemyState = EnemyState.WON;
                EndBattle();
            }
            else
            {
                playerBattleState.playerState = PlayerState.PLAYERTURN;
                playerBattleState.PlayerTurn();
            }
        });
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator EnemyGuard()
    {
        Debug.Log("Enemy Guard");
        enemyAction.Guard();
        yield return new WaitForSeconds(1f);
        playerBattleState.playerState = PlayerState.PLAYERTURN;
        playerBattleState.PlayerTurn();
    }

    public IEnumerator EnemyHeal()
    {
        Debug.Log("Enemy Heal");
        int healAmount = Mathf.FloorToInt(enemy.maxHp * Random.Range(0.2f, 0.25f));
        enemy.Heal(healAmount);
        enemyHealthBar.UpdateHealth();
        enemyHealthBar.UpdateEnergy();
        Debug.Log($"Enemy healed for {healAmount} HP.");

        yield return new WaitForSeconds(1f);
        playerBattleState.playerState = PlayerState.PLAYERTURN;
        playerBattleState.PlayerTurn();
    }

    public IEnumerator EnemyUltimate()
    {
        Debug.Log("Enemy Ultimate");
        enemy.currentEnergy -= PlayerBattleState.ultimateEnergyCost;
        int ultimateDamage = enemy.atk * 3; // Example ultimate damage calculation
        bool isDead = playerBattleState.player.TakeDamage(ultimateDamage);
        playerBattleState.playerHealthBar.UpdateHealth();
        enemyHealthBar.UpdateEnergy();
        Debug.Log($"Enemy used Ultimate for {ultimateDamage} damage. Remaining Energy: {enemy.currentEnergy}/{enemy.maxEnergy}");

        if (isDead)
        {
            enemyState = EnemyState.WON;
            EndBattle();
        }
        else
        {
            playerBattleState.playerState = PlayerState.PLAYERTURN;
            playerBattleState.PlayerTurn();
        }

        yield return new WaitForSeconds(1f);
    }

    public void EndBattle()
    {
        if (enemyState == EnemyState.WON)
        {
            Debug.Log("WON");
            // End the battle, perhaps load a victory screen or transition
        }
        else if (enemyState == EnemyState.LOST)
        {
            Debug.Log("LOST");
            // End the battle, perhaps load a defeat screen or transition
        }
    }
}
