using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client;

public class CF_PlayerAppearance : MonoBehaviourPunCallbacks
{

    [Header("Color Changing Stuff")]
    public Color blueTeamColor = Color.blue;
    public Color redTeamColor = Color.red;

    private string playerName = "";
    public TextMeshProUGUI nameText;


    private void Awake()
    {
        playerName = "Player " + photonView.Owner.ActorNumber;
        photonView.RPC("ChangeName", RpcTarget.All, playerName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("ChangeName", RpcTarget.All, playerName);
        base.OnPlayerEnteredRoom(newPlayer);
    }

    

    public void ChangeColorForTeam(string teamProp)
    {
        photonView.RPC("ChangeColor", RpcTarget.All, teamProp);
    }

    [PunRPC]
    private void ChangeColor(string team)
    {
        var color = Color.gray;
        
        if (team == "BLUE")
        {
            color = blueTeamColor;
        }
        else if (team == "RED")
        {
            color = redTeamColor;
        }

        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.material.color = color;
        }
    }

    [PunRPC]
    private void ChangeName(string name)
    {
        nameText.text = name;
    }

    
}
