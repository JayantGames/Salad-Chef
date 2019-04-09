using System.Collections;         
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjectForChef", order = 1)]
public class ScriptableObjectForChef : ScriptableObject
{
   //State machines
   public enum PLAYER
    {
        PLAYER_1,
        PLAYER_2
    }

    public enum PLAYER_STATE
    {
          CHOPPING,
          MOVING
    }

    public enum PICKUP
    {
        NONE,
        SPEED,
        TIME,
        SCORE
    }
    
    //Public States
    public PLAYER thisPlayer;
    public PLAYER_STATE thisPlayerState;
    public PICKUP currentPickup;

    //Player property variables  
    public GameObject pickedSalad;
   // public bool chopping;
    public float movementSpeed;
    public int playerTime;
    public int playerScore;
}
