using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockHolder : MonoBehaviour
{
    private int _value = 0;
    private TMP_Text text;
    [SerializeField] private Image image;
    [SerializeField] private ColorDatabase colorDatabase;
    public int Value
    {
        get => _value;
        set { _value = value;
            text.text = _value > 0 ? _value.ToString() : "";
            image.color = _value > 0 ? colorDatabase.ScoreToColor[_value] : Color.white;
        }
    }

    private void OnEnable()
    {
        text = GetComponentInChildren<TMP_Text>();
        Value = 0;
    }
}
