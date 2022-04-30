using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
//using EscapeRoom01;


	[System.Serializable]
	public class DefaultRoom
	{
		public string Name;
		public int sceneIndex;
		public byte maxPlayer;
	}

	public class NetworkManager : MonoBehaviourPunCallbacks
	{
		public bool EnableInput = false;
		bool enterPressed = false;
		//RoomOptions roomOptions = new RoomOptions();

		/*void Start()
		{
			roomOptions.MaxPlayers = 10;
			roomOptions.IsVisible = true;
			roomOptions.IsOpen = true;
			//ConnectToServer();
		}*/
		
		public List<DefaultRoom> defaultRooms;
		public GameObject roomUI;
		
		public GameObject connectingLabel;
		
		private DefaultRoom roomSettings;
		
		/// <summary>
		/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
		/// </summary>
		string gameVersion = "v1";
		
		bool triesToConnectToMaster = false;
		bool triesToConnectToRoom = false;
		
		bool NoSelection = true;
		
		private void Start()
		{
			//roomSettings = defaultRooms[0];
			connectingLabel.SetActive(false);
			//InstantConnect();
		}
		// Update is called once per frame
		private void Update()
		{
			
			if (!PhotonNetwork.IsConnected && !triesToConnectToMaster && enterPressed && !NoSelection)
			{
				ConnectToMaster();
			}
			if (PhotonNetwork.IsConnected && !triesToConnectToMaster && !triesToConnectToRoom && enterPressed && !NoSelection)
			{
				StartCoroutine(WaitFrameAndConnect());
			}
		}
		
		public void LoginSwitch()
		{
			if (!NoSelection)
			{
				enterPressed = true;
			}
		}
		
		public void InstantConnect()
		{
			roomSettings = defaultRooms[0];
			Debug.Log(roomSettings);
			enterPressed = true;
			NoSelection = false;
		}
		
		
		public void SelectRoom(int n)
		{
			NoSelection = false;
			roomSettings = defaultRooms[n];
		}
		
		public void ConnectToMaster()
		{
			PhotonNetwork.OfflineMode = false; //true would "fake" an online connection
			//getPlayerName();
			//PhotonNetwork.NickName = playerName; //we can use a input to change this 
			PhotonNetwork.AutomaticallySyncScene = true; //To call PhotonNetwork.LoadLevel()
			PhotonNetwork.GameVersion = gameVersion; //only people with the same game version can play together

			triesToConnectToMaster = true;
			//PhotonNetwork.ConnectToMaster(ip, port, appid); //manual connection
			PhotonNetwork.ConnectUsingSettings(); //automatic connection based on the config file
		}
		
		IEnumerator WaitFrameAndConnect()
		{
			triesToConnectToRoom = true;
			yield return new WaitForEndOfFrame();
			Debug.Log("Connecting");
			connectingLabel.SetActive(true);
			//progressLabel.SetActive(true);
			//connectedLabel.SetActive(false);
			//controlPanel.SetActive(false);
			ConnectToRoom();
		}
		
		public void ConnectToRoom()
		{
			if (!PhotonNetwork.IsConnected)
				return;

			triesToConnectToRoom = true;
			RoomOptions options = new RoomOptions();
			options.MaxPlayers = roomSettings.maxPlayer;
			PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, options, TypedLobby.Default);
			//PhotonNetwork.JoinOrCreateRoom(RoomName); //Create a specific room - Callback OnCreateRoomFailed
			//PhotonNetwork.JoinRoom(RoomName); //Join a specific room - Callback OnJoinRoomFailed

			//PhotonNetwork.JoinRandomRoom(); // Join a random room - Callback OnJoinRandomRoomFailed
		}
		
		
		public override void OnDisconnected(DisconnectCause cause)
		{
			base.OnDisconnected(cause);
			triesToConnectToMaster = false;
			triesToConnectToRoom = false;
			connectingLabel.SetActive(false);
			//progressLabel.SetActive(false);
			//controlPanel.SetActive(true);
			//connectedLabel.SetActive(false);
			Debug.Log(cause);
		}
		
		public override void OnConnectedToMaster()
		{
			base.OnConnectedToMaster();
			triesToConnectToMaster = false;
			Debug.Log("Connected to master!");
		}
		
		public override void OnJoinedRoom()
		{
			//Go to next scene after joining the room
			//progressLabel.SetActive(false);
			//connectedLabel.SetActive(true);
			//controlPanel.SetActive(false);
			connectingLabel.SetActive(false);
			base.OnJoinedRoom();
			Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);
			//Destroy(GameObject.Find("NewPlayerTanzim"));
			//LevelSelected = true;
			//connectedLabel.SetActive(false);
			//LevelSelectLabel.SetActive(true);
			//DaySelection.SetActive(true);
			//NightSelection.SetActive(false);
			//DaySelected = true;
			//NightSelected = false;
			//SceneManager.LoadScene(1); //go to the room scene
			SceneManager.LoadScene(roomSettings.sceneIndex);
		}
		
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			base.OnJoinRandomFailed(returnCode, message);
			//no room available
			//create a room (null as a name means "does not matter")
			PhotonNetwork.CreateRoom(roomSettings.Name, new RoomOptions { MaxPlayers = roomSettings.maxPlayer });
		}
		
		/*
		public void ConnectToServer()
		{
			PhotonNetwork.ConnectUsingSettings();
			Debug.Log("Try connect to server...");
		}
		
		public override void OnConnectedToMaster()
		{
			Debug.Log("Connected To Server.");
			base.OnConnectedToMaster();
			roomUI.SetActive(true);
			//PhotonNetwork.JoinLobby();
		}
		
		public override void OnJoinedLobby()
		{
			base.OnJoinedLobby();
			Debug.Log("WE JOINED THE LOBBY");
			roomUI.SetActive(true);
		}
		
		public void InitializeRoom(int defaultRoomIndex)
		{
			DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];
			
			//LOAD SCENE
			//PhotonNetwork.LoadLevel(roomSettings.sceneIndex);
			
			//CREATE THE ROOM
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = (byte) roomSettings.maxPlayer;
			roomOptions.IsVisible = true;
			roomOptions.IsOpen = true;
			PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions,TypedLobby.Default);
			
			//SceneManager.LoadScene(roomSettings.sceneIndex);
		}
		
		public override void OnJoinedRoom()
		{
			Debug.Log("Joined a Room");
			base.OnJoinedRoom();
			SceneManager.LoadScene(1);
		}
		
		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			Debug.Log("A new player joined the room");
			base.OnPlayerEnteredRoom(newPlayer);
		}*/
	}


