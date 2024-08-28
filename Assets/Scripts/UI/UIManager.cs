using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("Main")] 
    [SerializeField] private GameObject Main;
    [Header("Game")]
    [SerializeField] private GameObject Game;
    [Header("Setting")]
    [SerializeField] private GameObject Setting;

    [Header("result")] [SerializeField] private GameObject Result;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private HPIndicator hpIndicator;
    // [SerializeField] private Image FadeImg;

    [Header("Ranking")][SerializeField] private GameObject Ranking;
    [SerializeField] private TextMeshProUGUI rankingText;


    [Header("ScriptableObjects")]
    [SerializeField] private IntVariableSO Score;
    [SerializeField] private IntVariableSO Hp;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Settings")]
    [SerializeField] private float uiTransitionSec = 0.25f;
    [SerializeField] private float uiTransitionDistance = 5f;
    [Header("Difficulties")] private Sprite[] difficultySprites;
    private void Awake()
    {
        Score.Value = 0;
        Hp.Value = 3;
    }

    public void Initialize(int currentDifficulty, int unlockDifficulty)
    {
        Score.Value = 0;
        Hp.Value = 3;
        Main.GetComponent<MainScreen>().UpdateSprites(currentDifficulty,unlockDifficulty);
    }

    public void ShowPause()
    {
        SoundManager.Instance.Play("button2");
        Setting.SetActive(true);
        Setting.GetComponent<SettingScreen>().isRunning = true;
        GameManager.Instance.PauseGame();
    }

    public void ShowSetting()
    {
        Setting.SetActive(true);
    }

    public void ShowGame()
    {
        Main.SetActive(false);
        Game.SetActive(true);
        Setting.SetActive(false);
        if(GameManager.Instance.State == GameManager.GameState.Pause)
            GameManager.Instance.ResumeGame();
    }

    public void ShowMain()
    {
        Result.SetActive(false);
        Game.SetActive(false);
        Setting.SetActive(false);
        Main.SetActive(true);

    }

    public void ShowResult()
    {
        Result.SetActive(true);
    }

    // private IEnumerator ShowGameRoutine(GameObject gameObject)
    // {
    //     
    // }

    public void ShowRanking()
    {
        Ranking.SetActive(true);
    }

    public void HideRanking()
    { Ranking.SetActive(false); }
    

    // private IEnumerator FadeRoutine(float time, float from, float to, int frame = 32)
    // {
    //     float current = from;
    //     // Color color = FadeImg.
    //     while (current < to)
    //     {
    //
    //         yield return null;
    //     }
    // }
    //
    private void ChangeScore(int n) => scoreTMP.text = $"{n}";

    private void OnEnable()
    {
        Score.ValueChangeEvent.AddListener(ChangeScore);
        Hp.ValueChangeEvent.AddListener(hpIndicator.SetHP);
    }

    private void OnDisable()
    {
        Score.ValueChangeEvent.RemoveListener(ChangeScore);
        Hp.ValueChangeEvent.RemoveListener(hpIndicator.SetHP);
    }
}
