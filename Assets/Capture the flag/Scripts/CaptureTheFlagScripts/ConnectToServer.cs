/*
 * 
 * 
 * This is a test script used for running this scene on its own
 * 
 * 
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject networkPlayerPrefab;
    private GameObject spawnedPlayer;
    private ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CF_GameManager.Instance.UpdateGameState(GameState.Pregame);

        }
        if (playerProps.ContainsKey("Team"))
        {
            playerProps["Team"] = "NONE";
        }
        else
        {
            playerProps.Add("Team", "NONE");
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProps);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined! " + PhotonNetwork.CurrentRoom);

        //Instantiate Network Player when player joins room
        spawnedPlayer = PhotonNetwork.Instantiate(networkPlayerPrefab.name, transform.position, transform.rotation);

        //Setting "Team" property when player joins room
        if (playerProps.ContainsKey("Team"))
        {
            playerProps["Team"] = "NONE";
        }
        else
        {
            playerProps.Add("Team", "NONE");
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProps);

        if (PhotonNetwork.IsMasterClient)
        {
            CF_GameManager.Instance.UpdateGameState(GameState.Pregame);
        }
    }
}
