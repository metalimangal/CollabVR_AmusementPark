using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    string mapType;
    public TMP_Text OccupancyRate_Rooms_SocialSpace;
    public TMP_Text OccupancyRate_Rooms_escape_room1;
    public TMP_Text OccupancyRate_Rooms_escape_room_2;
    public TMP_Text OccupancyRate_Rooms_capture_the_flag;
    public TMP_Text OccupancyRate_Rooms_horror_house;
    public TMP_Text OccupancyRate_Rooms_hide_and_seek;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callbacks

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnEnterButtonClickedEscapeRoom1()
    {
        mapType = CollabVRConstants.MAP_TYPE_VALUE_ESCAPE_ROOM1;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    public void OnEnterButtonClickedEscapeRoom2()
    {

        mapType = CollabVRConstants.MAP_TYPE_VALUE_ESCAPE_ROOM2;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    public void OnEnterButtonClickedCaptureTheFlag()
    {

        mapType = CollabVRConstants.MAP_TYPE_VALUE_CAPTURE_THE_FLAG;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    public void OnEnterButtonClickedHorrorHouse()
    {

        mapType = CollabVRConstants.MAP_TYPE_VALUE_HORROR_HOUSE;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    public void OnEnterButtonClickedHideAndSeek()
    {

        mapType = CollabVRConstants.MAP_TYPE_VALUE_HIDE_AND_SEEK;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    public void OnEnterButtonClickedSocialSpace()
    {
        PhotonNetwork.LeaveRoom();
        mapType = CollabVRConstants.MAP_TYPE_VALUE_SOCIAL_SPACE;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }



    #endregion


    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
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
                switch ((string)mapType)
                {
                    case CollabVRConstants.MAP_TYPE_VALUE_CAPTURE_THE_FLAG:
                        PhotonNetwork.LoadLevel(""); //Load capture the flag
                        break;
                    case CollabVRConstants.MAP_TYPE_VALUE_ESCAPE_ROOM1:
                        PhotonNetwork.LoadLevel("EscapeRoom01/EscapeRoom01Scenes/DisjointScenes/ER01_Room01"); //Load escape room 1 // ADDED BY Syed Tanzim Mubarrat
                        break;
                    case CollabVRConstants.MAP_TYPE_VALUE_ESCAPE_ROOM2:
                        PhotonNetwork.LoadLevel(""); //Load escape room 2 
                        break;
                    case CollabVRConstants.MAP_TYPE_VALUE_HORROR_HOUSE:
                        PhotonNetwork.LoadLevel(""); //Load Horror House
                        break;
                    case CollabVRConstants.MAP_TYPE_VALUE_SOCIAL_SPACE:
                        PhotonNetwork.LoadLevel("World_Outdoor"); //Load Social Space
                        break;
                    case CollabVRConstants.MAP_TYPE_VALUE_HIDE_AND_SEEK:
                        PhotonNetwork.LoadLevel("HideAndSeek"); //Load Hide And Seek
                        break;
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player Joined: " + newPlayer.NickName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList.Count == 0)
        {
            OccupancyRate_Rooms_SocialSpace.text = 0 + "/" + 20;
            OccupancyRate_Rooms_escape_room1.text = 0 + "/" + 20;
            OccupancyRate_Rooms_escape_room_2.text = 0 + "/" + 20;
            OccupancyRate_Rooms_capture_the_flag.text = 0 + "/" + 20;
            OccupancyRate_Rooms_horror_house.text = 0 + "/" + 20;
            OccupancyRate_Rooms_hide_and_seek.text = 0 + "/" + 20;
        }

        foreach(RoomInfo room in roomList)
        {
            if (room.Name.Contains(CollabVRConstants.MAP_TYPE_VALUE_CAPTURE_THE_FLAG))
            {
                OccupancyRate_Rooms_capture_the_flag.text = room.PlayerCount + "/" + 20;
            }
            else if (room.Name.Contains(CollabVRConstants.MAP_TYPE_VALUE_SOCIAL_SPACE))
            {
                OccupancyRate_Rooms_SocialSpace.text = room.PlayerCount + "/" + 20;
            }
            else if (room.Name.Contains(CollabVRConstants.MAP_TYPE_VALUE_ESCAPE_ROOM1))
            {
                OccupancyRate_Rooms_escape_room1.text = room.PlayerCount + "/" + 20;
            }
            else if (room.Name.Contains(CollabVRConstants.MAP_TYPE_VALUE_ESCAPE_ROOM2))
            {
                OccupancyRate_Rooms_escape_room_2.text = room.PlayerCount + "/" + 20;
            }
            else if (room.Name.Contains(CollabVRConstants.MAP_TYPE_VALUE_HIDE_AND_SEEK))
            {
                OccupancyRate_Rooms_hide_and_seek.text = room.PlayerCount + "/" + 20;
            }
            else if (room.Name.Contains(CollabVRConstants.MAP_TYPE_VALUE_HORROR_HOUSE))
            {
                OccupancyRate_Rooms_horror_house.text = room.PlayerCount + "/" + 20;
            }
        }
    }

    #endregion

    #region Private Methods

    private void CreateAndJoinRoom()
    {
        string randomRoom = "Room_"+mapType + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = { CollabVRConstants.MAP_TYPE_KEY };

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { CollabVRConstants.MAP_TYPE_KEY, mapType} };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;

        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomRoom, roomOptions);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined the Lobby");
    }

    #endregion
}
