using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using UnityEngine.Events;

public class ER01_GameStart : MonoBehaviour
{
	public UnityEvent OnGameStart;
	public GameObject WaitingSign;
	public GameObject GameSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= GameSceneManager.GetComponent<GameSceneManager>().MaxPlayersAllowed)
		{
			WaitingSign.SetActive(false);
			OnGameStart.Invoke();
		}
    }
}
