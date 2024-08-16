using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class UIScreenBase : MonoBehaviour
    {
        [SerializeField] protected UIManager uiManager;
        [SerializeField] protected Button backButton;

        public virtual void Awake()
        {
            backButton.onClick.AddListener(OnBackClicked);
        }


        public virtual void OnBackClicked()
        {
            
        }
        
        public virtual void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}