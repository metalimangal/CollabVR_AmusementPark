using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CF_GameManager : MonoBehaviourPunCallbacks
{
    public static CF_GameManager Instance { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    public GameState currentState;

    [Header("Time Stuff")]
    public float totalMatchTime = 90; // Total Time for match
    public TextMeshProUGUI timeText;
    private float currentTime;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == GameState.Playing)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                DisplayTime(currentTime);
            }
            else
            {
                UpdateGameState(GameState.End);
            }
        }
    }

    private void DisplayTime(float currentTime)
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateGameState(GameState newState)
    {
        this.photonView.RPC("PunUpdateGameState", RpcTarget.All, newState);
    }

    [PunRPC]
    public void PunUpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Pregame:
                HandlePregame();
                break;
            case GameState.GameStart:
                HandleGameStart();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.End:
                HandleEnd();
                break;
        }
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePregame()
    {
        // Player can wander around to select team
    }

    private void HandleGameStart()
    {
        // Teleport player to spawn points, player can not move, count down until the game begins
        //          (done in CF_PlayerMovement)

        // Reset Score
        CF_ScoreManager.Instance.ResetScore();
        StartCoroutine(StartPlayInSeconds(5));
        
    }
    private void HandlePlaying()
    {
        // Called when game starts
        currentTime = totalMatchTime;
    }
    private void HandleEnd()
    {
        // Game ended
        timeText.text = "Game Over!";
    }

    public void StartGame()
    {
        UpdateGameState(GameState.GameStart);
    }

    IEnumerator StartPlayInSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        UpdateGameState(GameState.Playing);
    }
    
}
public enum GameState
{
    Pregame, GameStart, Playing, End
}
public enum Team
{
    BLUE, RED, NONE
}