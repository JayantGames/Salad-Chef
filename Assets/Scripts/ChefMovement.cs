using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefMovement : MonoBehaviour
{                               
    // Rigidbody reference for the chef.
    public Rigidbody2D rb;

    // Axis reference for movement.
    float horizonatalAxis;
    float verticalAxis;

    // Time that the chef will take in order to chop a vegetable;
    public float choppingTime = 3f;

    // As the name suggests this string refers to the ingredient/vegetable which is currently being chopped.
    public string currentlyChoppingIngredient;

    // A list of chopped ingredients
    public List<string> listOfChoppedIngredients;

    // Public references for each ingredient so that they can be accessed individually as well as collectively.
    public string ingredient1Id;
    public string ingredient2Id;
    public string ingredient3Id;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        // Checking for the current player state i.e. if the player is currently moving or chopping.
        if (GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayerState == ScriptableObjectForChef.PLAYER_STATE.MOVING)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            // Given specific controls to each player depending on their player selection.
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
            GetComponent<ChefDetails>().chefWaitingBar.gameObject.SetActive(true);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;

            if (choppingTime > 0)
            {
                choppingTime -= Time.deltaTime;
                float percent = choppingTime / 3;
                GetComponent<ChefDetails>().chefWaitingBar.fillAmount = Mathf.Lerp(0, 1, percent);
            }
            else
            {
                choppingTime = 3f;
                GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayerState = ScriptableObjectForChef.PLAYER_STATE.MOVING;
            }
        }
        else
        {
            GetComponent<ChefDetails>().chefWaitingBar.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        // Taking care of smooth player movement.
        Vector3 tempVect = new Vector3(horizonatalAxis, verticalAxis, 0);
        tempVect = tempVect.normalized * GetComponent<ChefDetails>().scriptableObjectForChef.movementSpeed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);
    }

    // Method for initializing the chopping process.
    public void startChopping()
    {
        if (GetComponent<ChefDetails>().pickedVegetables.Count > 0)
        {
            GetComponent<ChefDetails>().chefWaitingBar.fillAmount = 1;
            GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayerState = ScriptableObjectForChef.PLAYER_STATE.CHOPPING;
            listOfChoppedIngredients.Add(GetComponent<ChefDetails>().pickedVegetables[0]);
            GetComponent<ChefDetails>().checkCurrentlyChoppingIngredient(GetComponent<ChefDetails>().pickedVegetables[0]);
            GetComponent<ChefDetails>().pickedVegetables.Remove(GetComponent<ChefDetails>().pickedVegetables[0]);
            checkIngredientCount();
        }
    }

    // Method to check for the count of ingredients which the chef has chopped.
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

    // Method for creating specific id for the particular salad.
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
                ingredient3Id = GetComponent<ChefDetails>().pickedThreeVegetableSalad.ingredient3.Substring(0, 1);
            }

            GetComponent<ChefDetails>().pickedThreeVegetableSalad.id = ingredient1Id + "-" + ingredient2Id + "-" + ingredient3Id;
        }
    }

    // Method to add vegetable to the chef's picked up vegetable's list.
    public void addVegetableToList(string vegetable)
    {
        if (GetComponent<ChefDetails>().pickedVegetables.Count < 2)
        {
            if (!GetComponent<ChefDetails>().pickedVegetables.Contains(vegetable))
            {
                GetComponent<ChefDetails>().pickedVegetables.Add(vegetable);
                GetComponent<ChefDetails>().assignVegetablesToChef();
            }
        }
        else
        {
            Debug.Log("Only 2 vegetables are allowed");
        }
    }

    // Method for checking different type of collisions.
    private void OnTriggerStay2D(Collider2D other)
    {
        GetComponent<ChefDetails>().pickIndicator.SetActive(true);

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
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (other.gameObject.GetComponentInParent<CustomerInteraction>().twoVegetableSalad & !other.gameObject.GetComponentInParent<CustomerInteraction>().threeVegetableSalad)
                        {
                            other.gameObject.GetComponentInParent<CustomerInteraction>().recievedTwoVegetableSalad = GetComponent<ChefDetails>().pickedTwoVegetableSalad;
                        }
                        else
                        {
                            other.gameObject.GetComponentInParent<CustomerInteraction>().recievedThreeVegetableSalad = GetComponent<ChefDetails>().pickedThreeVegetableSalad;
                        }
                        other.gameObject.GetComponentInParent<CustomerInteraction>().orderRecieved(GetComponent<ChefDetails>());
                    }
                    break;
                }
            case ObjectType.TYPE.CHOPPING_BOARD:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        startChopping();
                        OnTriggerExit2D(other);
                    }
                    break;
                }
            case ObjectType.TYPE.TRASHCAN:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GetComponent<ChefDetails>().pickedVegetables.Clear();
                    }
                    break;
                }
            case ObjectType.TYPE.EXTRA_PLATE:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (string.IsNullOrEmpty(other.GetComponent<ObjectType>().extraVegetable))
                        {
                            other.GetComponent<ObjectType>().extraVegetable = GetComponent<ChefDetails>().pickedVegetables[0];
                            GetComponent<ChefDetails>().checkCurrentlyChoppingIngredient(GetComponent<ChefDetails>().pickedVegetables[0]);
                            GetComponent<ChefDetails>().pickedVegetables.Remove(GetComponent<ChefDetails>().pickedVegetables[0]);
                            checkIngredientCount();
                        }
                        else
                        {
                            addVegetableToList(other.GetComponent<ObjectType>().extraVegetable);
                            other.GetComponent<ObjectType>().extraVegetable = null;
                        }
                    }
                    break;
                }
            case ObjectType.TYPE.PICKUP:
                {
                    checkForPickup(other);
                    break;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<ChefDetails>().pickIndicator.SetActive(false);
    }

    // Method to check which type of pickup has been picked up by the chef.
    public void checkForPickup(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ObjectType>().objectType == ObjectType.TYPE.PICKUP)
        {
            if (collision.gameObject.GetComponent<ObjectType>().chef.GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayer == GetComponent<ChefDetails>().scriptableObjectForChef.thisPlayer)
            {
                switch (collision.gameObject.GetComponent<ObjectType>().objectPickupType)
                {
                    case ObjectType.PICKUP_TYPE.SPEED:
                        {
                            StartCoroutine(changePlayerSpeed());
                            break;
                        }
                    case ObjectType.PICKUP_TYPE.TIME:
                        {
                            GetComponent<ChefDetails>().scriptableObjectForChef.playerTime += 10;
                            Destroy(collision.gameObject);
                            break;
                        }
                    case ObjectType.PICKUP_TYPE.SCORE:
                        {
                            GetComponent<ChefDetails>().scriptableObjectForChef.playerScore += 10;
                            break;
                        }
                }
                Destroy(collision.gameObject);
            }
        }
    } 

    IEnumerator changePlayerSpeed()
    {
        GetComponent<ChefDetails>().scriptableObjectForChef.movementSpeed += 20;
        yield return new WaitForSeconds(10f);
        GetComponent<ChefDetails>().scriptableObjectForChef.movementSpeed -= 20;
    }
}
