using System;
using System.Linq;
using TMPro;
using UnityEngine;
using static ScoreManager;

namespace DefaultNamespace.UI
{
    public class RankingScreen : UIScreenBase
    {
        [SerializeField] private TextMeshProUGUI rankingText;
        [SerializeField] private ScoreManager scoreManager;

        public void OnEnable()
        {
            scoreManager.LoadScores();

            OnDifficultySelected(2);

        }

        public void UpdateRankingDisplay(int selectedDifficulty)
        {
            rankingText.text = "";

            var filteredScores = scoreManager.scoreData
                .Where(score => score.difficulty == selectedDifficulty)
                .OrderByDescending(score => score.score) 
                .ToList();

            for (int i = 0; i < filteredScores.Count; i++)
            {
                rankingText.text += $"{i + 1}. Score: {filteredScores[i].score}\n";
            }

            if (filteredScores.Count == 0)
            {
                rankingText.text = "No scores available for this difficulty.";
            }
        }


        public void OnDifficultySelected(int difficulty)
        {
            UpdateRankingDisplay(difficulty);
        }


        public void OnMainClicked()
        {
            GameManager.Instance.GoMain();
            gameObject.SetActive(false);
        }
    }


    /*
    [System.Serializable]
    public class ScoreData
    {
        public int difficulty;
        public int score;

        // Override ToString for proper display
        public override string ToString()
        {
            return $"Difficulty: {difficulty}, Score: {score}";
        }
    }*/
}