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
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private HPIndicator hpIndicator;
    // [SerializeField] private Image FadeImg;
    [Header("ScriptableObjects")]
    [SerializeField] private IntVariableSO Score;
    [SerializeField] private IntVariableSO Hp;
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
        Hp.Value = 0;
        Main.GetComponent<MainScreen>().UpdateSprites(currentDifficulty,unlockDifficulty);
    }

    public void ShowPause()
    {
        Setting.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void ShowSetting()
    {
        Setting.SetActive(true);
    }

    public void ShowGame()
    {
        scoreTMP.gameObject.SetActive(true);
        hpIndicator.gameObject.SetActive(true);
        if(GameManager.Instance.State == GameManager.GameState.Pause)
            GameManager.Instance.ResumeGame();
    }

    public void ShowMain()
    {
        Main.SetActive(true);

    }

    // private IEnumerator ShowGameRoutine(GameObject gameObject)
    // {
    //     
    // }
    

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
    private void ChangeScore(int n) => scoreTMP.text = $"{Score.Value}";

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
