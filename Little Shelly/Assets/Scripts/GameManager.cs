using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;

    // levels to move to on victory and lose
    public string levelMainMenu;
    public string levelAfterVictory;
    public string levelAfterGameOver;

    public static GameManager gm;
    public static bool gameIsPaused = false;

    private PlayerControls playerControls;

    private bool _waiting;


    private void Awake()
    {

        playerControls = new PlayerControls();

        // setup reference to game manager
        if (gm == null)
        {
            gm = GetComponent<GameManager>();
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup Player controls
        playerControls.PlatformsInput.PauseGame.performed += _ => Pause();
    }


    // Pause the game
    private void Pause()
    {
        if (gameIsPaused)
        {
            gameIsPaused = false;
            Resume();
        }
        else
        {
            gameIsPaused = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Resume the Game after Pause
    private void Resume()
    {
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    // Stop game and go back to the Main Menu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene(levelMainMenu);
    }


    // Menu displayed at Game Over
    public void GameOverMenu()
    {
        gameIsPaused = true;
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }


    public void ResetGame()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene(levelAfterGameOver);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
    }

    public void StopTime(float duration)
    {
        if (_waiting) return;
        Time.timeScale = 0f;
        StartCoroutine(StopTimeDuration(duration));
        
    }

    IEnumerator StopTimeDuration(float duration)
    {
        _waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        _waiting = false;
    }




}
