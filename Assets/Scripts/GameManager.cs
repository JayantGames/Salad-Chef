using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player1;
    public GameObject player2;
    public DIFFICULTY_LEVEL currentDifficultyLevel;
    public GAME_STATE currentGameState;

    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject pausedPanel;
    public GameObject gameOverPanel;

    public GameObject menuPanel;
    public GameObject difficultyPanel;
    public Button easyButton;
    public Button hardButton;

    public enum DIFFICULTY_LEVEL
    {
        EASY,
        HARD
    }

    public enum GAME_STATE
    {
        MAIN_MENU,
        DIFFICULTY,
        GAMEPLAY,
        PAUSED,
        GAMEOVER
    }
            
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player1 = GameObject.Find("Chef1");
        player2 = GameObject.Find("Chef2");
        easyButton = difficultyPanel.transform.Find("Easy").GetComponent<Button>();
        hardButton = difficultyPanel.transform.Find("Hard").GetComponent<Button>();

        enableMainMenuPanel();
    }

    private void Update()
    {
        if (currentDifficultyLevel == DIFFICULTY_LEVEL.EASY)
        {
            easyButton.interactable = false;
            hardButton.interactable = true;
        }
        else
        {
            easyButton.interactable = true;
            hardButton.interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentGameState)
            {
                case GAME_STATE.MAIN_MENU:
                    {
                        break;
                    }
                case GAME_STATE.DIFFICULTY:
                    {
                        enableMainMenuPanel();
                        break;
                    }
                case GAME_STATE.GAMEPLAY:
                    {
                        enablePausedPanel();
                        break;
                    }
                case GAME_STATE.PAUSED:
                    {
                        disablePausedPanel();
                        break;
                    }
                case GAME_STATE.GAMEOVER:
                    {
                        break;
                    }
            }
        }
    }

    public void setDifficultyToEasy()
    {
        currentDifficultyLevel = DIFFICULTY_LEVEL.EASY;
    }

    public void setDifficultyToHard()
    {
        currentDifficultyLevel = DIFFICULTY_LEVEL.HARD;
    }

    public void enableMainMenuPanel()
    {
        currentGameState = GAME_STATE.MAIN_MENU;
        mainMenuPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        pausedPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        menuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    public void enableDifficultyPanel()
    {
        currentGameState = GAME_STATE.DIFFICULTY;

        menuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void enableGameplayPanel()
    {
        currentGameState = GAME_STATE.GAMEPLAY;

        mainMenuPanel.SetActive(false);
        gameplayPanel.SetActive(true);   

        menuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
    }

    public void enablePausedPanel()
    {
        currentGameState = GAME_STATE.PAUSED;
                                        
        pausedPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void disablePausedPanel()
    {
        currentGameState = GAME_STATE.GAMEPLAY;

        pausedPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void enableGameOverPanel()
    {
        currentGameState = GAME_STATE.GAMEOVER;

        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void exitTheGame()
    {
        Application.Quit();
    }
}             