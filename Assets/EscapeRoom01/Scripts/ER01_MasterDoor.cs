using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class ER01_MasterDoor : MonoBehaviourPun
{
	public bool[] TileSlots; // 0 = book, 1 = oven, 2 = chest, 3 = downstairs
	public bool OpenDoorIndicator;
	
	private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
		pv = GetComponent<PhotonView>();
		for (int i = 0; i < TileSlots.Length - 1; i++)
		{
			TileSlots[i] = false;
		}
		OpenDoorIndicator = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(TileSlots[0] + " : " + TileSlots[1] + " : " + TileSlots[2] + " : " + TileSlots[3]);
		
		
		if (TileSlots[0] && TileSlots[1] && TileSlots[2] && TileSlots[3])
		{
			pv.RPC("RPC_OpenDoor", RpcTarget.AllBufferedViaServer, 0);
		}
    }
	
	public void ActivateTileSlot(int KeyIndex)
	{
		pv.RPC("RPC_ActivateTileSlot", RpcTarget.AllBufferedViaServer, KeyIndex);
	}
	
	[PunRPC]
	void RPC_ActivateTileSlot(int i)
	{
		TileSlots[i] = true;
	}
	
	[PunRPC]
	void RPC_OpenDoor(int i)
	{
		Destroy(gameObject);
	}
}
