using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RankingSystem/ScoreManager")]
public class ScoreManager : ScriptableObject
{

    [System.Serializable]
    public class ScoreData
    {
        public int difficulty;
        public int score;
    }


   [System.Serializable]
    private class ScoreDataList
    {
        public List<ScoreData> scoreDataList = new List<ScoreData>();
    }

    public List<ScoreData> scoreData = new List<ScoreData>();

    public int maxScores = 50; // Maximum number of scores to keep
    private string filePath; // Path to the JSON file

    private void OnEnable()
    {
        filePath = Path.Combine(Application.persistentDataPath, "scores.json");
        Debug.Log($"Scores will be saved at: {filePath}"); // Log the file path to the console
        LoadScores(); // Load scores when the game starts
    }

    public void AddScore(int difficulty, int score)
    {
        scoreData.Add(new ScoreData { difficulty = difficulty, score = score });

        // Sort scores in descending order
        scoreData.Sort((a, b) => b.score.CompareTo(a.score));

        // Trim the list to only keep the top `maxScores`
        if (scoreData.Count > maxScores)
        {
            scoreData.RemoveRange(maxScores, scoreData.Count - maxScores);
        }

        SaveScores(); // Save scores to JSON file
    }



    public void SaveScores()
    {
        ScoreDataList scoreDataList = new ScoreDataList { scoreDataList = scoreData };
        string json = JsonUtility.ToJson(scoreDataList);
        File.WriteAllText(filePath, json);
    }

    public void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            ScoreDataList loadedData = JsonUtility.FromJson<ScoreDataList>(json);

            scoreData = loadedData.scoreDataList;

        }
        else
        {
            Debug.LogWarning("Score file not found. Starting with an empty score list.");
        }
    }
}