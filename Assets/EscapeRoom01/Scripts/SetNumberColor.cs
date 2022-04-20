using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//
//Sets the color of the first MeshRenderer/SkinnedMeshRenderer found with GetComponentInChildren
//

public class SetNumberColor : MonoBehaviourPun
{
	Color playerColor;
	public Text DisplayText;
	public Image BG;
	public void SetNumberColorRPC(int n)
	{
		GetComponent<PhotonView>().RPC("RPC_SetNumberColor", RpcTarget.AllBuffered, n);
	}

	[PunRPC]
	void RPC_SetNumberColor(int n)
	{
		switch (n)
		{
			case 1:
				playerColor = Color.red;
				break;
			case 2:
				playerColor = Color.cyan;
				break;
			case 3:
				playerColor = Color.green;
				break;
			case 4:
				playerColor = Color.yellow;
				break;
			case 5:
				playerColor = Color.magenta;
				break;
			default:
				playerColor = Color.black;
				break;
		}
		playerColor = Color.Lerp(Color.white, playerColor, 0.5f);
		DisplayText.color = playerColor;
		BG.color = Color.black;
	}
}

