using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CF_ScoreManager : MonoBehaviourPun, IPunObservable
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

    public void AddScore(Team team, int score)
    {
        if (team == Team.BLUE)
        {
            scoreBlue += score;
            Debug.Log("Blue Scored!");
        }
        else if (team == Team.RED)
        {
            scoreRed += score;
            Debug.Log("Red Scored!");
        }
        else Debug.Log("Invalid team");

        photonView.RPC("SetScoreText", RpcTarget.All, scoreBlue.ToString(), scoreRed.ToString());
    }

    public void ResetScore() 
    {
        scoreBlue = 0;
        scoreRed = 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(scoreBlue);
            stream.SendNext(scoreRed);
        }
        else
        {
            scoreBlue = (int)stream.ReceiveNext();
            scoreRed = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void SetScoreText(string blue, string red)
    {
        scoreBlueText.text = blue.ToString();
        scoreRedText.text = red.ToString();
    }
}
