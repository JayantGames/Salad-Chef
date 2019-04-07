using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SaladsAPI_Manager : MonoBehaviour
{
    public string apiBaseUrl; 
    public List<TwoVegetableRecipe> twoVegetableRecipe;
    public List<ThreeVegetableRecipe> threeVegetableRecipe;
    // Start is called before the first frame update
    void Start()
    {
    //    StartCoroutine(callAPI());
    }

    IEnumerator callAPI()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiBaseUrl);
        yield return www.SendWebRequest();
        Debug.Log("Fetched Data : " + www.downloadHandler.text);
        

        if (www.isNetworkError || www.isHttpError)
        {
            // Debug.Log("Error : " + www.d);
            Debug.LogError(www);
            
        }
        else
        {
            parseSaladsJson(www.downloadHandler.text);
        }
        
    }

    void parseSaladsJson(string json)
    {
        Debug.Log("ParseSaladsJson is being called");
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
