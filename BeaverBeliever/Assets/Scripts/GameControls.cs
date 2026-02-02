using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isPaused;
    private bool tabPressed = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.isPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
        }

        if (Keyboard.current.tabKey.isPressed && tabPressed == false)
        {
            TogglePause();
        }

        tabPressed = Keyboard.current.tabKey.isPressed;

    }

    void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
    }
}
