using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client;

public class CF_PlayerAppearance : MonoBehaviourPunCallbacks
{
    public Team team = Team.NONE;

    [Header("Color Changing Stuff")]
    public Color blueTeamColor = Color.blue;
    public Color redTeamColor = Color.red;

    private string playerName = "";
    public TextMeshProUGUI nameText;


    private void Awake()
    {
        playerName = "Player " + photonView.ViewID;
        photonView.RPC("ChangeName", RpcTarget.All, playerName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("ChangeName", RpcTarget.All, playerName);
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey("Team") && photonView.Owner == targetPlayer)
        {
            Debug.Log("team property updated");
            photonView.RPC("ChangeTeam", RpcTarget.All, changedProps["Team"].ToString());
            ChangeColorForTeam(changedProps["Team"].ToString());
        }
    }

    private void ChangeColorForTeam(string teamProp)
    {
        if (teamProp == "BLUE")
        {
            photonView.RPC("ChangeColor", RpcTarget.All);
        }
        else if (teamProp == "RED")
        {
            photonView.RPC("ChangeColor", RpcTarget.All);
        }
        else
        {
            photonView.RPC("ChangeColor", RpcTarget.All);
        }
    }

    [PunRPC]
    private void ChangeColor()
    {
        if (photonView.IsMine)
        {
            var color = Color.gray;
            if (team == Team.BLUE)
            {
                color = blueTeamColor;
            }
            else if (team == Team.RED)
            {
                color = redTeamColor;
            }

            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.material.color = color;
            }
        }
        
    }

    [PunRPC]
    private void ChangeName(string name)
    {
        nameText.text = name;
    }

    [PunRPC]
    private void ChangeTeam(string t)
    {
        if (photonView.IsMine)
        {
            if (t == "BLUE")
            {
                team = Team.BLUE;
            }
            else if (t == "RED")
            {
                team = Team.RED;
            }
            else
            {
                team = Team.NONE;
            }
            gameObject.GetComponent<CF_Player>().team = team;
        }
    }
}
