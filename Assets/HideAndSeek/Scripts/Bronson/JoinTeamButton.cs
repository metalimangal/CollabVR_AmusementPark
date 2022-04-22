using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JoinTeamButton : MonoBehaviour
{
    public string teamToJoin;

    [System.NonSerialized] public string localPlayer;
    [System.NonSerialized] public bool canChangeTeams = true;

    private Button button;
    private TeamManager teamManager;

    void Start()
    {
        teamManager = FindObjectOfType(typeof(TeamManager)) as TeamManager;
        button = this.GetComponent<Button>();
        button.onClick.AddListener(ChangeTeam);

        HideAndSeekPlayer[] players = FindObjectsOfType(typeof(HideAndSeekPlayer)) as HideAndSeekPlayer[];
        if(players.Length == 0)
        {
            Debug.LogError("No players found.", this);
        }
        foreach(HideAndSeekPlayer player in players)
        {
            if (player.isLocalPlayer)
            {
                localPlayer = player.playerName;
            }
        }
    }

    public void ChangeTeam()
    {
        if (canChangeTeams)
        {
            teamManager.ChangeTeam(teamToJoin, localPlayer);
        }
    }
}
