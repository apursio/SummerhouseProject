using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safezone : MonoBehaviour
{
    public Transform targetArea; // Reference to the target area.
    public float detectionRadius = 8.0f; // Radius to detect if an item is near the target area
    private Dictionary<string, int> savedItemPoints = new Dictionary<string, int>();
    private static bool scoringProcessed;

    private void Start()
    {
        scoringProcessed = false;
        savedItemPoints.Add("granny", 500);
        savedItemPoints.Add("animal", 150);
        savedItemPoints.Add("badchoice", -50);
    }

    void Update()
    {
        if (GlobalVariableStorage.level3)
        {
            if (!scoringProcessed && Input.GetKeyDown(KeyCode.R) || !scoringProcessed && GlobalVariableStorage.taskTimeLeft <= 0)
            {
                scoringProcessed = true;
                Dictionary<string, int> itemCounts = new Dictionary<string, int>();
                int pointsCombined = 0;
                bool grannyPresent = false; // to check if granny is saved

                Collider[] colliders = Physics.OverlapSphere(targetArea.position, detectionRadius);
                foreach (Collider collider in colliders)
                {
                    string itemTag = collider.tag;

                    // Ignore wrong colliders
                    if (!savedItemPoints.ContainsKey(itemTag))
                        continue;

                    // Count occurrences of each saveable item type
                    if (itemCounts.ContainsKey(itemTag))
                    {
                        itemCounts[itemTag]++;
                    }
                    else
                    {
                        itemCounts.Add(itemTag, 1);
                    }

                    if (itemTag == "granny")
                    {
                        grannyPresent = true;
                    }
                }

                if (!grannyPresent)
                {
                    pointsCombined += -savedItemPoints["granny"];
                }

                // Add points based on the counts
                foreach (var kvp in itemCounts)
                {
                    string itemTag = kvp.Key;
                    int count = kvp.Value;
                    int points = savedItemPoints[itemTag] * count;
                    pointsCombined += points;
                }
                GlobalVariableStorage.actionScore = pointsCombined;
                GlobalVariableStorage.playerScore += GlobalVariableStorage.actionScore;
                GlobalVariableStorage.lastLevelDone = true;
            }
        }
    }
}
