using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using System;

public class LevelController : MonoBehaviour
{
    public TMP_InputField tmpIfTime;
    public TMP_InputField tmpIfPoints;
    public TMP_InputField tmpIfTimeScore;
    public TMP_InputField tmpIfActionScore;
    public float initialTime;
    public float taskTime;

    // Start is called before the first frame update

    void Start()
    {
        
        Time.timeScale = 1;
        GlobalVariableStorage.timeLeft = initialTime;
        GlobalVariableStorage.taskTimeLeft = taskTime;
        GlobalVariableStorage.fireIsOut = true;
        GlobalVariableStorage.actionScore = 0;
        GlobalVariableStorage.totalActionScore = 0;
        GlobalVariableStorage.playerScore = 0;
        StartCoroutine("updateLevel");


    }
    IEnumerator updateLevel()
    {
        float interval = 1f;
        for (; ; )
        {
            yield return new WaitForSeconds(interval);
            if (GlobalVariableStorage.timeLeft > 0)//jos aikaa on jäljellä vähennetään intervalli jäljellä olevasta ajasta
            {
                GlobalVariableStorage.timeLeft -= interval;
                if (GlobalVariableStorage.timeLeft == 180)
                {
                    GlobalVariableStorage.fireIsOut = false;
                    Debug.Log("Fire started");
                    StartCoroutine("countTaskTime");
                }
            }
            else //ei tee toistaiseksi mitään kun aika loppuu
            {
                Time.timeScale = 0;
            }
        }
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
        tmpIfTimeScore.text = "+" + GlobalVariableStorage.timeScore.ToString();

    }

    void DisplayActionScore()
    {
        // Debug.Log("Display action score");
        tmpIfActionScore.text = "+" + GlobalVariableStorage.actionScore.ToString();

    }

    IEnumerator waitForSecond()
    {
        yield return new WaitForSeconds(3f);
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
        GlobalVariableStorage.taskTimeLeft = 60; // Initialize taskTimeLeft before entering the loop

        for (; ; )
        {
            yield return new WaitForSeconds(interval);
            if (Input.GetKey(KeyCode.P))
            {
                Debug.Log("P key pressed");
                GlobalVariableStorage.fireIsOut = true;
                Debug.Log("Fire is out");
                Debug.Log("Task time left " +GlobalVariableStorage.taskTimeLeft);
                DisplayTimeScore();
                GlobalVariableStorage.playerScore = GlobalVariableStorage.playerScore + GlobalVariableStorage.timeScore;
                break;
            }
            

            if (GlobalVariableStorage.taskTimeLeft > 0)
            {
                GlobalVariableStorage.taskTimeLeft -= interval;
            }
            else
            {
                Debug.Log("Task Time over");
            }
        }
    }



    public void putOutFire()
    {
        GlobalVariableStorage.fireIsOut = false;
        Debug.Log("fire out");
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime(GlobalVariableStorage.timeLeft);
        DisplayPoints();
        DisplayActionScore();
    }
}
