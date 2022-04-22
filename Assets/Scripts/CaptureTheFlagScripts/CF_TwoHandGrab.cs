using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CF_TwoHandGrab : CF_WeaponGrab
{
    public XRSimpleInteractable secondHandGrabPoint;
    private IXRSelectInteractor secondInteractor;
    private Quaternion initialAttachRotation;
    public enum TwoHandRotationType
    {
        None, First, Second
    }
    public TwoHandRotationType twoHandRotationType;
    void Start()
    {
        secondHandGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        secondHandGrabPoint.selectExited.AddListener(OnSecondHandRelease);
        initialAttachRotation = attachTransform.rotation;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor != null && interactorsSelecting.Count > 0)
        {
            attachTransform.rotation = GetTwoHandRotation();
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
        Debug.Log("Second Hand Release");
        attachTransform.rotation = initialAttachRotation;
        secondInteractor = null;
    }

    private void OnSecondHandGrab(SelectEnterEventArgs arg0)
    {
        Debug.Log("Second Hand Grabbed");
        secondInteractor = arg0.interactorObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        attachTransform.rotation = initialAttachRotation;
        secondInteractor = null;
    }
}
