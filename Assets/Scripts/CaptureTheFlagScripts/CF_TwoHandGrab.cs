using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class CF_TwoHandGrab : CF_WeaponGrab, IPunOwnershipCallbacks
{
    public XRSimpleInteractable secondHandGrabPoint;
    private IXRSelectInteractor secondInteractor;
    private Quaternion initialAttachRotation;
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
            belongsTo = args.interactorObject.transform.root.GetComponent<CF_PlayerMovement>().team;
            SetOwnerName();
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        secondInteractor = null;
        belongsTo = Team.NONE;
        ownerName = "";

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
            Debug.Log("Gun Ownership Requested");
        }
        base.OnHoverEntered(args);
    }
}
