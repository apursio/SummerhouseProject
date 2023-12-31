using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using System;
using UnityEditor.Rendering;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor.Callbacks;
//using UnityEngine.UIElements;

public class LevelController : MonoBehaviour
{
    public GameObject DialogueBox0;
    public GameObject DialogueBox1;
    public GameObject DialogueBox2;
    public GameObject DialogueBox3;
    public GameObject DialogueBox4;
    public TMP_InputField tmpIfTime;
    public TMP_InputField tmpIfPoints;
    public TMP_Text TextTimeScore;
    public TMP_Text TextActionScore;
    public float initialTime;
    public float taskTime;
    public GameObject blanket;
    public GameObject extinguisher;
    public GameObject lid;
    public GameObject ScoreField;
    public GameObject fire1;
    public GameObject fire2;
    public GameObject fire3;
    public GameObject fire4;
    public GameObject fire5;
    public GameObject fire6;
    public GameObject smoke4;
    public GameObject smoke5;
    public GameObject smoke6;
    public GameObject steam;
    private int savePoints = 0;
    public GameObject endMsg;
    public TMP_Text CallText;
    AudioSource audioSource;

    // Start is called before the first frame update

    void Start()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        GlobalVariableStorage.taskTimeLeft = taskTime;
        GlobalVariableStorage.fireIsOut = false;
        GlobalVariableStorage.actionScore = 0;
        GlobalVariableStorage.totalActionScore = 0;
        GlobalVariableStorage.playerScore = 0;
        GlobalVariableStorage.safeTime = true;
        GlobalVariableStorage.level1 = false;
        GlobalVariableStorage.level2 = false;
        GlobalVariableStorage.level3 = false;
        GlobalVariableStorage.scoreElectricityBox = false;
        GlobalVariableStorage.lastLevelDone = false;
        GlobalVariableStorage.fireIsOutOfControl = false;
        TextTimeScore.enabled = false;
        TextActionScore.enabled = false;
        endMsg.SetActive(false);
        CallText.enabled = false;

        GlobalVariableStorage.allOut = false;
        fire1.SetActive(false);
        fire2.SetActive(false);
        fire3.SetActive(false);
        fire4.SetActive(false);
        fire5.SetActive(false);
        fire6.SetActive(false);
        smoke4.SetActive(false);
        smoke5.SetActive(false);
        smoke6.SetActive(false);
        steam.SetActive(true);
        DialogueBox0.SetActive(false);
        DialogueBox1.SetActive(false);
        DialogueBox2.SetActive(false);
        DialogueBox3.SetActive(false);
        DialogueBox4.SetActive(false);
        DialogueBox0.SetActive(true);

