using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("UIElements")]
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private HPIndicator hpIndicator;
    // [SerializeField] private Image FadeImg;
    [Header("ScriptableObjects")]
    [SerializeField] private IntVariableSO Score;
    [SerializeField] private IntVariableSO Hp;
    [Header("Settings")]
    [SerializeField] private float uiTransitionSec = 0.25f;
    [SerializeField] private float uiTransitionDistance = 5f;
    private void Awake()
    {
        Score.Value = 0;
        Hp.Value = 3;
    }

    public void ShowPause()
    {
        
    }

    public void ShowGame()
    {
        scoreTMP.gameObject.SetActive(true);
        hpIndicator.gameObject.SetActive(true);
    }

    public void ShowMain()
    {
        //TODO 애니메이션
        
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
    private void ChangeScore(int n) => scoreTMP.text = "Score : "+Score.Value;

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
