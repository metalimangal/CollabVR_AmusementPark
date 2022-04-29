using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class OwnershipTransfer : MonoBehaviourPun
{
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<OVRGrabbable>().isGrabbed)
        {
            GetComponent<PhotonView>().RequestOwnership();
        }
    }
}
