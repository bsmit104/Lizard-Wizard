using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    private float currentScore = 0f;
    private float highScore;

    [SerializeField] private TMP_Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetScore() {
        currentScore = 0f;
    }

    public float GetCurrentScore()
    {
        return currentScore;
    }

    public float GetHighScore()
    {
        return highScore;
    }

    public void AddScore(float score)
    {
        currentScore += score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (ScoreText != null)
        {
            ScoreText.SetText("Score: " + currentScore.ToString());
        }
    }
}
