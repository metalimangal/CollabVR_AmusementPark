using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CF_TeamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTeam(Team team)
    {
        PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team.ToString();
        Debug.Log("Assigned Team Custom Property");
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
