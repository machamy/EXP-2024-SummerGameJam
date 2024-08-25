using System;
using TMPro;
using UnityEngine;

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
            rankingText.text = "Top Scores: ";

            for (int i = 0; i < scoreManager.scores.Count; i++)
            {
                rankingText.text += $"{i + 1}. {scoreManager.scores[i]}\n";
            }
        }
        public void OnMainClicked()
        {
            GameManager.Instance.GoMain();
            gameObject.SetActive(false);
        }
    }
}