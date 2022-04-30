using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CF_TeamManager : MonoBehaviour
{
    public static CF_TeamManager Instance { get; private set; }
    ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
    public static event Action<Team> OnSetTeam;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void SetTeam(Team team)
    {
        if (!playerProps.ContainsKey("Team"))
        {
            playerProps.Add("Team", team.ToString());
        }
        playerProps["Team"] = team.ToString();
        PhotonNetwork.SetPlayerCustomProperties(playerProps);
        OnSetTeam?.Invoke(team);
    }

    public void SetTeamInt(int teamInt)
    {
        if (teamInt == 0)
        {
            SetTeam(Team.BLUE);
        }
        else if (teamInt == 1)
        {
            SetTeam(Team.RED);
        }
        else
        {
            SetTeam(Team.NONE);
        }
    }
}
