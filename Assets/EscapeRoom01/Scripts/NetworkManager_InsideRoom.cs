using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager_InsideRoom : MonoBehaviourPunCallbacks
{
	RoomOptions roomOptions = new RoomOptions();
    // Start is called before the first frame update
    void Start()
    {
		roomOptions.MaxPlayers = 10;
		roomOptions.IsVisible = true;
		roomOptions.IsOpen = true;
        ConnectToServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void ConnectToServer()
	{
		PhotonNetwork.ConnectUsingSettings();
		Debug.Log("Try connect to server...");
	}
	
	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected To Server.");
		base.OnConnectedToMaster();
		PhotonNetwork.JoinOrCreateRoom("Room 1",roomOptions,TypedLobby.Default);
	}
	
	public override void OnJoinedRoom()
	{
		Debug.Log("Joined a Room");
		base.OnJoinedRoom();
	}
	
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("A new player joined the room");
		base.OnPlayerEnteredRoom(newPlayer);
	}
}
