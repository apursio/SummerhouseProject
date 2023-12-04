using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using System;
using UnityEditor.Rendering;
using Unity.VisualScripting;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.Callbacks;

public class LevelController : MonoBehaviour
{
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
    //public float safeTime;
    public GameObject blanket;
    public GameObject extinguisher;
    public GameObject lid;
    public GameObject ScoreField;
    public GameObject fire1;
    public GameObject fire2;
    public GameObject fire3;
    public GameObject fire4;
    private int savePoints = 0;
    public GameObject endMsg;

    // Start is called before the first frame update

    void Start()
    {
        
        Time.timeScale = 1;
        //GlobalVariableStorage.timeLeft = initialTime;
        GlobalVariableStorage.taskTimeLeft = taskTime;
        //GlobalVariableStorage.safeTimeLeft = safeTime;
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
        TextTimeScore.enabled = false;
        TextActionScore.enabled = false;
        ScoreField.SetActive(false);
        endMsg.SetActive(false);
        
        GlobalVariableStorage.allOut = false;
        fire1.SetActive(false);
        fire2.SetActive(false);
        fire3.SetActive(false);
        fire4 .SetActive(false);
        DialogueBox1.SetActive(false);
        DialogueBox2.SetActive(false);
        DialogueBox3.SetActive(false);
        DialogueBox4.SetActive(false);

    StartCoroutine("countTaskTime");
    }

