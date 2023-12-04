using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;
using UnityEngine.Networking;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

public class BackendHandler : MonoBehaviour
{
    public TMP_Text highScoresTextArea;
    public TMP_Text logTextArea;
    public TMP_InputField playernameInput;
    public TMP_InputField scoreInput;
    bool updateHighScoreTextArea = false;
    private int fetchCounter = 0;
    string log = "";
    // const string urlBackendHighScoresFile = "https://niisku.lab.fi/~x119867/PHPBackend/highscores.json";
    const string urlBackendHighScores = "https://niisku.lab.fi/~x119867/PHPBackend/api/highscores.php";

    /*Ä
    const string jsonTestStr = "{ " +
        "\"scores\":[ " +
        "{\"id\":1, \"playername\":\"Matti\",  \"score\":20, \"playtime\": \"2020-21-11 08:20:00\"}, " +
        "{\"id\":2, \"playername\":\"Henkka\", \"score\":30, \"playtime\": \"2020-21-11 08:20:00\"}, " +
        "{\"id\":3, \"playername\":\"Ismo\",   \"score\":40, \"playtime\": \"2020-21-11 08:20:00\"} " +
        "] }";  
    */
    // vars
    HighScores hs; 
    
    // Start is called before the first frame update
    void Start()
    {         
        //Debug.Log("BackendHandler started");
        FetchHighScoresJSON();
        // conversion from JSON to object
        // hs = JsonUtility.FromJson<HighScores>(jsonTestStr);       
        // Debug.Log("HighScores name: " + hs.scores[0].playername);  
        // conversion from object to JSON
        // Debug.Log("HighScores as json: " + JsonUtility.ToJson(hs));     
    }

    
    // Update is called once per frame
    void Update()
    {
        //logTextArea.text = log;

        if (updateHighScoreTextArea)
        {
            highScoresTextArea.text = CreateHighScoreList();
            updateHighScoreTextArea = false;
        }
    }
    string CreateHighScoreList()
    {
        string hsList = "";
        if (hsList != null)
        {
            int len = (hs.scores.Length < 3) ? hs.scores.Length : 3;
            for (int i = 0; i < len; i++)
            {
                string truncatedName = hs.scores[i].playername.Length > 7
                    ? hs.scores[i].playername.Substring(0, 7)
                    : hs.scores[i].playername;

                hsList += string.Format("[ {0} ] {1,-7} {2,5}\n",
                    (i + 1), truncatedName, hs.scores[i].score);
            }
        }
        return hsList;
    }



    IEnumerator GetRequestForHighScores(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            InsertToLog("Request sent to " + uri);
            Debug.Log("Coroutine started");
            
            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            // Request and wait for reply
            yield return webRequest.SendWebRequest();
            // get raw data and convert it to string
            string resultStr = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                InsertToLog("Error encountered: " + webRequest.error);
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(resultStr);
                // create HighScore item from json string
                hs = JsonUtility.FromJson<HighScores>(resultStr);
                updateHighScoreTextArea = true;
                InsertToLog("Response received succesfully ");
                Debug.Log("Received(UTF8): " + resultStr);
                Debug.Log("Received(HS): " + JsonUtility.ToJson(hs));
                Debug.Log("Received(HS) name: " + hs.scores[0].playername);
            }
        }
    }
    /*
    public void FetchHighScoresJSONFile() 
    {
        fetchCounter++;
        Debug.Log("FetchHighScoresJSONFile button clicked");

        // first get just a json file
        StartCoroutine(GetRequestForHighScoresFile(urlBackendHighScoresFile));
    }


    IEnumerator GetRequestForHighScoresFile(string uri) 
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            InsertToLog("Request sent to " + uri);
            // set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for reply
            yield return webRequest.SendWebRequest();

            // get raw data and convert it to string
            string resultStr = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                InsertToLog("Error encountered: " + webRequest.error);
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // create HighScore item from json string
                hs = JsonUtility.FromJson<HighScores>(resultStr);

                InsertToLog("Response received succesfully ");
                Debug.Log("Received(UTF8): " + resultStr);
                Debug.Log("Received(HS): " + JsonUtility.ToJson(hs));
            }
        } 
    } */
    public void FetchHighScoresJSON() 
    {
        fetchCounter++;
        //Debug.Log("FetchHighScoresJSON button clicked");
        StartCoroutine(GetRequestForHighScores(urlBackendHighScores));
    }

    string InsertToLog(string s)
    {
        return log = "[" + fetchCounter + "]" + s + "\n" + log;
    }

    string GetLog() 
    {
        return log;
    }

    public void PostGameResults()
    {
        HighScore hsItem = new HighScore();
        hsItem.playername = playernameInput.text;
        //hsItem.score = float.Parse(scoreInput.text);
        hsItem.score = GlobalVariableStorage.playerScore;

        Debug.Log("PostGameResults button clicked: " + playernameInput.text + " with scores " + scoreInput.text);
        Debug.Log("hsItem: " + JsonUtility.ToJson(hsItem)); 
        StartCoroutine(PostRequestForHighScores(urlBackendHighScores, hsItem));
    }

    IEnumerator PostRequestForHighScores(string uri, HighScore hsItem)
    {

        string jsonPayload = JsonUtility.ToJson(hsItem);
        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            InsertToLog("POST request sent to " + uri);

            // Request and wait for reply
            yield return webRequest.SendWebRequest();
            // get raw data and convert it to string
            
            if (webRequest.result != UnityWebRequest.Result.Success) { 
                InsertToLog("Error encountered in post request: " + webRequest.error);
                Debug.Log("Error in post request: " + webRequest.error);
            }
            else{
                string resultStr = Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                InsertToLog("POST request succesful");
                Debug.Log("Received(UTF8): " + resultStr);
            }
        }
    } 

        }
