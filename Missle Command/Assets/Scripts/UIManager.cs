using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("UI Elements")]
    public GameObject pauseMenu;
    public GameObject gameOverScreen;
    public GameObject startScreen;
    public GameObject levelEnd;
    public GameObject gameCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Update()
    {
        UI();
    }

    public void UI()
    {
        if (levelEnd.active)
            GameUISwitch(false);

        if (!levelEnd.active)
            GameUISwitch(true);

        if (gameOverScreen.active)
        {
            GameUISwitch(false);
            LevelEndSwitch(false);
        }

        if (startScreen.active)
        {
            GameUISwitch(false);
            RocketShooting.Instance.enabled = false;
            Time.timeScale = 0;
        }

        if (pauseMenu.active)
        {
            GameUISwitch(false);
            LevelEndSwitch(false);
            GameOverSwitch(false);
            StartSwitch(false);
            RocketShooting.Instance.enabled = false;
            Time.timeScale = 0;
        }
        else
        {
            RocketShooting.Instance.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseSwitch(pauseMenu.active = !pauseMenu.active);
    }

    public void PauseSwitch(bool value)
    {
        pauseMenu.gameObject.SetActive(value);
    }

    public void GameOverSwitch(bool value)
    {
        gameOverScreen.gameObject.SetActive(value);
    }

    public void StartSwitch(bool value)
    {
        startScreen.gameObject.SetActive(value);
    }

    public void LevelEndSwitch(bool value)
    {
        levelEnd.gameObject.SetActive(value);
    }

    public void GameUISwitch(bool value)
    {
        gameCanvas.gameObject.SetActive(value);
    }

    public void StartGame()
    {
        StartSwitch(false);
        GameUISwitch(true);
        RocketShooting.Instance.enabled = true;
        Time.timeScale = 1;
    }

    public void Resume()
    {
        PauseSwitch(false);
        RocketShooting.Instance.enabled = true;
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
