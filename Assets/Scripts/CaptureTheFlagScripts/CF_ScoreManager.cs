using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CF_ScoreManager : MonoBehaviourPun
{
    public static CF_ScoreManager Instance { get; private set; }

    public static int scoreBlue = 0;
    public static int scoreRed = 0;

    public TextMeshProUGUI scoreBlueText;
    public TextMeshProUGUI scoreRedText;

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
        scoreBlueText.text = scoreBlue.ToString();
        scoreRedText.text = scoreRed.ToString();

    }

    public int GetScore(Team team)
    {
        if (team == Team.BLUE)
        {
            return scoreBlue;
        }
        else if (team == Team.RED)
        {
            return scoreRed;
        }
        else 
        {
            Debug.Log("Invalid Team");
            return 0;
        }
        
    }

    public void AddScore(Team team, int score)
    {
        if (team == Team.BLUE)
        {
            this.photonView.RPC("AddScoreBlue", RpcTarget.All, score);
            Debug.Log("Blue Scored!");
        }
        else if (team == Team.RED)
        {
            this.photonView.RPC("AddScoreRed", RpcTarget.All, score);
            Debug.Log("Red Scored!");
        }
        else Debug.Log("Invalid team");
    }

    public void ResetScore() 
    {
        this.photonView.RPC("PunResetScore", RpcTarget.All);
    }

    [PunRPC]
    void AddScoreBlue(int score)
    {
        scoreBlue += score;
    }

    [PunRPC]
    void AddScoreRed(int score)
    {
        scoreRed += score;
    }

    [PunRPC]
    void PunResetScore()
    {
        scoreBlue = 0;
        scoreRed = 0;
    }
}
