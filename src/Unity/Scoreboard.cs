using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    private List<GameObject> scoreEntries = new();
    private GameObject scoreEntryPrefab;
    private Transform container;
    
    public void Initialize(GameObject entryPrefab, List<ScoreManager.ScoreData> scores)
    {
        scoreEntryPrefab = entryPrefab;
        container = transform;

        UpdateEntries(scores);
    }
    
    public void UpdateEntries(List<ScoreManager.ScoreData> scores)
    {
        scores.Sort();

        // Ensure enough entries exist
        while (scoreEntries.Count < scores.Count)
        {
            GameObject newEntry = Instantiate(scoreEntryPrefab, container);
            scoreEntries.Add(newEntry);
        }
        
        for (int i = 0; i < scores.Count; i++)
        {
            GameObject entry = scoreEntries[i];
            entry.SetActive(true);

            TextMeshProUGUI textComponent = entry.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"{scores[i].name}: {scores[i].score}";
                textComponent.color = scores[i].color;
            }
        }
        
        for (int i = scores.Count; i < scoreEntries.Count; i++)
        {
            scoreEntries[i].SetActive(false);
        }
    }
    
    public void Resize(float width, float fontSize)
    {
        foreach (GameObject entry in scoreEntries)
        {
            if (entry.activeSelf)
            {
                RectTransform rectTransform = entry.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
                }

                TextMeshProUGUI textComponent = entry.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.fontSize = fontSize;
                }
            }
        }
    }
}
