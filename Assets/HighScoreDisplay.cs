using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private SharedInt score;
    [SerializeField] private HighScore highScore;
    [SerializeField] private TMP_Text text;
    private void Start()
    {
        text.text = highScore.MaxScore.ToString();
        score.valueChangeEvent.AddListener(CheckMaxScore);
    }

    private void CheckMaxScore()
    {
        text.text = highScore.MaxScore.ToString();
    }
}
