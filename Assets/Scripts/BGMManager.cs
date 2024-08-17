using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultNamespace.SoundManager;
using UnityEngine.Rendering;
using static GameManager;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource _titlebgmSource;  // Inspector���� �̸� �Ҵ�� Ÿ��Ʋ BGM �ҽ�
    [SerializeField] private AudioClip _titleBGMClip; // Ÿ��Ʋ BGM Ŭ��

    [SerializeField] private List<AudioSource> _playbgmSources = new List<AudioSource>(); // Inspector���� �Ҵ�� ���� �÷��� BGM �ҽ� ����Ʈ
    [SerializeField] private List<AudioClip> _playBGMClips = new List<AudioClip>(); // ���� �÷��� BGM Ŭ�� ����Ʈ

    public IntVariableSO score;

    void Start()
    {
        AssignClipsToSources(); // ���� �� ����� Ŭ���� ����� �ҽ��� �Ҵ�
        RunTitleMusic(); // �ʱ� ���� �� Ÿ��Ʋ ������ ���
        score.ValueChangeEvent.AddListener(OnScoreChange);
    }

    private void AssignClipsToSources()
    {
        if (_titlebgmSource != null && _titleBGMClip != null)
        {
            _titlebgmSource.clip = _titleBGMClip; // Ÿ��Ʋ BGM ����� Ŭ���� �ҽ��� �Ҵ�
        }

        for (int i = 0; i < _playbgmSources.Count && i < _playBGMClips.Count; i++)
        {
            _playbgmSources[i].clip = _playBGMClips[i]; // ���� �÷��� BGM ����� Ŭ���� ������ �ҽ��� �Ҵ�
        }
    }

    public void RunTitleMusic()
    {
        StopAllMusic(); // ���� ��� ���� ������ ��� ����
        if (_titlebgmSource.clip != null)
        {
            _titlebgmSource.Play(); // Ÿ��Ʋ BGM ���
        }
    }

    public void RunPlayMusic()
    {
        StopAllMusic(); // ���� ��� ���� ������ ��� ����
        foreach (var source in _playbgmSources)
        {
            if (source.clip != null)
            {
                source.Play(); // �� �÷��� BGM ���
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
