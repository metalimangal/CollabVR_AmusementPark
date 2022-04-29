using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Interactable : MonoBehaviour
{
    private Transform leftHand;
    private Transform rightHand;
    private readonly float closeDist = 0.3f;
    private bool isSelected = false;

    private EscapeManager escManager;

    public int itemId;

    private bool isFound = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("OVRPlayerController");
        leftHand = player.transform.GetChild(1).GetChild(0).GetChild(4);
        rightHand = player.transform.GetChild(1).GetChild(0).GetChild(5);

        escManager = GameObject.Find("Network Manager").GetComponent<EscapeManager>();
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
                if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
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
}
