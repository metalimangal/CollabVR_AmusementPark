using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using System;

public class CF_Flag : MonoBehaviourPunCallbacks
{
    public Team flagBelongsTo;
    public GameObject spawnPoint;
    public XRSocketInteractor flagSocket;

    private XRGrabInteractable interactable;
    private CF_NetworkGrab networkGrab;


    private void Awake()
    {
        networkGrab = GetComponent<CF_NetworkGrab>();
        interactable = GetComponent<XRGrabInteractable>();
    }

    public void ResetPosition()
    {
        interactable.enabled = false;
        gameObject.transform.position = spawnPoint.transform.position;
        gameObject.transform.rotation = spawnPoint.transform.rotation;
        interactable.enabled = true;

        Debug.Log(flagBelongsTo + "'s Flag Returned!");
    }

    public void CheckGrabberTeam()
    {
        if (flagBelongsTo == GrabberTeam())   // && !CheckIfScoreZone()
        {
            ResetPosition();
        }
    }

    private Team GrabberTeam()
    {
        if (!networkGrab.firstInteractorSelecting.transform.TryGetComponent(out ActionBasedController controller))
        {
            return Team.NONE;
        }

        if (networkGrab.firstInteractorSelecting.transform.root.TryGetComponent(out CF_PlayerMovement player))
        {
            return player.team;
        }

        return Team.NONE;
    }
}
