using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static ScoreManager;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace DefaultNamespace.UI
{
    public class RankingScreen : UIScreenBase
    {
        [SerializeField] private MainScreen _mainScreen;
        [SerializeField] private TextMeshProUGUI rankingText;
        [SerializeField] private ScoreManager scoreManager;
        
        [SerializeField] private GameObject levelBox;
        [SerializeField] private Button levelButton;
        [SerializeField] private int sellectedDifficulty = 0;

        public void OnEnable()
        {
            scoreManager.LoadScores();
            OnDifficultySelected(sellectedDifficulty);
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
            levelBox.SetActive(false);

            var ss = levelButton.spriteState;
            ss.selectedSprite = _mainScreen.levelSprites[difficulty];
            ss.pressedSprite = _mainScreen.levelPressedSprites[difficulty];
            levelButton.spriteState = ss;
            levelButton.GetComponent<Image>().sprite = ss.selectedSprite;
            
        }

        public void OnDifficultyButtonClicked()
        {
            SoundManager.Instance.Play("button2");
            levelBox.SetActive(!levelBox.activeSelf);
            
        }

        public override void OnBackClicked()
        {
            OnMainClicked();
        }

        public void OnMainClicked()
        {
            SoundManager.Instance.Play("button2");
            // GameManager.Instance.GoMain();
            levelBox.SetActive(false);
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