using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public Camera Camera;
    [Header("Main")] 
    [SerializeField] public GameObject Main;
    [Header("Game")]
    [SerializeField] private GameObject Game;
    [Header("Setting")]
    [SerializeField] private GameObject Setting;
    [Header("Pause")]
    [SerializeField] private GameObject Pause;
    [Header("Skin")]
    [SerializeField] private SkinScreen Skin;
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

    [Header("LastUI")] public UIScreenBase lastUI;
    private void Awake()
    {
        Score.Value = 0;
        Hp.Value = 3;
    }

    private void Start()
    {
        lastUI = Main.GetComponent<UIScreenBase>();
    }

    public void Initialize(int currentDifficulty, int unlockDifficulty)
    {
        Score.Value = 0;
        Hp.Value = 3;
        Main.GetComponent<MainScreen>().UpdateSprites(currentDifficulty,unlockDifficulty);
        Skin.SkinCheck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lastUI?.OnBackClicked();
        }
    }

    public void ShowPause(bool playsound = false)
    {
        if(playsound)
            SoundManager.Instance.Play("button2");
        Pause.SetActive(true);
        Pause.GetComponent<PauseScreen>().isRunning = true;
        lastUI = Pause.GetComponent<UIScreenBase>();
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
        Pause.SetActive(false);
        lastUI = Game.GetComponent<UIScreenBase>();
        if(GameManager.Instance.State == GameManager.GameState.Pause)
            GameManager.Instance.ResumeGame();
    }

    public void ShowMain()
    {
        Result.SetActive(false);
        Game.SetActive(false);
        Pause.SetActive(false);
        Setting.SetActive(false);
        Main.SetActive(true);
        lastUI = Main.GetComponent<UIScreenBase>();
    }

    public void ShowResult()
    {
        Result.SetActive(true);
        lastUI = Result.GetComponent<UIScreenBase>();
    }

    // private IEnumerator ShowGameRoutine(GameObject gameObject)
    // {
    //     
    // }

    public void ShowRanking()
    {
        Ranking.SetActive(true);
        lastUI = Ranking.GetComponent<UIScreenBase>();
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
        Hp.ValueChangeEvent.AddListener(PlayOuch);
    }

    private void OnDisable()
    {
        Score.ValueChangeEvent.RemoveListener(ChangeScore);
        Hp.ValueChangeEvent.RemoveListener(hpIndicator.SetHP);
        Hp.ValueChangeEvent.RemoveListener(PlayOuch);
    }

    private void PlayOuch(int nextHP)
    {
        if (nextHP < Hp.Value && (GameManager.Instance.State == GameManager.GameState.Running)) 
        {
            // SoundManager.Instance.Play("heart_ouch", volume:0.5f);
            StartCoroutine(CameraShake(5, 0.04f, 0.1f));
        }
    }

    public IEnumerator CameraShake(int times, float interval, float delta)
    {
        Vector3 initialPos = Camera.transform.position;
        Transform transform = Camera.transform;
        for (int i = 0; i < times; i++)
        {
            Vector3 deltaVector = new Vector3(Random.Range(-delta, +delta), Random.Range(-delta, +delta), 0);
            transform.position = initialPos + deltaVector;
            yield return new WaitForSeconds(interval);
        }

        transform.position = initialPos;
    }
}
