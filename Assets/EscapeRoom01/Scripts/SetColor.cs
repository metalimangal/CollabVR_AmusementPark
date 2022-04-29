using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using EscapeRoom01;


//
//Sets the color of the first MeshRenderer/SkinnedMeshRenderer found with GetComponentInChildren
//
	public class SetColor : MonoBehaviourPun
	{
		Color playerColor;
		public Text DisplayText;
		public void SetColorRPC(int n)
		{
			GetComponent<PhotonView>().RPC("RPC_SetColor", RpcTarget.AllBuffered, n);
			//Debug.Log("WE are setting the color");
		}

		[PunRPC]
		void RPC_SetColor(int n)
		{
			switch (n)
			{
				case 1:
					playerColor = Color.yellow;
					break;
				case 2:
					playerColor = Color.cyan;
					break;
				case 3:
					playerColor = Color.green;
					break;
				case 4:
					playerColor = Color.red;
					break;
				default:
					playerColor = Color.black;
					break;
			}
			//Debug.Log("WE are setting the color");
			playerColor = Color.Lerp(Color.white, playerColor, 0.5f);
			foreach (var item in GetComponentsInChildren<MeshRenderer>())
			{
				if (item != null)
				{
					item.material.color = playerColor;
				}
			}
			foreach (var item in GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				if (item != null)
				{
					item.material.color = playerColor;
				}
			}
			
			/*if(GetComponentInChildren<MeshRenderer>() != null)
				GetComponentInChildren<MeshRenderer>().material.color = playerColor;
			else if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
				GetComponentInChildren<SkinnedMeshRenderer>().material.color = playerColor;*/
			
			//DisplayText.color = playerColor;
		}
	}

