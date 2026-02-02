using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startMenu;

    private bool gameStarted = false;
    public bool GameStarted => gameStarted;

    void Start()
    {
        // Freeze game at launch
        Time.timeScale = 0f;
        startMenu.SetActive(true);
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;
        startMenu.SetActive(false);
    }
}
