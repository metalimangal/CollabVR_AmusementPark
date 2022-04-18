using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client;

public class CF_PlayerAppearance : MonoBehaviourPunCallbacks
{
    private Team team = Team.NONE;

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
        if (changedProps.ContainsKey("Team") && base.photonView.Owner == targetPlayer)
        {
            ChangeColorForTeam(changedProps["Team"].ToString());
        }
    }

    private void ChangeColorForTeam(string teamProp)
    {
        if (teamProp == "BLUE")
        {
            team = Team.BLUE;
            photonView.RPC("ChangeColor", RpcTarget.All);
        }
        else if (teamProp == "RED")
        {
            team = Team.RED;
            photonView.RPC("ChangeColor", RpcTarget.All);
        }
        else
        {
            team = Team.NONE;
            photonView.RPC("ChangeColor", RpcTarget.All);
        }
    }

    public void TakeDamage() {
        photonView.RPC("OnTakeDamage", RpcTarget.All);
    }

    IEnumerator TakeDmgCoroutine() {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);
        ChangeColor();
    }

    [PunRPC]
    private void OnTakeDamage() {
        StartCoroutine(TakeDmgCoroutine());
    }

    [PunRPC]
    private void ChangeColor()
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

    [PunRPC]
    private void ChangeName(string name)
    {
        nameText.text = name;
    }
}
