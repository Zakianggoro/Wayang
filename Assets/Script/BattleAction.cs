using System;
using System.Collections;
using UnityEngine;

public class BattleAction : MonoBehaviour
{
    private State state;
    private Vector3 slideToTarget;
    private Action onSlideComplete;
    private bool isGuarding;  // Status Guard
    private float guardReduction = 0.5f;  // Pengurangan kerusakan saat Guard

    private enum State
    {
        Idle,
        Busy,
        Sliding,
    }

    private void Awake()
    {
        state = State.Idle;
        isGuarding = false;  // Inisialisasi status Guard
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Sliding:
                float slideSpeed = 10f;
                transform.position += (slideToTarget - GetPosition()) * slideSpeed * Time.deltaTime;

                float targetReached = 0.5f;
                if (Vector3.Distance(GetPosition(), slideToTarget) < targetReached)
                {
                    transform.position = slideToTarget;
                    state = State.Idle;  // Set state back to Idle after sliding
                    onSlideComplete?.Invoke();  // Use null-conditional operator to prevent null reference
                }
                break;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Attack(BattleAction target, Action onAttackComplete)
    {
        Vector3 slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 1f;
        Vector3 start = GetPosition();

        Slide(slideTargetPosition, () =>
        {
            state = State.Busy;
            Vector3 dir = (target.GetPosition() - GetPosition()).normalized;
            Slide(start, () =>
            {
                state = State.Idle;
                onAttackComplete();
            });
        });
    }

    public void Slide(Vector3 slideToTarget, Action onSlideComplete)
    {
        this.slideToTarget = slideToTarget;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
    }

    public void Guard()
    {
        isGuarding = true;
        Debug.Log("Guarding! Damage will be reduced by 50%.");
    }

    public bool TakeDamage(int dmg)
    {
        if (isGuarding)
        {
            dmg = Mathf.FloorToInt(dmg * guardReduction);
            isGuarding = false;  // Reset guard status after taking damage
        }

        Stat stat = GetComponent<Stat>();
        return stat.TakeDamage(dmg);
    }
}
