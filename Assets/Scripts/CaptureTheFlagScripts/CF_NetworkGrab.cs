using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using System;

[RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
public class CF_NetworkGrab : XRGrabInteractable, IPunOwnershipCallbacks
{
    private PhotonView view;
    public static event Action OnFlagGrabbed;

    protected override void Awake()
    {
        base.Awake();
        
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start() {
        view = GetComponent<PhotonView>();
        PhotonNetwork.AddCallbackTarget(this);
    }
    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (PhotonNetwork.InRoom && args.interactorObject.GetType() != typeof(XRSocketInteractor))
        {
            view.RPC("InvokeGrabEvent", RpcTarget.All);
            if (!view.IsMine)
            {
                view.RequestOwnership();
                Debug.Log("Ownership Requested");
            }
        }
        base.OnSelectEntered(args);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("Ownership Request Received");

        if (targetView.gameObject != this.gameObject) {
            return;
        }

        if (targetView.Owner != requestingPlayer)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        foreach (var item in interactorsSelecting)
        {
            if (item.GetType() == typeof(XRSocketInteractor))
            {
                return true;
            }
        }
        bool isAlreadyGrabbed = interactorsSelecting.Count > 0 && !interactor.Equals(interactorsSelecting[0]);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership Request Transfered");
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        
    }

    private bool IsSelectedBySocket()
    {
        if (isSelected && firstInteractorSelecting.transform.TryGetComponent(out XRSocketInteractor socket))
        {
            if (interactorsSelecting.Count > 1) {
                return false;
            }
            return true;
        }
        return false;
    }

    [PunRPC]
    private void InvokeGrabEvent()
    {
        OnFlagGrabbed?.Invoke();
    }
}
