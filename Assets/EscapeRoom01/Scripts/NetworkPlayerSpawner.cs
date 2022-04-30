using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//using EscapeRoom01;


	public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
	{
		[SerializeField] GameObject PlayerPrefab;
		private GameObject spawnedPlayerPrefab;
		// Start is called before the first frame update
		void Start()
		{
			
		}

		// Update is called once per frame
		void Update()
		{
			
		}
		
		public override void OnJoinedRoom()
		{
			base.OnJoinedRoom();
			spawnedPlayerPrefab = PhotonNetwork.Instantiate("EscapeRoom01/" + PlayerPrefab.name, transform.position, transform.rotation);
		}
		
		public override void OnLeftRoom()
		{
			base.OnLeftRoom();
			PhotonNetwork.Destroy(spawnedPlayerPrefab);
		}
	}

