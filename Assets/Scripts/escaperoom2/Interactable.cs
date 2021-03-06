using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using Photon.Pun;

public class Interactable : MonoBehaviour
{
    private Transform leftHand;
    private Transform rightHand;
    private const float closeDist = 0.3f;
    private bool isSelected = false;

    private EscapeManager escManager;

    public int itemId;

    private bool isFound = false;

    private InputDevice inputDeviceLeft;
    private InputDevice inputDeviceRight;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("XR Rig").transform.GetChild(0).GetChild(1);
        rightHand = GameObject.Find("XR Rig").transform.GetChild(0).GetChild(2);

        escManager = GameObject.Find("[MANAGERS]").GetComponent<EscapeManager>();

        var inputDevicesLeft = new List<InputDevice>();
        var inputDevicesRight = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, inputDevicesLeft);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, inputDevicesRight);
        inputDeviceLeft = inputDevicesLeft[0];
        inputDeviceRight = inputDevicesRight[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFound)
        {
            float leftHandDist = Vector3.Distance(leftHand.position, transform.position);
            float rightHandDist = Vector3.Distance(rightHand.position, transform.position);
            if (leftHandDist < closeDist || rightHandDist < closeDist)
            {
                GetComponent<Outline>().enabled = true;
                isSelected = true;
            }
            else
            {
                GetComponent<Outline>().enabled = false;
                isSelected = false;
            }

            if (isSelected)
            {
                if (LeftButtonDown(CommonUsages.primaryButton) || RightButtonDown(CommonUsages.primaryButton))
                {
                    escManager.ShowInfo(itemId);
                    GetComponent<PhotonView>().RPC("SetFound", RpcTarget.All);
                }
            }
        }
        else
            GetComponent<Outline>().enabled = false;
    }

    [PunRPC]
    public void SetFound()
    {
        isFound = true;
    }

    private bool LeftButtonDown(InputFeatureUsage<bool> botton)
    {
        return inputDeviceLeft.TryGetFeatureValue(botton, out bool value) && value;
    }

    private bool RightButtonDown(InputFeatureUsage<bool> botton)
    {
        return inputDeviceRight.TryGetFeatureValue(botton, out bool value) && value;
    }
}
