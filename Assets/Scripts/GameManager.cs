using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    public enum GameState
    {
        Main,
        Pause,
        Running
    }
    
    public GameState State { get; private set; }

    public void Start()
    {
        GoMain();
    }

    public void GoMain()
    {
        State = GameState.Main;
        uiManager.ShowMain();
    }

    public void StartGame()
    {
        
    }


    private void Update()
    {
        
    }
}
