using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<ScoreData> playerScores = new();
    public GameObject scoreboardPrefab;
    public GameObject scoreEntryPrefab;
    private List<GameObject> activeScoreboards = new(); 

    [Serializable]
    public class ScoreData : IComparable<ScoreData>
    {
        public int score;
        public string name;
        public Color color;

        public ScoreData(int score, string name, Color color)
        {
            this.score = score;
            this.name = name;
            this.color = color;
        }

        public void setScore(int newScore)
        {
            score = newScore;
        }

        public int CompareTo(ScoreData other)
        {
            return other == null ? 1 : other.score.CompareTo(this.score);
        }
    }
    
    public GameObject CreateScoreboard(Vector2 position, Vector2 margin, Transform parent = null)
    {
        GameObject newScoreboard = Instantiate(scoreboardPrefab, new Vector3(0, 0, 0), Quaternion.identity, parent);
    
        RectTransform rectTransform = newScoreboard.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(position.x, position.y); 
            rectTransform.anchorMax = new Vector2(position.x, position.y);
            rectTransform.pivot = new Vector2(position.x, position.y); 
            // the object is created at z = -4000 for no reason whatsoever
            rectTransform.anchoredPosition = new Vector3(margin.x, margin.y);
            rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y, 1);
        }

        ScoreBoard scoreBoardScript = newScoreboard.GetComponent<ScoreBoard>();
        if (scoreBoardScript != null)
        {
            scoreBoardScript.Initialize(scoreEntryPrefab, playerScores);
            activeScoreboards.Add(newScoreboard);
        }

        if (parent != null)
        {
            newScoreboard.transform.SetSiblingIndex(0);
        }

        return newScoreboard;
    }
    
    public void UpdateAllScoreboards()
    {
        foreach (GameObject scoreboard in activeScoreboards)
        {
            ScoreBoard scoreBoardScript = scoreboard.GetComponent<ScoreBoard>();
            if (scoreBoardScript != null)
            {
                scoreBoardScript.UpdateEntries(playerScores);
            }
        }
    }
    
    public void ClearAllScoreboards()
    {
        foreach (GameObject scoreboard in activeScoreboards)
        {
            Destroy(scoreboard);
        }
        activeScoreboards.Clear();
    }
}
