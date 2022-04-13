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

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try connect to server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined! " + PhotonNetwork.CurrentRoom);

        //Instantiate Network Player when player joins room
        spawnedPlayer = PhotonNetwork.Instantiate(networkPlayerPrefab.name, transform.position, transform.rotation);

        //Setting "Team" property when player joins room
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = "none";
        }
        else
        {
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
            {
                { "Team", "none" },
                { "Name", "" }
            };
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            CF_GameManager.Instance.UpdateGameState(GameState.Pregame);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ReloadLevel();
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ReloadLevel();
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //PhotonNetwork.Destroy(spawnedPlayer);

    }
    private void ReloadLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Reloading Level");
            PhotonNetwork.LoadLevel(0);             // Might have to change this depending on the scene in the view;
        }
    }

}
