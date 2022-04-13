using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InputSync : MonoBehaviour
{
    public GameObject head;
    public GameObject right;
    public GameObject left;

    private Transform target;
    private Transform target_head;
    private Transform target_right;
    private Transform target_left;

    private PhotonView pv;
    private CharacterController cc;

    private Vector3 playerVelocity;
    private bool groundedPlayer = false;
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            target = transform.GetChild(3);

            if (target.name == "OVRPlayerController(Clone)")
            {
                target_head = target.GetChild(1).GetChild(0).GetChild(1);
                target_right = target.GetChild(1).GetChild(0).GetChild(5).GetChild(0);
                target_left = target.GetChild(1).GetChild(0).GetChild(4).GetChild(0);

            }

            else if (target.name == "PCplayer(Clone)")
            {
                target_head = target.GetChild(0);
                target_right = target.GetChild(0);
                target_left = target.GetChild(0);

                cc = target.GetComponent<CharacterController>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            head.transform.SetPositionAndRotation(target_head.position, target_head.rotation);
            right.transform.SetPositionAndRotation(target_right.position, target_right.rotation);
            right.transform.Rotate(0.0f, 0.0f, -90.0f);
            left.transform.SetPositionAndRotation(target_left.position, target_left.rotation);
            left.transform.Rotate(0.0f, 0.0f, 90.0f);

            if(cc != null)
            {
                groundedPlayer = cc.isGrounded;
                if (groundedPlayer && playerVelocity.y < 0)
                {
                    playerVelocity.y = 0f;
                }

                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                cc.Move(move * Time.deltaTime * playerSpeed);

                playerVelocity.y += gravityValue * Time.deltaTime;
                cc.Move(playerVelocity * Time.deltaTime);
            }
        }
    }
}
