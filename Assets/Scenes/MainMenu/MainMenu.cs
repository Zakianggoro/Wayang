using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Method to load the play scene
    public void PlayGame()
    {
        SceneManager.LoadScene(3); // Change 5 to the appropriate scene index for your Play scene
    }

    // Method to load the settings scene
    public void Settings()
    {
        SceneManager.LoadScene(2); // Change 6 to the appropriate scene index for your Settings scene
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("QuitGame called");
        Application.Quit();
    }
}