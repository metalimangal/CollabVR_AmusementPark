/*
 * 
 * 
 * This script inherits from xr grab interactable and contains
 * all of the gun functionalities (bad coding, sorry)
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class CF_WeaponGrab : XRGrabInteractable, IPunOwnershipCallbacks
{
    [Header("Gun Related Stuff")]
    public Team belongsTo = Team.NONE;
    public string ownerName = "";

    private string grabberTeam;

    public PhotonView view;

    protected override void Awake()
    {
        base.Awake();
        view = GetComponent<PhotonView>();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    private void Start()
    {
        view = GetComponent<PhotonView>();
        PhotonNetwork.AddCallbackTarget(this);
    }

    [PunRPC]
    private void RPCSetOwner(string name)
    {
        ownerName = name;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (PhotonNetwork.InRoom && !view.IsMine)
        {
            view.RequestOwnership();
        }
        base.OnHoverEntered(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (view.IsMine)
        {
            var grabber = firstInteractorSelecting.transform.root.GetComponent<CF_PlayerMovement>();
            view.RPC("SetTeam", RpcTarget.All, grabber.team.ToString());
            view.RPC("RPCSetOwner", RpcTarget.All, grabber.playerName);
            
        }
        view.RPC("EnableGravity", RpcTarget.AllBuffered, "false");
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        view.RPC("SetTeam", RpcTarget.All, " ");
        view.RPC("RPCSetOwner", RpcTarget.All, "");
        view.RPC("EnableGravity", RpcTarget.AllBuffered, "true");
        base.OnSelectExited(args);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isAlreadyGrabbed = interactorsSelecting.Count > 0 && !interactor.Equals(interactorsSelecting[0]);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {

        if (targetView.gameObject != this.gameObject)
        {
            return;
        }

        if (!isSelected && targetView.Owner != requestingPlayer)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("Gun Ownership Transfered Failed!");
    }

    [PunRPC]
    private void SetTeam(string t)
    {
        if (t == "BLUE")
        {
            belongsTo = Team.BLUE;
        }
        else if (t == "RED")
        {
            belongsTo = Team.RED;
        }
        else
        {
            belongsTo = Team.NONE;
        }
    }

    [PunRPC]
    private void EnableGravity(string state)
    {
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        if (state == "true")
        {
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
        else
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }
}
