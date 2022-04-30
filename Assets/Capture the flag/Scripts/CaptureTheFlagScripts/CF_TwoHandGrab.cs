using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class CF_TwoHandGrab : XRGrabInteractable, IPunOwnershipCallbacks
{
    public XRSimpleInteractable secondHandGrabPoint;
    private IXRSelectInteractor secondInteractor;
    private Quaternion initialAttachRotation;
   
    public Team belongsTo = Team.NONE;
    public string ownerName;

    public PhotonView view;

    public enum TwoHandRotationType
    {
        None, First, Second
    }
    public TwoHandRotationType twoHandRotationType;

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

    void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
        secondHandGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        secondHandGrabPoint.selectExited.AddListener(OnSecondHandRelease);
        secondHandGrabPoint.enabled = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor != null && interactorsSelecting.Count > 0)
        {
            firstInteractorSelecting.GetAttachTransform(this).rotation = GetTwoHandRotation();
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.GetAttachTransform(secondHandGrabPoint).position - firstInteractorSelecting.GetAttachTransform(this).position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.GetAttachTransform(secondHandGrabPoint).position - firstInteractorSelecting.GetAttachTransform(this).position, firstInteractorSelecting.GetAttachTransform(this).up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.GetAttachTransform(secondHandGrabPoint).position - firstInteractorSelecting.GetAttachTransform(this).position, secondInteractor.GetAttachTransform(secondHandGrabPoint).up);
        }
        return targetRotation;
    }

    private void OnSecondHandRelease(SelectExitEventArgs arg0)
    {
        if (firstInteractorSelecting != null) { firstInteractorSelecting.GetAttachTransform(this).localRotation = initialAttachRotation; }
        secondInteractor = null;
    }

    private void OnSecondHandGrab(SelectEnterEventArgs arg0)
    {
        secondInteractor = arg0.interactorObject;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        initialAttachRotation = firstInteractorSelecting.GetAttachTransform(this).localRotation;
        if (view.IsMine)
        {
            secondHandGrabPoint.enabled = true;
            var grabber = firstInteractorSelecting.transform.root.GetComponent<CF_PlayerMovement>();
            view.RPC("SetTeam", RpcTarget.All, grabber.team.ToString());
            view.RPC("RPCSetOwner", RpcTarget.All, grabber.playerName);
        }
        view.RPC("EnableGravity", RpcTarget.AllBuffered, "false");
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        secondInteractor = null;
        secondHandGrabPoint.enabled = false;
        view.RPC("SetTeam", RpcTarget.All, " ");
        view.RPC("RPCSetOwner", RpcTarget.All, "");
        view.RPC("EnableGravity", RpcTarget.AllBuffered, "true");

    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        firstInteractorSelecting.GetAttachTransform(this).localRotation = initialAttachRotation;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (PhotonNetwork.InRoom && !view.IsMine)
        {
            view.RequestOwnership();
        }
        base.OnHoverEntered(args);
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
        Debug.Log("Rifle Ownership Transfered Failed!");
    }

    [PunRPC]
    private void RPCSetOwner( string name)
    {
        ownerName = name;
    }
    [PunRPC]
    private void SetTeam(string t)
    {
        if (t ==  "BLUE")
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
