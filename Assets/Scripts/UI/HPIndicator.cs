using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPIndicator : MonoBehaviour
{
    [SerializeField] private Sprite FullHeartSprite;
    [SerializeField] private Sprite EmptyHeartSprite;
    [SerializeField] private Image[] Hearts;

    public int MaxHP => Hearts.Length;
    
    public void SetHP(int hp)
    {
        if (hp > MaxHP)
            hp = MaxHP;
        if (hp <= 0 && GameManager.Instance.State == GameManager.GameState.Running)
        {
            GameManager.Instance.GameOver();
        }
        for (int i = 0; i < hp-1; i++)
        {
            Hearts[i].sprite = FullHeartSprite;
        }

        for (int i = hp; i < MaxHP; i++)
        {
            Hearts[i].sprite = EmptyHeartSprite;
        }
    }
}
