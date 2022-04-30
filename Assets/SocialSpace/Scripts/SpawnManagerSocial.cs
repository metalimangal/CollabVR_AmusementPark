using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SpawnManagerSocial : MonoBehaviour
{
    [SerializeField]
    GameObject GenericVRPlayerPrefab;
    public UpdateUserInfo usrInfo;
    bool initalPlayer = false;

    public Vector3 spawnPosition;
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GameObject player = PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spawnPosition, Quaternion.identity);

            if (!initalPlayer)
            {
                initalPlayer = true;
                usrInfo = player.GetComponent<UpdateUserInfo>();
            }
        }
    }


    void Update()
    {
        
    }
}
