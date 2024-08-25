using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "RankingSystem/ScoreManager")]
public class ScoreManager : ScriptableObject
{
    public List<int> scores = new List<int>(); // A list to store scores
    public int maxScores = 100; // Maximum number of scores to keep
    private string filePath; // Path to the JSON file

    private void OnEnable()
    {
        filePath = Path.Combine(Application.persistentDataPath, "scores.json");
        Debug.Log($"Scores will be saved at: {filePath}"); // Log the file path to the console
        LoadScores(); // Load scores when the game starts
    }

    public void AddScore(int score)
    {
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); // Sort scores in descending order

        // Keep only the top scores
        if (scores.Count > maxScores)
        {
            scores.RemoveAt(scores.Count - 1);
        }

        SaveScores(); // Save scores to JSON file
    }

    public int GetBestScore()
    {
        if (scores.Count > 0)
        {
            return scores[0];
        }
        return 0;
    }

    public void SaveScores()
    {
        string json = JsonUtility.ToJson(new ScoreData(scores), true); // Convert the scores list to JSON
        File.WriteAllText(filePath, json); // Write the JSON to the file
    }

    public void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); // Read the JSON from the file
            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json); // Convert the JSON back to ScoreData
            scores = scoreData.scores; // Update the scores list
        }
        else
        {
            scores = new List<int>(); // Initialize an empty list if the file does not exist
        }
    }

    [System.Serializable]
    private class ScoreData
    {
        public List<int> scores;

        public ScoreData(List<int> scores)
        {
            this.scores = scores;
        }
    }
}