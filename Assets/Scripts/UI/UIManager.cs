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
    [SerializeField] private TextMeshProUGUI hpTMP;
    // [SerializeField] private Image FadeImg;
    [Header("ScriptableObjects")]
    [SerializeField] private IntVariableSO Score;
    [SerializeField] private IntVariableSO Hp;

    private void Awake()
    {
        Score.Value = 0;
        Hp.Value = 3;
    }


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
    }

    private void OnDisable()
    {
        Score.ValueChangeEvent.RemoveListener(ChangeScore);
    }
}
