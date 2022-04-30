using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class ControlKeyboard : MonoBehaviourPunCallbacks

{
    private Vector3 playerVelocity;
    private bool groundedPlayer = false;
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;
    private PhotonView pv;

    public CharacterController controller;

    private void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}