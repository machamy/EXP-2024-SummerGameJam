namespace DefaultNamespace.UI
{
    public class ScoreScreen:UIScreenBase
    {


        public void OnRestartClicked()
        {
            GameManager.Instance.InitializeGame();
            GameManager.Instance.StartGame();
            gameObject.SetActive(false);
        }

        public void OnMainClicked()
        {
            GameManager.Instance.GoMain();
            gameObject.SetActive(false);
        }
        
    }
}