using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraManager : MonoBehaviour
{
    public Camera userCamera;
    // Start is called before the first frame update
    void Start()
    {
        PhotonView pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            userCamera.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
