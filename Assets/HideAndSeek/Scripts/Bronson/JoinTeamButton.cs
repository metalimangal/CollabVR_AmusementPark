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

    //private Button button;
    public TeamManager teamManager;

    void Start()
    {
        //DelayedStart(0.1f);
        //teamManager = FindObjectOfType(typeof(TeamManager)) as TeamManager;
        //button = this.GetComponent<Button>();
        //button.onClick.AddListener(ChangeTeam);

        foreach (HideAndSeekPlayer player in FindObjectsOfTypeAll(typeof(HideAndSeekPlayer)))
        {
            if (player.isLocalPlayer)
            {
                localPlayer = player.playerName;
                Debug.Log(localPlayer);
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

    IEnumerator DelayedStart(float timeToWait)
    {
        yield return new WaitForSecondsRealtime(timeToWait);
        //teamManager = FindObjectOfType(typeof(TeamManager)) as TeamManager;
        //button = this.GetComponent<Button>();
        //button.onClick.AddListener(ChangeTeam);

        foreach (HideAndSeekPlayer player in FindObjectsOfTypeAll(typeof(HideAndSeekPlayer)))
        {
            if (player.isLocalPlayer)
            {
                localPlayer = player.playerName;
            }
        }
    }
}
