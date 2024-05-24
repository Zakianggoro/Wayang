using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] private Transform characterPrefab;
    [SerializeField] private Transform enemyPrefab;
    private BattleAction player;
    private BattleAction villain;
    private static BattleHandler instance;
    private State state;

    public static BattleHandler GetInstance()
    {
        return instance;
    }
    private enum State
    {
        WaitingForPlayer,
        Busy,
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        state = State.WaitingForPlayer; // Set the initial state
        player = spawn(true);
        villain = spawn(false);
    }

    private void Update()
    {
        if (state == State.WaitingForPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            state = State.Busy;
            player.Attack(villain, () =>
            {
                state = State.WaitingForPlayer;
            });
        }
    }

    private BattleAction spawn(bool isPlayer)
    {
        Transform prefab = isPlayer ? characterPrefab : enemyPrefab;
        Vector3 position = isPlayer ? new Vector3(-5, 0, 0) : new Vector3(5, 0, 0);
        Transform characterTransform = Instantiate(prefab, position, Quaternion.identity);
        BattleAction action = characterTransform.GetComponent<BattleAction>();
        return action;
    }
}
