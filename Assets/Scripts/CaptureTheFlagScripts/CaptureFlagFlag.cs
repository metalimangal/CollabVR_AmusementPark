using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFlagFlag : MonoBehaviour
{
    public Team flagBelongsTo;
    public GameObject spawnPoint;

    public void ResetPosition()
    {
        gameObject.transform.position = spawnPoint.transform.position;
        gameObject.transform.rotation = spawnPoint.transform.rotation;
    }

    public void CheckGrabberTeam()
    {
        //if grabber team == flagbelongsto
            //ResetPosition()
    }
}
