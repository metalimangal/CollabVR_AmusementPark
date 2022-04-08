using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagGameManager : MonoBehaviour
{
    public static CaptureFlagGameManager Instance { get; private set; }

    public static event Action<GameState> OnGameStateChanged;


    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.Pregame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Pregame:
                HandlePregame();
                break;
            case GameState.Waiting:
                HandleWaiting();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.End:
                HandleEnd();
                break;
        }
    }

    private void HandleEnd()
    {
        
    }

    private void HandlePlaying()
    {
        
    }

    private void HandleWaiting()
    {
        
    }

    private void HandlePregame()
    {
        
    }

    
}
public enum GameState
{
    Pregame, Waiting, Playing, End
}
public enum Team
{
    BLUE, RED
}