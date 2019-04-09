using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerInteraction : MonoBehaviour
{
    public enum CUSTOMER_STATE
    {
        WAITING,
        RECIEVED,
        ANGRY
    }

    public float maximumWaitingTime = 20f;
    public float currentWaitingTime = 20f;
    public CUSTOMER_STATE customerState;
    public GameObject saladPlate;
    public TwoVegetableRecipe orderedTwoVegetableSalad;
    public ThreeVegetableRecipe orderedThreeVegetableSalad;

    public TwoVegetableRecipe recievedTwoVegetableSalad;
    public ThreeVegetableRecipe recievedThreeVegetableSalad;

    public Image waitingBar;

    public bool twoVegetableSalad;
    public bool threeVegetableSalad;

    private void Start()
    {
        waitingBar.fillAmount = currentWaitingTime;
        requestRandomSalad();
    }

    public void requestRandomSalad()
    {
        if (SaladsAPI_Manager.Instance.saladsDataFetched)
        {
            if (GameManager.Instance.currentDifficultyLevel == GameManager.DIFFICULTY_LEVEL.EASY)
            {
                TwoVegetableRecipe randomTwoVegetableRecipe = SaladsAPI_Manager.Instance.twoVegetableRecipe[Random.Range(0, SaladsAPI_Manager.Instance.twoVegetableRecipe.Count)];
                Debug.Log("Random Salad : " + randomTwoVegetableRecipe.id);
                orderedTwoVegetableSalad = randomTwoVegetableRecipe;
                saladPlate.GetComponent<Image>().sprite = order(orderedTwoVegetableSalad.id);
                saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 12.5f);
                currentWaitingTime = 30f;

                twoVegetableSalad = true;
                threeVegetableSalad = false;
            }
            else
            {
                ThreeVegetableRecipe randomThreeVegetableRecipe = SaladsAPI_Manager.Instance.threeVegetableRecipe[Random.Range(0, SaladsAPI_Manager.Instance.threeVegetableRecipe.Count)];
                Debug.Log("Random Salad : " + randomThreeVegetableRecipe.id);
                orderedThreeVegetableSalad = randomThreeVegetableRecipe;
                saladPlate.GetComponent<Image>().sprite = order(orderedThreeVegetableSalad.id);
                saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 12.5f);
                currentWaitingTime = 35f;

                twoVegetableSalad = false;
                threeVegetableSalad = true;
            }
        }
        else
        {
            Invoke("requestRandomSalad", 0.5f);
        }
    }

    public Sprite order(string saladName)
    {
        Sprite sprite = Resources.Load<Sprite>("Salad_Images/" + saladName);
        Debug.Log("Salad_Images/" + saladName);
        return sprite;
    }

    private void Update()
    {
        if (twoVegetableSalad & !threeVegetableSalad)
        {
            if (saladPlate.GetComponent<Image>().sprite.name == orderedTwoVegetableSalad.id)
            {
                if (currentWaitingTime > 0)
                {
                    customerState = CUSTOMER_STATE.WAITING;
                    currentWaitingTime -= Time.deltaTime;
                    float percent = currentWaitingTime / maximumWaitingTime;
                    waitingBar.fillAmount = Mathf.Lerp(0, 1, percent);
                }
                else
                {
                    if (checkTwoVegetableSaladIngredients(recievedTwoVegetableSalad))
                    {
                        customerState = CUSTOMER_STATE.RECIEVED;
                        saladPlate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Rectangle_Plate");
                        saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 20f);
                        orderedTwoVegetableSalad.id = "";
                        orderedTwoVegetableSalad.ingredient1 = "";
                        orderedTwoVegetableSalad.ingredient2 = "";
                    }
                    else
                    {
                        customerState = CUSTOMER_STATE.ANGRY;
                        saladPlate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Rectangle_Plate");
                        saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 20f);
                        orderedTwoVegetableSalad.id = "";
                        orderedTwoVegetableSalad.ingredient1 = "";
                        orderedTwoVegetableSalad.ingredient2 = "";

                        Invoke("requestRandomSalad", 4f);
                    }
                }
            }
        }

        if (!twoVegetableSalad & threeVegetableSalad)
        {
            if (saladPlate.GetComponent<Image>().sprite.name == orderedThreeVegetableSalad.id)
            {
                if (currentWaitingTime > 0)
                {
                    customerState = CUSTOMER_STATE.WAITING;
                    currentWaitingTime -= Time.deltaTime;
                    float percent = currentWaitingTime / maximumWaitingTime;
                    waitingBar.fillAmount = Mathf.Lerp(0, 1, percent);
                }
                else
                {
                    if (checkThreeVegetableSaladIngredients(recievedThreeVegetableSalad))
                    {
                        customerState = CUSTOMER_STATE.RECIEVED;
                        saladPlate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Rectangle_Plate");
                        saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 20f);
                        orderedThreeVegetableSalad.id = "";
                        orderedThreeVegetableSalad.ingredient1 = "";
                        orderedThreeVegetableSalad.ingredient2 = "";
                        orderedThreeVegetableSalad.ingredient3 = "";
                    }
                    else
                    {
                        customerState = CUSTOMER_STATE.ANGRY;
                        saladPlate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Rectangle_Plate");
                        saladPlate.GetComponent<RectTransform>().sizeDelta = new Vector2(saladPlate.GetComponent<RectTransform>().sizeDelta.x, 20f);
                        orderedThreeVegetableSalad.id = "";
                        orderedThreeVegetableSalad.ingredient1 = "";
                        orderedThreeVegetableSalad.ingredient2 = "";
                        orderedThreeVegetableSalad.ingredient3 = "";

                        Invoke("requestRandomSalad", 4f);
                    }
                }
            }
        }
    }

    public void orderRecieved()
    {
        float remainingWaitingTime = currentWaitingTime;
        currentWaitingTime = 0;
    }

    public bool checkTwoVegetableSaladIngredients(TwoVegetableRecipe twoVegitableRecipe)
    {
        Debug.Log("Checking two vegetable salad ingredients");
        Debug.Log("Recieved recipe is : " + twoVegitableRecipe.id);

        if (twoVegitableRecipe.ingredient1 == orderedTwoVegetableSalad.ingredient1 ||
twoVegitableRecipe.ingredient1 == orderedTwoVegetableSalad.ingredient2 &&
twoVegitableRecipe.ingredient2 == orderedTwoVegetableSalad.ingredient1 ||
twoVegitableRecipe.ingredient2 == orderedTwoVegetableSalad.ingredient2)
        {
            Debug.Log("Delievered salad is correct");
            return true;
        }
        else
        {
            Debug.Log("Delievered salad is incorrect");
            return false;
        }
    }

    public bool checkThreeVegetableSaladIngredients(ThreeVegetableRecipe threeVegitableRecipe)
    {
        Debug.Log("Checking three vegetable salad ingredients");

        if (threeVegitableRecipe.ingredient1 == orderedThreeVegetableSalad.ingredient1 ||
threeVegitableRecipe.ingredient1 == orderedThreeVegetableSalad.ingredient2 ||
threeVegitableRecipe.ingredient1 == orderedThreeVegetableSalad.ingredient3 &&
threeVegitableRecipe.ingredient2 == orderedThreeVegetableSalad.ingredient1 ||
threeVegitableRecipe.ingredient2 == orderedThreeVegetableSalad.ingredient2 ||
threeVegitableRecipe.ingredient2 == orderedThreeVegetableSalad.ingredient3 &&
threeVegitableRecipe.ingredient3 == orderedThreeVegetableSalad.ingredient1 ||
threeVegitableRecipe.ingredient3 == orderedThreeVegetableSalad.ingredient2 ||
threeVegitableRecipe.ingredient3 == orderedThreeVegetableSalad.ingredient3)
        {
            Debug.Log("Delievered salad is correct");
            return true;
        }
        else
        {
            Debug.Log("Delievered salad is incorrect");
            return false;
        }
    }
}