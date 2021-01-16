using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorDatabase : ScriptableObject
{
    [SerializeField] private List<int> scores = new List<int>();
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private int membersNeeded = 20;

    private Dictionary<int, Color> scoreToColor;

    public Dictionary<int, Color> ScoreToColor
    {
        get
        {
            if (scoreToColor== null)
            {
                BuildDatabaseInternal();
                return scoreToColor;
            }
            else
            {
                return scoreToColor;
            }
        }
    }

    [ContextMenu("PopulateScoresWithoutColors")]
    public void PopulateScoreListWithoutColors()
    {
        scores.Clear();

        for (int i = 1; i < membersNeeded + 1; i++)
        {
            scores.Add((int)Mathf.Pow(2, i));
        }
    }
    
    [ContextMenu("PopulateScoresWithColors")]
    public void PopulateScoreList()
    {
        scores.Clear();
        colors.Clear();
        
        for (int i = 1; i < membersNeeded + 1; i++)
        {
            scores.Add((int)Mathf.Pow(2, i));
            colors.Add(Color.white);
        }
    }
    
    public void BuildDatabaseInternal()
    {
        scoreToColor = new Dictionary<int, Color>();
        
        scoreToColor.Clear();

        try
        {
            for (var i = 0; i < scores.Count; i++)
            {
                var score = scores[i];
                scoreToColor.Add(score, colors[i]);
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogWarning("Could not build database, probably scores and colors length does not match");
        } 

    }
    
    [ContextMenu("BuildDatabase")]
    public void BuildDatabase()
    {
        scoreToColor = new Dictionary<int, Color>();
        
        scoreToColor.Clear();

        try
        {
            for (var i = 0; i < scores.Count; i++)
            {
                var score = scores[i];
                scoreToColor.Add(score, colors[i]);
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogWarning("Could not build database, probably scores and colors length does not match");
        } 

    }
    [ContextMenu("DisplayDatabase")]
    public void DisplayDatabases()
    {
        foreach (var scoreToColor in scoreToColor)
        {
            Debug.Log(scoreToColor.Key  + " " + scoreToColor.Value);
        }
    }
}


