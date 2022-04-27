using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using EscapeRoom01;

using Photon.Realtime;
using Photon.Pun;


public class Timer : MonoBehaviourPun
{
	public string timerValue;
	
	public string timerValue_temp;
	bool timerActive = false;
	float currentTime;
	//public int startMinutes;
	// Start is called before the first frame update
	void Start()
	{
		currentTime = 0;
		StartTimer();
		//timerValue = "1:23";
	}

	// Update is called once per frame
	void Update()
	{
		if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
		{
		//if(PhotonNetwork.IsMasterClient)
		//{
			if (timerActive)
			{
				currentTime = currentTime + Time.deltaTime;
			}
			
			GetComponent<PhotonView>().RPC("RPC_UpdateTimer", RpcTarget.AllBufferedViaServer, currentTime);
		}
	}
	
	public void StartTimer()
	{
		timerActive = true;
	}
	
	public void StopTimer()
	{
		timerActive = false;
	}
	
	[PunRPC]
	void RPC_UpdateTimer(float n)
	{
		TimeSpan time = TimeSpan.FromSeconds(n);
		timerValue = time.Minutes.ToString() + ":" + time.Seconds.ToString();
	}
}

