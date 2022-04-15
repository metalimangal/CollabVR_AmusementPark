using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.XR.CoreUtils;
using Photon.Pun;


public class SetLocomotion : MonoBehaviour
{
    private Transform target;
    private PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.parent.GetChild(0);
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}
