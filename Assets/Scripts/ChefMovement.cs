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
    public float choppingTime = 3f;
    public string currentlyChoppingIngredient;
    public List<string> listOfChoppedIngredients;
    public string ingredient1Id;
    public string ingredient2Id;
    public string ingredient3Id;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        if (GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayerState == ScriptableObjectForChef.PLAYER_STATE.MOVING)
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

        if (GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayerState == ScriptableObjectForChef.PLAYER_STATE.CHOPPING)
        {
            if (choppingTime > 0)
            {
                choppingTime -= Time.deltaTime;
            }
            else
            {
                choppingTime = 3f;
                GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayerState = ScriptableObjectForChef.PLAYER_STATE.MOVING;
            }
            
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
    }
      
    public void startChopping()
    {
        if (GetComponent<ChefDetails>().pickedVegetables.Count > 0)
        {
            GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayerState = ScriptableObjectForChef.PLAYER_STATE.CHOPPING;
            listOfChoppedIngredients.Add(GetComponent<ChefDetails>().pickedVegetables[0]);                                     
            GetComponent<ChefDetails>().pickedVegetables.Remove(GetComponent<ChefDetails>().pickedVegetables[0]);
            checkIngredientCount();
        }
    }

    public void checkIngredientCount()
    {
        if (listOfChoppedIngredients.Count == 2)
        {
            GetComponent<ChefDetails>().pickedTwoVegetableSalad.ingredient1 = listOfChoppedIngredients[0];
            GetComponent<ChefDetails>().pickedTwoVegetableSalad.ingredient2 = listOfChoppedIngredients[1];
            GetComponent<ChefDetails>().pickedThreeVegetableSalad.ingredient1 = listOfChoppedIngredients[0];
            GetComponent<ChefDetails>().pickedThreeVegetableSalad.ingredient2 = listOfChoppedIngredients[1];
            setIngredientIds();
        }
        else if (listOfChoppedIngredients.Count == 3)
        {
            GetComponent<ChefDetails>().pickedThreeVegetableSalad.ingredient3 = listOfChoppedIngredients[2];
            setIngredientIds();
        }
    }

    public void setIngredientIds()
    {
        if (GetComponent<ChefDetails>().pickedTwoVegetableSalad.ingredient1 == "Chilli")
        {
            ingredient1Id = "Ch";
        }
        else
        {
            ingredient1Id = GetComponent<ChefDetails>().pickedTwoVegetableSalad.ingredient1.Substring(0, 1);
        }

        if (GetComponent<ChefDetails>().pickedTwoVegetableSalad.ingredient2 == "Chilli")
        {
            ingredient2Id = "Ch";
        }
        else
        {
            ingredient2Id = GetComponent<ChefDetails>().pickedTwoVegetableSalad.ingredient2.Substring(0, 1);
        }

        GetComponent<ChefDetails>().pickedTwoVegetableSalad.id = ingredient1Id + "-" + ingredient2Id;

        if (!string.IsNullOrEmpty(GetComponent<ChefDetails>().pickedThreeVegetableSalad.ingredient3))
        {
            if (GetComponent<ChefDetails>().pickedThreeVegetableSalad.ingredient3 == "Chilli")
            {
                ingredient3Id = "Ch";
            }
            else
            {
                ingredient3Id = GetComponent<ChefDetails>().pickedThreeVegetableSalad.ingredient3.Substring(0,1);
            }

            GetComponent<ChefDetails>().pickedThreeVegetableSalad.id = ingredient1Id + "-" + ingredient2Id + "-" + ingredient3Id;
        }
    }

    public void addVegetableToList(string vegetable)
    {
        if (GetComponent<ChefDetails>().pickedVegetables.Count < 2)
        {
            if (!GetComponent<ChefDetails>().pickedVegetables.Contains(vegetable))
            {
                GetComponent<ChefDetails>().pickedVegetables.Add(vegetable);
            }
        }
        else
        {
            Debug.Log("Only 2 vegetables are allowed");
        }
    }                                    
    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.GetComponent<ObjectType>().objectType)
        {
            case ObjectType.TYPE.VEGETABLE:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        addVegetableToList(other.name);
                    }
                    break;
                }
            case ObjectType.TYPE.CUSTOMER_PLATE:
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        other.gameObject.GetComponentInParent<CustomerInteraction>().currentWaitingTime = 0f;

                        if (other.gameObject.GetComponentInParent<CustomerInteraction>().twoVegetableSalad & !other.gameObject.GetComponentInParent<CustomerInteraction>().threeVegetableSalad)
                        {
                            other.gameObject.GetComponentInParent<CustomerInteraction>().recievedTwoVegetableSalad = GetComponent<ChefDetails>().pickedTwoVegetableSalad;
                        }
                        else
                        {
                            other.gameObject.GetComponentInParent<CustomerInteraction>().recievedThreeVegetableSalad = GetComponent<ChefDetails>().pickedThreeVegetableSalad;
                        }
                        other.gameObject.GetComponentInParent<CustomerInteraction>().orderRecieved();
                    }
                    break;
                }
            case ObjectType.TYPE.CHOPPING_BOARD:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        startChopping();
                    }
                    break;
                }
        }
    }            

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<ChefDetails>().pickIndicator.SetActive(false);
    }   
}
