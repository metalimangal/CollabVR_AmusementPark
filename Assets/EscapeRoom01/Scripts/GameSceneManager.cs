using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;
using System.Linq;
//using EscapeRoom01;


	public class GameSceneManager : MonoBehaviourPunCallbacks
	{
		public bool LimitedRoom = false;
		[SerializeField] int MainLobbySceneIndex = 0;
		[SerializeField] int ER01_LobbySceneIndex = 0;
		[SerializeField] GameObject PlayerPrefab;
		[SerializeField] GameObject ovrCameraRig;
		[SerializeField] Transform[] spawnPoints;
		public int MaxPlayersAllowed = 4;
		private GameObject spawnedPlayerPrefab;
		
		private PhotonView pv;
		
		#region For player numbering
		private GameObject Head, LeftHand, RightHand;
			
		public int CurrentPlayerNumber = 0;
		public static GameSceneManager instance;
		public bool ShouldLeaveRoom = false;
		public bool BackToMainLobby = false;
		public bool BackToSubLobby = false;
		
		public static Player[] SortedPlayers;

		/// <summary>
		/// OnPlayerNumberingChanged delegate. Use
		/// </summary>
		public delegate void PlayerNumberingChanged();
		/// <summary>
		/// Called everytime the room Indexing was updated. Use this for discrete updates. Always better than brute force calls every frame.
		/// </summary>
		public static event PlayerNumberingChanged OnPlayerNumberingChanged;


		/// <summary>Defines the room custom property name to use for room player indexing tracking.</summary>
		public string RoomPlayerIndexedProp;

		/// <summary>
		/// dont destroy on load flag for this Component's GameObject to survive Level Loading.
		/// </summary>
		public bool dontDestroyOnLoad = true;
			
		#endregion
		
		//public Text DebugText;
		private int n_PlayersInRoom;
		
		
		private void Awake()
		{
			pv = GetComponent<PhotonView>();
			CCManager.Instance.gameObject.SetActive(false);
			ShouldLeaveRoom = false;
			/// If the game starts in Room scene, and is not connected, sends the player back to Lobby scene to connect first.
			if (!PhotonNetwork.NetworkingClient.IsConnected)
			{
				//SceneManager.LoadScene(MainLobbySceneIndex);
				PhotonNetwork.LoadLevel("Login and Network/Scenes/HomeScene");
				return;
			}
			/////////////////////////////////
			
			
			
			
			
			
			// For player numbering
			
			n_PlayersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;
			
			if (instance != null && instance != this && instance.gameObject != null)
			{
				GameObject.DestroyImmediate(instance.gameObject);
			}

			instance = this;
			if (dontDestroyOnLoad)
			{ 
				DontDestroyOnLoad(this.gameObject);
			}

			this.RefreshData();
			
			// For player numbering
			
			/*
			if((PhotonNetwork.LocalPlayer.ActorNumber) <= spawnPoints.Length)
			{
				ovrCameraRig.transform.position = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position;
				ovrCameraRig.transform.rotation = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.rotation;
			}*/
		}
		
		// Start is called before the first frame update
		private void Start()
		{
			/*
			//Instantiate Head
			GameObject obj = (PhotonNetwork.Instantiate(headPrefab.name, OculusPlayer.instance.head.transform.position, OculusPlayer.instance.head.transform.rotation, 0));
			Head = obj;
			
			// Display player Nick Name and number
			obj.GetComponent<SetNickname>().SetNicknameRPC(PhotonNetwork.LocalPlayer.NickName);

			
			//Instantiate right hand
			obj = (PhotonNetwork.Instantiate(handRPrefab.name, OculusPlayer.instance.rightHand.transform.position, OculusPlayer.instance.rightHand.transform.rotation, 0));
			RightHand = obj;
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				toolsR.Add(obj.transform.GetChild(i).gameObject);
				if(i > 0)
					toolsR[i].transform.parent.GetComponent<PhotonView>().RPC("DisableTool", RpcTarget.AllBuffered, 1);
			}

			//Instantiate left hand
			obj = (PhotonNetwork.Instantiate(handLPrefab.name, OculusPlayer.instance.leftHand.transform.position, OculusPlayer.instance.leftHand.transform.rotation, 0));
			LeftHand = obj;
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				toolsL.Add(obj.transform.GetChild(i).gameObject);
				if (i > 0)
					toolsL[i].transform.parent.GetComponent<PhotonView>().RPC("DisableTool", RpcTarget.AllBuffered, 1);
			}*/
			
			
			spawnedPlayerPrefab = PhotonNetwork.Instantiate("EscapeRoom01/" + PlayerPrefab.name, transform.position, transform.rotation);
			Debug.Log("Spawned");
			
			if((PhotonNetwork.LocalPlayer.ActorNumber) <= spawnPoints.Length)
			{
				ovrCameraRig.transform.position = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position;
				ovrCameraRig.transform.rotation = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.rotation;
			}
			
			else if ((PhotonNetwork.LocalPlayer.ActorNumber) > spawnPoints.Length)
			{
				int n = (PhotonNetwork.LocalPlayer.ActorNumber) % (spawnPoints.Length);
				ovrCameraRig.transform.position = spawnPoints[n - 1].transform.position;
				ovrCameraRig.transform.rotation = spawnPoints[n - 1].transform.rotation;
			}
		}

		// Update is called once per frame
		void Update()
		{
			//Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);
			if (ShouldLeaveRoom)
			{
				LeaveRoom();
			}
			
			
			
			/*
			if (ShouldLeaveRoom && BackToSubLobby)
			{
				SceneManager.LoadScene(ER01_LobbySceneIndex);
			}
			
			else if (ShouldLeaveRoom && BackToMainLobby)
			{
				LeaveRoom();
			}
			*/
		}
		
		/*public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			spawnedPlayerPrefab = PhotonNetwork.Instantiate("EscapeRoom01/" + PlayerPrefab.name, transform.position, transform.rotation);
		}*/
		
		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom()
		{
			//base.OnLeftRoom();
			PhotonNetwork.Destroy(spawnedPlayerPrefab);
			// For player numbering
			PhotonNetwork.LocalPlayer.CustomProperties.Remove(PlayerNumbering.RoomPlayerIndexedProp);
			// For player numbering
			
			if (BackToSubLobby)
			{
				//SceneManager.LoadScene(ER01_LobbySceneIndex);
				SceneManager.LoadScene(MainLobbySceneIndex);
				//PhotonNetwork.LoadLevel("Login and Network/Scenes/HomeScene");
			}
			else if (BackToMainLobby)
			{
				//PhotonNetwork.Disconnect();
				//SceneManager.LoadScene(MainLobbySceneIndex);
				PhotonNetwork.LoadLevel("Login and Network/Scenes/HomeScene");
			}
			else
			{
				PhotonNetwork.LoadLevel("Login and Network/Scenes/HomeScene");
				//PhotonNetwork.Disconnect();
			}
			//PhotonNetwork.Disconnect();
			//SceneManager.LoadScene(ER01_LobbySceneIndex);
		}
		
		//If disconnected from server, returns to Lobby to reconnect
		/*public override void OnDisconnected(DisconnectCause cause)
		{
			base.OnDisconnected(cause);
			//SceneManager.LoadScene(MainLobbySceneIndex);
			PhotonNetwork.LoadLevel("Login and Network/Scenes/HomeScene");
		}*/
		
		
		
		
		
		
		
		
		
		// NEW ON DISCONNECT PROCESS //
		public override void OnDisconnected(DisconnectCause cause)
		{
			if (this.CanRecoverFromDisconnect(cause))
			{
				this.Recover();
			}
		}

		private bool CanRecoverFromDisconnect(DisconnectCause cause)
		{
			switch (cause)
			{
				// the list here may be non exhaustive and is subject to review
				case DisconnectCause.Exception:
				case DisconnectCause.ServerTimeout:
				case DisconnectCause.ClientTimeout:
				case DisconnectCause.DisconnectByServerLogic:
				case DisconnectCause.DisconnectByServerReasonUnknown:
					return true;
			}
			return false;
		}

		private void Recover()
		{
			if (!PhotonNetwork.ReconnectAndRejoin())
			{
				Debug.LogError("ReconnectAndRejoin failed, trying Reconnect");
				if (!PhotonNetwork.Reconnect())
				{
					Debug.LogError("Reconnect failed, trying ConnectUsingSettings");
					if (!PhotonNetwork.ConnectUsingSettings())
					{
						Debug.LogError("ConnectUsingSettings failed");
					}
				}
			}
		}
		// NEW ON DISCONNECT PROCESS //
		
		
		

		//So we stop loading scenes if we quit app
		private void OnApplicationQuit()
		{
			StopAllCoroutines();
		}
		








		
		#region For player numbering 2
		
		public override void OnJoinedRoom()
		{
			//base.OnJoinedRoom();
			this.RefreshData();
			//Debug.Log("Joined " + PhotonNetwork.LocalPlayer.GetPlayerNumber());
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			//base.OnPlayerEnteredRoom();
			this.RefreshData();
			//Debug.Log("Entered " + PhotonNetwork.LocalPlayer.GetPlayerNumber());
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			this.RefreshData();
		}
		
		
		public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
		{
			if (changedProps != null && changedProps.ContainsKey(PlayerNumbering.RoomPlayerIndexedProp))
			{
				this.RefreshData();
			}
			Debug.Log("Updated " + PhotonNetwork.LocalPlayer.GetPlayerNumber());
			
			
			// NEED CHANGES
			
			print(PhotonNetwork.LocalPlayer.GetPlayerNumber());
			spawnedPlayerPrefab.GetComponent<SetColor>().SetColorRPC(PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1);
			//spawnedPlayerPrefab.GetComponent<SetNicknameColor>().SetNicknameColorRPC(PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1);
			spawnedPlayerPrefab.GetComponent<SetNumber>().SetNumberRPC(PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1);
			spawnedPlayerPrefab.GetComponent<SetNumberColor>().SetNumberColorRPC(PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1);
			
			CurrentPlayerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1;
			//spawnedPlayerPrefab.transform.GetComponentInChildren<SetColor>().SetColorRPC(PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1);
			
			//spawnedPlayerPrefab.transform.GetComponentInChildren<SetColor>().SetColorRPC(PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1);
			
			// NEED CHANGES
			
			/*if((PhotonNetwork.LocalPlayer.GetPlayerNumber()) <= spawnPoints.Length + 1)
			{
				ovrCameraRig.transform.position = spawnPoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].transform.position;
				ovrCameraRig.transform.rotation = spawnPoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].transform.rotation;
			}*/
			
			/*if ((PhotonNetwork.LocalPlayer.GetPlayerNumber() + 1) > MaxPlayersAllowed)
			{
				LeaveRoom();
			}*/
		}
		
		
		
		public void RefreshData()
		{
			if (PhotonNetwork.CurrentRoom == null)
			{
				return;
			}

			if (PhotonNetwork.LocalPlayer.GetPlayerNumber() >= 0)
			{
				SortedPlayers = PhotonNetwork.CurrentRoom.Players.Values.OrderBy((p) => p.GetPlayerNumber()).ToArray();
				if (OnPlayerNumberingChanged != null)
				{
					OnPlayerNumberingChanged();
				}
				return;
			}


			HashSet<int> usedInts = new HashSet<int>();
			Player[] sorted = PhotonNetwork.PlayerList.OrderBy((p) => p.ActorNumber).ToArray();

			string allPlayers = "all players: ";
			foreach (Player player in sorted)
			{
				allPlayers += player.ActorNumber + "=" + RoomPlayerIndexedProp + ":"+player.GetPlayerNumber()+", ";

				int number = player.GetPlayerNumber();

				// if it's this user, select a number and break
				// else:
					// check if that user has a number
					// if not, break!
					// else remember used numbers

				if (player.IsLocal)
				{
					//Debug.Log ("PhotonNetwork.CurrentRoom.PlayerCount = " + PhotonNetwork.CurrentRoom.PlayerCount);

					// select a number
					for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
					{
						if (!usedInts.Contains(i))
						{
							player.SetPlayerNumber(i);
							break;
						}
					}
					// then break
					break;
				}
				else
				{
					if (number < 0)
					{
						break;
					}
					else
					{
						usedInts.Add(number);
					}
				}
			}

			//Debug.Log(allPlayers);
			//Debug.Log(PhotonNetwork.LocalPlayer.ToStringFull() + " has PhotonNetwork.player.GetPlayerNumber(): " + PhotonNetwork.LocalPlayer.GetPlayerNumber());

			SortedPlayers = PhotonNetwork.CurrentRoom.Players.Values.OrderBy((p) => p.GetPlayerNumber()).ToArray();
			if (OnPlayerNumberingChanged != null)
			{
				OnPlayerNumberingChanged();
			}
		}
		
		#endregion
		
		
		
		
		


		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
			//SceneManager.LoadScene(0);
			
			//PhotonNetwork.Destroy(spawnedPlayerPrefab);
			// For player numbering
			//PhotonNetwork.LocalPlayer.CustomProperties.Remove(PlayerNumbering.RoomPlayerIndexedProp);
			
			//PhotonNetwork.LoadLevel("Login and Network/Scenes/HomeScene");
		}
	}


