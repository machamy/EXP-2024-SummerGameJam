using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Database;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Database DB;
    
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BGMManager bgmManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DefaultNamespace.UI.RankingScreen rankingScreen;

    [SerializeField] private BridgeController bridge;

    public IntVariableSO hp;
    public IntVariableSO score;

    public float gravity = 3;
    public float waterGravity = 1;
    public static float Gravity => Instance.gravity;
    public static float WaterGravity => Instance.waterGravity;
    [Header("Debug")] public bool invincible = false;
    [field:SerializeField] public int currentDifficulty { get; set; }
    [field:SerializeField]public int unlockedDifficulty { get; set; }
    // public int highScore { get; private set; }

    public static float TimeStamp => Instance.levelManager.timestamp;
    public enum GameState
    {
        Main,
        Pause,
        Running,
        Score
    }

    public GameState State { get; private set; } = GameState.Main;

    public void Awake()
    {
        Instance = this; 
        PlayerPrefs.SetInt("u_diff", 2);
        currentDifficulty = PlayerPrefs.GetInt("c_diff", 0);
        unlockedDifficulty = PlayerPrefs.GetInt("u_diff", 2);
        // highScore = PlayerPrefs.GetInt("h_score", 0);
    }


    public void Start()
    {
        StartCoroutine(ReadyRoutine());

        // Debug.Log(Global.shipDirection);
        // Debug.Log(Global.carDirection);
        // Debug.Log(new Vector2(Mathf.Cos(239/180 * Mathf.PI), Mathf.Sin(239/180 * Mathf.PI)).normalized);
    }

    private IEnumerator ReadyRoutine()
    {
        levelManager.gameObject.SetActive(false);
        uiManager.Main.GetComponent<MainScreen>().startButton.interactable = false;
        yield return new WaitUntil(()=>DB.isReady);
        levelManager.gameObject.SetActive(true);
        uiManager.Main.GetComponent<MainScreen>().startButton.interactable = true;
        InitializeGame();
        print($"current difficulty : {currentDifficulty}");
        levelManager.Initialize(3);
    }

    /// <summary>
    /// 메인 화면으로
    /// </summary>
    public void GoMain()
    {
        State = GameState.Main;
        bgmManager.RunTitleMusic(); // 상태 변경에 따른 음악 실행
        uiManager.ShowMain();
        Time.timeScale = 1.0f;
        InitializeGame();
        levelManager.Initialize(3);
        bridge.autoplayEnabled = true;
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        State = GameState.Running;
        bgmManager.RunPlayMusic(); // 상태 변경에 따른 음악 실행
        hp.Value = 3;
        levelManager.Initialize(currentDifficulty);
        Time.timeScale = 1.0f;
        bridge.autoplayEnabled = false;
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
        var sm = SoundManager.Instance;
        var bgmVol = PlayerPrefs.GetFloat("bgm", 0.5f);
        var sfxVol = PlayerPrefs.GetFloat("sfx", 0.5f);
        sm.ChangeVolumeBGM(bgmVol);
        sm.ChangeVolumeEffect(sfxVol);
        uiManager.Initialize(currentDifficulty, unlockedDifficulty);
        levelManager.Initialize(currentDifficulty);
    }


    public void GameOver()
    {
        // PauseGame();
        
        State = GameState.Score;
        scoreManager.AddScore(currentDifficulty,score.Value);
        
        // 다음 난이도 해금
        if (currentDifficulty < 2 && score.Value >= levelManager.openScoreArr[currentDifficulty])
        {
            unlockedDifficulty = Math.Max(unlockedDifficulty, currentDifficulty + 1);
            PlayerPrefs.SetInt("u_diff",unlockedDifficulty);
            PlayerPrefs.Save();
        }
        
        uiManager.ShowResult();

        score.Value = 0;

    }

    public void ShowRankingScreen()
    {
        rankingScreen.UpdateRankingDisplay(currentDifficulty);
        uiManager.ShowRanking();
    }

    public void HideRankingScreen()
    {
        uiManager.HideRanking();
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (invincible && hp.Value < 3)
        {
            hp.Value = 3;
        }
        #endif
    }

    private void OnDisable()
    {
         PlayerPrefs.SetInt("c_diff", currentDifficulty);
         PlayerPrefs.SetInt("u_diff", unlockedDifficulty);
         PlayerPrefs.Save();
    }
}
