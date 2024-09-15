using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.UI
{
    public class GameScreen:UIScreenBase
    {
        public override void OnBackClicked()
        {
            uiManager.ShowPause(true);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                uiManager.ShowPause();
            }
        }
    }
}