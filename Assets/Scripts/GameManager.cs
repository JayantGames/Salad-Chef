using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Instance of this class created for the implementation of singleton design pattern.
    public static GameManager Instance;
       
    // Public references for elements used in the overall functionality of the game.
    public GameObject player1;
    public GameObject player2;
    public DIFFICULTY_LEVEL currentDifficultyLevel;
    public GAME_STATE currentGameState;
    public List<GameObject> chefTableObjectsList;

    // UI panel references.
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject pausedPanel;
    public GameObject gameOverPanel;

    // UI sub-panel references.
    public GameObject menuPanel;
    public GameObject difficultyPanel;
    public Button easyButton;
    public Button hardButton;

    // List of customers currently active in the game.
    public List<CustomerInteraction> Customers;

    // Local UI refrences to display player's properties and progress.
    public Text Player1Score;
    public Text Player1Time;
    public Text Player2Score;
    public Text Player2Time;
    int player1Score;
    int player1Time;
    int player2Score;
    int player2Time;
    public Text winnerText;
    public Text highScoreText;

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
        // Constantly updating player's score and time.
        player1Score = player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore;
        player1Time = (int)player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime;
        player2Score = player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore;
        player2Time = (int)player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime;

        Player1Score.text = "Score : " + player1Score.ToString();
        Player1Time.text = "Time : " + player1Time.ToString();
        Player2Score.text = "Score : " + player2Score.ToString();
        Player2Time.text = "Time : " + player2Time.ToString();

        // Detecting which type of difficulty has been selected by the user.
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

        // Checking for the gameover and gameplay conditions.
        if (currentGameState == GAME_STATE.GAMEPLAY)
        {
            if (player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime > 0)
            {
                player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime -= Time.deltaTime;
            }

            if (player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime > 0)
            {
                player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime -= Time.deltaTime;
            }

            if (player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime <= 0 && player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime <= 0)
            {
                currentGameState = GAME_STATE.GAMEOVER;
                checkForWinner();
                enableGameOverPanel();
            }
        }

        // Checking for the current screen player is on.
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

    // Method to create a random order for the customer.
    public void createRandomCustomerOrder()
    {                                                            
        if (currentGameState == GAME_STATE.GAMEPLAY)
        {
            int index = Random.Range(0, Customers.Count - 1); 
            if (Customers[index].customerState != CustomerInteraction.CUSTOMER_STATE.WAITING)
            {
                Customers[index].requestRandomSalad();
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
        CancelInvoke();
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
        startTheGame();

        mainMenuPanel.SetActive(false);
        pausedPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameplayPanel.SetActive(true);

        menuPanel.SetActive(false);
        difficultyPanel.SetActive(false);

        player1Score = 0;
        player1Time = 120;
        player2Score = 0;
        player2Time = 120;

        InvokeRepeating("createRandomCustomerOrder", 1f, 15f);
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
        CancelInvoke();
    }

    public void startTheGame()
    {
        // Resetting Chef 1/Player 1 data   
        
        player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore = 0;
        player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime = 120f;
        player1.GetComponent<ChefDetails>().scriptableObjectForChef.movementSpeed = 70;
        player1.GetComponent<ChefDetails>().pickedVegetables.Clear();
        player1.GetComponent<ChefDetails>().pickedTwoVegetableSalad = null;           
        player1.GetComponent<ChefDetails>().pickedThreeVegetableSalad = null;

        player1.GetComponent<ChefMovement>().currentlyChoppingIngredient = null;
        player1.GetComponent<ChefMovement>().listOfChoppedIngredients.Clear();

        // Resetting Chef 2/Player 2 data

        player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore = 0;
        player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerTime = 120f;
        player2.GetComponent<ChefDetails>().scriptableObjectForChef.movementSpeed = 70;
        player2.GetComponent<ChefDetails>().pickedVegetables.Clear();
        player2.GetComponent<ChefDetails>().pickedTwoVegetableSalad = null;
        player2.GetComponent<ChefDetails>().pickedThreeVegetableSalad = null;

        player2.GetComponent<ChefMovement>().currentlyChoppingIngredient = null;
        player2.GetComponent<ChefMovement>().listOfChoppedIngredients.Clear();

        // Resetting overall game data

        for (int i = 0; i < chefTableObjectsList.Count; i++)
        {
            chefTableObjectsList[i].GetComponent<ObjectType>().extraVegetable = null;
        }

        for (int i = 0; i < Customers.Count; i++)
        {
            Customers[i].GetComponent<CustomerInteraction>().maximumWaitingTime = 0;
            Customers[i].GetComponent<CustomerInteraction>().currentWaitingTime = 0;
            Customers[i].GetComponent<CustomerInteraction>().remainingWaitingTime = 0;
            Customers[i].GetComponent<CustomerInteraction>().customerState = CustomerInteraction.CUSTOMER_STATE.NOT_RECIEVED;
            Customers[i].GetComponent<CustomerInteraction>().orderedTwoVegetableSalad = null;
            Customers[i].GetComponent<CustomerInteraction>().orderedThreeVegetableSalad = null;
            Customers[i].GetComponent<CustomerInteraction>().recievedTwoVegetableSalad = null;
            Customers[i].GetComponent<CustomerInteraction>().recievedThreeVegetableSalad = null;
            Customers[i].GetComponent<CustomerInteraction>().twoVegetableSalad = false;
            Customers[i].GetComponent<CustomerInteraction>().threeVegetableSalad = false;
        }   
    }

    // Method to check for the winner when the game is over.
    public void checkForWinner()
    {
        if (player1Score > player2Score)
        {
            winnerText.text = "Winner : Player1";
            highScoreText.text = "HighScore : " + player1Score;
        }
        else
        {
            winnerText.text = "Winner : Player2";
            highScoreText.text = "HighScore : " + player2Score;
        }
    }

    public void exitTheGame()
    {
        Application.Quit();
    }
}