using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    public TMP_InputField tmpIfTime;
    public TMP_InputField tmpIfPoints;
    public float initialTime;
    public float timeScore; // ajasta tulevat pisteet
    public float actionScore; //toiminnoista tulevat pisteet > n‰ytet‰‰n pelaajalle valintoja tehdess‰
    public float playerScore; // pisteet yhteens‰
    private float timeLeft;
   

    // Start is called before the first frame update

    void Start()
    {
        timeLeft = initialTime;
        StartCoroutine("updateLevel");
    }
    IEnumerator updateLevel()
    {
        float interval = 1f;
        for(; ; )
        {
            yield return new WaitForSeconds(interval);
            if (timeLeft > 0)//jos aikaa on j‰ljell‰ v‰hennet‰‰n intervalli j‰ljell‰ olevasta ajasta
            {
                timeLeft -= interval;
            }
            else //ei tee toistaiseksi mit‰‰n kun aika loppuu
            { }    
        }
    }

    void DisplayTime (float timeToDisplay)
    {
        timeToDisplay += 0;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        tmpIfTime.text = string.Format("{0:00} : {1:00}", minutes, seconds); //muotoilee ajan minuuteiksi ja sekunneiksi
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime(timeLeft);
    }
}
