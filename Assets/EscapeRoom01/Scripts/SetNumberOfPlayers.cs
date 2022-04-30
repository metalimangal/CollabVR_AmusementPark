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

public class SetNumberOfPlayers : MonoBehaviourPunCallbacks
{
	public TextMeshProUGUI NumberOfPlayersText;
	public GameObject GameSceneManager;
	//public bool LimitedRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (GameSceneManager.GetComponent<GameSceneManager>().LimitedRoom)
		{
			NumberOfPlayersText.text = "Current players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + GameSceneManager.GetComponent<GameSceneManager>().MaxPlayersAllowed;
		}
		else
		{
			NumberOfPlayersText.text = "Current players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
		}
    }
}
