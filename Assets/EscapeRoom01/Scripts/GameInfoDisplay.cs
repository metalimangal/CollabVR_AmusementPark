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
	public GameObject GameSceneManager;
	
    // Start is called before the first frame update
    void Start()
    {
		var all = FindObjectsOfType<GameObject>();
		foreach ( var item in all ) { 
			if (item.tag.CompareTo("GameSceneManager") == 0) 
				GameSceneManager = item;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (GameSceneManager != null)
		{
			GameInfoText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + GameSceneManager.GetComponent<GameSceneManager>().MaxPlayersAllowed;
		}
		else if (GameSceneManager == null)
		{
			var all = FindObjectsOfType<GameObject>();
			foreach ( var item in all ) { 
			if (item.tag.CompareTo("GameSceneManager") == 0) 
				GameSceneManager = item;
			}
		}
    }
}
