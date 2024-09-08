using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class SkinScreen:UIScreenBase
    {
        [SerializeField] private BridgeController bridge;
        
        public List<SkinSO> skinList;
        [SerializeField] private int currentIdx = 0;
        public SkinSO CurrentSkin => skinList[currentIdx];
        [SerializeField] private Image bridgePreviewImage;

        private void OnEnable()
        {
            currentIdx = PlayerPrefs.GetInt("skin_idx",0);
            OnSkinChange(CurrentSkin);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("skin_idx",currentIdx);
        }

        public void Go(int idx)
        {
            if (idx >= skinList.Count)
            {
                Go(idx % skinList.Count);
                return;
            }
            if (idx < 0)
            {
                idx += skinList.Count;
            }
            currentIdx = idx;
            OnSkinChange(skinList[idx]);
        }
        public void OnNext() => Go(currentIdx += 1);
        public void OnPrev() => Go(currentIdx -= 1);

        public override void OnBackClicked()
        {
            gameObject.SetActive(false);
        }

        public void SkinCheck() => OnSkinChange(CurrentSkin);
        private void OnSkinChange(SkinSO skinSo)
        {
            bridge.SkinSo = skinSo;
            bridgePreviewImage.sprite = skinSo.sprite;
            bridgePreviewImage.color = skinSo.BridgeColor;
            // TODO 기타 코드
        }
    }
}