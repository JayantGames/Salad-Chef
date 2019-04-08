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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<ChefDetails>().pickIndicator.SetActive(true);

        switch (collision.gameObject.GetComponent<ObjectType>().objectType)
        {
            case ObjectType.TYPE.VEGETABLE:
                {
                    switch (collision.gameObject.tag)
                    {
                        case "Carrot":
                            {
                                Debug.Log("Collided with Carrot");
                                addVegetableToArray("Carrot");
                                break;
                            }
                        case "Chilli":
                            {
                                Debug.Log("Collided with Chilli");
                                addVegetableToArray("Chilli");
                                break;
                            }
                        case "Potato":
                            {
                                Debug.Log("Collided with Potato");
                                addVegetableToArray("Potato");
                                break;
                            }
                        case "Onion":
                            {
                                Debug.Log("Collided with Onion");
                                addVegetableToArray("Onion");
                                break;
                            }
                        case "Mushroom":
                            {
                                Debug.Log("Collided with Mushroom");
                                addVegetableToArray("Mushroom");
                                break;
                            }
                        case "Tomato":
                            {
                                Debug.Log("Collided with Tomato");
                                addVegetableToArray("Tomato");
                                break;
                            }
                    }
                    break;
                }
            case ObjectType.TYPE.CHOPPING_BOARD:
                {
                    Debug.Log("Collided with a chopping board");
                    break;
                }
            case ObjectType.TYPE.CUSTOMER_PLATE:
                {
                    Debug.Log("Collided with a customer plate");
                    break;
                }
            case ObjectType.TYPE.EXTRA_PLATE:
                {
                    Debug.Log("Collided with a extra plate");
                    break;
                }
            case ObjectType.TYPE.TRASHCAN:
                {
                    Debug.Log("Collided with Trashcan");
                    break;
                }
        }
    }

    public void addVegetableToArray(string vegetable)
    {                                              
        if (GetComponent<ChefDetails>().pickedVegetables.Count < 2)
        {
            if (!GetComponent<ChefDetails>().pickedVegetables.Contains(vegetable))
            {
                GetComponent<ChefDetails>().pickedVegetables.Add(vegetable);
            }
        }                                                                   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<ChefDetails>().pickIndicator.SetActive(false);
    }   
}
