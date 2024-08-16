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
    
    public void SetHP(int n)
    {
        if (n > MaxHP)
            n = 3;
        for (int i = 0; i < n; i++)
        {
            Hearts[i].sprite = FullHeartSprite;
        }

        for (int i = n; i < MaxHP; i++)
        {
            Hearts[i].sprite = EmptyHeartSprite;
        }
    }
}
