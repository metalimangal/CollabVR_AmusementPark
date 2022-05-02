using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CF_FlagGrabListener : MonoBehaviour
{
    private XRSocketInteractor socket;
    private void Awake()
    {
        socket = gameObject.GetComponent<XRSocketInteractor>();
        CF_NetworkGrab.OnFlagGrabbed += OnOnFlagGrabbed;
    }

    private void OnDestroy()
    {
        CF_NetworkGrab.OnFlagGrabbed -= OnOnFlagGrabbed;
    }

    private void OnOnFlagGrabbed()
    {
        StartCoroutine(DisableSocketCo());
    }

    IEnumerator DisableSocketCo()
    {
        socket.socketActive = false;
        yield return new WaitForSeconds(1);
        socket.socketActive = true;
    }
}
