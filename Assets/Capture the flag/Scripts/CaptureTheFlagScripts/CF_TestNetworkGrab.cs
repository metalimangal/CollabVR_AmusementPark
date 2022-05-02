using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class CF_TestNetworkGrab : XRGrabInteractable, IPunOwnershipCallbacks
{
    private PhotonView view;

    protected override void Awake()
    {
        base.Awake();

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

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (PhotonNetwork.InRoom && args.interactorObject.GetType() != typeof(XRSocketInteractor))
        {
            if (!view.IsMine)
            {
                view.RequestOwnership();
                Debug.Log("Ownership Requested");
            }
            else
            {

            }
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        view.RPC("EnableGravity", RpcTarget.AllBuffered, "true");
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView.gameObject != this.gameObject)
        {
            return;
        }

        if (targetView.Owner != requestingPlayer)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        view.RPC("EnableGravity", RpcTarget.AllBuffered, "false");
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {

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
