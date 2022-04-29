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
using TMPro;


public class GameInfoDisplay : MonoBehaviourPun
{
	public Text GameInfoText;
	public Text TimerText;
	public GameObject GameSceneManager;
	public GameObject Timer;
	
    // Start is called before the first frame update
    void Start()
    {
		var all = FindObjectsOfType<GameObject>();
		foreach ( var item in all ) { 
			if (item.tag.CompareTo("GameSceneManager") == 0) 
				GameSceneManager = item;
		}
		
		var all_timer = FindObjectsOfType<GameObject>();
		foreach ( var item in all_timer ) { 
			if (item.tag.CompareTo("timer") == 0) 
				Timer = item;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (GameSceneManager != null)
		{
			if (GameSceneManager.GetComponent<GameSceneManager>().LimitedRoom)
			{
				GameInfoText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + GameSceneManager.GetComponent<GameSceneManager>().MaxPlayersAllowed;
			}
			else
			{
				GameInfoText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
			}
			//GameInfoText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + GameSceneManager.GetComponent<GameSceneManager>().MaxPlayersAllowed;
		}
		else if (GameSceneManager == null)
		{
			var all = FindObjectsOfType<GameObject>();
			foreach ( var item in all ) { 
			if (item.tag.CompareTo("GameSceneManager") == 0) 
				GameSceneManager = item;
			}
		}
		
		if (Timer != null)
		{
			
			TimerText.text = Timer.GetComponent<Timer>().timerValue;
		}
		else if (Timer == null)
		{
			var all_timer = FindObjectsOfType<GameObject>();
			foreach ( var item in all_timer ) { 
				if (item.tag.CompareTo("timer") == 0) 
					Timer = item;
			}
		}
    }
}
