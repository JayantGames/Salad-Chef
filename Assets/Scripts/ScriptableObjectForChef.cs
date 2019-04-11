using System.Collections;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjectForChef", order = 1)]
public class ScriptableObjectForChef : ScriptableObject
{
    // State machine to check for player.
    public enum PLAYER
    {
        PLAYER_1,
        PLAYER_2
    }

    // State machine to check for player state.
    public enum PLAYER_STATE
    {
        CHOPPING,
        MOVING
    }

    // Public States
    public PLAYER thisPlayer;
    public PLAYER_STATE thisPlayerState;

    // Player property variables  
    public GameObject pickedSalad;
    public int movementSpeed;
    public float playerTime;
    public int playerScore;
}
