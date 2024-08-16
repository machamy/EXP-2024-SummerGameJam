using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float weight = 100f;
    [SerializeField] private IntVariableSO score;

    public void Initialize()
    {
        weight = 100f;
    }
    private void CheckScore(int n)
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
