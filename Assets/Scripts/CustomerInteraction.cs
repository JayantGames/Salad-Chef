using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomerInteraction : MonoBehaviour
{
    // State mahcine to check for the current state of the customer.
    public enum CUSTOMER_STATE
    {
        WAITING,
        CHECKING,
        RECIEVED,
        ANGRY,
        NOT_RECIEVED
    }

    // Public reference variables for each specific customer.
    public float maximumWaitingTime;
    public float currentWaitingTime;
    public float remainingWaitingTime;
    public CUSTOMER_STATE customerState;
    public GameObject saladPlate;
    public GameObject waitingBarGameobject;
    public Image waitingBar;

    // The two/three vegetable salad ordered by the customer.
    public TwoVegetableRecipe orderedTwoVegetableSalad;
    public ThreeVegetableRecipe orderedThreeVegetableSalad;

    // The two/three vegetable salad recieved by the customer.
    public TwoVegetableRecipe recievedTwoVegetableSalad;
    public ThreeVegetableRecipe recievedThreeVegetableSalad;

    public bool twoVegetableSalad;
    public bool threeVegetableSalad;
    public List<GameObject> pickupsList;

    // Request a random salad from the API data to order.
    public void requestRandomSalad()
    {
        if (SaladsAPI_Manager.Instance.saladsDataFetched)
        {
            if (GameManager.Instance.currentDifficultyLevel == GameManager.DIFFICULTY_LEVEL.EASY)
            {
                TwoVegetableRecipe randomTwoVegetableRecipe = SaladsAPI_Manager.Instance.twoVegetableRecipe[UnityEngine.Random.Range(0, SaladsAPI_Manager.Instance.twoVegetableRecipe.Count)];
                Debug.Log("Random Salad : " + randomTwoVegetableRecipe.id);
                orderedTwoVegetableSalad = randomTwoVegetableRecipe;
                saladPlate.GetComponent<Image>().sprite = order(orderedTwoVegetableSalad.id);
                saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 12.5f);
                maximumWaitingTime = 30f;
                currentWaitingTime = 30f;
                waitingBar.fillAmount = currentWaitingTime;

                twoVegetableSalad = true;
                threeVegetableSalad = false;
            }
            else
            {
                ThreeVegetableRecipe randomThreeVegetableRecipe = SaladsAPI_Manager.Instance.threeVegetableRecipe[UnityEngine.Random.Range(0, SaladsAPI_Manager.Instance.threeVegetableRecipe.Count)];
                Debug.Log("Random Salad : " + randomThreeVegetableRecipe.id);
                orderedThreeVegetableSalad = randomThreeVegetableRecipe;
                saladPlate.GetComponent<Image>().sprite = order(orderedThreeVegetableSalad.id);
                saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 12.5f);
                maximumWaitingTime = 35f;
                currentWaitingTime = 35f;
                waitingBar.fillAmount = currentWaitingTime;

                twoVegetableSalad = false;
                threeVegetableSalad = true;
            }
            customerState = CUSTOMER_STATE.WAITING;
        }
        else
        {
            Invoke("requestRandomSalad", 0.5f);
        }
    }

    public Sprite order(string saladName)
    {
        Sprite sprite = Resources.Load<Sprite>("Salad_Images/" + saladName);
        return sprite;
    }

    private void Update()
    {
        if (customerState == CUSTOMER_STATE.WAITING)
        {
            waitingBarGameobject.SetActive(true);
        }
        else
        {
            waitingBarGameobject.SetActive(false);
        }

        if (twoVegetableSalad & !threeVegetableSalad)
        {
            if (customerState == CUSTOMER_STATE.WAITING || customerState == CUSTOMER_STATE.ANGRY)
            {
                if (currentWaitingTime > 0)
                {
                    currentWaitingTime -= Time.deltaTime;
                    float percent = currentWaitingTime / maximumWaitingTime;
                    waitingBar.fillAmount = Mathf.Lerp(0, 1, percent);
                }
                else
                { 
                        if (customerState == CUSTOMER_STATE.WAITING)
                    {
                        GameManager.Instance.player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 10;
                        GameManager.Instance.player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 10;
                    }
                    else if (customerState == CUSTOMER_STATE.ANGRY)
                    {
                        GameManager.Instance.player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 5;
                        GameManager.Instance.player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 5;
                    }
                    customerState = CUSTOMER_STATE.NOT_RECIEVED;
                    ResetCustomerOrder();
                }
            }
        }

        if (!twoVegetableSalad & threeVegetableSalad)
        {
            if (customerState == CUSTOMER_STATE.WAITING || customerState == CUSTOMER_STATE.ANGRY)
            {
                if (currentWaitingTime > 0)
                {
                    currentWaitingTime -= Time.deltaTime;
                    float percent = currentWaitingTime / maximumWaitingTime;
                    waitingBar.fillAmount = Mathf.Lerp(0, 1, percent);
                }
                else
                {                                                                       
                    if (customerState == CUSTOMER_STATE.WAITING)
                    {
                        GameManager.Instance.player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 10;
                        GameManager.Instance.player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 10;
                    }
                    else if (customerState == CUSTOMER_STATE.ANGRY)
                    {
                        GameManager.Instance.player1.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 5;
                        GameManager.Instance.player2.GetComponent<ChefDetails>().scriptableObjectForChef.playerScore -= 5;
                    }

                    customerState = CUSTOMER_STATE.NOT_RECIEVED;
                    ResetCustomerOrder();
                }
            }
        }
    }

    public void orderRecieved(ChefDetails chef)
    {                                                
        if (customerState == CUSTOMER_STATE.WAITING)
        {
            customerState = CUSTOMER_STATE.CHECKING;
        }

        if (twoVegetableSalad && !threeVegetableSalad)
        {
            if (recievedTwoVegetableSalad != null)
            {
                if (checkTwoVegetableSaladIngredients(recievedTwoVegetableSalad))
                {                                   
                    customerState = CUSTOMER_STATE.RECIEVED;
                    ResetCustomerOrder();

                    remainingWaitingTime = currentWaitingTime;
                    currentWaitingTime = 0;

                    chef.scriptableObjectForChef.playerScore += 5;
                    if (remainingWaitingTime <= maximumWaitingTime)
                    {
                        spawnRandomPickup(chef);
                    }
                }
                else
                {
                    if (customerState == CUSTOMER_STATE.CHECKING)
                    {                                                  
                        customerState = CUSTOMER_STATE.ANGRY;
                        currentWaitingTime = currentWaitingTime / 2;
                    }
                    else if (customerState == CUSTOMER_STATE.ANGRY)
                    {                                                
                        customerState = CUSTOMER_STATE.NOT_RECIEVED;
                        ResetCustomerOrder();

                        remainingWaitingTime = currentWaitingTime;
                        currentWaitingTime = 0;

                        chef.scriptableObjectForChef.playerScore -= 10;
                    }
                }
            }
            else
            {
                if (customerState == CUSTOMER_STATE.CHECKING)
                {                                                 
                    customerState = CUSTOMER_STATE.ANGRY;
                    currentWaitingTime = currentWaitingTime / 2;
                }
                else if (customerState == CUSTOMER_STATE.ANGRY)
                {                                               
                    customerState = CUSTOMER_STATE.NOT_RECIEVED;
                    ResetCustomerOrder();

                    remainingWaitingTime = currentWaitingTime;
                    currentWaitingTime = 0;

                    chef.scriptableObjectForChef.playerScore -= 10;
                }
            }
        }
        else if (!twoVegetableSalad && threeVegetableSalad)
        {
            if (recievedThreeVegetableSalad != null)
            {
                if (checkThreeVegetableSaladIngredients(recievedThreeVegetableSalad))
                {
                    customerState = CUSTOMER_STATE.RECIEVED;
                    ResetCustomerOrder();

                    remainingWaitingTime = currentWaitingTime;
                    currentWaitingTime = 0;

                    chef.scriptableObjectForChef.playerScore += 5;
                    if (remainingWaitingTime >= maximumWaitingTime * 0.7f)
                    {
                        spawnRandomPickup(chef);
                    }
                }
                else
                {
                    if (customerState == CUSTOMER_STATE.WAITING)
                    {
                        customerState = CUSTOMER_STATE.ANGRY;
                        currentWaitingTime = currentWaitingTime / 2;
                    }
                    else if (customerState == CUSTOMER_STATE.ANGRY)
                    {
                        customerState = CUSTOMER_STATE.NOT_RECIEVED;
                        ResetCustomerOrder();

                        remainingWaitingTime = currentWaitingTime;
                        currentWaitingTime = 0;

                        chef.scriptableObjectForChef.playerScore -= 10;
                    }
                }
            }
            else
            {
                if (customerState == CUSTOMER_STATE.WAITING)
                {
                    customerState = CUSTOMER_STATE.ANGRY;
                    currentWaitingTime = currentWaitingTime / 2;
                }
                else if (customerState == CUSTOMER_STATE.ANGRY)
                {
                    customerState = CUSTOMER_STATE.NOT_RECIEVED;
                    ResetCustomerOrder();

                    remainingWaitingTime = currentWaitingTime;
                    currentWaitingTime = 0;

                    chef.scriptableObjectForChef.playerScore -= 10;
                }
            }
        }

        ResetChefDetails(chef);
    }

    public bool checkTwoVegetableSaladIngredients(TwoVegetableRecipe twoVegitableRecipe)
    {                                                                   
        string sortedOrderedTwoVegetableSalad = sortSalad(orderedTwoVegetableSalad.ingredient1 + orderedTwoVegetableSalad.ingredient2);
        string sortedTwoVegitableRecipe = sortSalad(twoVegitableRecipe.ingredient1 + twoVegitableRecipe.ingredient2);

        if (sortedOrderedTwoVegetableSalad == sortedTwoVegitableRecipe)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkThreeVegetableSaladIngredients(ThreeVegetableRecipe threeVegitableRecipe)
    {
        string sortedOrderedThreeVegetableSalad = sortSalad(orderedThreeVegetableSalad.ingredient1 + orderedThreeVegetableSalad.ingredient2 + orderedThreeVegetableSalad.ingredient3);
        string sortedThreeVegitableRecipe = sortSalad(threeVegitableRecipe.ingredient1 + threeVegitableRecipe.ingredient2 + threeVegitableRecipe.ingredient3);

        if (sortedOrderedThreeVegetableSalad == sortedThreeVegitableRecipe)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetCustomerOrder()
    {
        saladPlate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Rectangle_Plate");
        saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 20f);

        //Two vegetable salad
        if (orderedTwoVegetableSalad != null)
        {
            orderedTwoVegetableSalad.id = "";
            orderedTwoVegetableSalad.ingredient1 = "";
            orderedTwoVegetableSalad.ingredient2 = "";
        }

        //Three vegetable salad
        if (orderedThreeVegetableSalad != null)
        {
            orderedThreeVegetableSalad.id = "";
            orderedThreeVegetableSalad.ingredient1 = "";
            orderedThreeVegetableSalad.ingredient2 = "";
            orderedThreeVegetableSalad.ingredient3 = "";
        }
    }

    public void ResetChefDetails(ChefDetails chef)
    {
        chef.GetComponent<ChefDetails>().pickedVegetables.Clear();
        chef.GetComponent<ChefDetails>().pickedTwoVegetableSalad = null;
        chef.GetComponent<ChefDetails>().pickedThreeVegetableSalad = null;

        chef.GetComponent<ChefMovement>().currentlyChoppingIngredient = null;
        chef.GetComponent<ChefMovement>().listOfChoppedIngredients.Clear();
    }

    public string sortSalad(string input)
    {
        char[] characters = input.ToCharArray();
        Array.Sort(characters);
        return new string(characters);
    }

    public void spawnRandomPickup(ChefDetails chef)
    {
        GameObject randomPickup = pickupsList[UnityEngine.Random.Range(0, pickupsList.Count)];

        Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-450f, 450f), UnityEngine.Random.Range(-500f, -200f), 0f);
        randomPickup.GetComponent<ObjectType>().chef = chef;

       GameObject instantiatedPickup = Instantiate(randomPickup, transform.position, Quaternion.identity) as GameObject;
        instantiatedPickup.transform.SetParent(transform);
        instantiatedPickup.transform.localScale = new Vector3(2f, 2f, 2f);
        instantiatedPickup.transform.localPosition = randomPosition;
        
    }
}