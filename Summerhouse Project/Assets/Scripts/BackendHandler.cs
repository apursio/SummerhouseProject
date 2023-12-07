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
    public TMP_InputField playernameInput;
    public TMP_InputField scoreInput;
    bool updateHighScoreTextArea = false;
    private int fetchCounter = 0;

   
    const string urlBackendHighScores = "https://niisku.lab.fi/~x119867/PHPBackend/api/highscores.php";

    // vars
    HighScores hs; 
    
    // Start is called before the first frame update
    void Start()
    {         
        FetchHighScoresJSON();   
    }

    
    // Update is called once per frame
    void Update()
    {

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
            //InsertToLog("Request sent to " + uri);
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
                //InsertToLog("Error encountered: " + webRequest.error);
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(resultStr);
                // create HighScore item from json string
                hs = JsonUtility.FromJson<HighScores>(resultStr);
                updateHighScoreTextArea = true;
                
            }
        }
    }
    
    public void FetchHighScoresJSON() 
    {
        fetchCounter++;
        StartCoroutine(GetRequestForHighScores(urlBackendHighScores));
    }
   
}
