using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject startMenu;
    public SlideUpPanel pauseMenu;  

    [Header("State")]
    public bool GameStarted { get; private set; }
    public bool IsPaused { get; private set; }

    private bool tabHeld;

   public GameObject beaver;

    //public AudioSource audioSource;

    void Start()
    {
        // Start game paused at menu
        Time.timeScale = 0f;
        GameStarted = false;
        IsPaused = false;

        startMenu.SetActive(true);
        if (pauseMenu) pauseMenu.gameObject.SetActive(false);
        beaver.SetActive(false);
    }

    void Update()
    {  
        HandleInput();
    }

    void HandleInput()
    {   
        if (!GameStarted && startMenu.activeSelf)
            return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Restart
        if (keyboard.rKey.wasPressedThisFrame)
        {
            RestartScene();
        }

        // Quit
        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            if (!GameStarted)
            {
                QuitGame();
            }
            else
            {
                TogglePause();
            }
        }

        // Pause toggle (Tab)
        if (keyboard.tabKey.isPressed && !tabHeld && GameStarted)
        {
            TogglePause();
        }

        tabHeld = keyboard.tabKey.isPressed;
    }

    // ======================
    // GAME FLOW
    // ======================

    public void StartGame()
    {
        GameStarted = true;
        IsPaused = false;

        Time.timeScale = 1f;
        startMenu.SetActive(false);
        if (pauseMenu) pauseMenu.gameObject.SetActive(false);
        beaver.SetActive(true);

    }

    public void TogglePause()
    {
        if (!GameStarted) return;

        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;

        if (pauseMenu)
        {
            if (IsPaused)
                pauseMenu.Show();
            else
                pauseMenu.Hide();
        }
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (pauseMenu)
            pauseMenu.Hide();
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
