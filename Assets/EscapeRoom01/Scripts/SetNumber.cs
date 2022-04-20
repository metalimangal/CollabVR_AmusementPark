using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class SetNumber : MonoBehaviourPun
{
	public Text DisplayText;
	public int PlayerNumber;
	
	// Start is called before the first frame update
	/*void Start()
	{
		if (photonView.IsMine)
		{
			DisplayText.text = PlayerName;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (photonView.IsMine)
		{
			DisplayText.text = PlayerName;
		}
	}*/
	
	public void SetNumberRPC(int n)
	{
		GetComponent<PhotonView>().RPC("RPC_SetNumber", RpcTarget.AllBuffered, n);
	}
	
	[PunRPC]
	void RPC_SetNumber(int n)
	{
		PlayerNumber = n;
		DisplayText.text = PlayerNumber.ToString();
		//DisplayText.color = Color.white;
	}
}

