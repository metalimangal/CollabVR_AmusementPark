using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using Photon.Pun;

public class NetworkPlayer : MonoBehaviour
{
    public Transform player;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Vector3 pos = GameObject.Find("OVRPlayerController").transform.position;
            pos.y -= 1;
            player.position = pos;
            player.rotation = GameObject.Find("OVRPlayerController").transform.GetChild(1).GetChild(0).transform.rotation;
            MapPosition(head, XRNode.Head);
            MapPosition(leftHand, XRNode.LeftHand);
            MapPosition(rightHand, XRNode.RightHand);
        }
    }

    void MapPosition(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.localPosition = position;
        target.localRotation = rotation;
    }
}
