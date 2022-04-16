using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CF_TeamManager : MonoBehaviour
{
    ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();

    public void SetTeam(Team team)
    {
        if (!playerProps.ContainsKey("Team"))
        {
            playerProps.Add("Team", team.ToString());
        }
        playerProps["Team"] = team.ToString();
        PhotonNetwork.SetPlayerCustomProperties(playerProps);
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
