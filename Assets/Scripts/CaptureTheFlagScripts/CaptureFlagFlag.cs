using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class CaptureFlagFlag : MonoBehaviour
{
    public Team flagBelongsTo;
    public GameObject spawnPoint;

    public void ResetPosition()
    {
        gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        gameObject.transform.position = spawnPoint.transform.position;
        gameObject.transform.rotation = spawnPoint.transform.rotation;
        gameObject.GetComponent<XRGrabInteractable>().enabled = true;
    }

    public void CheckGrabberTeam()
    {
        //if grabber team == flagbelongsto
            //ResetPosition()
    }
}
