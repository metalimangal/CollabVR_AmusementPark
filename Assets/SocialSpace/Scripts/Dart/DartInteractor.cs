using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DartInteractor : MonoBehaviour
{
    public bool isGrabbed;
    public bool isRelease;
    public LineRenderer line;
    public GameObject TrajectoryOrigin;

    private List<Vector3> linePath = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        isGrabbed = false;
        isRelease = false;
    }

    void Update()
    {
        if (isRelease)
        {
            linePath.Add(TrajectoryOrigin.transform.position);
            line.positionCount = linePath.Count;
            line.SetPositions(linePath.ToArray());
        }

        if(linePath.Count > 10)
        {
            linePath.Clear();
        }
    }

    public void TakeInput()
    {
        isGrabbed = true;
        Debug.Log("is grabbed"); 
    }

    public void StopInput()
    {
        isGrabbed = false;
        isRelease = true;
        Debug.Log("is releasing");
    }

    public void desactivateInteractable()
    {
        GetComponent<XRGrabInteractable>().enabled = false;
    }
}
