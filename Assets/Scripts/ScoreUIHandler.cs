using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIHandler : MonoBehaviour
{
    [Header("Config")]
    private int currentScore = 0;
    private int targetScore = 0;
    private float updateSpeed = 5.0f;
    private bool isUpdatingScore = false;

    [Header("References")]
    private TextMeshProUGUI tmp;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int scoreToAdd)
    {
        targetScore += scoreToAdd; // First updates the target score with gained score

        if (!isUpdatingScore) // If already updating, it should still reach targetScore
        {
            StartCoroutine(UpdateScoreCoroutine());
        }
    }

    private IEnumerator UpdateScoreCoroutine()
    {
        isUpdatingScore = true;

        while (currentScore < targetScore)
        {
            currentScore = Mathf.Min(targetScore, currentScore + Mathf.CeilToInt(updateSpeed * Time.deltaTime)); // Returns smallest number out of target and an increased currentScore
            tmp.text = ("SCORE: " + currentScore);

            yield return null;
        }

        currentScore = targetScore; // currentScore likely went over target, so I set it manually

        isUpdatingScore = false;
    }
}
