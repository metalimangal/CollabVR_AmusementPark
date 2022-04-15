using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.XR.CoreUtils;
using Photon.Pun;


public class SetLocomotion : MonoBehaviour
{
    private Transform head;
    private Transform rig;
    private PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        head = transform.parent.GetChild(0);
        rig = transform.parent.parent;
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            transform.position = rig.position;
            transform.rotation = head.rotation;
        }
    }
}
