using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementTop : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player movement
    public int buildingIndex = 7; // Set the default building index to 7 or change it as needed

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isInTriggerArea = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Check if player is in the trigger area and presses 'E'
        if (isInTriggerArea)
        {
            Debug.Log("Player is in trigger area");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed");
                ChangeBuildingIndex(buildingIndex);
            }
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void ChangeBuildingIndex(int buildingIndex)
    {
        // Code to change the building index
        Debug.Log("Changing to building index: " + buildingIndex);
        SceneManager.LoadScene(buildingIndex);
    }

    // Methods to detect if the player is in the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BuildingTrigger"))
        {
            Debug.Log("Entered BuildingTrigger area");
            isInTriggerArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BuildingTrigger"))
        {
            Debug.Log("Exited BuildingTrigger area");
            isInTriggerArea = false;
        }
    }
}