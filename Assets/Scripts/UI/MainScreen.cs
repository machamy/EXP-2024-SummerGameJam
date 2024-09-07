using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class MainScreen : UIScreenBase
    {
        [SerializeField] private GameObject difficultySellector;
        [SerializeField] private GameObject SkinScreen;
        [SerializeField] private Button levelButton;
        [SerializeField] private Button RankingButton;
        [field:SerializeField] public Sprite[] levelSprites { get; private set; }
        [field:SerializeField] public Sprite[] levelPressedSprites{ get; private set; }
        [field:SerializeField] public Button[] difficultyButtons{ get; private set; }
        private bool isSellectingDiffculty;
        protected bool IsSellectingDiffculty
        {
            get => isSellectingDiffculty;
            set
            {
                isSellectingDiffculty = value;
                difficultySellector.SetActive(value);
                backButton.gameObject.SetActive(value);
            }
        }

        public override void Awake()
        {
            base.Awake();
        }

        public void UpdateSprites(int currentDifficulty, int unlockDifficulty)
        {
            UpdateLevelSprite(currentDifficulty);
            for (int i = 0; i <= unlockDifficulty; i++)
            {
                difficultyButtons[i].interactable = true;
            }
            for (int i = unlockDifficulty+1; i < 3; i++)
            {
                difficultyButtons[i].interactable = false;
            }
        }

        public void UpdateLevelSprite(int currentDifficulty)
        {
            var ss = levelButton.spriteState;
            ss.selectedSprite = levelSprites[currentDifficulty];
            ss.pressedSprite = levelPressedSprites[currentDifficulty];
            levelButton.spriteState = ss;
            levelButton.GetComponent<Image>().sprite = ss.selectedSprite;
        }

        public override void OnBackClicked()
        {
            SoundManager.Instance.Play("button2");
            if (IsSellectingDiffculty)
            {
                IsSellectingDiffculty = false;
            }
        }

        public void OnClickDifficulty()
        {
            SoundManager.Instance.Play("button2");
            IsSellectingDiffculty = !IsSellectingDiffculty;
        }

        public void OnChangeDifficulty(int n)
        {
            GameManager.Instance.currentDifficulty = n;
            IsSellectingDiffculty = false;
            UpdateLevelSprite(n);
            PlayerPrefs.SetInt("c_diff", n);
            print($"Change difficult {n}");
        }

        public void OnSettingClicked()
        {
            SoundManager.Instance.Play("button2");
            uiManager.ShowSetting();
        }

        public void OnStartClicked()
        {
            SoundManager.Instance.Play("button2");
            uiManager.ShowGame();
            GameManager.Instance.StartGame();
            
        }
        
        public void OnRankingClicked() { 
            SoundManager.Instance.Play("button2"); 
            uiManager.ShowRanking(); }

        public void OnSkinClicked()
        {
            SkinScreen.SetActive(true);
        }
    }
}