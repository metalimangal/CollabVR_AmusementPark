using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CF_PlayerName : MonoBehaviour
{
    public string playerName = "";

    private void Awake()
    {
        PhotonNetwork.LocalPlayer.CustomProperties["Name"] = playerName;
    }
}
