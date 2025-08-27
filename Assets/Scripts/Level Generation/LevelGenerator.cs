using System.Collections.Generic;
using UnityEngine;
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] 
    private List<Transform> levelParts; // Collection of prefabs that can be used as level parts

    [SerializeField] 
    private SnapPoint currentExitSnapPoint; // Reference to the last exit point where the next level part should connect

    private void Start()
    {
        GenerateNextLevelPart();
    }

    [ContextMenu("Generate Next Level Part")]
    private void GenerateNextLevelPart()
    {
        Transform nextLevelPart = Instantiate(GetRandomLevelPart());
        LevelPart levelPartScript = nextLevelPart.GetComponent<LevelPart>();

        // Connect the new part to the current exit point and update the exit point
        levelPartScript.SnapAndAlignLevelPart(currentExitSnapPoint);
        currentExitSnapPoint = levelPartScript.GetExitSnapPoint();
    }

    private Transform GetRandomLevelPart()
    {
        int randomIndex = Random.Range(0, levelParts.Count);
        Transform nextLevelPart = levelParts[randomIndex];
        levelParts.RemoveAt(randomIndex); // Remove used part to prevent repetition
        return nextLevelPart;
    }
}