    //IEnumerator updateLevel()
    //{
    //    float interval = 1f;
    //    for (; ; )
    //    {
    //        yield return new WaitForSeconds(interval);
    //        if (GlobalVariableStorage.timeLeft > 0)//jos aikaa on j�ljell� v�hennet��n intervalli j�ljell� olevasta ajasta
    //        {
    //            GlobalVariableStorage.timeLeft -= interval;
    //        }
    //        else //ei tee toistaiseksi mit��n kun aika loppuu
    //        {
    //            Time.timeScale = 0;
    //        }
    //    }
    //}

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
        // Debug.Log("Display action score");
        TextActionScore.text = "+" + GlobalVariableStorage.actionScore.ToString();
        TextActionScore.enabled = true;
        ;
    }

    
    IEnumerator waitForSecond()
    {
        yield return new WaitForSeconds(3f);
        TextTimeScore.enabled = false;
    }

    void DisplayPoints()
    {
        //Debug.Log("Display points");
        tmpIfPoints.text = GlobalVariableStorage.playerScore.ToString();
    }

    IEnumerator countTaskTime()
    {
        Debug.Log("Task time count started...");
        float interval = 1f;
        //bool continueRunning = true;
        //GlobalVariableStorage.taskTimeLeft = 60; // Initialize taskTimeLeft before entering the loop
        //GlobalVariableStorage.safeTimeLeft = 20;

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
            //else
            //{
            //    Debug.Log("Task Time over");
            //    MoveToNextLevel();
            //    yield return new WaitForSeconds(3f);
            //    GlobalVariableStorage.taskTimeLeft = 60;
            //}

            if (GlobalVariableStorage.level1) 
            {
                if (GlobalVariableStorage.fireIsOut)//(Input.GetKey(KeyCode.P))
                {
                    //GlobalVariableStorage.fireIsOut = true;
                    
                    Debug.Log("Fire is out level 1");
                    Debug.Log("Task time left " + GlobalVariableStorage.taskTimeLeft);
                    DisplayTimeScore();
                    GlobalVariableStorage.playerScore += GlobalVariableStorage.timeScore;
                    GlobalVariableStorage.taskTimeLeft = 0;
                    GlobalVariableStorage.fireIsOut = false;
                    MoveToNextLevel();
                    //break;
                }
                else if (GlobalVariableStorage.taskTimeLeft <= 0 || GlobalVariableStorage.fireIsOutOfControl) // if the time runs out, fire gets out of control
                {
                    Debug.Log("Time up1, Fire should get out of control");
                    FireOutOfControl();
                    fire4.SetActive(true);
                }
            }
            else if (GlobalVariableStorage.level2)
            {
                //GlobalVariableStorage.taskTimeLeft = 60;
                if (GlobalVariableStorage.fireIsOut)//(GlobalVariableStorage.fireIsOut)//
                {
                    //GlobalVariableStorage.fireIsOut = true;
                    Debug.Log("Fire is out level 2");
                    Debug.Log("Task time left " + GlobalVariableStorage.taskTimeLeft);
                    DisplayTimeScore();
                    GlobalVariableStorage.playerScore += GlobalVariableStorage.timeScore;
                    GlobalVariableStorage.taskTimeLeft = 0;
                    MoveToNextLevel();
                    //break;
                }
                else if (GlobalVariableStorage.taskTimeLeft <= 0) // if the time runs out, fire gets out of control
                {
                    Debug.Log("Time up2, Fire should get out of control");
                    FireOutOfControl();
                }
            }
            else if (GlobalVariableStorage.level3)
            {
                //GlobalVariableStorage.taskTimeLeft = 60;
                if (GlobalVariableStorage.lastLevelDone)
                {
                    DisplayTimeScore();
                    GlobalVariableStorage.playerScore += GlobalVariableStorage.timeScore;
                    Debug.Log("Task time left " + GlobalVariableStorage.taskTimeLeft);
                    GlobalVariableStorage.taskTimeLeft = 0;
                    Debug.Log(savePoints);
                    GlobalVariableStorage.level3 = false;
                    ScoreField.SetActive(true);
                    endMsg.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    void MoveToNextLevel()
    {
        // Your logic for moving to the next level goes here
        // For example, you can update level flags and perform other actions
        // You may also want to reset other relevant variables for the next level

        Debug.Log("in move to next level");
        if (GlobalVariableStorage.safeTime)
        {
            GlobalVariableStorage.safeTime = false;
            GlobalVariableStorage.level1 = true;
            GlobalVariableStorage.taskTimeLeft = 60;
            tmpIfTime.enabled = true;
            fire1.SetActive(true);
            DialogueBox1.SetActive(true);
            Debug.Log("to level 1");
        }
        else if (GlobalVariableStorage.level1)
        {
            GlobalVariableStorage.level1 = false;
            GlobalVariableStorage.level2 = true;
            //GlobalVariableStorage.fireIsOut = true;
            GlobalVariableStorage.taskTimeLeft = 60;
            fire2.SetActive(true);
            Debug.Log("to level 2");
            GlobalVariableStorage.scoreElectricityBox = true;
        }
        else if (GlobalVariableStorage.level2)
        {
            //GlobalVariableStorage.level2 = false;
            //GlobalVariableStorage.level3 = true;
            //GlobalVariableStorage.taskTimeLeft = 60;
            FireOutOfControl();
            fire3.SetActive(true);
            Debug.Log("to level 3");
            //ButtonEndGame.SetActive(true); // Call 112 button to be set visible later!
            //GlobalVariableStorage.fireIsOut = true;
        }
        else
        {
            return;
        }
        // Add more conditions as needed for additional levels
    }

    void FireOutOfControl()
    {
        GlobalVariableStorage.level1 = false;
        GlobalVariableStorage.level2 = false;
        GlobalVariableStorage.level3 = true;
        GlobalVariableStorage.fireIsOutOfControl = true;
        GlobalVariableStorage.taskTimeLeft = 60;
        DialogueBox4.SetActive(true);
        // Call 112 button to be set visible later!
        Debug.Log("OMG - to level 3");
    }

    public void Call112()
    {
        Debug.Log("Button clicked");
        GlobalVariableStorage.allOut = true;
        Debug.Log("112 called" +GlobalVariableStorage.allOut);
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalVariableStorage.safeTime == false) {
            DisplayTime(GlobalVariableStorage.taskTimeLeft);
        }
        
        DisplayPoints();
        DisplayActionScore();
    }
}

