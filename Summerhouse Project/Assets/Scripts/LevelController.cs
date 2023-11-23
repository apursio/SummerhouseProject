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
    public float initialTime;
    public float taskTime;
    public int timeScore; // ajasta tulevat pisteet
    public int actionScore; //toiminnoista tulevat pisteet > näytetään pelaajalle valintoja tehdessä
    public int totalTimeScore;
    public int totalActionScore;
    public int playerScore; // pisteet yhteensä
    public bool fireIsOut;
    private float timeLeft;
    private float taskTimeLeft;


    // Start is called before the first frame update

    void Start()
    {
        
        Time.timeScale = 1;
        timeLeft = initialTime;
        taskTimeLeft = taskTime;
        fireIsOut = true;
        actionScore = 0;
        totalActionScore = 0;
        playerScore = 0;
        StartCoroutine("updateLevel");


    }
    IEnumerator updateLevel()
    {
        float interval = 1f;
        for (; ; )
        {
            yield return new WaitForSeconds(interval);
            if (timeLeft > 0)//jos aikaa on jäljellä vähennetään intervalli jäljellä olevasta ajasta
            {
                timeLeft -= interval;
                if (timeLeft == 180)
                {
                    fireIsOut = false;
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
        timeScore = (int) taskTimeLeft*10; 
        tmpIfTimeScore.text = "+" + timeScore.ToString();
    }

    void DisplayPoints()
    {
        //Debug.Log("Display points");
        playerScore = totalTimeScore + totalActionScore;
        tmpIfPoints.text = playerScore.ToString();
    }

    IEnumerator countTaskTime()
    {
        Debug.Log("Task time count started...");
        float interval = 1f;
        taskTimeLeft = 60; // Initialize taskTimeLeft before entering the loop

        for (; ; )
        {
            yield return new WaitForSeconds(interval);
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("P key pressed");
                fireIsOut = true;
                Debug.Log("Fire is out");
                Debug.Log("Task time left " +taskTimeLeft);
                DisplayTimeScore();
                totalTimeScore = totalTimeScore + timeScore;
                break;
            }

            if (taskTimeLeft > 0)
            {
                taskTimeLeft -= interval;
            }
            else
            {
                Debug.Log("Task Time over");
            }
        }
    }



    public void putOutFire()
    {
        fireIsOut = false;
        Debug.Log("fire out");
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime(timeLeft);
        DisplayPoints();
    }
}
