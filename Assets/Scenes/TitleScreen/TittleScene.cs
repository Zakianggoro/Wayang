using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detects left mouse button click
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // Assuming the next scene is indexed at 1 in the Build Settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}