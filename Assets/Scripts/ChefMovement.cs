using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefMovement : MonoBehaviour
{
    public float speed = 100;
    public Rigidbody2D rb
;
    float horizonatalAxis;
    float verticalAxis;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        if (GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayer == ScriptableObjectForChef.PLAYER.PLAYER_1)
        {
            horizonatalAxis = Input.GetAxis("HorizontalP1");
            verticalAxis = Input.GetAxis("VerticalP1");
        }
        else
        {
            horizonatalAxis = Input.GetAxis("HorizontalP2");
            verticalAxis = Input.GetAxis("VerticalP2");
        }
           
    }

    private void FixedUpdate()
    {
        Vector3 tempVect = new Vector3(horizonatalAxis, verticalAxis, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);
    }
}
