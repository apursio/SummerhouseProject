using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class BackendHandler : MonoBehaviour
{
    const string jsonTestStr ="{ " 
        + "\"scores\":[ " 
        + "{\"id\":1, \"playername\":\"Matti\",  \"score\":20, \"playtime\": \"2020-21-11 08:20:00\"}, " 
        + "{\"id\":2, \"playername\":\"Hankka\", \"score\":30, \"playtime\": \"2020-21-11 08:20:00\"}, " 
        + "{\"id\":3, \"playername\":\"Ismo\",   \"score\":40, \"playtime\": \"2020-21-11 08:20:00\"} " 
        + "] }";     
    // vars
    HighScores hs;
    private int fetchCounter;

    // Start is called before the first frame update
    void Start()    
    {         
        Debug.Log("BackendHandler started");         
        hs = JsonUtility.FromJson<HighScores>(jsonTestStr);         
        Debug.Log("HighScores name: " + hs.scores[0].playername);         
        Debug.Log("HighScores as json: " + JsonUtility.ToJson(hs));     }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void FetchHighScoresJSONFile() 
    { 
        fetchCounter++; Debug.Log("FetchHighScoresJSONFile button clicked"); 
    }
    public void FetchHighScoresJSON()
    {
        fetchCounter++; Debug.Log("FetchHighScoresJSON button clicked");
}

    // :
    string log = "";

    // :
    string InsertToLog(string s)    
    {         
        return log = "[" + fetchCounter + "] " + s + "\n" + log;    
    }     
    string GetLog()     
    {         
        return log;     
    }

}
