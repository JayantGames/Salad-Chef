using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public enum CUSTOMER_STATE
    {
        WAITING,
        RECIEVED,
        ANGRY
    }

    private void Start()
    {
        player1 = GameObject.Find("Chef1");
        player2 = GameObject.Find("Chef2");         
    }   
    
}
