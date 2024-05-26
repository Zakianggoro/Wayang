using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public PlayerBattleState playerBattleState;

    public Button attackButton;
    public Button guardButton;
    public Button ultimateButton;
    public Button healButton;

    private void Start()
    {
        // Ensure playerBattleState is assigned
        if (playerBattleState == null)
        {
            playerBattleState = FindObjectOfType<PlayerBattleState>();
        }

        // Assign the button click listeners
        attackButton.onClick.AddListener(OnAttackButtonClicked);
        guardButton.onClick.AddListener(OnGuardButtonClicked);
        ultimateButton.onClick.AddListener(OnUltimateButtonClicked);
        healButton.onClick.AddListener(OnHealButtonClicked);

        // Disable buttons initially
        SetButtonsInteractable(false);
    }

    private void Update()
    {
        if (playerBattleState == null)
        {
            Debug.LogError("PlayerBattleState is not assigned.");
            return;
        }

        // Enable buttons only during the player's turn
        SetButtonsInteractable(playerBattleState.playerState == PlayerState.PLAYERTURN);
    }

    public void SetButtonsInteractable(bool interactable)
    {
        attackButton.interactable = interactable;
        guardButton.interactable = interactable;
        ultimateButton.interactable = interactable;
        healButton.interactable = interactable;
    }

    public void OnAttackButtonClicked()
    {
        if (playerBattleState.playerState == PlayerState.PLAYERTURN)
        {
            StartCoroutine(playerBattleState.PlayerAttack());
        }
    }

    public void OnGuardButtonClicked()
    {
        if (playerBattleState.playerState == PlayerState.PLAYERTURN)
        {
            StartCoroutine(playerBattleState.PlayerGuard());
        }
    }

    public void OnUltimateButtonClicked()
    {
        if (playerBattleState.playerState == PlayerState.PLAYERTURN && playerBattleState.player.currentEnergy >= playerBattleState.UltimateEnergyCost)
        {
            StartCoroutine(playerBattleState.PlayerUltimate());
        }
        else
        {
            Debug.Log("Not enough energy for Ultimate attack");
        }
    }

    public void OnHealButtonClicked()
    {
        if (playerBattleState.playerState == PlayerState.PLAYERTURN && playerBattleState.HealCount < playerBattleState.MaxHeals)
        {
            StartCoroutine(playerBattleState.PlayerHeal());
        }
        else if (playerBattleState.HealCount >= playerBattleState.MaxHeals)
        {
            Debug.Log("Heal limit reached");
        }
    }
}
