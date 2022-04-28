using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;


public class TimerDisplay : MonoBehaviourPun
{
	public GameObject timer;
	public Text timerText;
	
	private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
		pv = GetComponent<PhotonView>();
        //timer = GameObject.FindWithTag("timer");
		var all = FindObjectsOfType<GameObject>();
		foreach ( var item in all ) { 
			if (item.tag.CompareTo("timer") == 0) 
				timer = item;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (timer != null)
		{
			pv.RPC("RPC_SetTimerValue", RpcTarget.AllBufferedViaServer, 0);
			//timerText.text = timer.GetComponent<Timer>().timerValue;
			
		}
		else if (timer == null)
		{
			var all = FindObjectsOfType<GameObject>();
			foreach ( var item in all ) { 
			if (item.tag.CompareTo("timer") == 0) 
				timer = item;
			}
		}
    }
	
	[PunRPC]
	void RPC_SetTimerValue(int i)
	{
		timerText.text = timer.GetComponent<Timer>().timerValue;
	}
}
