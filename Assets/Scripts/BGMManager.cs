using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultNamespace.SoundManager;
using UnityEngine.Rendering;
using static GameManager;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource _titlebgmSource;  // Inspector에서 미리 할당된 타이틀 BGM 소스
    [SerializeField] private AudioClip _titleBGMClip; // 타이틀 BGM 클립

    [SerializeField] private List<AudioSource> _playbgmSources = new List<AudioSource>(); // Inspector에서 할당된 게임 플레이 BGM 소스 리스트
    [SerializeField] private List<AudioClip> _playBGMClips = new List<AudioClip>(); // 게임 플레이 BGM 클립 리스트

    public IntVariableSO score;

    void Start()
    {
        AssignClipsToSources(); // 시작 시 오디오 클립을 오디오 소스에 할당
        RunTitleMusic(); // 초기 실행 시 타이틀 음악을 재생
        score.ValueChangeEvent.AddListener(OnScoreChange);
    }

    private void AssignClipsToSources()
    {
        if (_titlebgmSource != null && _titleBGMClip != null)
        {
            _titlebgmSource.clip = _titleBGMClip; // 타이틀 BGM 오디오 클립을 소스에 할당
        }

        for (int i = 0; i < _playbgmSources.Count && i < _playBGMClips.Count; i++)
        {
            _playbgmSources[i].clip = _playBGMClips[i]; // 게임 플레이 BGM 오디오 클립을 각각의 소스에 할당
        }
    }

    public void RunTitleMusic()
    {
        StopAllMusic(); // 기존 재생 중인 음악을 모두 중지
        if (_titlebgmSource.clip != null)
        {
            _titlebgmSource.Play(); // 타이틀 BGM 재생
        }
    }

    public void RunPlayMusic()
    {
        StopAllMusic(); // 기존 재생 중인 음악을 모두 중지
        foreach (var source in _playbgmSources)
        {
            if (source.clip != null)
            {
                source.Play(); // 각 플레이 BGM 재생
            }
        }
        OnScoreChange(0);
    }

    public void OnScoreChange(int score)
    {
        if(GameManager.Instance.State != GameState.Running)
        {
            return;
        }

        switch (score)
        {
            case 0:
                for (int i = 0; i < _playbgmSources.Count && i < _playBGMClips.Count; i++)
                {
                    _playbgmSources[i].Play();
                }
                for (int i = 1; i < 5; i++) {
                    _playbgmSources[i].volume = 0;
                }
            break;
            case 10:
                _playbgmSources[1].volume = 1; break;
            case 20:
                _playbgmSources[2].volume = 1; break;
            case 30:
                _playbgmSources[3].volume = 1; break;
            case 40:
                for(int i = 0; i < 4; i++)
                {
                    _playbgmSources[i].volume = 0;
                }
                _playbgmSources[4].volume = 1; break;
       };
    }

    public void PlayGameBGM(AudioSource audioSource)
    {
    }
    private void StopAllMusic()
    {
        if (_titlebgmSource.isPlaying)
            _titlebgmSource.Stop();

        foreach (var source in _playbgmSources)
        {
            if (source.isPlaying)
                source.Stop();
        }
    }

    void Update()
    {

    }
}
