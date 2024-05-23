using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    private Rigidbody2D rb;
    // private Animator anim; // Commenting out Animator reference
    private bool facingRight = true;
    private float moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>(); // Commenting out Animator initialization
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        ProcessInput();

        // Animate
        Animation();

        
    }

    private void FixedUpdate()
    {
        // Move
        Movement();
    }

    private void ProcessInput()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
    }

    private void Movement()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private void Animation()
    {
        if (moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }

        // Commenting out animation setting code
        // if (anim != null)
        // {
        //     anim.SetBool("run", moveDirection != 0);
        // }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public bool canAttack()
    {
        return moveDirection == 0;
    }
}