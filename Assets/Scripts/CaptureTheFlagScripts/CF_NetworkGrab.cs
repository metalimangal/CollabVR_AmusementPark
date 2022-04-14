using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
public class CF_NetworkGrab : XRGrabInteractable, IPunOwnershipCallbacks
{
    private PhotonView view;

    protected override void Awake()
    {
        base.Awake();
        view = GetComponent<PhotonView>();
        PhotonNetwork.AddCallbackTarget(this);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (PhotonNetwork.InRoom)
        {
            view.RequestOwnership();
        }
        base.OnSelectEntered(args);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (!IsSelectedBySocket() && targetView == view)
        {
            view.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        
    }

    private bool IsSelectedBySocket()
    {
        if (isSelected && firstInteractorSelecting.transform.TryGetComponent(out XRSocketInteractor socket))
        {
            return true;
        }
        return false;
    }
}
