using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefDetails : MonoBehaviour
{
    // Scriptable object for the details regarding each chef. This approah is used to help the designers for the game.
    public ScriptableObjectForChef scriptableObjectForChef;

    // An image with the letter E indicating that if you now press 'E' key, you can interact with the particular object.
    public GameObject pickIndicator;

    // List of picked vegetables which the chef has currently picked up. As per the instructions, chefs cannot pick more than 2 vegatbles at the same time.
    public List<string> pickedVegetables;

    // The salad which chef gets after picking up two or three chopped vegetables.
    public TwoVegetableRecipe pickedTwoVegetableSalad;
    public ThreeVegetableRecipe pickedThreeVegetableSalad;

    // The ingredients picked by the chef after they were chopped.
    public GameObject pickedIngredient1;
    public GameObject pickedIngredient2;
    public GameObject pickedIngredient3;
    public List<GameObject> pickedIngredientsList;

    // The waiting bar on the right side of the chef to indicate that the particular chef is currently chopping and cannot move.
    public Image chefWaitingBar;

    // Method called when a chef interacts with a vegetable and the particular vegetable gets picked up by the chef
    public void assignVegetablesToChef()
    {
        switch (pickedVegetables.Count)
        {  
            case 1:
                {
                    pickedIngredient1.SetActive(true);   

                    pickedIngredient1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Vegetables/" + pickedVegetables[0]);
                    break;
                }
            case 2:
                {
                    pickedIngredient1.SetActive(true);
                    pickedIngredient2.SetActive(true);

                    pickedIngredient1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Vegetables/" + pickedVegetables[0]);
                    pickedIngredient2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Vegetables/" + pickedVegetables[1]);
                    break;
                }
            case 3:
                {
                    pickedIngredient1.SetActive(true);
                    pickedIngredient2.SetActive(true);
                    pickedIngredient3.SetActive(true);

                    pickedIngredient1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Vegetables/" + pickedVegetables[0]);
                    pickedIngredient2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Vegetables/" + pickedVegetables[1]);
                    pickedIngredient3.GetComponent<Image>().sprite = Resources.Load<Sprite>("Vegetables/" + pickedVegetables[2]);
                    break;
                }
        }
    }

    private void Update()
    {
        if (pickedVegetables.Count == 0)
        {
            pickedIngredient1.SetActive(false);
            pickedIngredient2.SetActive(false);
            pickedIngredient3.SetActive(false);
        }
    }    

    // Method portraying that which vegetable is being currently chopped.
    public void checkCurrentlyChoppingIngredient(string ingredientName)
    {
        for (int i = 0; i < pickedIngredientsList.Count; i++)
        {
            if (pickedIngredientsList[i].GetComponent<Image>().sprite != null)
            {
                if (pickedIngredientsList[i].GetComponent<Image>().sprite.name == ingredientName)
                {
                    pickedIngredientsList[i].SetActive(false);
                }
            }
        }
    }
}
