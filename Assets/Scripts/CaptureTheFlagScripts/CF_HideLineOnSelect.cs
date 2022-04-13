using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActionBasedController))]
public class CF_HideLineOnSelect : MonoBehaviour
{
    public bool hideLineOnSelect = true;
    
    
    private XRInteractorLineVisual lineVisual;

    private void Awake()
    {
        lineVisual = gameObject.GetComponent<XRInteractorLineVisual>();
    }

    public void LineInteractorVisual(bool state)
    {
        if (hideLineOnSelect)
        {
            lineVisual.enabled = state;
        }
    }
}
