using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableStorage : MonoBehaviour
{
    public static int timeScore; // ajasta tulevat pisteet
    public static int actionScore; //toiminnoista tulevat pisteet > näytetään pelaajalle valintoja tehdessä
    public static int availableScore;
    public static int totalTimeScore;
    public static int totalActionScore;
    public static int playerScore; // pisteet yhteensä
    public static bool fireIsOut;
    public static bool fireIsOutOfControl;
    public static float initialTime;
    public static float taskTime;
    public static float timeLeft;
    public static float taskTimeLeft;
    public static float safeTimeLeft;
    public static bool safeTime;
    public static bool level1;
    public static bool level2;
    public static bool level3;
    public static bool scoreElectricityBox; // sähkökaapista pisteet vain oikeaan aikaan ja kerran
    public static bool lastLevelDone; // level3 juttuja
    public static bool allOut;
    public static bool isKnobTurned;


    // Start is called before the first frame update
    void Start()
    {

    }
}


