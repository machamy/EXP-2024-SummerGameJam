using System;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace DefaultNamespace.UI
{
    public class PauseScreen:UIScreenBase
    {
        public bool isRunning = false;
        private SoundManager sm;
        private float bgmVol;
        private float sfxVol;

        [SerializeField] private Slider bgmSilder;
        [SerializeField] private Slider sfxSilder;
        private void OnEnable()
        {
            sm = SoundManager.Instance;
            bgmVol = PlayerPrefs.GetFloat("bgm", 0.5f);
            sfxVol = PlayerPrefs.GetFloat("sfx", 0.5f);

            bgmSilder.value = bgmVol;
            sfxSilder.value = sfxVol;
        }

        public void OnVolChangeBGM(float value)
        {
            bgmVol = value;
            sm.ChangeVolumeBGM(value);
        }

        public void OnVolChangeSFX(float value)
        {
            sfxVol = value;
            sm.ChangeVolumeEffect(value);
        }

        public override void OnBackClicked()
        {
            SoundManager.Instance.Play("button2");
            Exit();            
        }
        public void OnMainClicked()
        {
            SoundManager.Instance.Play("button2");
            GameManager.Instance.GoMain();
            PlayerPrefs.SetFloat("bgm", bgmVol);
            PlayerPrefs.SetFloat("sfx", sfxVol);
            PlayerPrefs.Save();
            gameObject.SetActive(false);
            // isRunning = false;
        }
        public void OnRestartClicked()
        {
            GameManager.Instance.InitializeGame();
            GameManager.Instance.StartGame();
            gameObject.SetActive(false);
        }

        public override void Exit()
        {
            SoundManager.Instance.Play("button2");
            PlayerPrefs.SetFloat("bgm", bgmVol);
            PlayerPrefs.SetFloat("sfx", sfxVol);
            PlayerPrefs.Save();
            // gameObject.SetActive(false);
            uiManager.ShowGame();
            // if (isRunning)
            //     GameManager.Instance.ResumeGame();
            // isRunning = false;
        }

    }
}