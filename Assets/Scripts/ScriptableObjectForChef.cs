using System.Collections;         
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjectForChef", order = 1)]
public class ScriptableObjectForChef : ScriptableObject
{
   //State machines
   public enum PLAYER
    {
        PLAYER_1,
        PLAYER_2
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
    public PICKUP currentPickup;

    //Player property variables
    public GameObject[] pickedVegetables;
    public GameObject pickedSalad;
    public bool chopping;
    public float movementSpeed;
    public int playerTime;
    public int playerScore;
}
