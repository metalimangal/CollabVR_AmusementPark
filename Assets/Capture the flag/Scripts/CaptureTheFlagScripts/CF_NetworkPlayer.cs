/*
 * 
 * This script is used for matching the movement of the Pun-instantiated prefab that represents the player.
 * It also handels all the animation synchronization for the player's hands as well.
 * 
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class CF_NetworkPlayer : MonoBehaviour
{
    public bool EnableNetworkAvatar = false;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform body;

    public Animator leftHandAni;
    public Animator rightHandAni;

    private PhotonView view;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    void Start()
    {
        view = GetComponent<PhotonView>();
        GameObject rig = GameObject.FindGameObjectWithTag("Player");
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");
        
        //Disable Renderer for your own "networked" avatar
        if (view.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = EnableNetworkAvatar;
            }
        }
    }

    void Update()
    {
        //Only allow movement of your own "networked" avatar
        if (view.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            //Body Position
            MapBody(body, headRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAni);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAni);
        }
    }
    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

    void MapBody(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
    }
}
