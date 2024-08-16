using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;


    public int currentDifficulty { get; private set; }
    public int unlockedDifficulty { get; private set; }
    public int highScore { get; private set; }
    
    public enum GameState
    {
        Main,
        Pause,
        Running
    }

    public GameState State { get; private set; } = GameState.Main;

    public void Awake()
    {
        currentDifficulty = PlayerPrefs.GetInt("c_diff", 0);
        unlockedDifficulty = PlayerPrefs.GetInt("u_diff", 0);
        highScore = PlayerPrefs.GetInt("h_score", 0);
    }

    public void Start()
    {
        InitializeGame();
        
        // Debug.Log(Global.shipDirection);
        // Debug.Log(Global.carDirection);
        // Debug.Log(new Vector2(Mathf.Cos(239/180 * Mathf.PI), Mathf.Sin(239/180 * Mathf.PI)).normalized);
    }

    /// <summary>
    /// 메인 화면으로
    /// </summary>
    public void GoMain()
    {
        State = GameState.Main;
        uiManager.ShowMain();
        InitializeGame();
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        State = GameState.Running;
    }

    /// <summary>
    /// 게임이 진행중일 떄 일시정지
    /// </summary>
    public void PauseGame()
    {
        if (State != GameState.Running)
            return;
        State = GameState.Pause;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 게임 진행으로 돌아감.
    /// </summary>
    public void ResumeGame()
    {
        State = GameState.Running;
        Time.timeScale = 1f;
    }

    public void InitializeGame()
    {
        uiManager.Initialize(currentDifficulty,unlockedDifficulty);
        levelManager.Initialize();
    }
    


    private void Update()
    {
        
    }
}
