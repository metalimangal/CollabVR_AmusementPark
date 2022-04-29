using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TeamManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [System.NonSerialized] public GameObject localPlayer;  //Stores the local player publically for easy access by other scripts

    public List<TeamList> teams = new List<TeamList>();

    [Tooltip("Sent before each team list update.  Must not be a possible player name and must be unique from other sent data.")] public string listStartCode = "Start Lists";

    private string newline = "\r\n";

    void Awake()
    {
        foreach(TeamList team in teams)
        {
            if (team.teamNameTextBox)
            {
                team.teamNameTextBox.text = team.teamName;
            }
        }
    }

    void Update()
    {
        foreach(TeamList team in teams)
        {
            if (team.teamListTextBox)
            {
                string temp = "";
                foreach(string teammate in team.teammates)
                {
                    temp = temp + teammate + newline;
                }
                team.teamListTextBox.text = temp;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            foreach(TeamList team in teams)
            {
                stream.SendNext(team.teammates);
            }
            //stream.SendNext(listStartCode);
            //foreach(TeamList team in teams)
            //{
            //    SendListString(stream, team.teammates, team.breakCode);
            //}
        }
        else
        {
            foreach (TeamList team in teams)
            {
                team.teammates = stream.ReceiveNext() as List<string>;
            }
            //string temp = (string)stream.PeekNext();
            //if(temp == listStartCode)
            //{
            //    stream.ReceiveNext();
            //    foreach(TeamList team in teams)
            //    {
            //        team.teammates = ReceiveListString(stream, team.breakCode);
            //    }
            //}
        }
    }

    public void RemovePlayer(string playerName)
    {
        foreach(TeamList team in teams)
        {
            if (team.teammates.Contains(playerName))
            {
                team.teammates.Remove(playerName);
            }
        }
    }

    public void ChangeTeam(string teamName, string playerName)
    {
        foreach(TeamList team in teams)
        {
            if(team.teamName == teamName)
            {
                if (!team.teammates.Contains(playerName))
                {
                    team.teammates.Add(playerName);
                }
            }
            else
            {
                if (team.teammates.Contains(playerName))
                {
                    team.teammates.Remove(playerName);
                }
            }
        }
    }

    public int RetrieveTeamIndex(string teamName)
    {
        //Returns -1 if no team is found
        int idx = -1;
        for(int i = 0; i < teams.Count; i++)
        {
            if(teams[i].teamName == teamName)
            {
                if(idx != -1)
                {
                    Debug.LogError("More than one team of the same name found.", this);
                }
                idx = i;
            }
        }
        return idx;
    }

    public TeamList RetrieveTeamOfName(string teamName)
    {
        TeamList foundTeam = null;
        foreach(TeamList team in teams)
        {
            if(team.teamName == teamName)
            {
                foundTeam = team;
            }
        }
        if (foundTeam == null)
        {
            Debug.LogError("No team found.", this);
        }
        return foundTeam;
    }

    [System.Serializable]
    public class TeamList
    {
        [SerializeField] public string teamName;
        [System.NonSerialized] public List<string> teammates = new List<string>();
        [SerializeField] [Tooltip("The code sent after the team list when syncing data.  Must not be a possible player name and must be unique.")] public string breakCode;
        [SerializeField] public Text teamNameTextBox;
        [SerializeField] public Text teamListTextBox;
    }

    private void SendListString(PhotonStream stream, List<string> list, string breakCode)
    {
        foreach(string item in list)
        {
            stream.SendNext(item);
        }
        stream.SendNext(breakCode);
    }

    private List<string> ReceiveListString(PhotonStream stream, string breakCode)
    {
        List<string> list = new List<string>();
        string temp = (string)stream.ReceiveNext();
        while (temp != breakCode)
        {
            list.Add(temp);
            temp = (string)stream.ReceiveNext();
        }
        return list;
    }
}
