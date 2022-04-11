using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class CF_WeaponGrab : XRGrabInteractable, IPunOwnershipCallbacks
{
    public Team belongsTo = Team.NONE;
    public Transform shootTransform;
    public ParticleSystem ps;

    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }


    [PunRPC]
    void Shoot()
    {
        ps.Play();
        Ray ray = new Ray(shootTransform.position, shootTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (TryGetComponent(out CF_Player enemyPlayer))
            {
                enemyPlayer.TakeDamage(30);
            }
        }
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        if (view.IsMine)
        {
            view.RPC("Shoot", RpcTarget.All);
        }
        base.OnActivated(args);
    }



    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        view.RequestOwnership();
        base.OnHoverEntered(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        belongsTo = args.interactorObject.transform.gameObject.GetComponent<CF_Player>().team;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        belongsTo = Team.NONE;
        base.OnSelectExited(args);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (!isSelected)
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
}
