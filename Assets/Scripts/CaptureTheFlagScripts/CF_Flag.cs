using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class CF_Flag : MonoBehaviour
{
    public Team flagBelongsTo;
    public GameObject spawnPoint;

    private CF_NetworkGrab networkGrab;

    private void Start()
    {
        networkGrab = GetComponent<CF_NetworkGrab>();
    }

    public void ResetPosition()
    {
        gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        gameObject.transform.position = spawnPoint.transform.position;
        gameObject.transform.rotation = spawnPoint.transform.rotation;
        gameObject.GetComponent<XRGrabInteractable>().enabled = true;

        Debug.Log(flagBelongsTo + "'s Flag Returned!");
    }

    public void CheckGrabberTeam()
    {
        if (flagBelongsTo != GrabberTeam() && !CheckIfScoreZone())
        {
            ResetPosition();
        }
    }

    private Team GrabberTeam()
    {
        Team grabberTeam = networkGrab.firstInteractorSelecting.transform.parent.parent.GetComponent<CF_PlayerMovement>().team;

        return grabberTeam;
    }

    private bool CheckIfScoreZone()
    {
        if (networkGrab.firstInteractorSelecting.transform.TryGetComponent(out CF_ScoreZone zone))
        {
            return true;
        }
        else return false;
    }

    
}