        StartCoroutine("countTaskTime");
    }


    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 0;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        tmpIfTime.text = string.Format("{0:00} : {1:00}", minutes, seconds); //muotoilee ajan minuuteiksi ja sekunneiksi
    }

    void DisplayTimeScore()
    {
        Debug.Log("Display time score");
        GlobalVariableStorage.timeScore = (int) GlobalVariableStorage.taskTimeLeft *10; 
        TextTimeScore.text = "+" + GlobalVariableStorage.timeScore.ToString();
        TextTimeScore.enabled = true;
        StartCoroutine("waitForSecond");

    }

    void DisplayActionScore()
    {
        if (GlobalVariableStorage.actionScore >= 0)
        {
            TextActionScore.text = "+" + GlobalVariableStorage.actionScore.ToString();
        }
        else
        {
            TextActionScore.text = GlobalVariableStorage.actionScore.ToString();
        }
        TextActionScore.enabled = true;
        StartCoroutine("waitForSecond");
    }


    IEnumerator waitForSecond()
    {
        yield return new WaitForSeconds(3f);
        TextTimeScore.enabled = false;
        TextActionScore.enabled = false;
    }

    void DisplayPoints()
    {
        tmpIfPoints.text = GlobalVariableStorage.playerScore.ToString();
    }

    IEnumerator countTaskTime()
    {
        Debug.Log("Task time count started...");
        float interval = 1f;

        for (; ; )
        {
            yield return new WaitForSeconds(interval);

            if (GlobalVariableStorage.taskTimeLeft > 0)
            {
                GlobalVariableStorage.taskTimeLeft -= interval;
            }
            if (GlobalVariableStorage.safeTime
                && GlobalVariableStorage.taskTimeLeft <= 0)
            {
                Debug.Log("Safe Time over");
                MoveToNextLevel();
            }

            if (GlobalVariableStorage.level1) 
            {
                if (GlobalVariableStorage.fireIsOut)
                {
                    Debug.Log("Fire is out level 1");
                    Debug.Log("Task time left " + GlobalVariableStorage.taskTimeLeft);
                    DisplayActionScore();
                    DisplayTimeScore();
                    GlobalVariableStorage.playerScore += GlobalVariableStorage.timeScore;
                    GlobalVariableStorage.taskTimeLeft = 0;
                    GlobalVariableStorage.fireIsOut = false;
                    DialogueBox1.SetActive(false);
                    DialogueBox3.SetActive(true);
                    yield return new WaitForSeconds(15f); // the next fire doesn't start immediately
                    MoveToNextLevel();
                }
                else if (GlobalVariableStorage.taskTimeLeft <= 0 || GlobalVariableStorage.fireIsOutOfControl) // if the time runs out, fire gets out of control
                {
                    Debug.Log("Time up1, Fire should get out of control");
                    FireOutOfControl();
                    fire4.SetActive(true);
                    smoke4.SetActive(true);
                }
            }
            else if (GlobalVariableStorage.level2)
            {
   
                if (GlobalVariableStorage.scoreElectricityBox && GlobalVariableStorage.isKnobTurned)
                {
                    GlobalVariableStorage.actionScore = 300;
                    DisplayActionScore();
                    GlobalVariableStorage.playerScore = GlobalVariableStorage.playerScore + GlobalVariableStorage.actionScore;
                    GlobalVariableStorage.scoreElectricityBox = false;
                }
                if (GlobalVariableStorage.fireIsOut)
                {  
                    Debug.Log("Fire is out level 2");
                    Debug.Log("Task time left " + GlobalVariableStorage.taskTimeLeft);
                    DisplayActionScore();
                    DisplayTimeScore();
                    GlobalVariableStorage.playerScore += GlobalVariableStorage.timeScore;
                    GlobalVariableStorage.taskTimeLeft = 0;
                    DialogueBox2.SetActive(false);
                    DialogueBox3.SetActive(true);
                    yield return new WaitForSeconds(15f); // the next fire doesn't start immediately
                    MoveToNextLevel();
                   
                }
                else if (GlobalVariableStorage.taskTimeLeft <= 0) // if the time runs out, fire gets out of control
                {
                    Debug.Log("Time up2, Fire should get out of control");
                    FireOutOfControl();
                    fire5.SetActive(true);
                    smoke5.SetActive(true);
                }
            }
            else if (GlobalVariableStorage.level3)
            {
                
                if (GlobalVariableStorage.lastLevelDone)
                {
                    DisplayActionScore();
                    DisplayTimeScore();
                    GlobalVariableStorage.playerScore += GlobalVariableStorage.timeScore;
                    Debug.Log("Task time left " + GlobalVariableStorage.taskTimeLeft);
                    GlobalVariableStorage.taskTimeLeft = 0;
                    Debug.Log(savePoints);
                    GlobalVariableStorage.level3 = false;
                    endMsg.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }
    void MoveToNextLevel()
    {
        // Logic for moving to the next level

        Debug.Log("in move to next level");
        if (GlobalVariableStorage.safeTime)
        {
            GlobalVariableStorage.safeTime = false;
            GlobalVariableStorage.level1 = true;
            GlobalVariableStorage.taskTimeLeft = 60;
            tmpIfTime.enabled = true;
            steam.SetActive(false);
            fire1.SetActive(true);
            DialogueBox0.SetActive(false);
            DialogueBox1.SetActive(true);
            Debug.Log("to level 1");
        }
        else if (GlobalVariableStorage.level1)
        {
            GlobalVariableStorage.level1 = false;
            GlobalVariableStorage.level2 = true;
            GlobalVariableStorage.taskTimeLeft = 60;
            fire2.SetActive(true);
            DialogueBox1.SetActive(false);
            DialogueBox3.SetActive(false);
            DialogueBox2.SetActive(true);
            Debug.Log("to level 2");
            GlobalVariableStorage.scoreElectricityBox = true;
        }
        else if (GlobalVariableStorage.level2)
        {
            FireOutOfControl();
            fire3.SetActive(true);
            fire6.SetActive(true);
            smoke6.SetActive(true);
            Debug.Log("to level 3");
        }
        else
        {
            return;
        }
    }

    void FireOutOfControl()
    {
        GlobalVariableStorage.level1 = false;
        GlobalVariableStorage.level2 = false;
        GlobalVariableStorage.level3 = true;
        GlobalVariableStorage.fireIsOutOfControl = true;
        GlobalVariableStorage.taskTimeLeft = 60;
        DialogueBox1.SetActive(false);
        DialogueBox3.SetActive(false);
        DialogueBox4.SetActive(true);
        CallText.enabled = true;
        audioSource = GameObject.Find("FireAlarm").GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void EndLevel(int buildIndex)
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


    // Update is called once per frame
    void Update()
    {
        if(GlobalVariableStorage.safeTime == false) {
            DisplayTime(GlobalVariableStorage.taskTimeLeft);
        }
        DisplayPoints();
        if(GlobalVariableStorage.lastLevelDone)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Space key pressed");
                EndLevel(2);
            }
        }
    }
}

