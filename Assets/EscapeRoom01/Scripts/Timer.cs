using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using EscapeRoom01;

using Photon.Realtime;
using Photon.Pun;


public class Timer : MonoBehaviourPunCallbacks, IPunObservable
{
	public string timerValue; // Observable variable
	
	public GameObject GameSceneManager;
	public string timerValue_temp;
	public bool timerActive = false;
	float currentTime;
	float startTime;
	//public int startMinutes;
	// Start is called before the first frame update
	void Start()
	{
		currentTime = 0;
		startTime = 0;
		//StartTimer();
		timerValue = "00:00";
	}

	// Update is called once per frame
	void Update()
	{
		//if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
		//if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
		//{
		//if (GameSceneManager.GetComponent<GameSceneManager>().CurrentPlayerNumber == 1)
		//{
		//if(PhotonNetwork.IsMasterClient)
		//{
		if (timerActive)
		{
			currentTime = currentTime + (startTime - Time.deltaTime);
		}
		TimeSpan time = TimeSpan.FromSeconds(currentTime);
		timerValue = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
		
		//GetComponent<PhotonView>().RPC("RPC_UpdateTimer", RpcTarget.AllBufferedViaServer, currentTime);
		//}
	}
	
	public void StartTimer()
	{
		startTime = Time.deltaTime;
		timerActive = true;
	}
	
	public void StopTimer()
	{
		timerActive = false;
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.IsWriting)
		{
			stream.SendNext(timerValue);
		}
		else
		{
			this.timerValue = (string)stream.ReceiveNext();
		}
	}
	
	[PunRPC]
	void RPC_UpdateTimer(float n)
	{
		TimeSpan time = TimeSpan.FromSeconds(n);
		timerValue = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
	}
}

