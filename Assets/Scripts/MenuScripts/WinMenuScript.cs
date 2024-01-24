using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuScript : MonoBehaviour
{

    [Header("References")]
    private TMP_Text grade;
    private TMP_Text scoreTMP;
    private int score;

    private void Start()
    {
        grade = GameObject.Find("Grading").GetComponent<TMP_Text>();
        scoreTMP = GameObject.Find("Score").GetComponent<TMP_Text>();
        score = PlayerPrefs.GetInt("Player Score"); // Player prefs is a quick way to do this. Better method would be a manager script that doesn't destroy, or saving to manual txt

        scoreTMP.text = "Score: " + score;

        if (score < 3000) // Anything below this is an F
        {
            grade.text = "Grade: F";
        }
        else if (score < 3500)
        {
            grade.text = "Grade: E";
        }
        else if (score < 4000)
        {
            grade.text = "Grade: D";
        }
        else if (score < 4500)
        {
            grade.text = "Grade: C";
        }
        else if (score < 5000)
        {
            grade.text = "Grade: B";
        }
        else if (score < 6000)
        {
            grade.text = "Grade: A";
        }
        else
        {
            grade.text = "Grade: S";
        }
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Debug.Log("Player quit");
        Application.Quit();
    }

}
