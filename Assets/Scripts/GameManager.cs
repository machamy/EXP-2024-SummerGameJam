using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Ready,
        Running
    }
    
    public GameState State { get; private set; }


    private void Update()
    {
        
    }
}
