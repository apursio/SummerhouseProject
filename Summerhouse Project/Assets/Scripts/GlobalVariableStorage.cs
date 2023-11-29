using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableStorage : MonoBehaviour
{
    public static int timeScore; // ajasta tulevat pisteet
    public static int actionScore; //toiminnoista tulevat pisteet > n‰ytet‰‰n pelaajalle valintoja tehdess‰
    public static int totalTimeScore;
    public static int totalActionScore;
    public static int playerScore; // pisteet yhteens‰
    public static bool fireIsOut;
    public static float initialTime;
    public static float taskTime;
    public static float timeLeft;
    public static float taskTimeLeft;
    public static bool safeTime;
    public static bool level1;
    public static bool level2;
    public static bool level3;


    // Start is called before the first frame update
    void Start()
    {

    }
}


