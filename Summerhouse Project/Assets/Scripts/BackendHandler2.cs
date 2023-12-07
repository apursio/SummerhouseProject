using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;
using UnityEngine.Networking;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine.SceneManagement;

public class BackendHandler2 : MonoBehaviour
{
    public TMP_InputField playernameInput;
    public TMP_InputField scoreInput;
    private bool posted;

    const string urlBackendHighScores = "https://niisku.lab.fi/~x119867/PHPBackend/api/highscores.php";

    // vars
    HighScoreDTO hs;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        posted = false;
        scoreInput.text = GlobalVariableStorage.playerScore.ToString();
        playernameInput.interactable = true;

        playernameInput.Select();
        playernameInput.ActivateInputField();
  
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return))
        {
            PostGameResults();
        }

        if (posted)
        {
            Restart(0);
        }
    }
    
    public void PostGameResults()
    {
        HighScoreDTO hsItem = new HighScoreDTO();
        if (string.IsNullOrEmpty(playernameInput.text))
        {
            hsItem.playername = "no name";
        } else
        {
            hsItem.playername = playernameInput.text;
        }
        
        hsItem.score = GlobalVariableStorage.playerScore;

        Debug.Log("PostGameResults button clicked: " + playernameInput.text + " with scores " + scoreInput.text);
        Debug.Log("hsItem: " + JsonUtility.ToJson(hsItem));
        StartCoroutine(PostRequestForHighScores(urlBackendHighScores, hsItem));
    }

    IEnumerator PostRequestForHighScores(string uri, HighScoreDTO hsItem)
    {

        string jsonPayload = JsonUtility.ToJson(hsItem);
        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");


            // Request and wait for reply
            yield return webRequest.SendWebRequest();
            // get raw data and convert it to string

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error in post request: " + webRequest.error);
            }
            else
            {
                string resultStr = Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                Debug.Log("Received(UTF8): " + resultStr);
                posted = true;
            }
        }
    }

    public void Restart (int buildIndex)
    {
        Debug.Log("EndLevel function called with build index: " + buildIndex);

        if (SceneManager.GetSceneByBuildIndex(buildIndex) != null)
        {
            Debug.Log("Loading scene with build index: " + buildIndex);
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            Debug.LogError("Invalid build index: " + buildIndex);
        }
    }

}
