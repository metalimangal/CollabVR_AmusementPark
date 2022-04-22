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

    private PhotonView view;

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

    private void SetOwnerName()
    {
        ownerName = firstInteractorSelecting.transform.root.GetComponent<CF_PlayerMovement>().playerName;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (PhotonNetwork.InRoom && !view.IsMine)
        {
            view.RequestOwnership();
            Debug.Log("Gun Ownership Requested");
        }
        base.OnHoverEntered(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (view.IsMine)
        {
            belongsTo = args.interactorObject.transform.root.GetComponent<CF_PlayerMovement>().team;
            SetOwnerName();
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        belongsTo = Team.NONE;
        ownerName = "";
        base.OnSelectExited(args);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isAlreadyGrabbed = interactorsSelecting.Count > 0 && !interactor.Equals(interactorsSelecting[0]);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("Gun Ownership Request Received");

        if (targetView.gameObject != this.gameObject) {
            return;
        }

        if (!isSelected && targetView.Owner != requestingPlayer)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Gun Ownership Transfered");
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("Gun Ownership Transfered Failed!");
    }
}
