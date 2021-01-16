using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text text;
    [SerializeField] private SharedInt score;

    private void Start()
    {
        score.Value = 0;
        score.valueChangeEvent.AddListener(UpdateText);
    }

    private void UpdateText()
    {
        text.text = score.Value.ToString();
    }
    
}
