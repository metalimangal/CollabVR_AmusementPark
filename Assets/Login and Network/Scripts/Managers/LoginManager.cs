using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField userInput;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callbacks
    public void ConnectAnon()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectToPhotonServer()
    {
        if(userInput != null)
        {
            PhotonNetwork.NickName = userInput.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    #region PhotonCallbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to Server");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server with user name: " + PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("HomeScene");
        //JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {

        Debug.Log("Local Player is: " + PhotonNetwork.NickName + " joined :" + PhotonNetwork.CurrentRoom.Name + ", player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(CollabVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CollabVRConstants.MAP_TYPE_KEY, out mapType))
                {
                    Debug.Log("Joined room with map:" + (string)mapType);
                }
        }
        PhotonNetwork.LoadLevel("HomeScene"); // Change to Social Space
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player Joined: " + newPlayer.NickName);
    }

    #endregion

    #region Private Methods

    private void CreateAndJoinRoom()
    {
        string randomRoom = "Room_" + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = { CollabVRConstants.MAP_TYPE_KEY };

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, CollabVRConstants.MAP_TYPE_VALUE_SOCIAL_SPACE } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;

        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoom, roomOptions);
    }

    #endregion
}
