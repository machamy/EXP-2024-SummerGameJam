using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BGMManager bgmManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DefaultNamespace.UI.RankingScreen rankingScreen; 


    public IntVariableSO hp;
    public IntVariableSO score;

    public int currentDifficulty { get; set; }
    public int unlockedDifficulty { get; set; }
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
        Instance = this;
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
        bgmManager.RunTitleMusic(); // 상태 변경에 따른 음악 실행
        uiManager.ShowMain();
        InitializeGame();
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        State = GameState.Running;
        bgmManager.RunPlayMusic(); // 상태 변경에 따른 음악 실행
        Time.timeScale = 1.0f;
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


    public void GameOver()
    {
        PauseGame();

        scoreManager.AddScore(score.Value);

        uiManager.ShowResult();
        rankingScreen.UpdateRankingDisplay();
        score.Value = 0;



    }

    public void ShowRankingScreen()
    {
        uiManager.ShowRanking();
    }

    public void HideRankingScreen()
    {
        uiManager.HideRanking();
    }

    private void Update()
    {
        
    }
}
