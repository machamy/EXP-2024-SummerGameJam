using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class ScoreScreen:UIScreenBase
    {
        [SerializeField] private IntVariableSO score;
        [SerializeField] private TextMeshProUGUI scoreTMP;

        public void OnEnable()
        {
            scoreTMP.text = $"{score.Value}";
        }

        public void OnRestartClicked()
        {
            GameManager.Instance.InitializeGame();
            GameManager.Instance.StartGame();
            gameObject.SetActive(false);
        }

        public void OnMainClicked()
        {
            GameManager.Instance.GoMain();
            gameObject.SetActive(false);
        }
        
    }
}