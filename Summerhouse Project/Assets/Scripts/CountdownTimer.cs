using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 180f; // 3 minutes

    void Update()
    {
        Debug.Log("Update method called.");
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
            Debug.Log("Time Remaining: " + timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            // Handle timer reaching zero -> end the game
            Debug.Log("Timer reached zero.");
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Assuming you have a TextMeshProUGUI component attached to the timerText variable
        timerText.text = timerString;
    }

}
