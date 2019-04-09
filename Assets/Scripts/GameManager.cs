using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player1;
    public GameObject player2;
    public DIFFICULTY_LEVEL currentDifficultyLevel;

    public enum DIFFICULTY_LEVEL
    {
        EASY,
        HARD
    }
            
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player1 = GameObject.Find("Chef1");
        player2 = GameObject.Find("Chef2");     
    }  
}
