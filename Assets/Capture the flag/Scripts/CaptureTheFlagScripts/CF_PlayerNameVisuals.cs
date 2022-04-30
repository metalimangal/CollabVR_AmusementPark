using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CF_PlayerNameVisuals : MonoBehaviour
{
    public GameObject head;
    public float offset = 0.45f;

    private void Update()
    {
        gameObject.transform.localPosition = head.transform.localPosition + offset * Vector3.up;
        gameObject.transform.rotation = Quaternion.identity;
    }
}
