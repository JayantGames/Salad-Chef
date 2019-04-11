using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SaladsAPI_Manager : MonoBehaviour
{
    // I have used this approach of fetching data from the API to demonstrate the fact that for making a game dynamic we can use API's.
    // The core benefit of using an API is that all the data can be changed from the backend and the game can be made generic so that it can handle the response well
    // and an update would not be required to make changes in the game.

    // Instance of this class created for the implementation of singleton design pattern.
    public static SaladsAPI_Manager Instance;
   
    // Base URL for the API to fetch the salad's data.
    public string apiBaseUrl; 

    // Public lists to store the relevant data once the fetching and parsing of the data is finished.
    public List<TwoVegetableRecipe> twoVegetableRecipe;
    public List<ThreeVegetableRecipe> threeVegetableRecipe;

    // Bool indicator to check if the data is completely fetched from the API.
    public bool saladsDataFetched;

    private void Awake()
    {
        Instance = this;
    }
                                                     
    void Start()
    {
        StartCoroutine(callAPI());
    }

    // Calling the Salads API.
    IEnumerator callAPI()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiBaseUrl);
        yield return www.SendWebRequest();                       

        if (www.isNetworkError || www.isHttpError)
        {
            saladsDataFetched = false;
            Debug.LogError(www);        
        }
        else
        {
            saladsDataFetched = true;
            parseSaladsJson(www.downloadHandler.text);
        }                 
    }

    // Parsing the json data after the API has been fetched.
    void parseSaladsJson(string json)
    {                                                      
        RootObject rootObject = JsonUtility.FromJson<RootObject>(json);

        twoVegetableRecipe = rootObject.two_vegetable_recipe;
        threeVegetableRecipe = rootObject.three_vegetable_recipe;    
    }                  
}

[System.Serializable]
public class TwoVegetableRecipe
{
    public string id;
    public string ingredient1;
    public string ingredient2;
}

[System.Serializable]
public class ThreeVegetableRecipe
{
    public string id;
    public string ingredient1;
    public string ingredient2;
    public string ingredient3;
}
[System.Serializable]
public class RootObject
{
    public List<TwoVegetableRecipe> two_vegetable_recipe;
    public List<ThreeVegetableRecipe> three_vegetable_recipe;
}
