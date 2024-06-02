using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private float currentScore = 0f;
    private float highScore;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetCurrentScore() {
        return currentScore;
    }

    public float GetHighScore() {
        return highScore;
    }

    public void AddScore(float score) {
        currentScore += score;
    }
}
