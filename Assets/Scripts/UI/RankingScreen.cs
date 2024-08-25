using System;
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
            UpdateRankingDisplay();

        }

        public void UpdateRankingDisplay()
        {
            rankingText.text = "";
            int i = 0;
            foreach (var score in scoreManager.scoreData)
            {
             
                rankingText.text += $"{i+1}  Score: {score.score}\n";
                i++;
            }
            
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