using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using UnityEngine.Events;

public class ER01_GameStart : MonoBehaviourPun
{
	public UnityEvent OnGameStart;
	public GameObject WaitingSign;
	public GameObject GameSceneManager;
	
	private bool GameStarted = false;
	private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= GameSceneManager.GetComponent<GameSceneManager>().MaxPlayersAllowed && !GameStarted)
		{
			GameStarted = true;
			pv.RPC("RPC_DisableWaitingSign", RpcTarget.AllBufferedViaServer, 1);
			OnGameStart.Invoke();
		}
    }
	
	[PunRPC]
	void RPC_DisableWaitingSign(int i)
	{
		WaitingSign.SetActive(false);
	}
}
