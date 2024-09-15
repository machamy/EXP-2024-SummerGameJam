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
        [SerializeField] private TextMeshProUGUI rankTexts1;
        [SerializeField] private TextMeshProUGUI rankTexts2;
        [SerializeField] private TextMeshProUGUI rankTexts3;
        [SerializeField] private TextMeshProUGUI rankTexts4;
        [SerializeField] private TextMeshProUGUI rankTexts5;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private Image[] trophyImages;

        [SerializeField] private GameObject levelBox;
        [SerializeField] private Button levelButton;
        [SerializeField] private int sellectedDifficulty = 0;

        public void OnEnable()
        {
            scoreManager.LoadScores();
            OnDifficultySelected(sellectedDifficulty);
        }

        /*  public void UpdateRankingDisplay(int selectedDifficulty)
          {
              rankTexts1.text = "";

              var filteredScores = scoreManager.scoreData
                  .Where(score => score.difficulty == selectedDifficulty)
                  .OrderByDescending(score => score.score) 
                  .ToList();

              for (int i = 0; i < filteredScores.Count; i++)
              {
                  rankTexts1.text += $"{i + 1}.{filteredScores[i].score}\n";
              }

              if (filteredScores.Count == 0)
              {
                  rankTexts1.text = " - ";
              }
          }*/

        public void UpdateRankingDisplay(int selectedDifficulty)
        {

            var filteredScores = scoreManager.scoreData
                .Where(score => score.difficulty == selectedDifficulty)
                .OrderByDescending(score => score.score)
                .ToList();


            rankTexts1.text = filteredScores.Count > 0 ? filteredScores[0].score.ToString() : "-";
            rankTexts2.text = filteredScores.Count > 1 ? filteredScores[1].score.ToString() : "-";
            rankTexts3.text = filteredScores.Count > 2 ? filteredScores[2].score.ToString() : "-";
            rankTexts4.text = filteredScores.Count > 3 ? filteredScores[3].score.ToString() : "-";
            rankTexts5.text = filteredScores.Count > 4 ? filteredScores[4].score.ToString() : "-";


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
            uiManager.lastUI = uiManager.Main.GetComponent<UIScreenBase>();
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